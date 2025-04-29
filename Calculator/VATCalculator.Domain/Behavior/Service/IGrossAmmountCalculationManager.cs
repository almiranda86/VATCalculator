using VATCalculator.Domain.Queries;

namespace VATCalculator.Domain.Behavior.Service
{
    public interface IGrossAmmountCalculationManager
    {
        public (decimal, decimal, decimal) CalculateFromGrossAmmount(string grossAmmount, string vatRate);
    }
}
