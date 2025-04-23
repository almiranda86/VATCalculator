using VATCalculator.Domain.Queries;

namespace VATCalculator.Domain.Behavior.Service
{
    public interface INetAmmountCalculationManager
    {
        public GetCalculationResult CalculateFromNetAmmount(string netAmmount, string vatRate);
    }
}
