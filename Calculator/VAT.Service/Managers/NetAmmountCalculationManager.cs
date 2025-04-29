using VATCalculator.Domain.Behavior.Service;

namespace VATCalculator.Service.Managers
{
    public class NetAmmountCalculationManager : INetAmmountCalculationManager
    {
        public (decimal, decimal, decimal) CalculateFromNetAmmount(string netAmmount, string vatRate)
        {
            var vatTaxAmmount = (decimal.Parse(netAmmount) * decimal.Parse(vatRate)) / 100;
            var grossAmmount = decimal.Parse(netAmmount) + vatTaxAmmount;
            var netAmmountResult = decimal.Parse(netAmmount);

            return new(netAmmountResult, grossAmmount, vatTaxAmmount);
        }
    }
}
