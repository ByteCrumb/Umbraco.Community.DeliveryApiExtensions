namespace Umbraco.Community.DeliveryApiExtensions.Controllers.Models;

/// <summary>
/// API Preview config.
/// </summary>
public class PreviewConfig
{
    /// <summary>
    /// Whether the preview content app is enabled.
    /// </summary>
    public required bool Enabled { get; set; }

    /// <summary>
    /// Preview options for media.
    /// </summary>
    public PreviewMediaConfig? Media { get; set; }

    /// <summary>
    /// The weight of the preview content app.
    /// </summary>
    public int? ContentAppWeight { get; set; }
}

/// <summary>
/// API Preview Media config.
/// </summary>
public class PreviewMediaConfig
{
    /// <summary>
    /// Whether the preview content app is enabled for media.
    /// </summary>
    public required bool Enabled { get; set; }
}
