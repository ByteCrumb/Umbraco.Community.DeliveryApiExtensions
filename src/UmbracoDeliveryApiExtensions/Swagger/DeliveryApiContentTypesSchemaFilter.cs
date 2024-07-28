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
public class DeliveryApiContentTypesSchemaFilter : ISchemaFilter, IDocumentFilter
{
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
    public virtual void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        SwaggerGenerationSettings settings = _typedSwaggerOptions.CurrentValue.SettingsFactory();

        if (settings is { UseOneOf: false, UseAllOf: false })
        {
            return;
        }

        ApplyPolymorphicContentType<IApiContent>(context, _contentTypeInfoService.GetContentTypes().Where(c => !c.IsElement).DistinctBy(c => c.Alias), contentType => (
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
        ));

        ApplyPolymorphicContentType<IApiElement>(context, _contentTypeInfoService.GetContentTypes().Where(c => !c.IsElement).DistinctBy(c => c.Alias), contentType => (
            $"{contentType.SchemaId}ContentModel",
            new OpenApiSchema
            {
                Type = "object",
                AdditionalPropertiesAllowed = false,
                AllOf = { new OpenApiSchema { Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = GetTypeSchemaId<IApiContent>(settings.UseOneOf) } } },
                Properties =
                {
                    ["properties"] = ContentTypePropertiesMapper(contentType, context),
                },
            }
        ));

        ApplyPolymorphicContentType(context, _contentTypeInfoService.GetContentTypes().Where(c => c.IsElement).DistinctBy(c => c.Alias), contentType => (
            $"{contentType.SchemaId}ElementModel",
            new OpenApiSchema
            {
                Type = "object",
                AdditionalPropertiesAllowed = false,
                AllOf = { new OpenApiSchema { Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = GetTypeSchemaId<IApiElement>(settings.UseOneOf) } } },
                Properties =
                {
                    ["properties"] = ContentTypePropertiesMapper(contentType, context),
                },
            }
        ));
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
            ApplyPolymorphicContentTypeSchema<IApiContentResponse, IApiContent>(
                schema,
                context,
                _contentTypeInfoService.GetContentTypes().Where(c => !c.IsElement).DistinctBy(c => c.Alias),
                settings,
                contentType => $"{contentType.SchemaId}ContentResponseModel"
            );
            return;
        }

        if (typeof(IApiContent) == context.Type)
        {
            ApplyPolymorphicContentTypeSchema<IApiContent, IApiElement>(
                schema,
                context,
                _contentTypeInfoService.GetContentTypes().Where(c => !c.IsElement).DistinctBy(c => c.Alias),
                settings,
                contentType => $"{contentType.SchemaId}ContentModel"
            );
            return;
        }

        if (typeof(IApiElement) == context.Type)
        {
            ApplyPolymorphicContentTypeSchema<IApiElement>(
                schema,
                context,
                _contentTypeInfoService.GetContentTypes().Where(c => c.IsElement).DistinctBy(c => c.Alias),
                settings,
                contentType => $"{contentType.SchemaId}ElementModel"
            );
            return;
        }
    }

    private static void ApplyPolymorphicContentType<TAncestor>(DocumentFilterContext context, IEnumerable<ContentTypeInfo> contentTypes, Func<ContentTypeInfo, (string SchemaId, OpenApiSchema Schema)> contentTypeSchemaMapper)
    {
        // Ensure ancestor is generated if not already
        _ = context.SchemaGenerator.GenerateSchema(typeof(TAncestor), context.SchemaRepository);

        ApplyPolymorphicContentType(context, contentTypes, contentTypeSchemaMapper);
    }

    private void ApplyPolymorphicContentTypeSchema<T, TAncestor>(OpenApiSchema schema, SchemaFilterContext context, IEnumerable<ContentTypeInfo> contentTypes, SwaggerGenerationSettings settings, Func<ContentTypeInfo, string> contentTypeSchemaIdMapper)
    {
        // Add ancestor to AllOf, so all properties are inherited
        schema.AllOf.Add(new OpenApiSchema
        {
            Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = GetTypeSchemaId<TAncestor>(settings.UseOneOf) },
        });

        ApplyPolymorphicContentTypeSchema<T>(schema, context, contentTypes, settings, contentTypeSchemaIdMapper);
    }

    private static void ApplyPolymorphicContentType(DocumentFilterContext context, IEnumerable<ContentTypeInfo> contentTypes, Func<ContentTypeInfo, (string SchemaId, OpenApiSchema Schema)> contentTypeSchemaMapper)
    {
        foreach (ContentTypeInfo contentType in contentTypes)
        {
            (string? schemaId, OpenApiSchema? openApiSchema) = contentTypeSchemaMapper(contentType);

            context.SchemaRepository.AddDefinition(schemaId, openApiSchema);
        }
    }

    private void ApplyPolymorphicContentTypeSchema<T>(OpenApiSchema schema, SchemaFilterContext context, IEnumerable<ContentTypeInfo> contentTypes, SwaggerGenerationSettings settings, Func<ContentTypeInfo, string> contentTypeSchemaIdMapper)
    {
        OpenApiSchema? originalSchema = null;
        if (settings.UseOneOf)
        {
            // Swashbuckle doesn't allow us to return a new inline schema.
            // So, we clone the original schema instead, and register the clone with a "Base" schemaId suffix.
            // This allows us to clear the original inline schema, and modify it to add any applicable OneOf entries.

            originalSchema = schema;
            schema = new OpenApiSchema(originalSchema);
            //schema.OneOf.Clear();

            context.SchemaRepository.Schemas.TryAdd(GetTypeSchemaId<T>(true), schema);

            ClearSchema(originalSchema);
        }

        schema.Discriminator = new OpenApiDiscriminator
        {
            PropertyName = "contentType",
        };

        foreach (ContentTypeInfo contentType in contentTypes)
        {
            OpenApiSchema contentTypeSchema = new()
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.Schema,
                    Id = contentTypeSchemaIdMapper(contentType),
                },
            };

            schema.Discriminator.Mapping[contentType.Alias] = contentTypeSchema.Reference.ReferenceV3;
            originalSchema?.OneOf.Add(contentTypeSchema);
        }
    }

    private static OpenApiSchema ContentTypePropertiesMapper(ContentTypeInfo contentType, DocumentFilterContext context)
    {
        return context.SchemaRepository.AddDefinition(
            $"{contentType.SchemaId}PropertiesModel",
            new OpenApiSchema
            {
                Type = "object",
                AdditionalPropertiesAllowed = true,
                AllOf = contentType.CompositionSchemaIds.Select(c => new OpenApiSchema
                {
                    Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = $"{c}PropertiesModel" },
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
