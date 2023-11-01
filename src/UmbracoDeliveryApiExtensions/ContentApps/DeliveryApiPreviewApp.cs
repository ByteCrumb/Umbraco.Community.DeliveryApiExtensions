using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
    private readonly IOptionsMonitor<DeliveryApiSettings> _deliveryApiSettings;
    private static string? _contentApiPath;
    private static string? _mediaApiPath;

    public DeliveryApiPreviewApp(
        LinkGenerator linkGenerator,
        IOptionsMonitor<DeliveryApiSettings> deliveryApiSettings)
    {
        _deliveryApiSettings = deliveryApiSettings;
        _contentApiPath ??= linkGenerator.GetUmbracoApiService<PreviewController>(nameof(PreviewController.GetContent)) ?? "";
        _mediaApiPath ??= linkGenerator.GetUmbracoApiService<PreviewController>(nameof(PreviewController.GetMedia)) ?? "";
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
            IContent content when !_contentApiPath.IsNullOrEmpty() => new ContentApp
            {
                Alias = DeliveryApiPreviewAppAlias,
                Name = "API",
                Icon = "icon-code",
                View = "/App_Plugins/DeliveryApiExtensions/preview.html",
                Weight = -100,
                ViewModel = new { apiPath = $"{_contentApiPath!.TrimEnd('/')}/{content.Key}" }
            },
            IMedia media when !_mediaApiPath.IsNullOrEmpty() && _deliveryApiSettings.CurrentValue.Media.Enabled => new ContentApp
            {
                Alias = DeliveryApiPreviewAppAlias,
                Name = "API",
                Icon = "icon-code",
                View = "/App_Plugins/DeliveryApiExtensions/preview.html",
                Weight = -100,
                ViewModel = new { apiPath = $"{_mediaApiPath!.TrimEnd('/')}/{media.Key}" }
            },
            _ => null
        };
    }
}
