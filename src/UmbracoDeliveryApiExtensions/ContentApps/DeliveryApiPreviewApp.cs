using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Models.Entities;
using Umbraco.Cms.Core.Models.Membership;
using Umbraco.Community.DeliveryApiExtensions.Controllers;
using Umbraco.Extensions;

namespace Umbraco.Community.DeliveryApiExtensions.ContentApps;

public class DeliveryApiPreviewApp : IContentAppFactory
{
    public const string DeliveryApiPreviewAppAlias = "deliveryApiPreview";
    private readonly LinkGenerator _linkGenerator;
    private readonly IOptionsMonitor<DeliveryApiSettings> _deliveryApiSettings;

    public DeliveryApiPreviewApp(
        LinkGenerator linkGenerator,
        IOptionsMonitor<DeliveryApiSettings> deliveryApiSettings)
    {
        _linkGenerator = linkGenerator;
        _deliveryApiSettings = deliveryApiSettings;
    }

    public ContentApp? GetContentAppFor(object source, IEnumerable<IReadOnlyUserGroup> userGroups)
    {
        // Only show the content app if the Delivery API is enabled
        if (!_deliveryApiSettings.CurrentValue.Enabled)
        {
            return null;
        }

        return source switch
        {
            IEntity { Id: 0 } => null, // Do not show the content app when creating new content/media
            IContent content => new ContentApp
            {
                Alias = DeliveryApiPreviewAppAlias,
                Name = "API",
                Icon = "icon-code",
                View = "/App_Plugins/DeliveryApiExtensions/preview.html",
                Weight = -100,
                ViewModel = new
                {
                    apiPath = $"{GetApiPath(nameof(PreviewController.GetContent))}/{content.Key}",
                    entityType = Constants.UdiEntityType.Document
                }
            },
            IMedia media when _deliveryApiSettings.CurrentValue.Media.Enabled => new ContentApp
            {
                Alias = DeliveryApiPreviewAppAlias,
                Name = "API",
                Icon = "icon-code",
                View = "/App_Plugins/DeliveryApiExtensions/preview.html",
                Weight = -100,
                ViewModel = new
                {
                    apiPath = $"{GetApiPath(nameof(PreviewController.GetMedia))}/{media.Key}",
                    entityType = Constants.UdiEntityType.Media
                }
            },
            _ => null
        };
    }

    private string? GetApiPath(string action) => _linkGenerator.GetUmbracoApiService<PreviewController>(action)?.TrimEnd('/');
}
