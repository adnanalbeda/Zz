#if DEBUG
using Asp.Versioning.ApiExplorer;

namespace Zz;

public static partial class MiddleWaresInjection
{
    public static Microsoft.AspNetCore.Builder.WebApplication UseZzSwagger(
        this Microsoft.AspNetCore.Builder.WebApplication app
    )
    {
        if (!app.Environment.IsDevelopment())
            return app;

        app.UseSwagger();
        app.UseSwaggerUI(o =>
        {
            var apiVersionDescriptionProvider =
                app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            foreach (
                var description in apiVersionDescriptionProvider.ApiVersionDescriptions.Select(x =>
                    x.GroupName
                )
            )
            {
                o.SwaggerEndpoint(
                    $"/swagger/{description}/swagger.json",
                    $"API - {description.ToUpper()}"
                );
            }
        });

        return app;
    }
}
#endif
