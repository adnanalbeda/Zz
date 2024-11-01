using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Zz.Api.Swagger;

public class SwaggerEnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            schema.Description =
                "Enum values:\n"
                + string.Join(
                    ", \r\n",
                    Enum.GetNames(context.Type)
                        .Select(enumName =>
                            $"{enumName} = {(int)Enum.Parse(context.Type, enumName)}"
                        )
                );
        }
    }
}
