using VATCalculator.Domain.Queries;

namespace VATCalculator.Domain.Behavior.Service
{
    public interface IGrossAmmountCalculationManager
    {
        public GetCalculationResult CalculateFromGrossAmmount(string grossAmmount, string vatRate);
    }
}
