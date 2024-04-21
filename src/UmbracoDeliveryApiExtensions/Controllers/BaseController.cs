using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Api.Common.Attributes;
using Umbraco.Cms.Api.Common.Filters;
using Umbraco.Cms.Web.Common.Authorization;
using Umbraco.Cms.Web.Common.Routing;

namespace Umbraco.Community.DeliveryApiExtensions.Controllers;

/// <summary>
/// Base Delivery API Extensions controller class.
/// </summary>
[ApiController]
[MapToApi(Constants.Api.ApiName)]
[Authorize(Policy = AuthorizationPolicies.BackOfficeAccess)]
[JsonOptionsName(Cms.Core.Constants.JsonOptionsNames.BackOffice)]
[BackOfficeRoute("delivery-api-extensions/[controller]")]
public abstract class BaseController : ControllerBase
{
}
