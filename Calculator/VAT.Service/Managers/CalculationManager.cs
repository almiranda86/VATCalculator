using FluentValidation;
using VATCalculator.Domain.Behavior.Service;
using VATCalculator.Domain.Core.ResultPattern;
using VATCalculator.Domain.Queries;

namespace VATCalculator.Service.Managers
{
    public class CalculationManager : ICalculationManager
    {
        private readonly IValidator<GetCalculationRequest> _validator;
        private readonly INetAmmountCalculationManager _netAmmountCalculationManager;
        private readonly IGrossAmmountCalculationManager _grossAmmountCalculationManager;
        private readonly IVatTaxAmmountCalculationManager _vatTaxAmmountCalculationManager;
        public CalculationManager(IValidator<GetCalculationRequest> validator,
            INetAmmountCalculationManager netAmmountCalculationManager,
            IGrossAmmountCalculationManager grossAmmountCalculationManager,
            IVatTaxAmmountCalculationManager vatTaxAmmountCalculationManager)
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _netAmmountCalculationManager = netAmmountCalculationManager ?? throw new ArgumentNullException(nameof(netAmmountCalculationManager));
            _grossAmmountCalculationManager = grossAmmountCalculationManager ?? throw new ArgumentNullException(nameof(grossAmmountCalculationManager));
            _vatTaxAmmountCalculationManager = vatTaxAmmountCalculationManager ?? throw new ArgumentNullException(nameof(vatTaxAmmountCalculationManager));
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
            var (netAmmountResult, grossAmmount, vatTaxAmmount) = _netAmmountCalculationManager.CalculateFromNetAmmount(netAmmount, vatRate);
            return new GetCalculationResult((double)netAmmountResult, (double)grossAmmount, (double)vatTaxAmmount);
        }

        private GetCalculationResult CalculateFromGrossAmmount(string grossAmmount, string vatRate)
        {
            var (netAmmount, grossAmmountResult, vatTaxAmmount) = _grossAmmountCalculationManager.CalculateFromGrossAmmount(grossAmmount, vatRate);
            return new GetCalculationResult((double)netAmmount, (double)grossAmmountResult, (double)vatTaxAmmount);
        }

        public GetCalculationResult CalculateFromVatTaxAmmount(string vatTaxAmmount, string vatRate)
        {
            var (netAmmount, grossAmmount, vatTaxAmmountResult) = _vatTaxAmmountCalculationManager.CalculateFromVatTaxAmmount(vatTaxAmmount, vatRate);
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
