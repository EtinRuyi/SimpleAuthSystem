namespace SimpleAuthSystem.Application.ApiResponse
{
    public class Result<T>
    {
        public T Data { get; private set; }
        public bool IsSuccess { get; private set; }
        public Error? Error { get; private set; }
        public string Message { get; private set; }
        public string ResponseCode { get; private set; }
        public DateTime Timestamp { get; private set; }

        private Result(T data, bool isSuccess, Error error, string message, string responseCode)
        {
            if (isSuccess && (error != null))
                throw new InvalidOperationException("Success result cannot have an error or message.");

            Data = data;
            IsSuccess = isSuccess;
            Error = error;
            Message = message ?? string.Empty;
            ResponseCode = responseCode ?? (isSuccess ? "00" : "99");
            Timestamp = DateTime.UtcNow;
        }

        public static Result<T> Success(T data, string message = null)
        {
            return new Result<T>(data, true, null, message, "00");
        }

        public static Result<T> Failure(Error error, string message = null, string responseCode = null)
        {
            return new Result<T>(default, false, error, message, responseCode ?? "99");
        }

        public static Result<T> Failure(string errorMessage, int errorCode = 99, string responseCode = null)
        {
            return new Result<T>(default, false, new Error(errorCode, errorMessage), null, responseCode ?? "99");
        }

        public static Result<T> Combine(params Result<T>[] results)
        {
            foreach (var result in results)
            {
                if (!result.IsSuccess)
                    return Failure(result.Error, result.Message, result.ResponseCode);
            }
            return Success(default, "All operations succeeded");
        }
    }

    public class Error
    {
        public int Code { get; }
        public string Message { get; }

        public Error(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
