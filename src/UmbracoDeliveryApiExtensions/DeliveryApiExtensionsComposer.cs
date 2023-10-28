using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace Umbraco.Community.DeliveryApiExtensions;

internal class DeliveryApiExtensionsComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.ManifestFilters().Append<DeliveryApiExtensionsManifestFilter>();
    }
}