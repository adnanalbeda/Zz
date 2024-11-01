namespace System;

public static partial class ZzDependencyInjectionExtensions
{
    public static IHostApplicationBuilder ConfigureZzCors(this IHostApplicationBuilder builder)
    {
        Console.WriteLine("Configure CORs...");
        builder.Services.AddCors(opt =>
        {
            opt.AddPolicy(
                "DefaultCorsPolicy",
                policy =>
                {
                    policy
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithExposedHeaders("WWW-Authenticate", "Pagination");
#if DEBUG
                    if (builder.Environment.IsDevelopment())
                    {
                        policy.SetIsOriginAllowed(_ => true);
                    }
                    else
                    {
#endif
                        var origins = builder.Configuration.GetValue<string>(
                            "AppConfigs:AllowedCorsOrigins"
                        );
                        if (string.IsNullOrEmpty(origins))
                            return;
                        policy.WithOrigins(
                            origins.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                        );
#if DEBUG
                    }
#endif
                }
            );
        });
        return builder;
    }
}
