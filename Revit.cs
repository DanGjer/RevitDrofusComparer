using Microsoft.VisualBasic;

namespace RevitDrofusComparer;

public static class RevitHelper
{
    public static List<RevitElement> GetRevitElements(Document document, AssistantArgs AssistantArgs)
    {
        var revitInstances = new List<RevitElement>();
        var categoryIds = AssistantArgs.RevitCategories.Select(idString => new ElementId(long.Parse(idString))).ToList();
        var collector = new FilteredElementCollector(document).WhereElementIsNotElementType().WherePasses(new ElementMulticategoryFilter(categoryIds)).ToElementIds();
        
            foreach (var elemId in collector)
            {
                using var element = document.GetElement(elemId);
                var occurrenceId = 0;
                var idParam = element.LookupParameter("drofus_occurrence_id");
                if(idParam != null)
                    {
                    occurrenceId = idParam.StorageType switch
                    {
                        StorageType.Integer => idParam.AsInteger(),
                        StorageType.String => int.TryParse(idParam.AsString(), out var parsed) ? parsed : 0,
                        _ => 0
                    };
                    }
                revitInstances.Add(new RevitElement
                {
                    ElementId = elemId.IntegerValue,
                    Guid = element.LookupParameter("IfcGUID")?.AsString() ?? "",
                    OccurrenceId = occurrenceId,
                    Tag = element.LookupParameter("FOB_Merkestreng")?.AsString() ?? ""
                });
            }
            return revitInstances;
    }

    public class RevitElement
    {
        public int ElementId { get; set; } = 0;
        public string Guid { get; set; } = "";
        public int OccurrenceId { get; set; } = 0;
        public string Tag {get; set;} = "";
    }
}