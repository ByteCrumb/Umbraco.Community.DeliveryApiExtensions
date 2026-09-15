using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Community.DeliveryApiExtensions.Configuration.Options;
using Umbraco.Extensions;

namespace Umbraco.Community.DeliveryApiExtensions.Configuration;

/// <summary>
/// Extensions for <see cref="IUmbracoBuilder"/> to add DeliveryApiExtensions.
/// </summary>
public static class UmbracoBuilderExtensions
{
    /// <summary>
    /// Registers the necessary services and configuration for DeliveryApiExtensions.
    /// </summary>
    public static void AddDeliveryApiExtensions(this IUmbracoBuilder builder)
    {
        IConfigurationSection configSection = builder.Config.GetSection<DeliveryApiExtensionsOptions>();
        _ = builder.Services.AddOptions<DeliveryApiExtensionsOptions>(configSection);

        // Preview
        builder.AddPreview(configSection);
    }

    internal static void AddPreview(this IUmbracoBuilder builder, IConfigurationSection configSection)
    {
        IConfigurationSection previewConfigSection = configSection.GetSection<PreviewOptions>();
        _ = builder.Services.AddOptions<PreviewOptions>(previewConfigSection);

        IConfigurationSection mediaConfigSection = previewConfigSection.GetSection<MediaOptions>();
        _ = builder.Services.AddOptions<MediaOptions>(mediaConfigSection);
    }
}
