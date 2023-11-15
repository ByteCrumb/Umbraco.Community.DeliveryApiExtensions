namespace Umbraco.Community.DeliveryApiExtensions.Models;

public class ContentTypePropertyInfo
{
    public required string Alias { get; set; }

    public required Type Type { get; set; }

    public bool Inherited { get; set; }
}
