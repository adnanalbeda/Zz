#if DEBUG
using Microsoft.AspNetCore.HttpLogging;

namespace System;

public static partial class ZzDependencyInjectionExtensions
{
    public static IHostApplicationBuilder ConfigureZzDebugLogging(
        this IHostApplicationBuilder builder
    )
    {
        if (!builder.Environment.IsDevelopment())
            return builder;
        // Just cause it says view doesn't necessarily mean an html.
        // It's just name of a pattern.
        // The view in this case is api json result.
        Console.WriteLine("Configure Debug Logging...");

        builder.Services.AddHttpLogging(o =>
        {
            o.LoggingFields =
                // Request
                HttpLoggingFields.RequestMethod
                | HttpLoggingFields.RequestScheme
                | HttpLoggingFields.RequestProtocol
                | HttpLoggingFields.RequestPath
                | HttpLoggingFields.RequestQuery
                | HttpLoggingFields.RequestBody
                | HttpLoggingFields.RequestHeaders
                | HttpLoggingFields.RequestProperties
                | HttpLoggingFields.RequestPropertiesAndHeaders
                // Response
                | HttpLoggingFields.ResponseStatusCode
                | HttpLoggingFields.ResponseHeaders
                | HttpLoggingFields.ResponseBody;
            o.CombineLogs = true;
        });
        return builder;
    }
}
#endif
