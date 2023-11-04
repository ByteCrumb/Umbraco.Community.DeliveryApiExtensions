using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
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
    private readonly LinkGenerator _linkGenerator;
    private readonly IOptions<Configuration.Options.DeliveryApiExtensionsOptions> _options;
    private readonly IOptionsMonitor<DeliveryApiSettings> _deliveryApiSettings;

    public DeliveryApiPreviewApp(
        LinkGenerator linkGenerator,
        IOptions<Configuration.Options.DeliveryApiExtensionsOptions> options,
        IOptionsMonitor<DeliveryApiSettings> deliveryApiSettings)
    {
        _linkGenerator = linkGenerator;
        _options = options;
        _deliveryApiSettings = deliveryApiSettings;
    }

    public ContentApp? GetContentAppFor(object source, IEnumerable<IReadOnlyUserGroup> userGroups)
    {
        // Only show the content app if the Delivery API is enabled AND the user belongs to one of the allowed user groups
        if (!DeliveryApiIsEnabled() || !UserBelongsToAllowedGroup(userGroups))
        {
            return null;
        }

        return source switch
        {
            IEntity { Id: 0 } => null, // Do not show the content app when creating new content/media
            IContent content => CreateBaseContentApp(new
            {
                apiPath = $"{GetApiPath(nameof(PreviewController.GetContent))}/{content.Key}",
                entityType = Cms.Core.Constants.UdiEntityType.Document
            }),
            IMedia media when _deliveryApiSettings.CurrentValue.Media.Enabled => CreateBaseContentApp(new
            {
                apiPath = $"{GetApiPath(nameof(PreviewController.GetMedia))}/{media.Key}",
                entityType = Cms.Core.Constants.UdiEntityType.Media
            }),
            _ => null
        };
    }

    private ContentApp CreateBaseContentApp(object? viewModel) => new()
    {
        Alias = Constants.PreviewAppAlias,
        Name = Constants.PreviewAppName,
        Icon = Constants.PreviewAppIcon,
        View = Constants.PreviewAppView,
        Weight = _options.Value.Preview.ContentAppWeight,
        ViewModel = viewModel
    };

    private string? GetApiPath(string action) => _linkGenerator.GetUmbracoApiService<PreviewController>(action)?.TrimEnd('/');

    private bool DeliveryApiIsEnabled() => _deliveryApiSettings.CurrentValue.Enabled;

    private bool UserBelongsToAllowedGroup(IEnumerable<IReadOnlyUserGroup> userGroups) =>
        _options.Value.Preview.AllowedUserGroupAliases.Any(g => userGroups.Any(ug => ug.Alias == g));
}
