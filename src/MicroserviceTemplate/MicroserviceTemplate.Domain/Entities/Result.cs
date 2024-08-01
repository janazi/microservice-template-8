namespace MicroserviceTemplate.Domain.Entities
{
    public sealed class Result<T> where T : class
    {
        public T? Value { get; }
        public bool IsSuccess => Error is null;
        public Exception? Error { get; }
        public string? Message { get; }

        private Result(Exception error)
        {
            Error = error;
            Message = error.Message;
        }

        private Result(T value) => Value = value;

        public static Result<T> Success(T value) => new(value);
        public static Result<T> Failure(Exception error) => new(error);
        public static implicit operator Result<T>(T value) => Success(value);
        public static implicit operator Result<T>(Exception error) => Failure(error);
    }
}
