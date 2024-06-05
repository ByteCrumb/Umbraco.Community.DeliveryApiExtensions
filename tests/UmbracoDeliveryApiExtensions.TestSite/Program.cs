using Umbraco.Cms.Api.Common.OpenApi;
using Umbraco.Cms.Web.Common.ApplicationBuilder;
using UmbracoDeliveryApiExtensions.TestSite.Custom;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddDeliveryApi()
    .AddComposers()
    .Build();

// Always enable swagger (also in Production, which is the environment used by the tests)
builder.Services.Configure<UmbracoPipelineOptions>(options =>
{
    options.PipelineFilters.RemoveAll(filter => filter is SwaggerRouteTemplatePipelineFilter);
    options.AddFilter(new AlwaysEnabledSwaggerPipelineFilter("UmbracoApiCommon"));
});

WebApplication app = builder.Build();

await app.BootUmbracoAsync();

app.UseUmbraco()
    .WithMiddleware(u =>
    {
        u.UseBackOffice();
        u.UseWebsite();
    })
    .WithEndpoints(u =>
    {
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });

await app.RunAsync();
