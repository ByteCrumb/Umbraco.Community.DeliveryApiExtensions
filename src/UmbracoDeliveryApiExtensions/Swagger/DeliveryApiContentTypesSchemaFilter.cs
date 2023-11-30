using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Umbraco.Cms.Api.Common.OpenApi;
using Umbraco.Cms.Core.Models.DeliveryApi;
using Umbraco.Community.DeliveryApiExtensions.Configuration.Options;
using Umbraco.Community.DeliveryApiExtensions.Models;
using Umbraco.Community.DeliveryApiExtensions.Services;

namespace Umbraco.Community.DeliveryApiExtensions.Swagger;

/// <summary>
/// <see cref="ISchemaFilter"/> for adding typed content type schemas to the swagger document."/>
/// </summary>
public class DeliveryApiContentTypesSchemaFilter : ISchemaFilter
{
    private const string ContentDiscriminator = "contentType";
    private const string MediaDiscriminator = "mediaType";

    private readonly IOptionsMonitor<TypedSwaggerOptions> _typedSwaggerOptions;
    private readonly IContentTypeInfoService _contentTypeInfoService;
    private readonly ISchemaIdSelector _schemaIdSelector;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DeliveryApiContentTypesSchemaFilter" /> class.
    /// </summary>
    public DeliveryApiContentTypesSchemaFilter(
        IOptionsMonitor<TypedSwaggerOptions> typedSwaggerOptions,
        IContentTypeInfoService contentTypeInfoService,
        ISchemaIdSelector schemaIdSelector)
    {
        _typedSwaggerOptions = typedSwaggerOptions;
        _contentTypeInfoService = contentTypeInfoService;
        _schemaIdSelector = schemaIdSelector;
    }

    /// <inheritdoc/>
    public virtual void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        SwaggerGenerationSettings settings = _typedSwaggerOptions.CurrentValue.SettingsFactory();

        if (settings is { UseOneOf: false, UseAllOf: false })
        {
            return;
        }

        if (typeof(IApiContentResponse) == context.Type)
        {
            ApplyPolymorphicContentType<IApiContentResponse, IApiContent>(schema, context, _contentTypeInfoService.GetContentTypes().Where(c => !c.IsElement), settings, contentType => (
                $"{contentType.SchemaId}ContentResponseModel",
                new OpenApiSchema
                {
                    Type = "object",
                    AdditionalPropertiesAllowed = false,
                    AllOf =
                    {
                        new OpenApiSchema { Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = GetTypeSchemaId<IApiContentResponse>(settings.UseOneOf) } },
                        new OpenApiSchema { Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = $"{contentType.SchemaId}ContentModel" } },
                    },
                }
            ), ContentDiscriminator);
            return;
        }

        if (typeof(IApiContent) == context.Type)
        {
            ApplyPolymorphicContentType<IApiContent, IApiElement>(schema, context, _contentTypeInfoService.GetContentTypes().Where(c => !c.IsElement), settings, contentType => (
                $"{contentType.SchemaId}ContentModel",
                new OpenApiSchema
                {
                    Type = "object",
                    AdditionalPropertiesAllowed = false,
                    AllOf = { new OpenApiSchema { Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = GetTypeSchemaId<IApiContent>(settings.UseOneOf) } } },
                    Properties =
                    {
                        ["properties"] = ContentTypePropertiesMapper(contentType, context, "Content"),
                    },
                }
            ), ContentDiscriminator);
            return;
        }

        if (typeof(IApiElement) == context.Type)
        {
            ApplyPolymorphicContentType<IApiElement>(schema, context, _contentTypeInfoService.GetContentTypes().Where(c => c.IsElement), settings, contentType => (
                $"{contentType.SchemaId}ElementModel",
                new OpenApiSchema
                {
                    Type = "object",
                    AdditionalPropertiesAllowed = false,
                    AllOf = { new OpenApiSchema { Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = GetTypeSchemaId<IApiElement>(settings.UseOneOf) } } },
                    Properties =
                    {
                        ["properties"] = ContentTypePropertiesMapper(contentType, context, "Content"),
                    },
                }
            ), ContentDiscriminator);
            return;
        }

        if (typeof(ApiMediaWithCropsResponse) == context.Type)
        {
            ApplyPolymorphicContentType<ApiMediaWithCropsResponse, ApiMediaWithCrops>(schema, context, _contentTypeInfoService.GetMediaTypes(), settings, contentType => (
                $"{contentType.SchemaId}CustomMediaWithCropsResponseModel",
                new OpenApiSchema
                {
                    Type = "object",
                    AdditionalPropertiesAllowed = false,
                    AllOf =
                    {
                        new OpenApiSchema { Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = GetTypeSchemaId<ApiMediaWithCropsResponse>(settings.UseOneOf) } },
                        new OpenApiSchema { Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = $"{contentType.SchemaId}CustomMediaWithCropsModel" } },
                    },
                }
            ), MediaDiscriminator);
            return;
        }

        if (typeof(ApiMediaWithCrops) == context.Type)
        {
            ApplyPolymorphicContentType<ApiMediaWithCrops, IApiMedia>(schema, context, _contentTypeInfoService.GetMediaTypes(), settings, contentType => (
                $"{contentType.SchemaId}CustomMediaWithCropsModel",
                new OpenApiSchema
                {
                    Type = "object",
                    AdditionalPropertiesAllowed = false,
                    AllOf =
                    {
                        new OpenApiSchema { Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = GetTypeSchemaId<ApiMediaWithCrops>(settings.UseOneOf) } },
                        new OpenApiSchema { Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = $"{contentType.SchemaId}CustomMediaModel" } },
                    },
                }
            ), MediaDiscriminator);
            return;
        }

        if (typeof(IApiMedia) == context.Type)
        {
            ApplyPolymorphicContentType<IApiMedia>(schema, context, _contentTypeInfoService.GetMediaTypes(), settings, contentType => (
                $"{contentType.SchemaId}CustomMediaModel",
                new OpenApiSchema
                {
                    Type = "object",
                    AdditionalPropertiesAllowed = false,
                    AllOf = { new OpenApiSchema { Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = GetTypeSchemaId<IApiMedia>(settings.UseOneOf) } } },
                    Properties =
                    {
                        ["properties"] = ContentTypePropertiesMapper(contentType, context, "Media"),
                    },
                }
            ), ContentDiscriminator);
            return;
        }
    }

    private void ApplyPolymorphicContentType<T, TAncestor>(OpenApiSchema schema, SchemaFilterContext context, IEnumerable<ContentTypeInfo> contentTypes, SwaggerGenerationSettings settings, Func<ContentTypeInfo, (string SchemaId, OpenApiSchema Schema)> contentTypeSchemaMapper, string discriminator)
    {
        // Ensure ancestor is generated if not already
        context.SchemaGenerator.GenerateSchema(typeof(TAncestor), context.SchemaRepository);

        // Add ancestor to AllOf, so all properties are inherited
        schema.AllOf.Add(new OpenApiSchema
        {
            Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = GetTypeSchemaId<TAncestor>(settings.UseOneOf) },
        });

        ApplyPolymorphicContentType<T>(schema, context, contentTypes, settings, contentTypeSchemaMapper, discriminator);
    }

    private void ApplyPolymorphicContentType<T>(OpenApiSchema schema, SchemaFilterContext context, IEnumerable<ContentTypeInfo> contentTypes, SwaggerGenerationSettings settings, Func<ContentTypeInfo, (string SchemaId, OpenApiSchema Schema)> contentTypeSchemaMapper, string discriminator)
    {
        OpenApiSchema? originalSchema = null;
        if (settings.UseOneOf)
        {
            // Swashbuckle doesn't allow us to return a new inline schema.
            // So, we clone the original schema instead, and register the clone with a "Base" schemaId suffix.
            // This allows us to clear the original inline schema, and modify it to add any applicable OneOf entries.

            originalSchema = schema;
            schema = new OpenApiSchema(originalSchema);

            context.SchemaRepository.Schemas.Add(GetTypeSchemaId<T>(true), schema);

            ClearSchema(originalSchema);
        }

        schema.Discriminator = new OpenApiDiscriminator
        {
            PropertyName = discriminator,
        };

        foreach (ContentTypeInfo contentType in contentTypes)
        {
            (string? schemaId, OpenApiSchema? openApiSchema) = contentTypeSchemaMapper(contentType);

            OpenApiSchema? contentTypeSchema = context.SchemaRepository.AddDefinition(schemaId, openApiSchema);

            schema.Discriminator.Mapping[contentType.Alias] = contentTypeSchema.Reference.ReferenceV3;
            originalSchema?.OneOf.Add(contentTypeSchema);
        }
    }

    private static OpenApiSchema ContentTypePropertiesMapper(ContentTypeInfo contentType, SchemaFilterContext context, string type)
    {
        return context.SchemaRepository.AddDefinition(
            $"{contentType.SchemaId}{type}PropertiesModel",
            new OpenApiSchema
            {
                Type = "object",
                AdditionalPropertiesAllowed = true,
                AllOf = contentType.CompositionSchemaIds.Select(c => new OpenApiSchema
                {
                    Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = $"{c}{type}PropertiesModel" },
                }).ToList(),
                Properties = contentType.Properties
                    .Where(p => !p.Inherited) // Filter out composition properties, as they are handled by AllOf
                    .ToDictionary(
                        p => p.Alias,
                        p =>
                        {
                            OpenApiSchema propertySchema = context.SchemaGenerator.GenerateSchema(p.Type, context.SchemaRepository);
                            propertySchema.Nullable = true;
                            return propertySchema;
                        }
                    ),
            }
        );
    }

    private string GetTypeSchemaId<T>(bool baseSuffix)
    {
        return _schemaIdSelector.SchemaId(typeof(T)) + (baseSuffix ? "Base" : null);
    }

    private static void ClearSchema(OpenApiSchema schema)
    {
        schema.AllOf.Clear();
        schema.OneOf.Clear();
        schema.AnyOf.Clear();
        schema.Required.Clear();
        schema.Properties.Clear();
        schema.AdditionalProperties = null;
        schema.Discriminator = null;
    }
}
