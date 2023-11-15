namespace Umbraco.Community.DeliveryApiExtensions.Models;

public class ContentTypeInfo
{
    public required string Alias { get; set; }

    public required string SchemaId { get; set; }

    public required List<string> CompositionSchemaIds { get; set; }

    public required List<ContentTypePropertyInfo> Properties { get; set; }

    public bool IsElement { get; set; }

    public bool IsComposition { get; set; }
}
