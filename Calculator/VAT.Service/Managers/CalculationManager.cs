using FluentValidation;
using VATCalculator.Domain.Behavior.Service;
using VATCalculator.Domain.Core.ResultPattern;
using VATCalculator.Domain.Queries;

namespace VATCalculator.Service.Managers
{
    public class CalculationManager : ICalculationManager, INetAmmountCalculationManager, IGrossAmmountCalculationManager, IVatTaxAmmountCalculationManager
    {
        private readonly IValidator<GetCalculationRequest> _validator;

        public CalculationManager(IValidator<GetCalculationRequest> validator)
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
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


        public GetCalculationResult CalculateFromNetAmmount(string netAmmount, string vatRate)
        {
            var vatTaxAmmount = (decimal.Parse(netAmmount) * decimal.Parse(vatRate)) / 100;
            var grossAmmount = decimal.Parse(netAmmount) + vatTaxAmmount;
            var netAmmountResult = decimal.Parse(netAmmount);

            return new GetCalculationResult((double)netAmmountResult, (double)grossAmmount, (double)vatTaxAmmount);
        }

        public GetCalculationResult CalculateFromGrossAmmount(string grossAmmount, string vatRate)
        {
            var vatTaxAmmount = (decimal.Parse(grossAmmount) * decimal.Parse(vatRate)) / (100 + decimal.Parse(vatRate));
            var netAmmount = decimal.Parse(grossAmmount) - vatTaxAmmount;
            var grossAmmountResult = decimal.Parse(grossAmmount);

            return new GetCalculationResult((double)netAmmount, (double)grossAmmountResult, (double)vatTaxAmmount);
        }

        public GetCalculationResult CalculateFromVatTaxAmmount(string vatTaxAmmount, string vatRate)
        {
            var netAmmount = (decimal.Parse(vatTaxAmmount) * 100) / decimal.Parse(vatRate);
            var grossAmmount = decimal.Parse(vatTaxAmmount) + netAmmount;
            var vatTaxAmmountResult = decimal.Parse(vatTaxAmmount);

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
