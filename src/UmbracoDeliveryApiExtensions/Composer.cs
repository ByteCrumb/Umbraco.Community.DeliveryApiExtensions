using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Community.DeliveryApiExtensions.Configuration;
using Umbraco.Community.DeliveryApiExtensions.Configuration.Options;
using Umbraco.Community.DeliveryApiExtensions.ContentApps;
using Umbraco.Community.DeliveryApiExtensions.Swagger;

namespace Umbraco.Community.DeliveryApiExtensions;

public sealed class Composer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        _ = builder.ManifestFilters().Append<ManifestFilter>();

        IConfigurationSection configSection = builder.Config.GetSection<DeliveryApiExtensionsOptions>();
        _ = builder.Services.AddOptions<DeliveryApiExtensionsOptions>(configSection);

        // Preview
        builder.ConfigurePreview(configSection);

        // Swagger
        builder.ConfigureSwagger();
    }
}

internal static class ComposerExtensions
{
    public static void ConfigurePreview(this IUmbracoBuilder builder, IConfigurationSection configSection)
    {
        _ = builder.ContentApps().Append<DeliveryApiPreviewApp>();
        IConfigurationSection previewConfigSection = configSection.GetSection<PreviewOptions>();
        _ = builder.Services.AddOptions<PreviewOptions>(previewConfigSection);

        IConfigurationSection mediaConfigSection = previewConfigSection.GetSection<MediaOptions>();
        _ = builder.Services.AddOptions<MediaOptions>(mediaConfigSection);
    }

    public static void ConfigureSwagger(this IUmbracoBuilder builder)
    {
        // Enable generation of typed content responses based on CMS content types
        _ = builder.Services.Configure<SwaggerGenOptions>(options =>
        {
            options.SupportNonNullableReferenceTypes();

            // UseOneOfForPolymorphism is disabled as we are consuming the swagger with NSwag
            // If using other code generations tools, like Orval, it should be enabled for better compatibility
            //options.UseOneOfForPolymorphism();

            options.UseAllOfForInheritance();

            options.SchemaFilter<DeliveryApiContentTypesSchemaFilter>();
        });
    }
}
