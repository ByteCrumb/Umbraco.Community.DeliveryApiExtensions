using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
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
    private readonly ILogger<DeliveryApiContentTypesSchemaFilter> _logger;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DeliveryApiContentTypesSchemaFilter" /> class.
    /// </summary>
    public DeliveryApiContentTypesSchemaFilter(
        IOptionsMonitor<TypedSwaggerOptions> typedSwaggerOptions,
        IContentTypeInfoService contentTypeInfoService,
        ISchemaIdSelector schemaIdSelector,
        ILogger<DeliveryApiContentTypesSchemaFilter> logger)
    {
        _typedSwaggerOptions = typedSwaggerOptions;
        _contentTypeInfoService = contentTypeInfoService;
        _schemaIdSelector = schemaIdSelector;
        _logger = logger;
    }

    /// <inheritdoc/>
    public virtual void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        SwaggerGenerationSettings settings = _typedSwaggerOptions.CurrentValue.SettingsFactory();

        if (settings is { UseOneOf: false, UseAllOf: false } || !HasMarker(context))
        {
            return;
        }

        ApplyPolymorphicContentType<IApiContent>(context,
        _contentTypeInfoService.GetContentTypes().Where(c => !c.IsElement).DistinctBy(c => c.Alias),
        contentType => (
            $"{contentType.SchemaId}ContentResponseModel",
            new OpenApiSchema
            {
                Type = JsonSchemaType.Object,
                AdditionalPropertiesAllowed = false,
                AllOf =
                [
                    new OpenApiSchemaReference(GetTypeSchemaId<IApiContentResponse>(settings.UseOneOf)),
                    new OpenApiSchemaReference($"{contentType.SchemaId}ContentModel")
                ],
            }
        ));

        ApplyPolymorphicContentType<IApiElement>(context,
            _contentTypeInfoService.GetContentTypes().Where(c => !c.IsElement).DistinctBy(c => c.Alias),
            contentType => (
                $"{contentType.SchemaId}ContentModel",
                new OpenApiSchema
                {
                    Type = JsonSchemaType.Object,
                    AdditionalPropertiesAllowed = false,
                    AllOf =
                    [
                        new OpenApiSchemaReference(GetTypeSchemaId<IApiContent>(settings.UseOneOf)),
                    ],
                    Properties = new Dictionary<string, IOpenApiSchema>
                    {
                        ["properties"] = ContentTypePropertiesMapper(contentType, context)
                    },
                }
            ));

        ApplyPolymorphicContentType(context,
            _contentTypeInfoService.GetContentTypes().Where(c => c.IsElement).DistinctBy(c => c.Alias),
            contentType => (
                $"{contentType.SchemaId}ElementModel",
                new OpenApiSchema
                {
                    Type = JsonSchemaType.Object,
                    AdditionalPropertiesAllowed = false,
                    AllOf = [
                        new OpenApiSchemaReference(GetTypeSchemaId<IApiElement>(settings.UseOneOf))
                    ],
                    Properties = new Dictionary<string, IOpenApiSchema>
                    {
                        ["properties"] = ContentTypePropertiesMapper(contentType, context)
                    },
                }
            ));

        RemoveMarker(swaggerDoc);
    }

    /// <inheritdoc/>
    public virtual void Apply(IOpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema is not OpenApiSchema openApiSchema)
        {
            return;
        }

        SwaggerGenerationSettings settings = _typedSwaggerOptions.CurrentValue.SettingsFactory();

        if (settings is { UseOneOf: false, UseAllOf: false })
        {
            return;
        }

        if (typeof(IApiContentResponse) == context.Type)
        {
            ApplyPolymorphicContentTypeSchema<IApiContentResponse, IApiContent>(
                openApiSchema,
                context,
                _contentTypeInfoService.GetContentTypes()
                    .Where(c => !c.IsElement)
                    .DistinctBy(c => c.Alias),
                settings,
                contentType => $"{contentType.SchemaId}ContentResponseModel"
            );

            AddMarker(context);
            return;
        }

        if (typeof(IApiContent) == context.Type)
        {
            ApplyPolymorphicContentTypeSchema<IApiContent, IApiElement>(
                openApiSchema,
                context,
                _contentTypeInfoService.GetContentTypes().Where(c => !c.IsElement).DistinctBy(c => c.Alias),
                settings,
                contentType => $"{contentType.SchemaId}ContentModel"
            );

            AddMarker(context);
            return;
        }

        if (typeof(IApiElement) == context.Type)
        {
            ApplyPolymorphicContentTypeSchema<IApiElement>(
                openApiSchema,
                context,
                _contentTypeInfoService.GetContentTypes().Where(c => c.IsElement).DistinctBy(c => c.Alias),
                settings,
                contentType => $"{contentType.SchemaId}ElementModel"
            );

            AddMarker(context);
            return;
        }
    }

    private static void ApplyPolymorphicContentType<TAncestor>(DocumentFilterContext context,
        IEnumerable<ContentTypeInfo> contentTypes,
        Func<ContentTypeInfo, (string SchemaId, OpenApiSchema Schema)> contentTypeSchemaMapper)
    {
        // Ensure ancestor is generated if not already
        _ = context.SchemaGenerator.GenerateSchema(typeof(TAncestor), context.SchemaRepository);

        ApplyPolymorphicContentType(context, contentTypes, contentTypeSchemaMapper);
    }

    private void ApplyPolymorphicContentTypeSchema<T, TAncestor>(
        OpenApiSchema schema,
        SchemaFilterContext context,
        IEnumerable<ContentTypeInfo> contentTypes,
        SwaggerGenerationSettings settings,
        Func<ContentTypeInfo, string> contentTypeSchemaIdMapper)
    {
        // Add ancestor to AllOf, so all properties are inherited
        schema.AllOf ??= [];
        schema.AllOf.Add(new OpenApiSchemaReference(GetTypeSchemaId<TAncestor>(settings.UseOneOf)));

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
            schema = (OpenApiSchema) originalSchema.CreateShallowCopy();
            schema.Required?.Remove("properties");
            schema.Properties?.Remove("properties");

            context.SchemaRepository.Schemas.TryAdd(GetTypeSchemaId<T>(true), schema);

            ClearSchema(originalSchema);
        }

        schema.Discriminator = new OpenApiDiscriminator
        {
            PropertyName = "contentType",
            Mapping = new Dictionary<string, OpenApiSchemaReference>(),
        };

        foreach (ContentTypeInfo contentType in contentTypes)
        {
            OpenApiSchemaReference contentTypeSchema = new(contentTypeSchemaIdMapper(contentType));

            schema.Discriminator.Mapping[contentType.Alias] = contentTypeSchema;
            if (originalSchema is null)
            {
                continue;
            }

            originalSchema.OneOf ??= [];
            originalSchema.OneOf.Add(contentTypeSchema);
        }
    }

    private OpenApiSchemaReference ContentTypePropertiesMapper(ContentTypeInfo contentType, DocumentFilterContext context)
    {
        return context.SchemaRepository.AddDefinition(
            $"{contentType.SchemaId}PropertiesModel",
            new OpenApiSchema
            {
                Type = JsonSchemaType.Object,
                AdditionalPropertiesAllowed = false,
                AllOf = [.. contentType.CompositionSchemaIds.Select(c => new OpenApiSchemaReference($"{c}PropertiesModel"))],
                Properties = contentType.Properties
                    .Where(p => !p.Inherited) // Filter out composition properties, as they are handled by AllOf
                    .ToDictionary(
                        p => p.Alias, IOpenApiSchema (p) =>
                        {
                            IOpenApiSchema propertySchema;
                            try
                            {
                                propertySchema = context.SchemaGenerator.GenerateSchema(p.Type, context.SchemaRepository);
                                if (propertySchema is OpenApiSchema schema)
                                {
                                    schema.Type |= JsonSchemaType.Null;
                                }
                            }
                            catch (Exception ex)
                            {
                                // Just default to any type in case of error
                                propertySchema = new OpenApiSchema();

                                _logger.LogWarning(ex, "Failed to generate {PropertyType} schema of {PropertyEditorAlias} for property {PropertyAlias} of content type {ContentTypeAlias}", p.Type, p.EditorAlias, p.Alias, contentType.Alias);
                            }

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
        schema.AllOf?.Clear();
        schema.OneOf?.Clear();
        schema.AnyOf?.Clear();
        schema.Required?.Clear();
        schema.Properties?.Clear();
        schema.AdditionalProperties = null;
        schema.Discriminator = null;
    }

    private const string MarkerId = "__marker__";
    private static bool HasMarker(DocumentFilterContext context)
    {
        return context.SchemaRepository.Schemas.ContainsKey(MarkerId);
    }

    private static void AddMarker(SchemaFilterContext context)
    {
        context.SchemaRepository.Schemas.TryAdd(MarkerId, new OpenApiSchema());
    }

    private static void RemoveMarker(OpenApiDocument swaggerDoc)
    {
        swaggerDoc.Components?.Schemas?.Remove(MarkerId);
    }
}
