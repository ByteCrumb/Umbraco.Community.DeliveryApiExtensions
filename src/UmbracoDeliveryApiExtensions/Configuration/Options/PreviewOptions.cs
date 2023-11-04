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
    /// The aliases of the allowed user groups.
    /// </summary>
    public List<string> AllowedUserGroupAliases { get; set; } = new() { Cms.Core.Constants.Security.AdminGroupAlias };

    /// <summary>
    /// The weight of the preview content app.
    /// </summary>
    public int ContentAppWeight { get; set; } = 0;
}
