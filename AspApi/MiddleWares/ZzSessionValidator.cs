using System.Net;
using System.Net.Mime;
using Microsoft.OpenApi.Writers;
using Zz.App.Core;
using Zz.DataBase.Identity;

namespace Zz;

public class SessionValidatorMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScope _scope;
    private readonly IUserAccessor _userAccessor;
    private readonly UserSessionsDataContext _sessionsDataContext;

    public SessionValidatorMiddleware(
        RequestDelegate next,
        IServiceScopeFactory serviceScopeFactory
    )
    {
        this._next = next;
        this._scope = serviceScopeFactory.CreateAsyncScope();
        this._sessionsDataContext =
            this._scope.ServiceProvider.GetRequiredService<UserSessionsDataContext>();
        this._userAccessor = this._scope.ServiceProvider.GetRequiredService<IUserAccessor>();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        using (this._scope)
        {
            using (this._sessionsDataContext)
            {
                var sid = _userAccessor.SessionId;

                if (string.IsNullOrWhiteSpace(sid))
                {
                    await _next(context);
                    return;
                }

                if (!Id22.TryParse(sid, out Id22 sessionId))
                {
                    await InvalidateRequest(context);

                    context.Request.Headers.Remove("Authentication");
                    context.Response.Headers.Remove("Authentication");

                    return;
                }
                var encryptedSid = context.Request.Headers["SessionKey"][0];

                if (string.IsNullOrWhiteSpace(encryptedSid))
                {
                    await _sessionsDataContext.Sessions.EndSessionAsync(sessionId);
                    await InvalidateRequest(context);
                    context.Request.Headers.Remove("Authentication");
                    context.Response.Headers.Remove("Authentication");
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

                    context.Request.Headers.Remove("Authentication");
                    context.Response.Headers.Remove("Authentication");
                    return;
                }
            }
        }
        await _next(context);
    }

    private static async Task InvalidateRequest(HttpContext context)
    {
        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
        context.Response.ContentType = MediaTypeNames.Application.Json;
        await context.Response.WriteAsJsonAsync(Result);
    }

    private record struct InvalidResult(string Code = "SESSION_ENDED");

    private static InvalidResult Result = new();
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
