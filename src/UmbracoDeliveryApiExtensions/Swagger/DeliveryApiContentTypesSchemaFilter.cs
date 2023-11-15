using System.Reflection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Umbraco.Cms.Api.Common.OpenApi;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.DeliveryApi;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors.DeliveryApi;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;
using Umbraco.Community.DeliveryApiExtensions.Configuration.Options;
using Umbraco.Extensions;

namespace Umbraco.Community.DeliveryApiExtensions.Swagger;

/// <summary>
/// <see cref="ISchemaFilter"/> for adding typed content type schemas to the swagger document."/>
/// </summary>
public class DeliveryApiContentTypesSchemaFilter : ISchemaFilter
{
    private readonly IOptionsMonitor<TypedSwaggerOptions> _typedSwaggerOptions;
    private readonly IContentTypeService _contentTypeService;
    private readonly IPublishedContentTypeFactory _publishedContentTypeFactory;
    private readonly ISchemaIdSelector _schemaIdSelector;
    private readonly IShortStringHelper _shortStringHelper;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DeliveryApiContentTypesSchemaFilter" /> class.
    /// </summary>
    public DeliveryApiContentTypesSchemaFilter(IOptionsMonitor<TypedSwaggerOptions> typedSwaggerOptions, IContentTypeService contentTypeService, IPublishedContentTypeFactory publishedContentTypeFactory, ISchemaIdSelector schemaIdSelector, IShortStringHelper shortStringHelper)
    {
        _typedSwaggerOptions = typedSwaggerOptions;
        _contentTypeService = contentTypeService;
        _publishedContentTypeFactory = publishedContentTypeFactory;
        _schemaIdSelector = schemaIdSelector;
        _shortStringHelper = shortStringHelper;
    }

    /// <inheritdoc/>
    public virtual void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        SwaggerGenerationSettings settings = _typedSwaggerOptions.CurrentValue.SettingsFactory();

        if (settings is { UseOneOf: false, UseAllOf: false })
        {
            return;
        }

        if (typeof(IApiElement) == context.Type)
        {
            HandleIApiElement(schema, context, settings);
            return;
        }

        if (typeof(IApiContent) == context.Type)
        {
            HandleIApiContent(schema, context, settings);
            return;
        }

        if (typeof(IApiContentResponse) == context.Type)
        {
            HandleIApiContentResponse(schema, context, settings);
            return;
        }
    }

    private void HandleIApiElement(OpenApiSchema schema, SchemaFilterContext context, SwaggerGenerationSettings settings)
    {
        OpenApiSchema? originalSchema = null;
        if (settings.UseOneOf)
        {
            // Swashbuckle doesn't allow us to return an inline schema
            // So we instead clone the current schema as IApiElementBase and update the current schema to be OneOf

            originalSchema = schema;
            schema = new OpenApiSchema(originalSchema);

            context.SchemaRepository.Schemas.Add(GetTypeSchemaId<IApiElement>(true), schema);

            ClearSchema(originalSchema);
        }

        schema.Discriminator = new OpenApiDiscriminator
        {
            PropertyName = "contentType",
        };

        foreach (IContentType contentType in _contentTypeService.GetAll().Where(c => c.IsElement))
        {
            IPublishedContentType publishedContentType = _publishedContentTypeFactory.CreateContentType(contentType);

            OpenApiSchema? contentTypeSchema = context.SchemaRepository.AddDefinition(
                $"{GetContentTypeSchemaId(contentType)}ElementModel",
                new OpenApiSchema
                {
                    Type = "object",
                    AllOf = { new OpenApiSchema { Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = GetTypeSchemaId<IApiElement>(settings.UseOneOf) } } },
                    AdditionalPropertiesAllowed = false,
                    Properties =
                    {
                        ["properties"] = context.SchemaRepository.AddDefinition(
                            $"{GetContentTypeSchemaId(contentType)}PropertiesModel",
                            new OpenApiSchema
                            {
                                Type = "object",
                                AdditionalPropertiesAllowed = true,
                                AllOf = contentType.ContentTypeComposition.Select(c => new OpenApiSchema { Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = $"{GetContentTypeSchemaId(c)}PropertiesModel" } }).ToList(),
                                Properties = publishedContentType.PropertyTypes
                                    .Where(p => contentType.PropertyTypes.Any(x => x.Alias == p.Alias)) // Filter out composition properties
                                    .ToDictionary(
                                        p => p.Alias,
                                        p =>
                                        {
                                            OpenApiSchema propertySchema = context.SchemaGenerator.GenerateSchema(GetPropertyType(p), context.SchemaRepository);
                                            propertySchema.Nullable = true;
                                            return propertySchema;
                                        }
                                    ),
                            }
                        ),
                    },
                }
            );

            schema.Discriminator.Mapping[contentType.Alias] = contentTypeSchema.Reference.ReferenceV3;
            originalSchema?.OneOf.Add(contentTypeSchema);
        }
    }

    private void HandleIApiContent(OpenApiSchema schema, SchemaFilterContext context, SwaggerGenerationSettings settings)
    {
        // Ensure IApiElement is generated if not already
        context.SchemaGenerator.GenerateSchema(typeof(IApiElement), context.SchemaRepository);

        OpenApiSchema? originalSchema = null;
        if (settings.UseOneOf)
        {
            originalSchema = schema;
            schema = new OpenApiSchema(originalSchema);
            context.SchemaRepository.Schemas.Add(GetTypeSchemaId<IApiContent>(true), schema);

            ClearSchema(originalSchema);
        }

        schema.AllOf.Add(new OpenApiSchema
        {
            Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = GetTypeSchemaId<IApiElement>(settings.UseOneOf) },
        });
        schema.Discriminator = new OpenApiDiscriminator
        {
            PropertyName = "contentType",
        };

        foreach (IContentType contentType in _contentTypeService.GetAll().Where(c => !c.IsElement))
        {
            IPublishedContentType publishedContentType = _publishedContentTypeFactory.CreateContentType(contentType);

            OpenApiSchema? contentTypeSchema = context.SchemaRepository.AddDefinition(
                $"{GetContentTypeSchemaId(contentType)}ContentModel",
                new OpenApiSchema
                {
                    Type = "object",
                    AdditionalPropertiesAllowed = false,
                    AllOf =
                    {
                        new OpenApiSchema
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.Schema,
                                Id = GetTypeSchemaId<IApiContent>(settings.UseOneOf),
                            },
                        },
                    },
                    Properties =
                    {
                        ["properties"] = context.SchemaRepository.AddDefinition(
                            $"{GetContentTypeSchemaId(contentType)}PropertiesModel",
                            new OpenApiSchema
                            {
                                Type = "object",
                                AdditionalPropertiesAllowed = true,
                                AllOf = contentType.ContentTypeComposition.Select(c => new OpenApiSchema { Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = $"{GetContentTypeSchemaId(c)}PropertiesModel" } }).ToList(),
                                Properties = publishedContentType.PropertyTypes
                                    .Where(p => contentType.PropertyTypes.Any(x => x.Alias == p.Alias)) // Filter out composition properties
                                    .ToDictionary(
                                        p => p.Alias,
                                        p =>
                                        {
                                            OpenApiSchema propertySchema = context.SchemaGenerator.GenerateSchema(GetPropertyType(p), context.SchemaRepository);
                                            propertySchema.Nullable = true;
                                            return propertySchema;
                                        }
                                    ),
                            }
                        ),
                    },
                }
            );

            schema.Discriminator.Mapping[contentType.Alias] = contentTypeSchema.Reference.ReferenceV3;
            originalSchema?.OneOf.Add(contentTypeSchema);
        }
    }

    private void HandleIApiContentResponse(OpenApiSchema schema, SchemaFilterContext context, SwaggerGenerationSettings settings)
    {
        // Ensure IApiContent is generated if not already
        context.SchemaGenerator.GenerateSchema(typeof(IApiContent), context.SchemaRepository);

        OpenApiSchema? originalSchema = null;
        if (settings.UseOneOf)
        {
            originalSchema = schema;
            schema = new OpenApiSchema(originalSchema);
            context.SchemaRepository.Schemas.Add(GetTypeSchemaId<IApiContentResponse>(true), schema);

            ClearSchema(originalSchema);
        }

        schema.AllOf.Add(new OpenApiSchema
        {
            Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = GetTypeSchemaId<IApiContent>(settings.UseOneOf) },
        });
        schema.Discriminator = new OpenApiDiscriminator
        {
            PropertyName = "contentType",
        };

        foreach (IContentType contentType in _contentTypeService.GetAll().Where(c => !c.IsElement))
        {
            OpenApiSchema? contentTypeSchema = context.SchemaRepository.AddDefinition(
                $"{GetContentTypeSchemaId(contentType)}ContentResponseModel",
                new OpenApiSchema
                {
                    Type = "object",
                    AdditionalPropertiesAllowed = false,
                    AllOf =
                    {
                        new OpenApiSchema
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.Schema,
                                Id = GetTypeSchemaId<IApiContentResponse>(settings.UseOneOf),
                            },
                        },
                        new OpenApiSchema
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = $"{GetContentTypeSchemaId(contentType)}ContentModel" },
                        },
                    },
                }
            );

            schema.Discriminator.Mapping[contentType.Alias] = contentTypeSchema.Reference.ReferenceV3;
            originalSchema?.OneOf.Add(contentTypeSchema);
        }
    }

    private string GetContentTypeSchemaId(IContentTypeBase contentType)
    {
        // This is what ModelsBuilder currently also uses
        return contentType.Alias.ToCleanString(_shortStringHelper, CleanStringType.ConvertCase | CleanStringType.PascalCase);
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

    // The DeliveryApi property value type is currently not exposed by Umbraco
    // https://github.com/umbraco/Umbraco-CMS/pull/15150
    // TODO: Remove this when Umbraco exposes the delivery api property value type
    private static readonly FieldInfo? ConverterField = typeof(PublishedPropertyType).GetField("_converter", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
    private static Type GetPropertyType(IPublishedPropertyType publishedPropertyType)
    {
        Type modelClrType = publishedPropertyType.ModelClrType;

        if (ConverterField?.GetValue(publishedPropertyType) is IDeliveryApiPropertyValueConverter propertyValueConverter)
        {
            return propertyValueConverter.GetDeliveryApiPropertyValueType(publishedPropertyType);
        }

        return modelClrType;
    }
}
