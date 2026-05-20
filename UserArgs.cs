namespace RevitDrofusComparer;

public class AssistantArgs
{

    [Description("Select a parameter to include"), ControlData(ToolTip = "")]
    [RevitAutoFill(RevitAutoFillSource.SharedParameters)]
    public string? RevitParam1 { get; set; }

    [Description("Select a parameter to include"), ControlData(ToolTip = "")]
    [RevitAutoFill(RevitAutoFillSource.SharedParameters)]
    public string? RevitParam2 { get; set; }
}