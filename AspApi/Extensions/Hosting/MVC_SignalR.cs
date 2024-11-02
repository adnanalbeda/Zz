using System.Text.Json.Serialization;

namespace System;

public static partial class ZzDependencyInjectionExtensions
{
    public static IHostApplicationBuilder ConfigureZzMvcSignalR(
        this IHostApplicationBuilder builder
    )
    {
        // Just cause it says view doesn't necessarily mean an html.
        // It's just name of a pattern.
        // The view in this case is api json result.
        Console.WriteLine("Configure Mvc and SignalR routings...");

        builder
            .Services.AddControllers()
            .AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                x.JsonSerializerOptions.MaxDepth = 7;
                x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
                x.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
                x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        builder.Services.AddSignalR();

        return builder;
    }
}
