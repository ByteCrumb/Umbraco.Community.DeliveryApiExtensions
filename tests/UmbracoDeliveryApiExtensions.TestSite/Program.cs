using UmbracoDeliveryApiExtensions.TestSite;

Host.CreateDefaultBuilder(args)
    .ConfigureUmbracoDefaults()
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStaticWebAssets();
        webBuilder.UseStartup<Startup>();
    })
    .Build()
    .Run();
