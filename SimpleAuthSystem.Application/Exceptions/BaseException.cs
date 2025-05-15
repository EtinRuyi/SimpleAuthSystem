namespace SimpleAuthSystem.Application.Exceptions
{
    public abstract class BaseException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        protected BaseException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }

    public class BadRequestException : BaseException
    {
        public BadRequestException(string message)
            : base(HttpStatusCode.BadRequest, message) { }
    }

    public class UnauthorizedException : BaseException
    {
        public UnauthorizedException(string message)
            : base(HttpStatusCode.Unauthorized, message) { }
    }

    public class ForbiddenException : BaseException
    {
        public ForbiddenException(string message)
            : base(HttpStatusCode.Forbidden, message) { }
    }

    public class NotFoundException : BaseException
    {
        public NotFoundException(string message)
            : base(HttpStatusCode.NotFound, message) { }
    }

}
