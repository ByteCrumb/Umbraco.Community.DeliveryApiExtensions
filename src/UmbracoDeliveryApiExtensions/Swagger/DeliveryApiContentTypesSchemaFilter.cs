using System.Reflection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Umbraco.Cms.Api.Common.OpenApi;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.DeliveryApi;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors.DeliveryApi;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;
using Umbraco.Extensions;

namespace Umbraco.Community.DeliveryApiExtensions.Swagger;

/// <summary>
/// <see cref="ISchemaFilter"/> for adding typed content type schemas to the swagger document."/>
/// </summary>
public class DeliveryApiContentTypesSchemaFilter : ISchemaFilter
{
    private readonly IOptions<SwaggerGenOptions> _swaggerGenOptions;
    private readonly IContentTypeService _contentTypeService;
    private readonly IPublishedContentTypeFactory _publishedContentTypeFactory;
    private readonly ISchemaIdSelector _schemaIdSelector;
    private readonly IShortStringHelper _shortStringHelper;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DeliveryApiContentTypesSchemaFilter" /> class.
    /// </summary>
    public DeliveryApiContentTypesSchemaFilter(IOptions<SwaggerGenOptions> swaggerGenOptions, IContentTypeService contentTypeService, IPublishedContentTypeFactory publishedContentTypeFactory, ISchemaIdSelector schemaIdSelector, IShortStringHelper shortStringHelper)
    {
        _swaggerGenOptions = swaggerGenOptions;
        _contentTypeService = contentTypeService;
        _publishedContentTypeFactory = publishedContentTypeFactory;
        _schemaIdSelector = schemaIdSelector;
        _shortStringHelper = shortStringHelper;
    }

    /// <inheritdoc/>
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        FixEnums(schema, context);

        // Orval/Swashbuckle compatible, NOT supported by NSwag
        bool useOneOfForPolymorphism = _swaggerGenOptions.Value.SchemaGeneratorOptions.UseOneOfForPolymorphism;

        if (!useOneOfForPolymorphism && !_swaggerGenOptions.Value.SchemaGeneratorOptions.UseAllOfForInheritance)
        {
            return;
        }

        if (typeof(IApiElement) == context.Type)
        {
            HandleIApiElement(schema, context, useOneOfForPolymorphism);
            return;
        }

        if (typeof(IApiContent) == context.Type)
        {
            HandleIApiContent(schema, context, useOneOfForPolymorphism);
            return;
        }

        if (typeof(IApiContentResponse) == context.Type)
        {
            HandleIApiContentResponse(schema, context, useOneOfForPolymorphism);
            return;
        }
    }

    private void HandleIApiElement(OpenApiSchema schema, SchemaFilterContext context, bool useOneOfForPolymorphism)
    {
        OpenApiSchema? originalSchema = null;
        if (useOneOfForPolymorphism)
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

        foreach (IContentType contentType in _contentTypeService.GetAll())
        {
            IPublishedContentType publishedContentType = _publishedContentTypeFactory.CreateContentType(contentType);

            OpenApiSchema? contentTypeSchema = context.SchemaRepository.AddDefinition(
                $"{GetContentTypeSchemaId(contentType)}ElementModel",
                new OpenApiSchema
                {
                    Type = "object",
                    AllOf = { new OpenApiSchema { Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = GetTypeSchemaId<IApiElement>(useOneOfForPolymorphism) } } },
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

    private void HandleIApiContent(OpenApiSchema schema, SchemaFilterContext context, bool useOneOfForPolymorphism)
    {
        // Ensure IApiElement is generated if not already
        context.SchemaGenerator.GenerateSchema(typeof(IApiElement), context.SchemaRepository);

        OpenApiSchema? originalSchema = null;
        if (useOneOfForPolymorphism)
        {
            originalSchema = schema;
            schema = new OpenApiSchema(originalSchema);
            context.SchemaRepository.Schemas.Add(GetTypeSchemaId<IApiContent>(true), schema);

            ClearSchema(originalSchema);
        }

        schema.AllOf.Add(new OpenApiSchema
        {
            Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = GetTypeSchemaId<IApiElement>(useOneOfForPolymorphism) }
        });
        schema.Discriminator = new OpenApiDiscriminator
        {
            PropertyName = "contentType",
        };

        foreach (IContentType contentType in _contentTypeService.GetAll().Where(c => !c.IsElement))
        {
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
                                Id = GetTypeSchemaId<IApiContent>(useOneOfForPolymorphism),
                            },
                        },
                        new OpenApiSchema
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = $"{GetContentTypeSchemaId(contentType)}ElementModel" },
                        },
                    },
                }
            );

            schema.Discriminator.Mapping[contentType.Alias] = contentTypeSchema.Reference.ReferenceV3;
            originalSchema?.OneOf.Add(contentTypeSchema);
        }
    }

    private void HandleIApiContentResponse(OpenApiSchema schema, SchemaFilterContext context, bool useOneOfForPolymorphism)
    {
        // Ensure IApiContent is generated if not already
        context.SchemaGenerator.GenerateSchema(typeof(IApiContent), context.SchemaRepository);

        OpenApiSchema? originalSchema = null;
        if (useOneOfForPolymorphism)
        {
            originalSchema = schema;
            schema = new OpenApiSchema(originalSchema);
            context.SchemaRepository.Schemas.Add(GetTypeSchemaId<IApiContentResponse>(true), schema);

            ClearSchema(originalSchema);
        }

        schema.AllOf.Add(new OpenApiSchema
        {
            Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = GetTypeSchemaId<IApiContent>(useOneOfForPolymorphism) }
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
                                Id = GetTypeSchemaId<IApiContentResponse>(useOneOfForPolymorphism),
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

    // HACK: The DeliveryApi property value type is currently not exposed by Umbraco
    private static readonly FieldInfo? ConverterField = typeof(PublishedPropertyType).GetField("_converter", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
    private static Type GetPropertyType(IPublishedPropertyType publishedPropertyType)
    {
        Type modelClrType = publishedPropertyType.ModelClrType;

        return ConverterField?.GetValue(publishedPropertyType) switch
        {
            BlockListPropertyValueConverter =>
                // HACK: Needed due to umbraco returning the wrong type
                // https://github.com/umbraco/Umbraco-CMS/pull/14728
                typeof(ApiBlockListModel),
            IDeliveryApiPropertyValueConverter propertyValueConverter => propertyValueConverter
                .GetDeliveryApiPropertyValueType(publishedPropertyType),
            _ => modelClrType,
        };
    }

    // HACK: Needed because Umbraco generates invalid enum schemas
    // https://github.com/umbraco/Umbraco-CMS/pull/14727
    private static void FixEnums(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (!context.Type.IsEnum)
        {
            return;
        }

        schema.Type = "string";
        schema.Format = null;
    }
}
