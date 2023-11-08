namespace Umbraco.Community.DeliveryApiExtensions.Configuration.Options;

/// <summary>
/// Preview options.
/// </summary>
public class PreviewOptions
{
    /// <summary>
    /// Whether the preview content app is enabled.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Preview options for media.
    /// </summary>
    public MediaOptions Media { get; set; } = new();

    /// <summary>
    /// The aliases of the allowed user groups.
    /// Defaults to empty, which allows all user groups.
    /// </summary>
    public List<string> AllowedUserGroupAliases { get; set; } = new();

    /// <summary>
    /// The weight of the preview content app.
    /// </summary>
    public int ContentAppWeight { get; set; }
}
