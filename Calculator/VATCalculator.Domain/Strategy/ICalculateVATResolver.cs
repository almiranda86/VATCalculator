using VATCalculator.Domain.Behavior.Service;

namespace VATCalculator.Domain.Strategy
{
    public interface ICalculateVATResolver
    {
        ICalculateVAT Resolver(EVatCalculationMethods method);
    }
}
