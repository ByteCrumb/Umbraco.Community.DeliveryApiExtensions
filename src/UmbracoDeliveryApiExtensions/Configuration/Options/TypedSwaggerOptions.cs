namespace Umbraco.Community.DeliveryApiExtensions.Configuration.Options;

/// <summary>
/// Typed swagger options
/// </summary>
public class TypedSwaggerOptions
{
    /// <summary>
    /// Whether the typed swagger feature is enabled
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// The generation mode to use.
    /// </summary>
    /// <remarks>
    /// If set to 'Auto', the swagger will be generated with the 'UseOneOfForPolymorphism' and 'UseAllOfForInheritance' options.
    /// To configure it yourself, set this to 'Manual' and call the following from your custom code: <code>services.Configure&lt;SwaggerGenOptions&gt;(options => ...)</code>
    /// </remarks>
    public SwaggerGenerationMode Mode { get; set; } = SwaggerGenerationMode.Auto;
}
