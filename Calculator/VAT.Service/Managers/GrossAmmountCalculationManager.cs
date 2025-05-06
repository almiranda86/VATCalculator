using VATCalculator.Domain.Behavior.Service;
using VATCalculator.Domain.Strategy;

namespace VATCalculator.Service.Managers
{
    public class GrossAmmountCalculationManager : ICalculateVAT
    {
        public EVatCalculationMethods eVatCalculationMethod { get; set; } = EVatCalculationMethods.GrossAmmount;

        public (decimal, decimal, decimal) Calculate(string grossAmmount, string vatRate)
        {
            var vatTaxAmmount = (decimal.Parse(grossAmmount) * decimal.Parse(vatRate)) / (100 + decimal.Parse(vatRate));
            var netAmmount = decimal.Parse(grossAmmount) - vatTaxAmmount;
            var grossAmmountResult = decimal.Parse(grossAmmount);

            return new (netAmmount, grossAmmountResult, vatTaxAmmount);
        }
    }
}
