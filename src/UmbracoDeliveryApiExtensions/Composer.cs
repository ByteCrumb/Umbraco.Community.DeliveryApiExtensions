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
        _ = builder.ManifestFilters().Append<ManifestFilter>();

        IConfigurationSection configSection = builder.Config.GetSection<DeliveryApiExtensionsOptions>();
        _ = builder.Services.AddOptions<DeliveryApiExtensionsOptions>(builder.Config);

        IConfigurationSection previewConfig = configSection.GetSection<PreviewOptions>();
        _ = builder.Services.AddOptions<PreviewOptions>(configSection);
        _ = builder.Services.AddOptions<MediaOptions>(previewConfig);

        _ = builder.ContentApps().Append<DeliveryApiPreviewApp>();
    }
}
