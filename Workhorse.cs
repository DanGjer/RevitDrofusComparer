using System.Windows.Controls.Primitives;

namespace RevitDrofusComparer;

public static class Workhorse
{
    public static void Check1 (List<DrofusHelper.DrofusElement> allDrofusElements, List<RevitHelper.RevitElement>allRevitElements)
    {
        var drofusIds = allDrofusElements.Select(x => x.DrofusId).ToHashSet();
        foreach (var element in allRevitElements)
        {
            if (!drofusIds.Contains(element.OccurrenceId))
            {
                element.Status = RevitHelper.RevitElement.RevitStatus.MissingInDrofus;
            }
            else
            {
                element.Status = RevitHelper.RevitElement.RevitStatus.OK;
            }
        }
    }

    public static void Check2 (List<DrofusHelper.DrofusElement> allDrofusElements, List<RevitHelper.RevitElement> allRevitElements, Document document)
    {
        string? modname = document.ProjectInformation.LookupParameter("model_name_drofus")?.AsString();
        var revitIds = allRevitElements.Select(x => x.OccurrenceId).ToHashSet();

        foreach (var occurrence in allDrofusElements)
        {
            if (occurrence.DrofusModelName == modname)
            {
                if (revitIds.Contains(occurrence.DrofusId))
                {
                    occurrence.Status = DrofusHelper.DrofusElement.DrofusStatus.OK;
                }
                else
                {
                    occurrence.Status = DrofusHelper.DrofusElement.DrofusStatus.MissingInRevit;
                }
            }
        }
    }
    
}