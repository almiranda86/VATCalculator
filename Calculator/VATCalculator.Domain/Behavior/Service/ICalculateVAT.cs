using VATCalculator.Domain.Strategy;

namespace VATCalculator.Domain.Behavior.Service
{
    public interface ICalculateVAT
    {
        public EVatCalculationMethods eVatCalculationMethod { get; set; }
        public (decimal, decimal, decimal) Calculate(string baseAmmount, string vatRate);
    }
}
