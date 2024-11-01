using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Zz;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger,
        IHostEnvironment env
    )
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
#if DEBUG
            _logger.LogDebug(await context.Request.BodyToStringAsync());
#endif

            await _next(context);
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(
                ex,
                """
                =!=!=!=!=!=!=!=!=!=!=
                CANCELLED:{TID}
                =!=!=!=!=!=!=!=!=!=!=
                """,
                context.TraceIdentifier
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                """
                =!=!=!=!=!=!=!=!=!=!=
                ERROR:{TID}
                {EX_MSG}
                {EX_INNER}
                {EX_INNER2}
                =!=!=!=!=!=!=!=!=!=!=
                """,
                context.TraceIdentifier,
                ex.Message,
                ex.InnerException?.Message,
                ex.InnerException?.InnerException?.Message
            );

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            string errorMessage;
            string? errorDetails;
#if DEBUG
            if (_env.IsDevelopment())
            {
                errorMessage =
                    $"request id: - {context.TraceIdentifier} !! - exception message: {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += " -- Inner Message: " + ex.InnerException.Message;
                }
                errorDetails = ex.StackTrace;
            }
            else
            {
#endif
                errorMessage = "Server Error";
                errorDetails =
                    "If the issue is not resolved within few minutes, "
                    + "please contact us and send us the issue number: "
                    + "'"
                    + context.TraceIdentifier
                    + "'.";
#if DEBUG
            }
#endif

            var response = (
                context.Response.StatusCode,
                ErrorMessage: errorMessage,
                ErrorDetails: errorDetails
            );

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
    }
}

public static partial class MiddleWaresInjection
{
    public static void UseZzExceptionMiddleware(
        this Microsoft.AspNetCore.Builder.WebApplication app
    )
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }
}
