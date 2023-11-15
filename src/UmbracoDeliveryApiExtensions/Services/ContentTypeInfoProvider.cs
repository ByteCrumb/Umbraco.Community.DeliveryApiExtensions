using System.Reflection;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors.DeliveryApi;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;
using Umbraco.Community.DeliveryApiExtensions.Models;
using Umbraco.Extensions;

namespace Umbraco.Community.DeliveryApiExtensions.Services;

public interface IContentTypeInfoService
{
    ICollection<ContentTypeInfo> GetContentTypes();
}

internal class ContentTypeInfoService : IContentTypeInfoService
{
    private readonly IContentTypeService _contentTypeService;
    private readonly IPublishedContentTypeFactory _publishedContentTypeFactory;
    private readonly IShortStringHelper _shortStringHelper;

    public ContentTypeInfoService(IContentTypeService contentTypeService, IPublishedContentTypeFactory publishedContentTypeFactory, IShortStringHelper shortStringHelper)
    {
        _contentTypeService = contentTypeService;
        _publishedContentTypeFactory = publishedContentTypeFactory;
        _shortStringHelper = shortStringHelper;
    }

    public ICollection<ContentTypeInfo> GetContentTypes()
    {
        List<ContentTypeInfo> result = new();
        HashSet<string> compositionAliases = new();

        foreach (IContentType contentType in _contentTypeService.GetAll())
        {
            HashSet<string> ownPropertyAliases = contentType.PropertyTypes.Select(p => p.Alias).ToHashSet();
            IPublishedContentType publishedContentType = _publishedContentTypeFactory.CreateContentType(contentType);

            result.Add(new ContentTypeInfo
            {
                Alias = contentType.Alias,
                SchemaId = GetContentTypeSchemaId(contentType),
                CompositionSchemaIds = contentType.ContentTypeComposition.Select(GetContentTypeSchemaId).ToList(),
                Properties = publishedContentType.PropertyTypes.Select(p => new ContentTypePropertyInfo { Alias = p.Alias, Type = GetPropertyType(p), Inherited = !ownPropertyAliases.Contains(p.Alias) }).ToList(),
                IsElement = contentType.IsElement,
                IsComposition = false,
            });

            compositionAliases.UnionWith(contentType.CompositionAliases());
        }

        result.ForEach(c => c.IsComposition = compositionAliases.Contains(c.Alias));

        return result;
    }

    private string GetContentTypeSchemaId(IContentTypeBase contentType)
    {
        // This is what ModelsBuilder currently also uses
        return contentType.Alias.ToCleanString(_shortStringHelper, CleanStringType.ConvertCase | CleanStringType.PascalCase);
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
