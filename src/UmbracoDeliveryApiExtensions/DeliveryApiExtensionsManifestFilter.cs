using System.Reflection;
using Umbraco.Cms.Core.Manifest;

namespace Umbraco.Community.DeliveryApiExtensions;

internal class DeliveryApiExtensionsManifestFilter : IManifestFilter
{
    public void Filter(List<PackageManifest> manifests)
    {
        Assembly assembly = typeof(DeliveryApiExtensionsManifestFilter).Assembly;

        manifests.Add(new PackageManifest
        {
            PackageName = "DeliveryApiExtensions",
            Version = assembly.GetName()?.Version?.ToString(3) ?? "0.1.0",
            AllowPackageTelemetry = true,
            Scripts = new string[] {
                "/App_Plugins/DeliveryApiExtensions/delivery-api-extensions.iife.js",
                "/App_Plugins/DeliveryApiExtensions/preview.controller.js"
            },
            Stylesheets = new string[]
            {
            }
        });
    }
}
