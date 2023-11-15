using System.Text.Json.Serialization;
using Umbraco.Community.DeliveryApiExtensions.Swagger;

namespace Umbraco.Community.DeliveryApiExtensions.Configuration.Options;

/// <summary>
/// Typed swagger options
/// </summary>
public class TypedSwaggerOptions
{
    /// <summary>
    /// Default <see cref="SettingsFactory"/> implementation based on the configured <see cref="Mode"/>.
    /// </summary>
    [JsonIgnore]
    public static readonly Func<SwaggerGenerationMode, SwaggerGenerationSettings> DefaultSettingsFactory = mode => new SwaggerGenerationSettings
    {
        UseOneOf = mode == SwaggerGenerationMode.Auto,
        UseAllOf = mode is SwaggerGenerationMode.Auto or SwaggerGenerationMode.Compatibility,
    };

    /// <summary>
    ///     Initializes a new instance of the <see cref="TypedSwaggerOptions" /> class.
    /// </summary>
    public TypedSwaggerOptions()
    {
        SettingsFactory = () => DefaultSettingsFactory(Mode);
    }

    /// <summary>
    /// Whether the typed swagger feature is enabled
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// The swagger generation mode to use.
    /// Defaults to 'Auto'.
    /// </summary>
    public SwaggerGenerationMode Mode { get; set; } = SwaggerGenerationMode.Auto;

    /// <summary>
    /// The factory for the settings to be used for swagger generation.
    /// Defaults to <see cref="DefaultSettingsFactory"/>.
    /// </summary>
    [JsonIgnore]
    public Func<SwaggerGenerationSettings> SettingsFactory { get; set; }
}
