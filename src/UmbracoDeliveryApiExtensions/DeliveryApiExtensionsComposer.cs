using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Community.DeliveryApiExtensions.ContentApps;

namespace Umbraco.Community.DeliveryApiExtensions;

internal class DeliveryApiExtensionsComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        _ = builder.ManifestFilters().Append<DeliveryApiExtensionsManifestFilter>();
        _ = builder.ContentApps().Append<DeliveryApiPreviewApp>();
    }
}
