using Microsoft.Extensions.Configuration;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Community.DeliveryApiExtensions.Configuration;
using Umbraco.Community.DeliveryApiExtensions.Configuration.Options;

namespace Umbraco.Community.DeliveryApiExtensions;

public sealed class Composer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        _ = builder.ManifestFilters().Append<ManifestFilter>();

        IConfigurationSection configSection = builder.Config.GetSection<DeliveryApiExtensionsOptions>();
        _ = builder.Services.AddOptions<DeliveryApiExtensionsOptions>(configSection);

        // Preview
        builder.ConfigurePreview(configSection);

        // TypedSwagger
        builder.ConfigureSwagger(configSection);
    }
}