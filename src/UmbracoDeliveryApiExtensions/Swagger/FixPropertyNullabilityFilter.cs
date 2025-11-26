using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Umbraco.Community.DeliveryApiExtensions.Swagger;

internal sealed class FixPropertyNullabilityFilter : ISchemaFilter
{
    public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Properties is not { Count: > 0 })
        {
            return;
        }

        Dictionary<string, MemberInfo> typeMembers = context.Type
            .GetMembers(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance)
            .Where(t => t is FieldInfo or PropertyInfo)
            .ToDictionary(GetPropertyName, t => t, StringComparer.OrdinalIgnoreCase);

        foreach (KeyValuePair<string, IOpenApiSchema> property in schema.Properties)
        {
            if (property.Value is not OpenApiSchemaReference openApiSchemaReference
                || typeMembers.GetValueOrDefault(property.Key) is not { } memberInfo)
            {
                continue;
            }

            Type fieldType = memberInfo switch
            {
                FieldInfo fieldInfo => fieldInfo.FieldType,
                PropertyInfo propertyInfo => propertyInfo.PropertyType,
                _ => throw new NotSupportedException(),
            };

            bool nullable = fieldType.IsValueType ? Nullable.GetUnderlyingType(fieldType) != null : !memberInfo.IsNonNullableReferenceType();
            if (nullable)
            {
                schema.Required?.Remove(property.Key);
            }
        }
    }

    private static string GetPropertyName(MemberInfo memberInfo)
    {
        if (memberInfo.GetCustomAttribute<JsonPropertyNameAttribute>() is { } nameAttribute)
        {
            return nameAttribute.Name;
        }

        return memberInfo.Name;
    }
}
