namespace Umbraco.Community.DeliveryApiExtensions.Models;

/// <summary>
/// Represents the subset of content type information that is needed for typed swagger schema generation.
/// </summary>
public class ContentTypeInfo
{
    /// <summary>
    /// Content type alias.
    /// </summary>
    public required string Alias { get; set; }

    /// <summary>
    /// Content type schema id.
    /// </summary>
    public required string SchemaId { get; set; }

    /// <summary>
    /// List of schema ids of the content type's compositions.
    /// </summary>
    public required List<string> CompositionSchemaIds { get; set; }

    /// <summary>
    /// List of content type's properties.
    /// </summary>
    public required List<ContentTypePropertyInfo> Properties { get; set; }

    /// <summary>
    /// Whether the content type is an element type.
    /// </summary>
    public bool IsElement { get; set; }

    /// <summary>
    /// Whether the content type is used as a composition.
    /// </summary>
    public bool IsComposition { get; set; }
}
