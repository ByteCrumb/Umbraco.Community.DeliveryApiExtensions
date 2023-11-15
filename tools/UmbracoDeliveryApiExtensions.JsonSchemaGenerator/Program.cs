using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using CommandLine;
using NJsonSchema;
using NJsonSchema.Generation;
using UmbracoDeliveryApiExtensions.JsonSchemaGenerator;

CommandLineArguments arguments = Parser.Default.ParseArguments<CommandLineArguments>(args).Value;

JsonSchemaGenerator schemaGenerator = new(
    new SystemTextJsonSchemaGeneratorSettings
    {
        SchemaNameGenerator = new PrefixedSchemaNameGenerator(arguments.PackageName),
        SerializerOptions = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
        },
        IgnoreObsoleteProperties = true,
        AllowReferencesWithProperties = true,
    });

Assembly assembly = Assembly.LoadFrom(arguments.InputDll);
Type type = assembly.GetType(arguments.TargetType) ?? throw new InvalidOperationException($"Type {arguments.TargetType} couldn't be found.");

JsonSchema typeSchema = schemaGenerator.Generate(type);
JsonSchema wrapperSchema = new()
{
    Title = $"{arguments.PackageName}Schema",
    Type = JsonObjectType.Object,
    Properties =
    {
        [arguments.PackageName] = new JsonSchemaProperty
        {
            Type = JsonObjectType.Object,
            Reference = typeSchema,
        },
    },
    Definitions =
    {
        [typeSchema.Title!] = typeSchema,
    },
};

await File.WriteAllTextAsync(arguments.OutputFile, wrapperSchema.ToJson());
