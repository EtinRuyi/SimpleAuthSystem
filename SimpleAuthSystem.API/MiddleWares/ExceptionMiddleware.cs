namespace SimpleAuthSystem.API.MiddleWares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred during request processing");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            (HttpStatusCode statusCode, Result<object> result) = MapExceptionToResult(ex);

            _logger.LogError(ex, "Error processing request: {Message}", ex.Message);

            context.Response.StatusCode = (int)statusCode;
            string jsonResponse = JsonConvert.SerializeObject(result, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            await context.Response.WriteAsync(jsonResponse);
        }

        private (HttpStatusCode, Result<object>) MapExceptionToResult(Exception ex)
        {
            return ex switch
            {
                BaseException baseEx => MapBaseException(baseEx),
                ArgumentException argEx => (
                    HttpStatusCode.BadRequest,
                    Result<object>.Failure(new Error(400, argEx.Message), "Invalid argument provided")
                ),
                InvalidOperationException invOpEx => (
                    HttpStatusCode.Conflict,
                    Result<object>.Failure(new Error(409, invOpEx.Message), "Operation conflict")
                ),
                _ => (
                    HttpStatusCode.InternalServerError,
                    Result<object>.Failure(new Error(500, "An unexpected error occurred"), "Internal server error")
                )
            };
        }

        private (HttpStatusCode, Result<object>) MapBaseException(BaseException ex)
        {
            return ex switch
            {
                BadRequestException badReq => (
                    badReq.StatusCode,
                    Result<object>.Failure(new Error(400, badReq.Message), "Bad request")
                ),
                UnauthorizedException unauth => (
                    unauth.StatusCode,
                    Result<object>.Failure(new Error(401, unauth.Message), "Unauthorized access")
                ),
                ForbiddenException forbid => (
                    forbid.StatusCode,
                    Result<object>.Failure(new Error(403, forbid.Message), "Access forbidden")
                ),
                NotFoundException notFound => (
                    notFound.StatusCode,
                    Result<object>.Failure(new Error(404, notFound.Message), "Resource not found")
                ),
                _ => (
                    HttpStatusCode.InternalServerError,
                    Result<object>.Failure(new Error(500, ex.Message), "Internal server error")
                )
            };
        }
    }
}
