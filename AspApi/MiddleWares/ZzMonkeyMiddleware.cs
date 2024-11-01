#if DEBUG
namespace Zz;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

// You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
public class MonkeyMiddleware
{
    private readonly RequestDelegate _next;

    public MonkeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext, ILogger<MonkeyMiddleware> logger)
    {
        if (httpContext.Request.Method.Equals("POST"))
        {
            // TODO, find a way to resolve this issue on server side. It's ready on client side.
            await _next(httpContext);
            return;
        }
        var val = Random.Shared.Next(0, 22);
        if (val > 18)
        {
            logger.LogWarning("Oops, a monkey found this request!");
            if (val % 2 == 0)
            {
                logger.LogError("The Monkey ate the request first.");
                httpContext.Abort();
                return;
            }
            await _next(httpContext);
            logger.LogError("The Monkey ate the request last.");
            httpContext.Abort();
        }
        await _next(httpContext);
    }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static partial class MiddleWaresInjection
{
    public static IApplicationBuilder UseZzMonkeyMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<MonkeyMiddleware>();
    }
}
#endif
