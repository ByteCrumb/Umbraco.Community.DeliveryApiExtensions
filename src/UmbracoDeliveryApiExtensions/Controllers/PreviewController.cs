using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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

/// <summary>
///     <see cref="Controller"/> for previewing delivery api content and media.
/// </summary>
[PluginController(Constants.ApiAreaName)]
public class PreviewController : UmbracoAuthorizedJsonController
{
    private readonly ILogger<PreviewController> _logger;
    private readonly IBackOfficeSecurityAccessor _backOfficeSecurityAccessor;
    private readonly IEntityService _entityService;
    private readonly IRequestCultureService _requestCultureService;
    private readonly AppCaches _appCaches;
    private readonly JsonOptions _deliveryApiJsonOptions;

    /// <summary>
    ///     Initializes a new instance of the <see cref="PreviewController" /> class.
    /// </summary>
    public PreviewController(
        ILogger<PreviewController> logger,
        IBackOfficeSecurityAccessor backOfficeSecurityAccessor,
        IEntityService entityService,
        IRequestCultureService requestCultureService,
        AppCaches appCaches,
        IOptionsSnapshot<JsonOptions> jsonOptions)
    {
        _logger = logger;
        _backOfficeSecurityAccessor = backOfficeSecurityAccessor;
        _entityService = entityService;
        _requestCultureService = requestCultureService;
        _appCaches = appCaches;

        _deliveryApiJsonOptions = jsonOptions.Get(Cms.Core.Constants.JsonOptionsNames.DeliveryApi);
    }

    /// <summary>
    ///     Retrieves the delivery api response for the content with the specified id.
    /// </summary>
    [HttpGet]
    public IActionResult GetContent(
        [FromRoute] Guid id,
        [FromHeader(Name = "Accept-Language")] string? language,
        [FromServices] IApiPublishedContentCache contentCache,
        [FromServices] IApiContentResponseBuilder responseBuilder)
    {
        try
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

            return new JsonResult(apiContentResponse, _deliveryApiJsonOptions.JsonSerializerOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while trying to preview content with id {Id}", id);
            return Problem(title: "An error occurred.");
        }
    }

    /// <summary>
    ///     Retrieves the delivery api response for the media with the specified id.
    /// </summary>
    [HttpGet]
    public IActionResult GetMedia(
        [FromRoute] Guid id,
        [FromHeader(Name = "Accept-Language")] string? language,
        [FromServices] IPublishedSnapshotAccessor publishedSnapshotAccessor,
        [FromServices] IApiMediaWithCropsResponseBuilder responseBuilder)
    {
        try
        {
            SetCulture(language);

            IPublishedMediaCache mediaCache = publishedSnapshotAccessor.GetRequiredPublishedSnapshot().Media
                                              ?? throw new InvalidOperationException("Could not obtain the published media cache");

            IPublishedContent? media = mediaCache.GetById(id);
            if (media is null || !UserHasAccessToMediaNode(media))
            {
                return Problem(statusCode: StatusCodes.Status404NotFound);
            }

            return new JsonResult(responseBuilder.Build(media), _deliveryApiJsonOptions.JsonSerializerOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while trying to preview media with id {Id}", id);
            return Problem(title: "An error occurred.");
        }
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
            Cms.Core.Constants.System.RecycleBinContent);
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
            Cms.Core.Constants.System.RecycleBinMedia);
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
