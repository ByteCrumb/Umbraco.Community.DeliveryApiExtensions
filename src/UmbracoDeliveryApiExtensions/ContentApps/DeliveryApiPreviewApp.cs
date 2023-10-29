using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Models.Membership;

namespace Umbraco.Community.DeliveryApiExtensions.ContentApps;

public class DeliveryApiPreviewApp : IContentAppFactory
{
    public const string DeliveryApiPreviewAppAlias = "deliveryApiPreview";
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
            IContent => new ContentApp
            {
                Alias = DeliveryApiPreviewAppAlias,
                Name = "API Preview",
                Icon = "icon-code",
                View = "/App_Plugins/DeliveryApiExtensions/preview.html",
                Weight = -100
            },
            IMedia when _deliveryApiSettings.CurrentValue.Media.Enabled => new ContentApp
            {
                Alias = DeliveryApiPreviewAppAlias,
                Name = "API Preview",
                Icon = "icon-code",
                View = "/App_Plugins/DeliveryApiExtensions/preview.html",
                Weight = -100
            },
            _ => null
        };
    }
}
