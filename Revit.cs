namespace RevitDrofusComparer;

public static class RevitHelper
{
    public static List<RevitElement> GetRevitElements(Document document, AssistantArgs AssistantArgs)
    {
         var revitInstances = new List<RevitElement>();
         var collector = new FilteredElementCollector(document);
         var elements = collector.WhereElementIsNotElementType().ToElements();
            foreach (var element in elements)
            {
                element.LookupParameter(AssistantArgs.RevitParam1)?.AsString();
                revitInstances.Add(new RevitElement
                {
                    ElementId = element.Id.IntegerValue,
                    Guid = element.LookupParameter("IfcGUID")?.AsString() ?? "",
                    ParameterValue = element.LookupParameter(AssistantArgs.RevitParam1)?.AsString() ?? "",
                    ParameterValue2 = element.LookupParameter(AssistantArgs.RevitParam2)?.AsString() ?? ""
                });
            }
            return revitInstances;
    }

    public class RevitElement
    {
        public int ElementId { get; set; } = 0;
        public string Guid { get; set; } = "";
        public string ParameterValue { get; set; } = "";
        public string ParameterValue2 { get; set; } = "";
    }
}