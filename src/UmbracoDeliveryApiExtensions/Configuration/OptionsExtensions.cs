using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Umbraco.Extensions;

namespace Umbraco.Community.DeliveryApiExtensions.Configuration;

internal static class OptionsExtensions
{
    /// <summary>
    /// Adds and binds the provided <typeparamref name="TOptions"/> class to the provided section.
    /// </summary>
    public static OptionsBuilder<TOptions> AddOptions<TOptions>(this IServiceCollection services, IConfigurationSection configurationSection)
        where TOptions : class
    {
        return services.AddOptions<TOptions>().Bind(configurationSection).ValidateDataAnnotations();
    }

    /// <summary>
    /// Gets the default configuration section for the provided <typeparamref name="TOptions"/>.
    /// </summary>
    public static IConfigurationSection GetSection<TOptions>(this IConfiguration configuration)
    {
        return configuration.GetSection(typeof(TOptions).Name.TrimEnd("Options"));
    }
}
