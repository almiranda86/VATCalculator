using VATCalculator.Domain.Behavior.Service;

namespace VATCalculator.Service.Managers
{
    public class VatTaxAmmountCalculationManager : IVatTaxAmmountCalculationManager
    {
        public (decimal, decimal, decimal) CalculateFromVatTaxAmmount(string vatTaxAmmount, string vatRate)
        {
            var netAmmount = (decimal.Parse(vatTaxAmmount) * 100) / decimal.Parse(vatRate);
            var grossAmmount = decimal.Parse(vatTaxAmmount) + netAmmount;
            var vatTaxAmmountResult = decimal.Parse(vatTaxAmmount);

            return new (netAmmount, grossAmmount, vatTaxAmmountResult);
        }
    }
}
