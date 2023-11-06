using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
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
    private readonly IOptionsMonitor<Configuration.Options.DeliveryApiExtensionsOptions> _options;

    public DeliveryApiPreviewApp(
        LinkGenerator linkGenerator,
        IOptionsMonitor<Configuration.Options.DeliveryApiExtensionsOptions> options)
    {
        _linkGenerator = linkGenerator;
        _options = options;
    }

    public ContentApp? GetContentAppFor(object source, IEnumerable<IReadOnlyUserGroup> userGroups)
    {
        // Only show the content app if preview is enabled and the user belongs to one of the allowed user groups
        if (!PreviewIsEnabled() || !UserBelongsToAllowedGroup(userGroups))
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
            IMedia media when _options.CurrentValue.Preview.Media.Enabled => CreateBaseContentApp(new
            {
                apiPath = $"{GetApiPath(nameof(PreviewController.GetMedia))}/{media.Key}",
                entityType = Cms.Core.Constants.UdiEntityType.Media
            }),
            _ => null
        };
    }

    private ContentApp CreateBaseContentApp(object? viewModel)
    {
        return new ContentApp
        {
            Alias = Constants.PreviewAppAlias,
            Name = Constants.PreviewAppName,
            Icon = Constants.PreviewAppIcon,
            View = Constants.PreviewAppView,
            Weight = _options.CurrentValue.Preview.ContentAppWeight,
            ViewModel = viewModel,
        };
    }

    private string? GetApiPath(string action)
    {
        return _linkGenerator.GetUmbracoApiService<PreviewController>(action)?.TrimEnd('/');
    }

    private bool UserBelongsToAllowedGroup(IEnumerable<IReadOnlyUserGroup> userGroups)
    {
        List<string> allowedUserGroupAliases = _options.CurrentValue.Preview.AllowedUserGroupAliases;

        return allowedUserGroupAliases is { Count: 0 } || userGroups.Select(g => g.Alias).ContainsAny(allowedUserGroupAliases);
    }

    private bool PreviewIsEnabled()
    {
        return _options.CurrentValue.Preview.Enabled;
    }
}
