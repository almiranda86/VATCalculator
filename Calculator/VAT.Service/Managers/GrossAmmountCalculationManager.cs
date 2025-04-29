using VATCalculator.Domain.Behavior.Service;

namespace VATCalculator.Service.Managers
{
    public class GrossAmmountCalculationManager : IGrossAmmountCalculationManager
    {
        public (decimal, decimal, decimal) CalculateFromGrossAmmount(string grossAmmount, string vatRate)
        {
            var vatTaxAmmount = (decimal.Parse(grossAmmount) * decimal.Parse(vatRate)) / (100 + decimal.Parse(vatRate));
            var netAmmount = decimal.Parse(grossAmmount) - vatTaxAmmount;
            var grossAmmountResult = decimal.Parse(grossAmmount);

            return new (netAmmount, grossAmmountResult, vatTaxAmmount);
        }
    }
}
