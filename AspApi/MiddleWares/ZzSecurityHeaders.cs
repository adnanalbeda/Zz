using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.HttpOverrides;

namespace Zz;

public static partial class MiddleWaresInjection
{
    public static void UseZzProxySecurityHeaders(
        this Microsoft.AspNetCore.Builder.WebApplication app
    )
    {
        //============ Check and Apply Security Headers

        // Apply when using NGINX as reverse proxy.
        app.UseForwardedHeaders(
            new ForwardedHeadersOptions
            {
                ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
            }
        );

        // All Headers to be managed by Nginx

        app.UseXContentTypeOptions();
        app.UseReferrerPolicy(opt => opt.NoReferrer());
        app.UseXXssProtection(opt => opt.EnabledWithBlockMode());
        app.UseXfo(opt => opt.Deny());

        // app.UseCors("DefaultCorsPolicy");

        // app.UseHttpsRedirection(); // Don't use with nginx, according to MS.

        // Add Strict Transport Security
        // ATTENTION !! DO NOT !! use in development !! NEVER.
        // #if DEBUG
        //         if (app.Environment.IsProduction())
        // #endif
        //             app.Use(
        //                 async (context, next) =>
        //                 {
        //                     context.Response.Headers.Append(
        //                         "Strict-Transport-Security",
        //                         "max-age=31536000"
        //                     );
        //                     await next.Invoke();
        //                 }
        //             );
    }
}
