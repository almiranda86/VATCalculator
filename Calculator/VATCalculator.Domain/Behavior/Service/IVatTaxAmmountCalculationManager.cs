using VATCalculator.Domain.Queries;

namespace VATCalculator.Domain.Behavior.Service
{
    public interface IVatTaxAmmountCalculationManager
    {
        public GetCalculationResult CalculateFromVatTaxAmmount(string vatTaxAmmount, string vatRate);
    }
}
