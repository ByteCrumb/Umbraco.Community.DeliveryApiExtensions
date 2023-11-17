using System.Text.Json;
using nswag;

IUmbracoApi umbracoApi = new UmbracoApi("https://localhost:44363", new HttpClient());

IApiContentResponseModel page = await umbracoApi.GetContentItemByPathAsync("/", expand: "all");
RenderPage(page);

Console.ReadLine();

void RenderPage(IApiContentResponseModel content)
{
    Console.WriteLine($"  Name: {content.Name}");
    Console.WriteLine($"  Path: {content.Route?.Path}");

    if (content is TestPageContentResponseModel testPage)
    {
        RenderTestPage(testPage);
    }
}

void RenderTestPage(TestPageContentResponseModel content)
{
    TestPagePropertiesModel? properties = content.Properties;

    Console.WriteLine("\n  **Common**");
    Print("textString", properties?.TextString);
    Print("textArea", properties?.TextArea);
    Print("datePickerWithTime", properties?.DatePickerWithTime);
    Print("datePicker", properties?.DatePicker);
    Print("toggle", properties?.Toggle);
    Print("numeric", properties?.Numeric);
    Print("decimal", properties?.Decimal);
    Print("slider", properties?.Slider);
    Print("tags", properties?.Tags);
    Print("email", properties?.Email);

    Console.WriteLine("\n  **Pickers**");
    Print("colorPicker", properties?.ColorPicker);
    Print("contentPicker", "<tested below>");
    Print("eyeDropperColorPicker", properties?.EyeDropperColorPicker);
    Print("urlPicker", properties?.UrlPicker);
    Print("multinodeTreepicker", "<tested below>");

    Console.WriteLine("\n  **Rich content**");
    Print("richText", properties?.RichText);
    Print("blockGrid", "<tested below>");
    Print("markdown", properties?.Markdown);

    Console.WriteLine("\n  **People**");
    Print("memberGroupPicker", properties?.MemberGroupPicker);
    Print("memberPicker", properties?.MemberPicker);
    Print("userPicker", properties?.UserPicker);

    Console.WriteLine("\n  **Lists**");
    Print("blockList", "<tested below>");
    Print("checkboxList", properties?.CheckboxList);
    Print("dropdown", properties?.Dropdown);
    Print("radiobox", properties?.Radiobox);
    Print("repeatableTextstrings", properties?.RepeatableTextstrings);

    Console.WriteLine("\n  **Media**");
    Print("uploadFile", properties?.UploadFile);
    Print("imageCropper", properties?.ImageCropper);
    Print("mediaPicker", properties?.MediaPicker);

    Console.WriteLine("\n  **Content Picker**");
    Print("name", properties?.ContentPicker?.Name);
    Print("route>path", properties?.ContentPicker?.Route?.Path);
    Print("properties>textString", (properties?.ContentPicker as TestPageContentModel)?.Properties?.TextString);

    Console.WriteLine("\n  **Multinode Treepicker**");
    Print("name", properties?.MultinodeTreepicker?.FirstOrDefault()?.Name);
    Print("route>path", properties?.MultinodeTreepicker?.FirstOrDefault()?.Route?.Path);
    Print("properties>textString", (properties?.MultinodeTreepicker?.FirstOrDefault() as TestPageContentModel)?.Properties?.TextString);

    Console.WriteLine("\n  **Block List**");

    foreach ((ApiBlockItemModel block, int i) in content.Properties?.BlockList?.Items?.Select((b, i) => (b, i)) ?? Enumerable.Empty<(ApiBlockItemModel, int)>())
    {
        Console.WriteLine($"    Block[{i}]:");
        RenderBlock(block);
    }

    Console.WriteLine("\n  **Block Grid**");
    foreach ((ApiBlockGridItemModel block, int i) in content.Properties?.BlockGrid?.Items?.Select((b, i) => (b, i)) ?? Enumerable.Empty<(ApiBlockGridItemModel, int)>())
    {
        Console.WriteLine($"    Block[{i}]:");
        RenderBlock(block);
    }

    Console.WriteLine("\n  **From composition(s)**");
    Print("  sharedToggle", properties?.SharedToggle);
    Print("  sharedString", properties?.SharedString);
    Print("  sharedRadiobox", properties?.SharedRadiobox);
    Print("  sharedRichText", properties?.SharedRichText);
}

void RenderBlock(ApiBlockItemModel block)
{
    Console.WriteLine($"      Type: {block.Content?.GetType().Name}");
    switch (block.Content)
    {
        case TestBlockElementModel testBlock:
        {
            Console.WriteLine($"      String: {testBlock.Properties?.String}");
            Console.WriteLine($"      Multinode Treepicker: {testBlock.Properties?.MultinodeTreepicker?.FirstOrDefault()?.Id}");
            Console.WriteLine($"      Shared string: {testBlock.Properties?.SharedString}");
            ApiBlockItemModel? nestedBlock = testBlock.Properties?.Blocks?.Items?.FirstOrDefault();
            if (nestedBlock is not null)
            {
                Console.WriteLine("      **Nested block**");
                RenderBlock(nestedBlock);
            }

            break;
        }

        case TestBlock2ElementModel testBlock2:
        {
            Console.WriteLine($"      Shared string (testBlock2): {testBlock2.Properties?.SharedString}");
            if (block.Settings is BlockSettingsElementModel settings)
            {
                Console.WriteLine($"      Block id (settings): {settings.Properties?.AnchorId}");
            }

            break;
        }

        default:
            Console.WriteLine("      Unknown block type	");
            break;
    }
}

void Print(string propertyName, object? value) =>
    Console.WriteLine($"    {propertyName} ({value?.GetType().Name}): {JsonSerializer.Serialize(value)}");
