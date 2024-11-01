using Asp.Versioning;

namespace System;

public static partial class ZzDependencyInjectionExtensions
{
    public static IHostApplicationBuilder ConfigureZzVersioning(
        this IHostApplicationBuilder builder
    )
    {
        Console.WriteLine("Configure Versioning...");
        var bld = builder
            .Services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            })
            .AddMvc();
#if DEBUG
        if (builder.Environment.IsDevelopment())
            bld.AddApiExplorer(options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v' major [.minor] [-status]"
                options.GroupNameFormat = "'v'VVV";
                // note: this option is only necessary when versioning by URL segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;
                // if we have both parts, decided how to format the group
                // from the example: "Sales - v1"
                options.FormatGroupName = (group, version) => $"{group} - {version}";
            });
#endif
        return builder;
    }
}
