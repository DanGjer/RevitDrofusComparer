namespace RevitDrofusComparer;

public class AssistantArgs
{
    [Description("Select categories to include"), ControlData(ToolTip = "")]
    [ControlType(ControlType.ListBox), ControlSettings("CompactMode", "true")]
    [RevitAutoFill(RevitAutoFillSource.Categories)]
    public List<string>? RevitCategories { get; set; }

    [Description("Velg hvor rapport skal lagres"), ControlData(ToolTip ="")]
    [ControlType(ControlType.Browse), ControlSettings("SelectFolder", "true")]
    public string? UserFilePath {get; set;}


}