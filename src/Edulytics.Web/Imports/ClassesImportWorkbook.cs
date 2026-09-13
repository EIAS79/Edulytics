using System.IO.Compression;
using System.Xml.Linq;

namespace Edulytics.Web.Imports;

public static class ClassesImportWorkbook
{
    private static readonly XNamespace SpreadsheetNs =
        "http://schemas.openxmlformats.org/spreadsheetml/2006/main";

    private static readonly XNamespace OfficeRelationshipNs =
        "http://schemas.openxmlformats.org/officeDocument/2006/relationships";

    private static readonly XNamespace PackageRelationshipNs =
        "http://schemas.openxmlformats.org/package/2006/relationships";

    private static readonly XNamespace ContentTypeNs =
        "http://schemas.openxmlformats.org/package/2006/content-types";

    public static byte[] Create(IEnumerable<string> gradeLevels)
    {
        var levels = gradeLevels
            .Select(x => x.Trim())
            .Where(x => x.Length > 0)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        if (levels.Length == 0)
            throw new InvalidOperationException("At least one grade level is required.");

        using var stream = new MemoryStream();
        using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true))
        {
            WriteXml(archive, "[Content_Types].xml", ContentTypes());
            WriteXml(archive, "_rels/.rels", RootRelationships());
            WriteXml(archive, "xl/workbook.xml", Workbook(levels.Length));
            WriteXml(archive, "xl/_rels/workbook.xml.rels", WorkbookRelationships());
            WriteXml(archive, "xl/worksheets/sheet1.xml", ClassesSheet());
            WriteXml(archive, "xl/worksheets/sheet2.xml", AvailableValuesSheet(levels));
        }

        return stream.ToArray();
    }

    private static XDocument ContentTypes() =>
        new(
            new XElement(
                ContentTypeNs + "Types",
                new XElement(
                    ContentTypeNs + "Default",
                    new XAttribute("Extension", "rels"),
                    new XAttribute("ContentType", "application/vnd.openxmlformats-package.relationships+xml")),
                new XElement(
                    ContentTypeNs + "Default",
                    new XAttribute("Extension", "xml"),
                    new XAttribute("ContentType", "application/xml")),
                new XElement(
                    ContentTypeNs + "Override",
                    new XAttribute("PartName", "/xl/workbook.xml"),
                    new XAttribute("ContentType", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml")),
                new XElement(
                    ContentTypeNs + "Override",
                    new XAttribute("PartName", "/xl/worksheets/sheet1.xml"),
                    new XAttribute("ContentType", "application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml")),
                new XElement(
                    ContentTypeNs + "Override",
                    new XAttribute("PartName", "/xl/worksheets/sheet2.xml"),
                    new XAttribute("ContentType", "application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml"))));

    private static XDocument RootRelationships() =>
        new(
            new XElement(
                PackageRelationshipNs + "Relationships",
                new XElement(
                    PackageRelationshipNs + "Relationship",
                    new XAttribute("Id", "rId1"),
                    new XAttribute("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument"),
                    new XAttribute("Target", "xl/workbook.xml"))));

    private static XDocument Workbook(int levelCount)
    {
        var lastRow = levelCount + 1;

        return new XDocument(
            new XElement(
                SpreadsheetNs + "workbook",
                new XAttribute(XNamespace.Xmlns + "r", OfficeRelationshipNs),
                new XElement(
                    SpreadsheetNs + "sheets",
                    new XElement(
                        SpreadsheetNs + "sheet",
                        new XAttribute("name", "Classes"),
                        new XAttribute("sheetId", "1"),
                        new XAttribute(OfficeRelationshipNs + "id", "rId1")),
                    new XElement(
                        SpreadsheetNs + "sheet",
                        new XAttribute("name", "Available values"),
                        new XAttribute("sheetId", "2"),
                        new XAttribute("state", "hidden"),
                        new XAttribute(OfficeRelationshipNs + "id", "rId2"))),
                new XElement(
                    SpreadsheetNs + "definedNames",
                    new XElement(
                        SpreadsheetNs + "definedName",
                        new XAttribute("name", "GradeLevels"),
                        $"'Available values'!$A$2:$A${lastRow}"))));
    }

    private static XDocument WorkbookRelationships() =>
        new(
            new XElement(
                PackageRelationshipNs + "Relationships",
                new XElement(
                    PackageRelationshipNs + "Relationship",
                    new XAttribute("Id", "rId1"),
                    new XAttribute("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet"),
                    new XAttribute("Target", "worksheets/sheet1.xml")),
                new XElement(
                    PackageRelationshipNs + "Relationship",
                    new XAttribute("Id", "rId2"),
                    new XAttribute("Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet"),
                    new XAttribute("Target", "worksheets/sheet2.xml"))));

    private static XDocument ClassesSheet() =>
        new(
            new XElement(
                SpreadsheetNs + "worksheet",
                new XElement(
                    SpreadsheetNs + "sheetData",
                    new XElement(
                        SpreadsheetNs + "row",
                        new XAttribute("r", "1"),
                        InlineCell("A1", "GradeLevel"),
                        InlineCell("B1", "Name"))),
                new XElement(
                    SpreadsheetNs + "dataValidations",
                    new XAttribute("count", "1"),
                    new XElement(
                        SpreadsheetNs + "dataValidation",
                        new XAttribute("type", "list"),
                        new XAttribute("allowBlank", "0"),
                        new XAttribute("showDropDown", "0"),
                        new XAttribute("showInputMessage", "1"),
                        new XAttribute("promptTitle", "GradeLevel"),
                        new XAttribute("prompt", "Select a GradeLevel configured in Edulytics."),
                        new XAttribute("showErrorMessage", "1"),
                        new XAttribute("errorTitle", "Choose a GradeLevel"),
                        new XAttribute("error", "Select a GradeLevel from the dropdown."),
                        new XAttribute("sqref", "A2:A1001"),
                        new XElement(SpreadsheetNs + "formula1", "GradeLevels")))));

    private static XDocument AvailableValuesSheet(IReadOnlyList<string> levels)
    {
        var rows = new List<XElement>
        {
            new(
                SpreadsheetNs + "row",
                new XAttribute("r", "1"),
                InlineCell("A1", "GradeLevel"))
        };

        for (var index = 0; index < levels.Count; index++)
        {
            var rowNumber = index + 2;
            rows.Add(
                new XElement(
                    SpreadsheetNs + "row",
                    new XAttribute("r", rowNumber),
                    InlineCell($"A{rowNumber}", levels[index])));
        }

        return new XDocument(
            new XElement(
                SpreadsheetNs + "worksheet",
                new XElement(SpreadsheetNs + "sheetData", rows)));
    }

    private static XElement InlineCell(string reference, string value) =>
        new(
            SpreadsheetNs + "c",
            new XAttribute("r", reference),
            new XAttribute("t", "inlineStr"),
            new XElement(
                SpreadsheetNs + "is",
                new XElement(SpreadsheetNs + "t", value)));

    private static void WriteXml(
        ZipArchive archive,
        string path,
        XDocument document)
    {
        var entry = archive.CreateEntry(path, CompressionLevel.Fastest);
        using var entryStream = entry.Open();
        document.Save(entryStream);
    }
}
