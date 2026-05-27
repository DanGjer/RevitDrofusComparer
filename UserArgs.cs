namespace RevitDrofusComparer;

public class AssistantArgs
{
    [Description("Select categories to include"), ControlData(ToolTip = "")]
    [ControlType(ControlType.ListBox), ControlSettings("CompactMode", "true")]
    [RevitAutoFill(RevitAutoFillSource.Categories)]
    public List<string>? RevitCategories { get; set; }


}