using Newtonsoft.Json;
using System.Net.Mime;
using Visonex.Idmg.ExternalWebApi.Exceptions;

namespace Visonex.Idmg.ExternalWebApi.Middlewares
{
    public class ExceptionMiddleware
    {
        #region Properties
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        #endregion Properties

        #region Public Methods
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
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            string response;
            context.Response.ContentType = MediaTypeNames.Application.Json;

            if (exception is HttpResponseException httpResponseException)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = httpResponseException.Status;

                response = JsonConvert.SerializeObject(new
                {
                    Text = httpResponseException.Msg
                });

                _logger.LogInformation("HttpResponseException {Status} {Response}", httpResponseException.Status, response);
            }
            else
            {
                response = JsonConvert.SerializeObject(new { error = "Internal Server Error" });
                context.Response.ContentType = MediaTypeNames.Application.Json;
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                _logger.LogError(exception, exception.Message);
            }

            await context.Response.WriteAsync(response);
        }

        #endregion Private Methods
    }
}
