using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Community.DeliveryApiExtensions.Configuration;
using Umbraco.Community.DeliveryApiExtensions.Configuration.Options;
using Umbraco.Community.DeliveryApiExtensions.ContentApps;

namespace Umbraco.Community.DeliveryApiExtensions;

// ReSharper disable once UnusedMember.Global
internal class Composer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        _ = builder.ManifestFilters().Append<ManifestFilter>();

        IConfigurationSection configSection = builder.Config.GetSection<DeliveryApiExtensionsOptions>();
        _ = builder.Services.AddOptions<DeliveryApiExtensionsOptions>(builder.Config);
        _ = builder.Services.AddOptions<PreviewOptions>(configSection);

        DeliveryApiExtensionsOptions? options = configSection.Get<DeliveryApiExtensionsOptions>();
        if (options?.Preview.Enabled == true)
        {
            _ = builder.ContentApps().Append<DeliveryApiPreviewApp>();
        }
    }
}
