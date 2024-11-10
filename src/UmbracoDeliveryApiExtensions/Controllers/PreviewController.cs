using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Actions;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Cms.Core.Models.DeliveryApi;
using Umbraco.Cms.Core.Models.Membership;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Services.AuthorizationStatus;
using Umbraco.Cms.Infrastructure.DeliveryApi;
using Umbraco.Community.DeliveryApiExtensions.Configuration.Options;
using Umbraco.Community.DeliveryApiExtensions.Controllers.Models;
using Umbraco.Extensions;

namespace Umbraco.Community.DeliveryApiExtensions.Controllers;

/// <summary>
///     <see cref="Controller"/> for previewing delivery api content and media.
/// </summary>
[ApiVersion("2.0")]
public sealed class PreviewController : BaseController
{
    private readonly ILogger<PreviewController> _logger;
    private readonly IBackOfficeSecurityAccessor _backOfficeSecurityAccessor;
    private readonly IEntityService _entityService;
    private readonly IRequestCultureService _requestCultureService;
    private readonly IContentPermissionService _contentPermissionService;
    private readonly IMediaPermissionService _mediaPermissionService;
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
        IContentPermissionService contentPermissionService,
        IMediaPermissionService mediaPermissionService,
        AppCaches appCaches,
        IOptionsSnapshot<JsonOptions> jsonOptions)
    {
        _logger = logger;
        _backOfficeSecurityAccessor = backOfficeSecurityAccessor;
        _entityService = entityService;
        _requestCultureService = requestCultureService;
        _contentPermissionService = contentPermissionService;
        _mediaPermissionService = mediaPermissionService;
        _appCaches = appCaches;

        _deliveryApiJsonOptions = jsonOptions.Get(Cms.Core.Constants.JsonOptionsNames.DeliveryApi);
    }

    /// <summary>
    /// Gets the API preview config.
    /// </summary>
    [HttpGet("config")]
    public PreviewConfig GetConfig(
        [FromServices] IOptionsMonitor<PreviewOptions> optionsMonitor)
    {
        PreviewConfig response = new() { Enabled = false };
        IUser? user = _backOfficeSecurityAccessor.BackOfficeSecurity?.CurrentUser;
        if (user is null)
        {
            return response;
        }

        PreviewOptions options = optionsMonitor.CurrentValue;

        // If preview is not set as enabled, no need to check anything else.
        if (!options.Enabled)
        {
            return response;
        }

        // If there are no allowed user groups set, allow admins by default.
        if (options.AllowedUserGroupAliases.Count == 0)
        {
            if (!user.IsAdmin())
            {
                return response;
            }
        }
        // Check if the user belongs to one of the allowed user groups.
        else if (!user.Groups.Any(group => options.AllowedUserGroupAliases.Contains(group.Alias)))
        {
            return response;
        }

        response.Enabled = options.Enabled;
        response.Media = new PreviewMediaConfig { Enabled = options.Media.Enabled, };

        return response;
    }

    /// <summary>
    /// Retrieves the delivery api response for the content with the specified id.
    /// </summary>
    [HttpGet("content/{id}")]
    public async Task<IActionResult> GetContent(
        [FromRoute] Guid id,
        [FromHeader(Name = "Accept-Language")] string? language,
        [FromServices] IApiPublishedContentCache contentCache,
        [FromServices] IApiContentResponseBuilder responseBuilder)
    {
        try
        {
            SetCulture(language);

            IPublishedContent? content = await contentCache.GetByIdAsync(id);
            if (content is null || !await UserHasAccessToContentNode(id))
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
    /// Retrieves the delivery api response for the media with the specified id.
    /// </summary>
    [HttpGet("media/{id}")]
    public async Task<IActionResult> GetMedia(
        [FromRoute] Guid id,
        [FromHeader(Name = "Accept-Language")] string? language,
        [FromServices] IPublishedMediaCache publishedMediaCache,
        [FromServices] IApiMediaWithCropsResponseBuilder responseBuilder)
    {
        try
        {
            SetCulture(language);

            IPublishedContent? media = publishedMediaCache.GetById(id);
            if (media is null || !await UserHasAccessToMediaNode(id))
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

    private async Task<bool> UserHasAccessToContentNode(Guid key)
    {
        IUser? user = _backOfficeSecurityAccessor.BackOfficeSecurity?.CurrentUser;
        if (user is null)
        {
            return false;
        }

        ContentAuthorizationStatus status = await _contentPermissionService.AuthorizeAccessAsync(user, key, ActionBrowse.ActionLetter);
        return status == ContentAuthorizationStatus.Success;
    }

    private async Task<bool> UserHasAccessToMediaNode(Guid key)
    {
        IUser? user = _backOfficeSecurityAccessor.BackOfficeSecurity?.CurrentUser;
        if (user is null)
        {
            return false;
        }

        MediaAuthorizationStatus status = await _mediaPermissionService.AuthorizeAccessAsync(user, key);
        return status == MediaAuthorizationStatus.Success;
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
