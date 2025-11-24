using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Models.DeliveryApi;
using Umbraco.Community.DeliveryApiExtensions.Configuration.Options;
using Umbraco.Community.DeliveryApiExtensions.Services;
using Umbraco.Community.DeliveryApiExtensions.Swagger;
using Umbraco.Extensions;

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
        IConfigurationSection configSection = builder.Config.GetSection<DeliveryApiExtensionsOptions>();
        _ = builder.Services.AddOptions<DeliveryApiExtensionsOptions>(configSection);

        // Preview
        builder.AddPreview(configSection);

        // TypedSwagger
        builder.AddTypedSwagger(configSection);
    }

    internal static void AddPreview(this IUmbracoBuilder builder, IConfigurationSection configSection)
    {
        IConfigurationSection previewConfigSection = configSection.GetSection<PreviewOptions>();
        _ = builder.Services.AddOptions<PreviewOptions>(previewConfigSection);

        IConfigurationSection mediaConfigSection = previewConfigSection.GetSection<MediaOptions>();
        _ = builder.Services.AddOptions<MediaOptions>(mediaConfigSection);
    }

    internal static void AddTypedSwagger(this IUmbracoBuilder builder, IConfigurationSection configSection)
    {
        IConfigurationSection typedSwaggerConfigSection = configSection.GetSection<TypedSwaggerOptions>();
        TypedSwaggerOptions? typedSwaggerOptions = typedSwaggerConfigSection.Get<TypedSwaggerOptions>();
        if (typedSwaggerOptions?.Enabled == false)
        {
            return;
        }

        _ = builder.Services.AddSingleton<IContentTypeInfoService, ContentTypeInfoService>();
        _ = builder.Services.AddOptions<TypedSwaggerOptions>(typedSwaggerConfigSection);

        _ = builder.Services.PostConfigure<SwaggerGenOptions>(options =>
        {
            switch (typedSwaggerOptions?.Mode ?? SwaggerGenerationMode.Auto)
            {
                case SwaggerGenerationMode.Auto:
                    options.UseOneOfForPolymorphism();
                    options.UseAllOfForInheritance();
                    break;

                case SwaggerGenerationMode.Compatibility:
                    options.SchemaGeneratorOptions.UseOneOfForPolymorphism = false;
                    options.UseAllOfForInheritance();
                    break;
                case SwaggerGenerationMode.Manual:
                default:
                    break;
            }

            options.SupportNonNullableReferenceTypes();

            options.SchemaFilter<DeliveryApiContentTypesSchemaFilter>();
            options.DocumentFilter<DeliveryApiContentTypesSchemaFilter>();

            options.SchemaFilter<FixPropertyNullabilityFilter>();

            Func<Type, IEnumerable<Type>> currentSubTypesSelector = options.SchemaGeneratorOptions.SubTypesSelector;
            options.SelectSubTypesUsing(baseType =>
            {
                List<Type> handledTypes = [
                    typeof(IApiElement),
                    typeof(IApiContent),
                    typeof(IApiMediaWithCrops),
                    typeof(IApiContentResponse),
                    typeof(IApiMediaWithCropsResponse),
                ];

                if (handledTypes.Contains(baseType))
                {
                    return [];
                }

                List<Type> result = [.. currentSubTypesSelector(baseType)];

                if (result.Count == 1 && result[0] == baseType)
                {
                    return baseType.Assembly.GetTypes().Where(type => type.IsSubclassOf(baseType));
                }

                return result;
            });
        });
    }
}
