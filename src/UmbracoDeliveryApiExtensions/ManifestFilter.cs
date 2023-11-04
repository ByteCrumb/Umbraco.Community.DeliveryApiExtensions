using System.Reflection;
using Umbraco.Cms.Core.Manifest;

namespace Umbraco.Community.DeliveryApiExtensions;

internal class ManifestFilter : IManifestFilter
{
    public void Filter(List<PackageManifest> manifests)
    {
        Assembly assembly = typeof(ManifestFilter).Assembly;

        manifests.Add(new PackageManifest
        {
            PackageName = "DeliveryApiExtensions",
            Version = assembly.GetName().Version?.ToString(3) ?? "0.1.0",
            AllowPackageTelemetry = true,
            Scripts = new[] {
                "/App_Plugins/DeliveryApiExtensions/delivery-api-extensions.iife.js"
            },
            Stylesheets = Array.Empty<string>()
        });
    }
}
