using VATCalculator.Domain.Queries;

namespace VATCalculator.Domain.Behavior.Service
{
    public interface IVatTaxAmmountCalculationManager
    {
        public (decimal, decimal, decimal) CalculateFromVatTaxAmmount(string vatTaxAmmount, string vatRate);
    }
}
