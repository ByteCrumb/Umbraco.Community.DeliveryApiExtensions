using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;
using Umbraco.Community.DeliveryApiExtensions.Models;
using Umbraco.Extensions;

namespace Umbraco.Community.DeliveryApiExtensions.Services;

/// <summary>
/// Service responsible for providing content type information.
/// </summary>
/// <remarks>
/// Used for typed swagger schema generation.
/// </remarks>
public interface IContentTypeInfoService
{
    /// <summary>
    /// Gets all the available content types.
    /// </summary>
    ICollection<ContentTypeInfo> GetContentTypes();
}

internal sealed class ContentTypeInfoService : IContentTypeInfoService
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
        List<ContentTypeInfo> result = [];
        HashSet<string> compositionAliases = [];

        foreach (IContentType contentType in _contentTypeService.GetAll())
        {
            HashSet<string> ownPropertyAliases = contentType.PropertyTypes.Select(p => p.Alias).ToHashSet();
            IPublishedContentType publishedContentType = _publishedContentTypeFactory.CreateContentType(contentType);

            result.Add(new ContentTypeInfo
            {
                Alias = contentType.Alias,
                SchemaId = GetContentTypeSchemaId(contentType),
                CompositionSchemaIds = contentType.ContentTypeComposition.Select(GetContentTypeSchemaId).ToList(),
                Properties = publishedContentType.PropertyTypes.Select(p => new ContentTypePropertyInfo { Alias = p.Alias, Type = p.DeliveryApiModelClrType, Inherited = !ownPropertyAliases.Contains(p.Alias) }).ToList(),
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
}
