using VATCalculator.Domain.Core.ResultPattern;
using VATCalculator.Domain.Queries;

namespace VATCalculator.Domain.Behavior.Service
{
    public interface ICalculationManager
    {
        Task<Result<GetCalculationResult>> HandleCalculation(GetCalculationRequest request, CancellationToken cancellationToken);
    }
}
