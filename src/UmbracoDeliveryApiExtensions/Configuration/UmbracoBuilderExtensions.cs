using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Community.DeliveryApiExtensions.Configuration.Options;
using Umbraco.Community.DeliveryApiExtensions.ContentApps;
using Umbraco.Community.DeliveryApiExtensions.Services;
using Umbraco.Community.DeliveryApiExtensions.Swagger;

namespace Umbraco.Community.DeliveryApiExtensions.Configuration;

/// <summary>
/// Extensions for <see cref="IUmbracoBuilder"/> to add DeliveryApiExtensions.
/// </summary>
public static class UmbracoBuilderExtensions
{
    /// <summary>
    /// Registers the necessary services and configuration for DeliveryApiExtensions.
    /// </summary>
    public static void AddDeliveryApiExtensions(this IUmbracoBuilder builder)
    {
        _ = builder.ManifestFilters().Append<ManifestFilter>();

        IConfigurationSection configSection = builder.Config.GetSection<DeliveryApiExtensionsOptions>();
        _ = builder.Services.AddOptions<DeliveryApiExtensionsOptions>(configSection);

        // Preview
        builder.AddPreview(configSection);

        // TypedSwagger
        builder.AddTypedSwagger(configSection);
    }

    internal static void AddPreview(this IUmbracoBuilder builder, IConfigurationSection configSection)
    {
        _ = builder.ContentApps().Append<DeliveryApiPreviewAppFactory>();
        IConfigurationSection previewConfigSection = configSection.GetSection<PreviewOptions>();
        _ = builder.Services.AddOptions<PreviewOptions>(previewConfigSection);

        IConfigurationSection mediaConfigSection = previewConfigSection.GetSection<MediaOptions>();
        _ = builder.Services.AddOptions<MediaOptions>(mediaConfigSection);
    }

    internal static void AddTypedSwagger(this IUmbracoBuilder builder, IConfigurationSection configSection)
    {
        IConfigurationSection typedSwaggerConfigSection = configSection.GetSection<TypedSwaggerOptions>();
        TypedSwaggerOptions? typedSwaggerOptions = typedSwaggerConfigSection.Get<TypedSwaggerOptions>();
        if (typedSwaggerOptions is null || !typedSwaggerOptions.Enabled)
        {
            return;
        }

        _ = builder.Services.AddSingleton<IContentTypeInfoService, ContentTypeInfoService>();
        _ = builder.Services.AddOptions<TypedSwaggerOptions>(typedSwaggerConfigSection);

        _ = builder.Services.Configure<SwaggerGenOptions>(options =>
        {
            switch (typedSwaggerOptions.Mode)
            {
                case SwaggerGenerationMode.Auto:
                    options.UseOneOfForPolymorphism();
                    options.UseAllOfForInheritance();
                    break;

                case SwaggerGenerationMode.Compatibility:
                    options.UseAllOfForInheritance();
                    break;
                case SwaggerGenerationMode.Manual:
                default:
                    break;
            }

            options.SchemaFilter<EnumSchemaFilter>();
            options.SchemaFilter<DeliveryApiContentTypesSchemaFilter>();
        });
    }
}
