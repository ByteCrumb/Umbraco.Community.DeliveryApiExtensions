using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Community.DeliveryApiExtensions.Configuration;

namespace Umbraco.Community.DeliveryApiExtensions.Composers;

/// <summary>
/// Default <see cref="IComposer"/> for configuring Delivery API Extensions.
/// </summary>
public sealed class DeliveryApiExtensionsComposer : IComposer
{
    /// <inheritdoc/>
    public void Compose(IUmbracoBuilder builder)
    {
        builder.AddDeliveryApiExtensions();
    }
}
