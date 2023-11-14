namespace Umbraco.Community.DeliveryApiExtensions.Configuration.Options;

/// <summary>
///     The swagger generation mode to use.
/// </summary>
public enum SwaggerGenerationMode
{
    /// <summary>
    ///     Swagger will be generated with the 'UseOneOfForPolymorphism' and 'UseAllOfForInheritance' options.
    ///     <para>Suitable for most generators like <see href="https://openapi-ts.pages.dev">openapi-typescript</see> and <see href="https://orval.dev">orval</see>.</para>
    /// </summary>
    Auto,

    /// <summary>
    ///     Swagger will be generated with only the 'UseAllOfForInheritance' option enabled.
    ///     <para>Suitable for generators that don't support polymorphism using OneOf like <see href="https://github.com/RicoSuter/NSwag">NSwag</see>.</para>
    /// </summary>
    Compatibility,

    /// <summary>
    ///     <para>Swagger options will not be configured, allowing full customization.</para>
    ///     It can be configured from your codebase using:
    ///     <code>
    ///     services.Configure&lt;SwaggerGenOptions&gt;(options =&gt; ...)
    ///     services.Configure&lt;TypedSwaggerOptions&gt;(options =&gt; ...)
    ///     </code>
    /// </summary>
    Manual,
}
