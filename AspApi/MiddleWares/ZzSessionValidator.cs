using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Writers;
using Zz.App.Core;
using Zz.DataBase.Identity;

namespace Zz;

public class SessionValidatorMiddleware
{
    private readonly RequestDelegate _next;

    public SessionValidatorMiddleware(RequestDelegate next)
    {
        this._next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Check if the endpoint is null (no endpoint matched the request path)
        var endpoint = context.GetEndpoint();
        if (endpoint == null)
        {
            await _next(context);
            return;
        }

        var athEP = endpoint.Metadata.Any(x => x is AuthorizeAttribute);

        // Check if the endpoint allows anonymous access
        var isAnonymousAllowed =
            endpoint.Metadata.Any(metadata => metadata is IAllowAnonymous) || !athEP;

        // If anonymous access is allowed, proceed to the next middleware
        if (isAnonymousAllowed)
        {
            await _next(context);
            return;
        }

        IUserAccessor _userAccessor = context.RequestServices.GetRequiredService<IUserAccessor>();
        UserSessionsDataContext _sessionsDataContext =
            context.RequestServices.GetRequiredService<UserSessionsDataContext>();

        var sid = _userAccessor.SessionId;

        if (string.IsNullOrWhiteSpace(sid))
        {
            await _next(context);
            return;
        }

        if (!Id22.TryParse(sid, out Id22 sessionId))
        {
            await InvalidateRequest(context);
            return;
        }
        var encryptedSid = context.Request.Headers.FirstOrDefault(x => x.Key == "SessionKey").Value;

        if (string.IsNullOrWhiteSpace(encryptedSid))
        {
            await _sessionsDataContext.Sessions.EndSessionAsync(sessionId);
            await InvalidateRequest(context);
            return;
        }

        if (
            !await _sessionsDataContext
                .Sessions.AsQueryable()
                .ValidateSessionAsync(sessionId, encryptedSid)
        )
        {
            await _sessionsDataContext.Sessions.EndSessionAsync(sessionId);
            await InvalidateRequest(context);
            return;
        }

        await _next(context);
    }

    private static async Task InvalidateRequest(HttpContext context)
    {
        string key = "Authentication";
        context.Request.Headers.Remove(key);
        context.Response.Headers.Remove(key);

        key = "access_token";
        context.Response.Cookies.Delete(key);

        var athEP =
            context.User.Identity?.IsAuthenticated is true
            && context.GetEndpoint()?.Metadata?.GetMetadata<AuthorizeAttribute>() is object;

        if (athEP)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            context.Response.ContentType = MediaTypeNames.Application.Json;
            await context.Response.WriteAsJsonAsync(Result);
        }
    }

    private record struct InvalidResult(string Code = "SESSION_ENDED");

    private static InvalidResult Result = new("SESSION_ENDED");
}

public static partial class MiddleWaresInjection
{
    public static void UseZzSessionValidatorMiddleware(
        this Microsoft.AspNetCore.Builder.WebApplication app
    )
    {
        app.UseMiddleware<SessionValidatorMiddleware>();
    }
}
