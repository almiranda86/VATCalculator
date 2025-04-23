using FluentValidation;
using System.Text.RegularExpressions;
using VATCalculator.Domain.Core;
using VATCalculator.Domain.Queries;

namespace VATCalculator.Service.Validations
{
    public sealed partial class GetCalculationRequestValidation : AbstractValidator<GetCalculationRequest>
    {
        public GetCalculationRequestValidation()
        {
            RuleFor(x => x).Custom(NeedsToBeValidRequest);
            RuleFor(x => x).Custom(NeedsToHaveASingleValue);
            RuleFor(x => x).Custom(NeedsToBeNumeric);
            RuleFor(x => x).Custom(NeedsToBeGreaterThanZero);
            RuleFor(x => x).Custom(NeedsToBeAValidTaxRate);
        }

        private void NeedsToBeValidRequest(GetCalculationRequest request, ValidationContext<GetCalculationRequest> context)
        {
            if (request.NetAmmount == string.Empty && 
                request.GrossAmount == string.Empty && 
                request.VatTaxAmmount == string.Empty &&
                request.VatRate == string.Empty)
            {
                context.AddFailure("The request is invalid. All values are empty. - NetAmmount | GrossAmount | VatTaxAmmount | VatRate");
            }
        }

        private void NeedsToHaveASingleValue(GetCalculationRequest request, ValidationContext<GetCalculationRequest> context)
        {
            if (request.NetAmmount != string.Empty && request.GrossAmount != string.Empty && request.VatTaxAmmount != string.Empty)
            {
                context.AddFailure("Only 1 of 3 of the values must be provided. - Net Ammount | Gross Amount | Vat Tax Ammount");
            }
        }

        private void NeedsToBeNumeric(GetCalculationRequest request, ValidationContext<GetCalculationRequest> context)
        {
            var regex = RegexForValidNumbers();

            if (!string.IsNullOrEmpty(request.NetAmmount) && !regex.IsMatch(request.NetAmmount))
                context.AddFailure("NetAmmount needs to be a number");

            if (!string.IsNullOrEmpty(request.GrossAmount) && !regex.IsMatch(request.GrossAmount))
                context.AddFailure("GrossAmount needs to be a number");

            if (!string.IsNullOrEmpty(request.VatTaxAmmount) && !regex.IsMatch(request.VatTaxAmmount))
                context.AddFailure("VatTaxAmmount needs to be a number");

            if (!string.IsNullOrEmpty(request.VatRate) && !regex.IsMatch(request.VatRate))
                context.AddFailure("VatRate needs to be a number");
        }

        private void NeedsToBeGreaterThanZero(GetCalculationRequest request, ValidationContext<GetCalculationRequest> context)
        {
            if (int.TryParse(request.NetAmmount, out var netAmmount))
            {
                if (netAmmount <= 0)
                    context.AddFailure("NetAmmount needs to be greater than Zero");
            }

            if (int.TryParse(request.GrossAmount, out var grossAmmount))
            {
                if (grossAmmount <= 0)
                    context.AddFailure("GrossAmount needs to be greater than Zero");
            }

            if (int.TryParse(request.VatTaxAmmount, out var vatTaxAmmount))
            {
                if (vatTaxAmmount <= 0)
                    context.AddFailure("VatTaxAmmount needs to be greater than Zero");
            }

            if (int.TryParse(request.VatRate, out var vatTaxRate))
            {
                if (vatTaxRate <= 0)
                    context.AddFailure("VatRate needs to be greater than Zero");
            }
        }

        private void NeedsToBeAValidTaxRate(GetCalculationRequest request, ValidationContext<GetCalculationRequest> context)
        {
            if (int.TryParse(request.VatRate, out var vatTaxRate))
            {
                if (vatTaxRate != Constants.VAT_RATE_10 && vatTaxRate != Constants.VAT_RATE_13 && vatTaxRate != Constants.VAT_RATE_20)
                {
                    context.AddFailure("VatRate must be 10, 13 or 20");
                }
            }
        }


        /// <summary>
        /// A Regex for Valid Numbers.
        /// A number could start with a period, EX: .32
        /// A number could also end with a period, EX: 32.
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex("^[+-]?\\d*\\.\\d+$|^[+-]?\\d+(\\.\\d*)?$")]
        private static partial Regex RegexForValidNumbers();
    }
}
