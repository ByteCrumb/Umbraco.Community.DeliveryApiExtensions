using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Umbraco.Community.DeliveryApiExtensions.Swagger;

internal sealed class FixPropertyNullabilityFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Properties is not { Count: > 0 })
        {
            return;
        }

        Dictionary<string, MemberInfo> typeMembers = context.Type
            .GetMembers(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance)
            .Where(t => t is FieldInfo or PropertyInfo)
            .ToDictionary(GetPropertyName, t => t, StringComparer.OrdinalIgnoreCase);

        foreach (KeyValuePair<string, OpenApiSchema> property in schema.Properties)
        {
            if (property.Value.Reference == null || property.Value.Nullable || typeMembers.GetValueOrDefault(property.Key) is not { } memberInfo)
            {
                continue;
            }

            Type fieldType = memberInfo switch
            {
                FieldInfo fieldInfo => fieldInfo.FieldType,
                PropertyInfo propertyInfo => propertyInfo.PropertyType,
                _ => throw new NotSupportedException(),
            };

            property.Value.Nullable = fieldType.IsValueType ? Nullable.GetUnderlyingType(fieldType) != null : !memberInfo.IsNonNullableReferenceType();
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
