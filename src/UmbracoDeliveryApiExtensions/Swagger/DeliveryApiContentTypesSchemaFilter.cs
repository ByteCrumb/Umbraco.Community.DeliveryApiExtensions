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
    private readonly IOptionsMonitor<TypedSwaggerOptions> _typedSwaggerOptions;
    private readonly IContentTypeInfoService _contentTypeInfoService;
    private readonly ISchemaIdSelector _schemaIdSelector;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DeliveryApiContentTypesSchemaFilter" /> class.
    /// </summary>
    public DeliveryApiContentTypesSchemaFilter(IOptionsMonitor<TypedSwaggerOptions> typedSwaggerOptions, IContentTypeInfoService contentTypeInfoService, ISchemaIdSelector schemaIdSelector)
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

        ICollection<ContentTypeInfo> contentTypes = _contentTypeInfoService.GetContentTypes();

        if (typeof(IApiElement) == context.Type)
        {
            HandleIApiElement(schema, context, contentTypes, settings);
            return;
        }

        if (typeof(IApiContent) == context.Type)
        {
            HandleIApiContent(schema, context, contentTypes, settings);
            return;
        }

        if (typeof(IApiContentResponse) == context.Type)
        {
            HandleIApiContentResponse(schema, context, contentTypes, settings);
            return;
        }
    }

    private void HandleIApiElement(OpenApiSchema schema, SchemaFilterContext context, IEnumerable<ContentTypeInfo> contentTypes, SwaggerGenerationSettings settings)
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

        foreach (ContentTypeInfo contentType in contentTypes.Where(c => c.IsElement))
        {

            OpenApiSchema? contentTypeSchema = context.SchemaRepository.AddDefinition(
                $"{contentType.SchemaId}ElementModel",
                new OpenApiSchema
                {
                    Type = "object",
                    AllOf = { new OpenApiSchema { Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = GetTypeSchemaId<IApiElement>(settings.UseOneOf) } } },
                    AdditionalPropertiesAllowed = false,
                    Properties =
                    {
                        ["properties"] = context.SchemaRepository.AddDefinition(
                            $"{contentType.SchemaId}PropertiesModel",
                            new OpenApiSchema
                            {
                                Type = "object",
                                AdditionalPropertiesAllowed = true,
                                AllOf = contentType.CompositionSchemaIds.Select(c => new OpenApiSchema { Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = $"{c}PropertiesModel" } }).ToList(),
                                Properties = contentType.Properties
                                    .Where(p => !p.Inherited) // Filter out composition properties
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
                        ),
                    },
                }
            );

            schema.Discriminator.Mapping[contentType.Alias] = contentTypeSchema.Reference.ReferenceV3;
            originalSchema?.OneOf.Add(contentTypeSchema);
        }
    }

    private void HandleIApiContent(OpenApiSchema schema, SchemaFilterContext context, IEnumerable<ContentTypeInfo> contentTypes, SwaggerGenerationSettings settings)
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

        foreach (ContentTypeInfo contentType in contentTypes.Where(c => !c.IsElement))
        {
            OpenApiSchema? contentTypeSchema = context.SchemaRepository.AddDefinition(
                $"{contentType.SchemaId}ContentModel",
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
                            $"{contentType.SchemaId}PropertiesModel",
                            new OpenApiSchema
                            {
                                Type = "object",
                                AdditionalPropertiesAllowed = true,
                                AllOf = contentType.CompositionSchemaIds.Select(c => new OpenApiSchema { Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = $"{c}PropertiesModel" } }).ToList(),
                                Properties = contentType.Properties
                                    .Where(p => !p.Inherited) // Filter out composition properties
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
                        ),
                    },
                }
            );

            schema.Discriminator.Mapping[contentType.Alias] = contentTypeSchema.Reference.ReferenceV3;
            originalSchema?.OneOf.Add(contentTypeSchema);
        }
    }

    private void HandleIApiContentResponse(OpenApiSchema schema, SchemaFilterContext context, IEnumerable<ContentTypeInfo> contentTypes, SwaggerGenerationSettings settings)
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

        foreach (ContentTypeInfo contentType in contentTypes.Where(c => !c.IsElement))
        {
            OpenApiSchema? contentTypeSchema = context.SchemaRepository.AddDefinition(
                $"{contentType.SchemaId}ContentResponseModel",
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
                            Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = $"{contentType.SchemaId}ContentModel" },
                        },
                    },
                }
            );

            schema.Discriminator.Mapping[contentType.Alias] = contentTypeSchema.Reference.ReferenceV3;
            originalSchema?.OneOf.Add(contentTypeSchema);
        }
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
