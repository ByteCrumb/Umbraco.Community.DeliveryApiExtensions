using NJsonSchema.Generation;

namespace UmbracoDeliveryApiExtensions.JsonSchemaGenerator;

internal sealed class PrefixedSchemaNameGenerator : DefaultSchemaNameGenerator
{
    private readonly string _prefix;

    public PrefixedSchemaNameGenerator(string prefix)
    {
        _prefix = prefix;
    }

    public override string Generate(Type type)
    {
        string schemaName = base.Generate(type);

        return schemaName.StartsWith(_prefix, StringComparison.OrdinalIgnoreCase) ? schemaName : _prefix + schemaName;
    }
}
