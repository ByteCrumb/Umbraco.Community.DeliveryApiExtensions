namespace Umbraco.Community.DeliveryApiExtensions.Configuration.Options;

/// <summary>
/// TypedSwagger options
/// </summary>
public class TypedSwaggerOptions
{
    /// <summary>
    /// Whether the typed swagger is enabled
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// The polymorphism mode to use.
    /// </summary>
    /// <remarks>
    /// If set to 'Auto', the swagger will be generated with the UseOneOfForPolymorphism and UseAllOfForInheritance options.
    /// To configure it yourself, set this to 'Manual' and call the following from your custom code: <code>services.Configure&lt;SwaggerGenOptions&gt;(options => ...)</code>
    /// </remarks>
    public PolymorphismMode PolymorphismMode { get; set; } = PolymorphismMode.Auto;
}

/// <summary>
/// The polymorphism mode to use.
/// </summary>
/// <remarks>
/// If set to 'Auto', the swagger will be generated with the 'UseOneOfForPolymorphism' and 'UseAllOfForInheritance' options.
/// Not all client generators work best with these options, so if you need a different configuration, set this to 'Manual' and call the following from your custom code: <code>services.Configure&lt;SwaggerGenOptions&gt;(options => ...)</code>
/// </remarks>
public enum PolymorphismMode
{
    Auto,
    Manual
}
