namespace VATCalculator.Domain.Core.ResultPattern
{
    public sealed class Result<T> where T : notnull
    {
        public T? ResponseModel { get; } = default;
        public List<Error> Errors { get; protected set; } = [];
        public bool IsSuccessful => Errors.Count == 0;
        public bool IsFailed => !IsSuccessful;

        private Result(T responseModel)
        {
            ResponseModel = responseModel;
        }

        protected Result(List<Error> errors)
        {
            Errors = errors;
        }

        protected Result(Error error)
        {
            Errors.Add(error);
        }

        public static Result<T> Ok(T responseModel)
        {
            return new Result<T>(responseModel);
        }

        public static Result<T> Fail(List<Error> errors)
        {
            var validErrors = errors.Where(e => e is not null).ToList();
            if (validErrors.Count == 0)
            {
                validErrors.Add(new Error("Fail result is created without any error."));
            }

            return new Result<T>(validErrors);
        }

        public static Result<T> Fail(Error error)
        {
            if (error is not null)
            {
                var errors = new List<Error>
                {
                    error
                };
                return new Result<T>(errors);
            }

            return new Result<T>(new List<Error> { new Error("Fail result is created without any error.") });
        }

        public static implicit operator Result<T>(Error error)
        {
            return Fail(error);
        }

        public static implicit operator Result<T>(List<Error> errors)
        {
            return Fail(errors);
        }

        public static implicit operator Result<T>(T reponseModel)
        {
            return new Result<T>(reponseModel);
        }
    }
}
