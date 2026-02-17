namespace DistributedLibrary.Main.Common
{
    internal enum ResultStatus
    {
        Success,
        Conflict,
        NotFound,
        Exception
    }
    /// <summary>
    /// This class is used as a return type by our handlers to isolate business layer from presentation
    /// </summary>
    internal sealed class Result
    {
        public ResultStatus Status { get; }
        public string? ErrorMessage { get; }
        public bool IsSuccess => Status == ResultStatus.Success;

        private Result(ResultStatus status, string? errorMessage)
        {
            Status = status;
            ErrorMessage = errorMessage;
        }

        public static Result Success() => new(ResultStatus.Success, null);
        public static Result Conflict(string error) => new(ResultStatus.Conflict, error);
        public static Result NotFound(string error) => new(ResultStatus.NotFound, error);
        public static Result Exception(string error) => new(ResultStatus.Exception, error);
    }
    /// <summary>
    /// Generic version of the class to return values
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class Result<T>
    {
        public T? Value { get; }
        public ResultStatus Status { get; }
        public string? ErrorMessage { get; }
        public bool IsSuccess => Status == ResultStatus.Success;

        private Result(T? value, ResultStatus status,  string? errorMessage)
        {
            Value = value;
            Status = status;
            ErrorMessage = errorMessage;
        }

        public static Result<T> Success(T value) => new(value, ResultStatus.Success, null);
        public static Result<T> Conflict(string error) => new(default, ResultStatus.Conflict, error);  
        public static Result<T> NotFound(string error) => new(default, ResultStatus.NotFound, error);
        public static Result<T> Exception(string error) => new(default, ResultStatus.Exception, error);
    }
}
