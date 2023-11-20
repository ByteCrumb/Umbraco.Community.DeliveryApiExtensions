using Umbraco.Cms.Api.Common.OpenApi;
using Umbraco.Cms.Web.Common.ApplicationBuilder;

namespace UmbracoDeliveryApiExtensions.TestSite.Custom;

public static class UmbracoBuilderExtensions
{
    public static IUmbracoBuilder ConfigureSwagger(this IUmbracoBuilder builder)
    {
        builder.Services.Configure<UmbracoPipelineOptions>(options =>
        {
            options.PipelineFilters.RemoveAll(filter => filter is SwaggerRouteTemplatePipelineFilter);
            options.AddFilter(new AlwaysEnabledSwaggerPipelineFilter("MyApi"));
        });
        return builder;
    }
}
