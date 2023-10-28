using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Models.Membership;

namespace Umbraco.Community.DeliveryApiExtensions;

internal class DeliveryApiExtensionsComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.ManifestFilters().Append<DeliveryApiExtensionsManifestFilter>();
        builder.ContentApps().Append<DeliveryApiPreviewApp>();
    }
}

public class DeliveryApiPreviewApp : IContentAppFactory
{
    private readonly IOptionsMonitor<DeliveryApiSettings> _deliveryApiSettings;

    public DeliveryApiPreviewApp(IOptionsMonitor<DeliveryApiSettings> deliveryApiSettings) => _deliveryApiSettings = deliveryApiSettings;

    public ContentApp? GetContentAppFor(object source, IEnumerable<IReadOnlyUserGroup> userGroups)
    {
        // Only show the content app if the Delivery API is enabled
        if (!_deliveryApiSettings.CurrentValue.Enabled)
        {
            return null;
        }

        // TODO: Filter on DisallowedContentTypeAliases

        return source switch
        {
            IContent or IMedia => new ContentApp
            {
                Alias = "deliveryApiPreview",
                Name = "API Preview",
                Icon = "icon-code",
                View = "/App_Plugins/DeliveryApiExtensions/preview.html",
                Weight = 0
            },
            _ => null
        };
    }
}