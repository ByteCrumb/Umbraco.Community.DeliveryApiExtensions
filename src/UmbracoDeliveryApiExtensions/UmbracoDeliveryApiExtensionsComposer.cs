using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace Umbraco.Community.UmbracoDeliveryApiExtensions
{
    internal class UmbracoDeliveryApiExtensionsComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.ManifestFilters().Append<UmbracoDeliveryApiExtensionsManifestFilter>();
        }
    }
}
