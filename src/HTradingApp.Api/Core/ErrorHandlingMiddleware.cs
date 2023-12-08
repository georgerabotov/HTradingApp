using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace HTradingApp.Api.Core
{
    public class ErrorHandlingMiddleWare : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {

            string result;
            var code = HttpStatusCode.InternalServerError;

            var errorMessage = exception.Message + exception.InnerException ?? "\nInner exception: " + exception.InnerException.Message;

            if (exception is ValidationException)
            {
                errorMessage = errorMessage.Replace("Validation failed: \r\n -- ", "");

                int innerExceptionIndex = errorMessage.IndexOf("\nInner exception", StringComparison.InvariantCulture);
                if (innerExceptionIndex >= 0)
                    errorMessage = errorMessage.Remove(innerExceptionIndex);

                result = JsonSerializer.Serialize(new { errorMessage });
            }
            else
            {
                result = errorMessage;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}

