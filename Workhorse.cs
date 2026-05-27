namespace RevitDrofusComparer;

public static class Workhorse
{
    public static HashSet<int> Check1 (HashSet<int> revitOccurrenceIds, List<DrofusHelper.DrofusElement> drofusElements)
    {
        HashSet<int> elementsNotInDrofus = [];
        var drofusIds = drofusElements.Select(x => x.DrofusId).ToHashSet();
        foreach (int instance in revitOccurrenceIds)
        {
            if (!drofusIds.Contains(instance))
            {
                elementsNotInDrofus.Add(instance);
            }
        }

        return elementsNotInDrofus;
    }

    public static HashSet<int> Check2 (HashSet<int> revitOccurrenceIds, List<DrofusHelper.DrofusElement> drofusElements, Document document)
    {
        string? modname = document.ProjectInformation.LookupParameter("model_name_drofus")?.AsString();
        HashSet<int> elementsNotInRevit = [];
        var drofusIds = drofusElements.Where(x => x.DrofusModelName == modname.ToString()).Select(x => x.DrofusId).ToHashSet();

        foreach (int occurrence in drofusIds)
        {
            if (!revitOccurrenceIds.Contains(occurrence))
            {
                elementsNotInRevit.Add(occurrence);
            }
        }

        return elementsNotInRevit;
    }

    public static void Check1Lookup (List<int> revitIds, List<RevitHelper.RevitElement> revitElements)
    {
        
    }
    
}