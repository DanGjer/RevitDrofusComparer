namespace RevitDrofusComparer;

public class RevitDrofusComparerCommand : IRevitExtension<AssistantArgs>
{
    public IExtensionResult Run(IRevitExtensionContext context, AssistantArgs args, CancellationToken cancellationToken)
    {
        var document = context.UIApplication.ActiveUIDocument?.Document;

        if (document is null)
            return Result.Text.Failed("Revit has no active model open");
        
        if (string.IsNullOrWhiteSpace(args.UserFilePath))
            return Result.Text.Failed("No file path specified");

        var client = new dRofusClientFactory().Create(document);

        var queryFilter = Filter.In("article_id_responsibility_responsibility","RIE","AUT");

        var drofusQuery = Query.List()
                .Select("Id", "article_id_name", "classification_number", "occurrence_data_17_11_11_10", "ifc_guids_text_property", "occurrence_classification_156_classification_entry_id_code", "is_sub_occurrence", "parent_occurrence_id_id", "parent_occurrence_id_article_id_name")
                .Filter(queryFilter);

        var queryResult = client.GetOccurrences(drofusQuery);
        var json = System.Text.Json.JsonSerializer.Serialize(queryResult);
        var drofusElements = System.Text.Json.JsonSerializer.Deserialize<List<DrofusHelper.DrofusElement>>(json) ?? new List<DrofusHelper.DrofusElement>();

        var revitElements = RevitHelper.GetRevitElements(document, args);
        Workhorse.Check1(drofusElements, revitElements);
        Workhorse.Check2(drofusElements, revitElements, document);

        var revitElemsMissingInDrofus = revitElements.Where(x => x.Status == RevitHelper.RevitElement.RevitStatus.MissingInDrofus);
        var drofusOccurrencesMissingInRevit = drofusElements.Where(x => x.Status == DrofusHelper.DrofusElement.DrofusStatus.MissingInRevit);
        

        var modNameParam = document.ProjectInformation.LookupParameter("model_name_drofus");
        string modName = modNameParam?.AsString() ?? "";
        var revitMissing = revitElemsMissingInDrofus.ToList();
        var drofusMissing = drofusOccurrencesMissingInRevit.ToList();

        string fileName = $"RevitDrofusComparer_{modName}.xlsx";
        var reportPath = System.IO.Path.Combine(args.UserFilePath, fileName);

        ExportReportXlsx(reportPath, revitMissing, drofusMissing);

        return Result.Text.Succeeded("RevitDrofusComparer framework is ready.");
    }

    private static void ExportReportXlsx(
        string path,
        List<RevitHelper.RevitElement> revitRows,
        List<DrofusHelper.DrofusElement> drofusRows)
    {
        if (System.IO.File.Exists(path))
            System.IO.File.Delete(path); // overwrite previous file

        using var wb = new ClosedXML.Excel.XLWorkbook();

        var ws1 = wb.Worksheets.Add("RevitMissingInDrofus");
        ws1.Cell(1, 1).Value = "ElementId";
        ws1.Cell(1, 2).Value = "OccurrenceId";
        ws1.Cell(1, 3).Value = "Tag";
        ws1.Cell(1, 4).Value = "Guid";
        ws1.Cell(1, 5).Value = "FamilyName";
        ws1.Cell(1, 6).Value = "Status";

        for (int i = 0; i < revitRows.Count; i++)
        {
            int r = i + 2;
            var x = revitRows[i];
            ws1.Cell(r, 1).Value = x.ElementId;
            ws1.Cell(r, 2).Value = x.OccurrenceId;
            ws1.Cell(r, 3).Value = x.Tag;
            ws1.Cell(r, 4).Value = x.Guid;
            ws1.Cell(r, 5).Value = x.FamilyName;
            ws1.Cell(r, 6).Value = x.Status.ToString();
        }

        var ws2 = wb.Worksheets.Add("DrofusMissingInRevit");
        ws2.Cell(1, 1).Value = "DrofusId";
        ws2.Cell(1, 2).Value = "DrofusTag";
        ws2.Cell(1, 3).Value = "DrofusArticleName";
        ws2.Cell(1, 4).Value = "DrofusModelName";
        ws2.Cell(1, 5).Value = "DrofusGuid";
        ws2.Cell(1, 6).Value = "OmegaStatus";
        ws2.Cell(1, 7).Value = "Status";
        ws2.Cell(1, 8).Value = "IsSubItem";
        ws2.Cell(1, 9).Value = "HostId";
        ws2.Cell(1, 10).Value = "HostItemName";

        for (int i = 0; i < drofusRows.Count; i++)
        {
            int r = i + 2;
            var x = drofusRows[i];
            ws2.Cell(r, 1).Value = x.DrofusId;
            ws2.Cell(r, 2).Value = x.DrofusTag ?? "";
            ws2.Cell(r, 3).Value = x.DrofusArticleName ?? "";
            ws2.Cell(r, 4).Value = x.DrofusModelName ?? "";
            ws2.Cell(r, 5).Value = x.DrofusGuid ?? "";
            ws2.Cell(r, 6).Value = x.OmegaStatus ?? "";
            ws2.Cell(r, 7).Value = x.Status.ToString();
            ws2.Cell(r, 8).Value = x.IsSubItem;
            ws2.Cell(r, 9).Value = x.HostId;
            ws2.Cell(r, 10).Value = x.HostItemName ?? "";
        }

        int ws1LastRow = Math.Max(1, revitRows.Count + 1);
        int ws2LastRow = Math.Max(1, drofusRows.Count + 1);

        ws1.Range(1, 1, ws1LastRow, 6).SetAutoFilter();
        ws2.Range(1, 1, ws2LastRow, 10).SetAutoFilter();

        ws1.SheetView.FreezeRows(1);
        ws2.SheetView.FreezeRows(1);

        ws1.Columns().AdjustToContents();
        ws2.Columns().AdjustToContents();

        wb.SaveAs(path);
    }
}