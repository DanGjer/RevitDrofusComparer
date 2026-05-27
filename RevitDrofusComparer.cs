namespace RevitDrofusComparer;

public class RevitDrofusComparerCommand : IRevitExtension<AssistantArgs>
{
    public IExtensionResult Run(IRevitExtensionContext context, AssistantArgs args, CancellationToken cancellationToken)
    {
        var document = context.UIApplication.ActiveUIDocument?.Document;

        if (document is null)
            return Result.Text.Failed("Revit has no active model open");

        var client = new dRofusClientFactory().Create(document);

        var queryFilter = Filter.And(Filter.Eq("article_id_responsibility_responsibility", "RIE"));

        var drofusQuery = Query.List()
                .Select("Id", "article_id_name", "classification_number", "occurrence_data_17_11_11_10", "ifc_guids_text_property")
                .Filter(queryFilter);

        var queryResult = client.GetOccurrences(drofusQuery);
        var json = System.Text.Json.JsonSerializer.Serialize(queryResult);
        var drofusElements = System.Text.Json.JsonSerializer.Deserialize<List<DrofusHelper.DrofusElement>>(json) ?? new List<DrofusHelper.DrofusElement>();

        var revitElements = RevitHelper.GetRevitElements(document, args);
        var revitOccurrenceIds = revitElements.Select(e => e.OccurrenceId).Where(id => id > 0).ToHashSet();

        var check1ResultIds = Workhorse.Check1(revitOccurrenceIds, drofusElements);
        var check2ResultIds = Workhorse.Check2(revitOccurrenceIds, drofusElements, document);

        return Result.Text.Succeeded("RevitDrofusComparer framework is ready.");
    }
}