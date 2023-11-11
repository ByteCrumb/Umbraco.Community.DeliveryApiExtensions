using CommandLine;

namespace UmbracoDeliveryApiExtensions.JsonSchemaGenerator;
internal sealed class CommandLineArguments
{
    [Option('i', "inputFile", Required = true)]
    public required string InputDll { get; set; }

    [Option('t', "targetType", Required = true)]
    public required string TargetType { get; set; }

    [Option('o', "outputFile", Required = true)]
    public required string OutputFile { get; set; }

    [Option('n', "packageName", Required = false)]
    public string PackageName { get; set; } = "DeliveryApiExtensions";
}
