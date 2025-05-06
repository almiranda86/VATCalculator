using VATCalculator.Domain.Behavior.Service;
using VATCalculator.Domain.Strategy;

namespace VATCalculator.Service.Managers
{
    public class NetAmmountCalculationManager : ICalculateVAT
    {
        public EVatCalculationMethods eVatCalculationMethod { get; set; } = EVatCalculationMethods.NetAmmount;
        public (decimal, decimal, decimal) Calculate(string netAmmount, string vatRate)
        {
            var vatTaxAmmount = (decimal.Parse(netAmmount) * decimal.Parse(vatRate)) / 100;
            var grossAmmount = decimal.Parse(netAmmount) + vatTaxAmmount;
            var netAmmountResult = decimal.Parse(netAmmount);

            return new(netAmmountResult, grossAmmount, vatTaxAmmount);
        }
    }
}
