using VATCalculator.Domain.Behavior.Service;
using VATCalculator.Domain.Strategy;

namespace VATCalculator.Service.Managers
{
    public class VatTaxAmmountCalculationManager : ICalculateVAT
    {
        public EVatCalculationMethods eVatCalculationMethod { get; set; } = EVatCalculationMethods.VatTaxAmmount;

        public (decimal, decimal, decimal) Calculate(string vatTaxAmmount, string vatRate)
        {
            var netAmmount = (decimal.Parse(vatTaxAmmount) * 100) / decimal.Parse(vatRate);
            var grossAmmount = decimal.Parse(vatTaxAmmount) + netAmmount;
            var vatTaxAmmountResult = decimal.Parse(vatTaxAmmount);

            return new (netAmmount, grossAmmount, vatTaxAmmountResult);
        }
    }
}
