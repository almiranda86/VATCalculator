namespace VATCalculator.Domain.Behavior.Service
{
    public interface INetAmmountCalculationManager
    {
        public (decimal, decimal, decimal) CalculateFromNetAmmount(string netAmmount, string vatRate);
    }
}
