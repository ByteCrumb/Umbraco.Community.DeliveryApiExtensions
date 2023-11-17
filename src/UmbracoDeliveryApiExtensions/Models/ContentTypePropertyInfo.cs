namespace Umbraco.Community.DeliveryApiExtensions.Models;

/// <summary>
/// Represents the subset of content type property information that is needed for typed swagger schema generation.
/// </summary>
public class ContentTypePropertyInfo
{
    /// <summary>
    /// Property alias.
    /// </summary>
    public required string Alias { get; set; }

    /// <summary>
    /// Property delivery api type.
    /// </summary>
    public required Type Type { get; set; }

    /// <summary>
    /// Whether this property is inherited from a composition.
    /// </summary>
    public bool Inherited { get; set; }
}
