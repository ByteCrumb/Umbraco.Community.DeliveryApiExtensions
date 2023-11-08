using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Community.DeliveryApiExtensions.Configuration.Options;
using Umbraco.Community.DeliveryApiExtensions.ContentApps;
using Umbraco.Community.DeliveryApiExtensions.Swagger;

namespace Umbraco.Community.DeliveryApiExtensions.Configuration;

internal static class UmbracoBuilderExtensions
{
    public static void ConfigurePreview(this IUmbracoBuilder builder, IConfigurationSection configSection)
    {
        _ = builder.ContentApps().Append<DeliveryApiPreviewApp>();
        IConfigurationSection previewConfigSection = configSection.GetSection<PreviewOptions>();
        _ = builder.Services.AddOptions<PreviewOptions>(previewConfigSection);

        IConfigurationSection mediaConfigSection = previewConfigSection.GetSection<MediaOptions>();
        _ = builder.Services.AddOptions<MediaOptions>(mediaConfigSection);
    }

    public static void ConfigureSwagger(this IUmbracoBuilder builder, IConfigurationSection configSection)
    {
        IConfigurationSection typedSwaggerConfigSection = configSection.GetSection<TypedSwaggerOptions>();
        TypedSwaggerOptions? typedSwaggerOptions = typedSwaggerConfigSection.Get<TypedSwaggerOptions>();
        if (typedSwaggerOptions is null || !typedSwaggerOptions.Enabled)
        {
            return;
        }

        _ = builder.Services.AddOptions<TypedSwaggerOptions>(typedSwaggerConfigSection);

        _ = builder.Services.Configure<SwaggerGenOptions>(options =>
        {
            if (typedSwaggerOptions.Mode == SwaggerGenerationMode.Auto)
            {
                options.UseOneOfForPolymorphism();
                options.UseAllOfForInheritance();
            }

            options.SchemaFilter<DeliveryApiContentTypesSchemaFilter>();
        });
    }
}
