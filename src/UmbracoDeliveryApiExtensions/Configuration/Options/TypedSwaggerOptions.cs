namespace Umbraco.Community.DeliveryApiExtensions.Configuration.Options;

/// <summary>
/// Typed swagger options
/// </summary>
public class TypedSwaggerOptions
{
    /// <summary>
    /// Whether the typed swagger feature is enabled
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// The swagger generation mode to use.
    /// Defaults to 'Auto'.
    /// </summary>
    public SwaggerGenerationMode Mode { get; set; } = SwaggerGenerationMode.Auto;
}
