namespace VATCalculator.Domain.Core.ResultPattern
{
    public record Error : ResultBase
    {
        public Error(string Message) : base(Message)
        {
        }
    }
}
