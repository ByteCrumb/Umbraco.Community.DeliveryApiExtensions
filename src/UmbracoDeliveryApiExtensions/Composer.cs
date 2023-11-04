using Microsoft.Extensions.Configuration;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Community.DeliveryApiExtensions.Configuration;
using Umbraco.Community.DeliveryApiExtensions.Configuration.Options;
using Umbraco.Community.DeliveryApiExtensions.ContentApps;

namespace Umbraco.Community.DeliveryApiExtensions;

public sealed class Composer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        IConfigurationSection configSection = builder.Config.GetSection<DeliveryApiExtensionsOptions>();
        _ = builder.Services.AddOptions<DeliveryApiExtensionsOptions>(configSection);

        IConfigurationSection previewConfigSection = configSection.GetSection<PreviewOptions>();
        _ = builder.Services.AddOptions<PreviewOptions>(previewConfigSection);

        IConfigurationSection mediaConfigSection = previewConfigSection.GetSection<MediaOptions>();
        _ = builder.Services.AddOptions<MediaOptions>(mediaConfigSection);

        _ = builder.ManifestFilters().Append<ManifestFilter>();
        _ = builder.ContentApps().Append<DeliveryApiPreviewApp>();
    }
}
