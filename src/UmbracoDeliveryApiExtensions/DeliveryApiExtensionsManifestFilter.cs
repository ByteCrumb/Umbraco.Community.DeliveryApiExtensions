using Umbraco.Cms.Core.Manifest;

namespace Umbraco.Community.DeliveryApiExtensions;

internal class DeliveryApiExtensionsManifestFilter : IManifestFilter
{
    public void Filter(List<PackageManifest> manifests)
    {
        var assembly = typeof(DeliveryApiExtensionsManifestFilter).Assembly;

        manifests.Add(new PackageManifest
        {
            PackageName = "DeliveryApiExtensions",
            Version = assembly.GetName()?.Version?.ToString(3) ?? "0.1.0",
            AllowPackageTelemetry = true,
            Scripts = new string[] {
                // List any Script files
                // Urls should start '/App_Plugins/DeliveryApiExtensions/' not '/wwwroot/DeliveryApiExtensions/', e.g.
                // "/App_Plugins/DeliveryApiExtensions/Scripts/scripts.js"
            },
            Stylesheets = new string[]
            {
                // List any Stylesheet files
                // Urls should start '/App_Plugins/DeliveryApiExtensions/' not '/wwwroot/DeliveryApiExtensions/', e.g.
                // "/App_Plugins/DeliveryApiExtensions/Styles/styles.css"
            }
        });
    }
}