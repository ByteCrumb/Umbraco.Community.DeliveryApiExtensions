using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Umbraco.Community.DeliveryApiExtensions.Swagger;

/// <summary>
///     Schema filter to correct the type of enum schemas.
/// </summary>
/// <remarks>
///    This is needed because Umbraco generates invalid enum schemas
///    https://github.com/umbraco/Umbraco-CMS/pull/14727
/// </remarks>
// TODO: Remove this when fix is released by Umbraco
internal sealed class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (!context.Type.IsEnum)
        {
            return;
        }

        schema.Type = "string";
        schema.Format = null;
    }

}
