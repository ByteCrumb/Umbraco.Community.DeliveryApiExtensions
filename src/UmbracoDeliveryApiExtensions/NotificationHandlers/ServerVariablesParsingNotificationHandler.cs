using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Umbraco.Cms.Api.Delivery.Controllers;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Community.DeliveryApiExtensions.ContentApps;
using Umbraco.Extensions;

namespace Umbraco.Community.DeliveryApiExtensions.NotificationHandlers;

public class ServerVariablesParsingNotificationHandler : INotificationHandler<ServerVariablesParsingNotification>
{
    private readonly LinkGenerator _linkGenerator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ServerVariablesParsingNotificationHandler(LinkGenerator linkGenerator,
        IHttpContextAccessor httpContextAccessor)
    {
        _linkGenerator = linkGenerator;
        _httpContextAccessor = httpContextAccessor;
    }

    public void Handle(ServerVariablesParsingNotification notification)
    {
        if (_httpContextAccessor.HttpContext == null)
        {
            return;
        }

        var contentApiEndpoint = _linkGenerator.GetPathByAction(
            _httpContextAccessor.HttpContext,
            nameof(ByIdContentApiController.ById),
            ControllerExtensions.GetControllerName<ByIdContentApiController>(),
            new { id = Guid.Empty, version = "1.0" }
            );

        var mediaApiEndpoint = _linkGenerator.GetPathByAction(
            _httpContextAccessor.HttpContext,
            nameof(ByIdMediaApiController.ById),
            ControllerExtensions.GetControllerName<ByIdMediaApiController>(),
            new { id = Guid.Empty, version = "1.0" }
        );

        notification.ServerVariables.Add(DeliveryApiPreviewApp.DeliveryApiPreviewAppAlias, new
        {
            contentApiEndpoint,
            mediaApiEndpoint
        });
    }
}
