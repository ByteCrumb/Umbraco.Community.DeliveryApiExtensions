using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Community.DeliveryApiExtensions.ContentApps;
using Umbraco.Community.DeliveryApiExtensions.NotificationHandlers;

namespace Umbraco.Community.DeliveryApiExtensions;

internal class DeliveryApiExtensionsComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        _ = builder.ManifestFilters().Append<DeliveryApiExtensionsManifestFilter>();
        _ = builder.ContentApps().Append<DeliveryApiPreviewApp>();
        _ = builder.AddNotificationHandler<ServerVariablesParsingNotification, ServerVariablesParsingNotificationHandler>();
    }
}
