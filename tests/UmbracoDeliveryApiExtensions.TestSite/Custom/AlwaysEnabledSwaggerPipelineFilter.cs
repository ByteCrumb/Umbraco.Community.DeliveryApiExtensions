using Umbraco.Cms.Api.Common.OpenApi;

namespace UmbracoDeliveryApiExtensions.TestSite.Custom;

public class AlwaysEnabledSwaggerPipelineFilter : SwaggerRouteTemplatePipelineFilter
{
    public AlwaysEnabledSwaggerPipelineFilter(string name) : base(name) { }

    protected override bool SwaggerIsEnabled(IApplicationBuilder applicationBuilder)
    {
        return true;
    }
}
