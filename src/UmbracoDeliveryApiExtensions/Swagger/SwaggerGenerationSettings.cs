namespace Umbraco.Community.DeliveryApiExtensions.Swagger;

/// <summary>
/// Swagger generation settings to be used by <see cref="DeliveryApiContentTypesSchemaFilter"/>.
/// </summary>
public class SwaggerGenerationSettings
{
    /// <summary>
    /// Controls whether to use the OneOf extension for polymorphism.
    /// </summary>
    public bool UseOneOf { get; set; }

    /// <summary>
    /// Controls whether to use the AllOf extension for polymorphism and/or inheritance.
    /// </summary>
    public bool UseAllOf { get; set; }
}
