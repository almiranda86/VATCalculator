using FluentValidation;
using VATCalculator.Domain.Behavior.Service;
using VATCalculator.Domain.Core.ResultPattern;
using VATCalculator.Domain.Queries;
using VATCalculator.Domain.Strategy;

namespace VATCalculator.Service.Managers
{
    public class CalculationManager : ICalculationManager
    {
        private readonly IValidator<GetCalculationRequest> _validator;
        private readonly ICalculateVATResolver _resolver;
        public CalculationManager(IValidator<GetCalculationRequest> validator,
            ICalculateVATResolver resolver)
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        }

        public async Task<Result<GetCalculationResult>> HandleCalculation(GetCalculationRequest request, CancellationToken cancellationToken)
        {
            var response = new GetCalculationResult();

            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            if (ShouldCalculateFromNetAmount(request))
            {
                response = CalculateFromNetAmmount(request.NetAmmount, request.VatRate);
                return response;
            }

            if (ShouldCalculateFromGrossAmmount(request))
            {
                response = CalculateFromGrossAmmount(request.GrossAmount, request.VatRate);
                return response;
            }

            if (ShouldCalculateFromVatTaxAmmnount(request))
            {
                response = CalculateFromVatTaxAmmount(request.VatTaxAmmount, request.VatRate);
                return response;
            }

            return new Error($"Something went wrong while executing {nameof(CalculationManager)}");
        }


        private GetCalculationResult CalculateFromNetAmmount(string netAmmount, string vatRate)
        {
            var calculateVAT = _resolver.Resolver(EVatCalculationMethods.NetAmmount);

            var (netAmmountResult, grossAmmount, vatTaxAmmount) = calculateVAT.Calculate(netAmmount, vatRate);
            return new GetCalculationResult((double)netAmmountResult, (double)grossAmmount, (double)vatTaxAmmount);
        }

        private GetCalculationResult CalculateFromGrossAmmount(string grossAmmount, string vatRate)
        {
            var calculateVAT = _resolver.Resolver(EVatCalculationMethods.GrossAmmount);

            var (netAmmount, grossAmmountResult, vatTaxAmmount) = calculateVAT.Calculate(grossAmmount, vatRate);
            return new GetCalculationResult((double)netAmmount, (double)grossAmmountResult, (double)vatTaxAmmount);
        }

        public GetCalculationResult CalculateFromVatTaxAmmount(string vatTaxAmmount, string vatRate)
        {
            var calculateVAT = _resolver.Resolver(EVatCalculationMethods.VatTaxAmmount);

            var (netAmmount, grossAmmount, vatTaxAmmountResult) = calculateVAT.Calculate(vatTaxAmmount, vatRate);
            return new GetCalculationResult((double)netAmmount, (double)grossAmmount, (double)vatTaxAmmountResult);
        }


        private static bool ShouldCalculateFromVatTaxAmmnount(GetCalculationRequest request) =>
            !string.IsNullOrEmpty(request.VatTaxAmmount) && (string.IsNullOrEmpty(request.NetAmmount) && string.IsNullOrEmpty(request.GrossAmount));


        private static bool ShouldCalculateFromGrossAmmount(GetCalculationRequest request) =>
            !string.IsNullOrEmpty(request.GrossAmount) && (string.IsNullOrEmpty(request.NetAmmount) && string.IsNullOrEmpty(request.VatTaxAmmount));

        private static bool ShouldCalculateFromNetAmount(GetCalculationRequest request) =>
            !string.IsNullOrEmpty(request.NetAmmount) && (string.IsNullOrEmpty(request.GrossAmount) && string.IsNullOrEmpty(request.VatTaxAmmount));
    }
}
