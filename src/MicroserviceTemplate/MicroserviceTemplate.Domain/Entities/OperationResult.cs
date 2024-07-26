namespace MicroserviceTemplate.Domain.Entities;

public class OperationResult<T>
{
    public OperationResult(T result)
    {
        Result = result;
        IsSuccess = true;
    }

    public OperationResult(Exception error, bool isConcurrencyError = false)
    {
        Error = error;
        IsSuccess = false;
        IsConcurrencyError = isConcurrencyError;
    }
    public bool IsSuccess { get; private set; }
    public bool IsConcurrencyError { get; private set; }
    public T? Result { get; set; }
    public Exception? Error { get; private set; }
}