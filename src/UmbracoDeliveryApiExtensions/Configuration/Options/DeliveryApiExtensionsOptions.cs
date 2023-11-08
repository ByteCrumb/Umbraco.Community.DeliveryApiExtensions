namespace Umbraco.Community.DeliveryApiExtensions.Configuration.Options;

/// <summary>
/// Delivery API Extensions options.
/// </summary>
public class DeliveryApiExtensionsOptions
{
    /// <summary>
    /// Preview options.
    /// </summary>
    public PreviewOptions Preview { get; set; } = new();

    /// <summary>
    /// Typed swagger options.
    /// </summary>
    public TypedSwaggerOptions TypedSwagger { get; set; } = new();
}
