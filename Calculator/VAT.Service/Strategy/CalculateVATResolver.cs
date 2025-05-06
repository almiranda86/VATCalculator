using VATCalculator.Domain.Behavior.Service;
using VATCalculator.Domain.Strategy;

namespace VATCalculator.Service.Strategy
{
    public class CalculateVATResolver : ICalculateVATResolver
    {
        private readonly IEnumerable<ICalculateVAT> _vatCalculatorMethods;
        public CalculateVATResolver(IEnumerable<ICalculateVAT> vatCalculateMethods)
        {
            _vatCalculatorMethods = vatCalculateMethods ?? throw new ArgumentNullException(nameof(vatCalculateMethods));
        }

        public ICalculateVAT Resolver(EVatCalculationMethods method)
        {
            var response = _vatCalculatorMethods.FirstOrDefault(x => x.eVatCalculationMethod.Equals(method));

            if(response != null)
            {
                return response;
            }

            throw new ArgumentNullException("VAT Calculation Method not implemented");
        }
    }
}
