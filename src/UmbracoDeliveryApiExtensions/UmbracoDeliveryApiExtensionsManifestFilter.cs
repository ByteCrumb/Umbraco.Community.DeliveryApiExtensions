using Umbraco.Cms.Core.Manifest;

namespace Umbraco.Community.UmbracoDeliveryApiExtensions
{
    internal class UmbracoDeliveryApiExtensionsManifestFilter : IManifestFilter
    {
        public void Filter(List<PackageManifest> manifests)
        {
            var assembly = typeof(UmbracoDeliveryApiExtensionsManifestFilter).Assembly;

            manifests.Add(new PackageManifest
            {
                PackageName = "UmbracoDeliveryApiExtensions",
                Version = assembly.GetName()?.Version?.ToString(3) ?? "0.1.0",
                AllowPackageTelemetry = true,
                Scripts = new string[] {
                    // List any Script files
                    // Urls should start '/App_Plugins/UmbracoDeliveryApiExtensions/' not '/wwwroot/UmbracoDeliveryApiExtensions/', e.g.
                    // "/App_Plugins/UmbracoDeliveryApiExtensions/Scripts/scripts.js"
                },
                Stylesheets = new string[]
                {
                    // List any Stylesheet files
                    // Urls should start '/App_Plugins/UmbracoDeliveryApiExtensions/' not '/wwwroot/UmbracoDeliveryApiExtensions/', e.g.
                    // "/App_Plugins/UmbracoDeliveryApiExtensions/Styles/styles.css"
                }
            });
        }
    }
}
