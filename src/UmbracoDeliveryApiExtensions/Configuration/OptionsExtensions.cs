using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Umbraco.Extensions;

namespace Umbraco.Community.DeliveryApiExtensions.Configuration;

internal static class OptionsExtensions
{
    /// <summary>
    /// Adds, binds and validates the provided <see cref="TOptions"/> class using the default section name.
    /// </summary>
    public static OptionsBuilder<TOptions> AddOptions<TOptions>(this IServiceCollection services, IConfiguration configuration)
        where TOptions : class =>
        services.AddOptions<TOptions>().Bind(configuration.GetSection<TOptions>()).ValidateDataAnnotations().ValidateOnStart();

    /// <summary>
    /// Gets the default configuration section for the provided <see cref="TOptions"/>.
    /// </summary>
    public static IConfigurationSection GetSection<TOptions>(this IConfiguration configuration)
        => configuration.GetSection(typeof(TOptions).Name.TrimEnd("Options"));
}
