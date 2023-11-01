using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.DeliveryApi;
using Umbraco.Cms.Core.Models.Membership;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.DeliveryApi;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Extensions;

namespace Umbraco.Community.DeliveryApiExtensions.Controllers;

[PluginController(DeliveryApiExtensionsConstants.ApiAreaName)]
public class PreviewController : UmbracoAuthorizedJsonController
{
    private readonly IBackOfficeSecurityAccessor _backOfficeSecurityAccessor;
    private readonly IEntityService _entityService;
    private readonly IRequestCultureService _requestCultureService;
    private readonly AppCaches _appCaches;

    public PreviewController(
        IBackOfficeSecurityAccessor backOfficeSecurityAccessor,
        IEntityService entityService,
        IRequestCultureService requestCultureService,
        AppCaches appCaches)
    {
        _backOfficeSecurityAccessor = backOfficeSecurityAccessor;
        _entityService = entityService;
        _requestCultureService = requestCultureService;
        _appCaches = appCaches;
    }

    [HttpGet]
    public IActionResult GetContent(
        [FromRoute] Guid id,
        [FromHeader(Name = "Accept-Language")] string? language,
        [FromServices] IApiPublishedContentCache contentCache,
        [FromServices] IApiContentResponseBuilder responseBuilder)
    {
        SetCulture(language);

        IPublishedContent? content = contentCache.GetById(id);
        if (content is null || !UserHasAccessToContentNode(content))
        {
            return Problem(statusCode: StatusCodes.Status404NotFound);
        }

        IApiContentResponse? apiContentResponse = responseBuilder.Build(content);
        if (apiContentResponse is null)
        {
            return Problem(statusCode: StatusCodes.Status404NotFound);
        }

        return Ok(apiContentResponse);
    }

    [HttpGet]
    public IActionResult GetMedia(
        [FromRoute] Guid id,
        [FromServices] IPublishedSnapshotAccessor publishedSnapshotAccessor,
        [FromServices] IApiMediaWithCropsResponseBuilder responseBuilder)
    {
        IPublishedMediaCache mediaCache = publishedSnapshotAccessor.GetRequiredPublishedSnapshot().Media
                                          ?? throw new InvalidOperationException("Could not obtain the published media cache");

        IPublishedContent? media = mediaCache.GetById(id);
        if (media is null || !UserHasAccessToMediaNode(media))
        {
            return Problem(statusCode: StatusCodes.Status404NotFound);
        }

        return Ok(responseBuilder.Build(media));
    }

    private bool UserHasAccessToContentNode(IPublishedContent content)
    {
        IUser? user = _backOfficeSecurityAccessor.BackOfficeSecurity?.CurrentUser;
        if (user is null)
        {
            return false;
        }

        return ContentPermissions.HasPathAccess(
            content.Path,
            user.CalculateContentStartNodeIds(_entityService, _appCaches),
            Constants.System.RecycleBinContent);
    }

    private bool UserHasAccessToMediaNode(IPublishedContent media)
    {
        IUser? user = _backOfficeSecurityAccessor.BackOfficeSecurity?.CurrentUser;
        if (user is null)
        {
            return false;
        }

        return ContentPermissions.HasPathAccess(
            media.Path,
            user.CalculateMediaStartNodeIds(_entityService, _appCaches),
            Constants.System.RecycleBinContent);
    }

    private void SetCulture(string? culture)
    {
        if (culture.IsNullOrWhiteSpace())
        {
            return;
        }

        _requestCultureService.SetRequestCulture(culture);
    }
}
