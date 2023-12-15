using Umbraco.Cms.Api.Common.OpenApi;
using Umbraco.Cms.Web.Common.ApplicationBuilder;
using Umbraco.Community.DeliveryApiExtensions.Configuration.Options;
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

// Allow overriding the swagger generation mode using a query string parameter.
builder.Services.AddOptions<TypedSwaggerOptions>().Configure<IHttpContextAccessor>(
    (options, httpContextAccessor) =>
    {
        options.SettingsFactory = () =>
            Enum.TryParse(httpContextAccessor.HttpContext?.Request.Query["mode"], ignoreCase: true, out SwaggerGenerationMode mode)
                ? TypedSwaggerOptions.DefaultSettingsFactory(mode)
                : TypedSwaggerOptions.DefaultSettingsFactory(options.Mode);
    }
);

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
        u.UseInstallerEndpoints();
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });

await app.RunAsync();
