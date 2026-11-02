using System.Globalization;
using System.IO.Compression;
using System.Xml.Linq;
using Edulytics.Services.Assessments;

namespace Edulytics.Web.Assessments;

public sealed record AssessmentResultsWorkbookRow(
    Guid StudentProfileId,
    byte[]? ResultRowVersion,
    IReadOnlyList<decimal> Scores);

public sealed record AssessmentResultsWorkbookImport(
    Guid AssessmentId,
    IReadOnlyList<Guid> QuestionIds,
    IReadOnlyList<AssessmentResultsWorkbookRow> Rows);

public static class AssessmentResultsWorkbook
{
    public const string ContentType =
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    private const string TemplateVersion = "assessment-results-v1";

    private static readonly XNamespace SpreadsheetNs =
        "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
    private static readonly XNamespace OfficeRelationshipNs =
        "http://schemas.openxmlformats.org/officeDocument/2006/relationships";
    private static readonly XNamespace PackageRelationshipNs =
        "http://schemas.openxmlformats.org/package/2006/relationships";
    private static readonly XNamespace ContentTypeNs =
        "http://schemas.openxmlformats.org/package/2006/content-types";

    public static byte[] Create(AssessmentResultsWorkspace workspace)
    {
        ArgumentNullException.ThrowIfNull(workspace);
        if (workspace.Questions.Count == 0)
            throw new InvalidOperationException("Assessment must contain questions.");

        using var stream = new MemoryStream();
        using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true))
        {
            WriteXml(archive, "[Content_Types].xml", ContentTypes());
            WriteXml(archive, "_rels/.rels", RootRelationships());
            WriteXml(archive, "xl/workbook.xml", Workbook());
            WriteXml(archive, "xl/_rels/workbook.xml.rels", WorkbookRelationships());
            WriteXml(archive, "xl/worksheets/sheet1.xml", ResultsSheet(workspace));
            WriteXml(archive, "xl/worksheets/sheet2.xml", MetadataSheet(workspace));
        }

        return stream.ToArray();
    }

    public static AssessmentResultsWorkbookImport Parse(
        Stream workbookStream,
        Guid expectedAssessmentId)
    {
        ArgumentNullException.ThrowIfNull(workbookStream);
        if (!workbookStream.CanRead)
            throw new InvalidDataException("Workbook stream is not readable.");

        using var archive = new ZipArchive(workbookStream, ZipArchiveMode.Read, leaveOpen: true);
        var sharedStrings = ReadSharedStrings(archive);
        var results = ReadSheet(archive, "xl/worksheets/sheet1.xml", sharedStrings);
        var metadata = ReadSheet(archive, "xl/worksheets/sheet2.xml", sharedStrings);

        var assessmentText = Value(metadata, 1, 2);
        if (!Guid.TryParse(assessmentText, out var assessmentId) ||
            assessmentId == Guid.Empty ||
            assessmentId != expectedAssessmentId)
        {
            throw new InvalidDataException("Workbook does not belong to this assessment.");
        }

        if (!string.Equals(Value(metadata, 4, 2), TemplateVersion, StringComparison.Ordinal))
            throw new InvalidDataException("Unsupported assessment results workbook version.");

        var questionIds = new List<Guid>();
        for (var column = 2; ; column++)
        {
            var value = Value(metadata, 2, column);
            if (string.IsNullOrWhiteSpace(value))
                break;
            if (!Guid.TryParse(value, out var questionId) || questionId == Guid.Empty)
                throw new InvalidDataException("Workbook contains an invalid question identifier.");
            questionIds.Add(questionId);
        }

        if (questionIds.Count == 0 || questionIds.Distinct().Count() != questionIds.Count)
            throw new InvalidDataException("Workbook question metadata is invalid.");

        var rows = new List<AssessmentResultsWorkbookRow>();
        foreach (var row in results.Keys.Where(x => x >= 2).OrderBy(x => x))
        {
            var studentIdText = Value(results, row, 1);
            if (string.IsNullOrWhiteSpace(studentIdText))
                continue;
            if (!Guid.TryParse(studentIdText, out var studentId) || studentId == Guid.Empty)
                throw new InvalidDataException($"Invalid student identifier on row {row}.");

            var scoreTexts = Enumerable.Range(0, questionIds.Count)
                .Select(index => Value(results, row, 4 + index))
                .ToArray();
            if (scoreTexts.All(string.IsNullOrWhiteSpace))
                continue;
            if (scoreTexts.Any(string.IsNullOrWhiteSpace))
                throw new InvalidDataException($"All question scores are required on row {row}.");

            var scores = new decimal[scoreTexts.Length];
            for (var index = 0; index < scoreTexts.Length; index++)
            {
                if (!decimal.TryParse(
                        scoreTexts[index],
                        NumberStyles.Number,
                        CultureInfo.InvariantCulture,
                        out var score))
                {
                    throw new InvalidDataException(
                        $"Invalid numeric score on row {row}, question {index + 1}.");
                }
                scores[index] = score;
            }

            byte[]? rowVersion = null;
            var versionText = Value(results, row, 2);
            if (!string.IsNullOrWhiteSpace(versionText))
            {
                try
                {
                    rowVersion = Convert.FromBase64String(versionText);
                }
                catch (FormatException)
                {
                    throw new InvalidDataException($"Invalid result version on row {row}.");
                }
            }

            rows.Add(new AssessmentResultsWorkbookRow(studentId, rowVersion, scores));
        }

        if (rows.Select(x => x.StudentProfileId).Distinct().Count() != rows.Count)
            throw new InvalidDataException("Workbook contains duplicate student rows.");

        return new AssessmentResultsWorkbookImport(assessmentId, questionIds, rows);
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

    private static XDocument Workbook() =>
        new(
            new XElement(
                SpreadsheetNs + "workbook",
                new XAttribute(XNamespace.Xmlns + "r", OfficeRelationshipNs),
                new XElement(
                    SpreadsheetNs + "sheets",
                    new XElement(
                        SpreadsheetNs + "sheet",
                        new XAttribute("name", "Results"),
                        new XAttribute("sheetId", "1"),
                        new XAttribute(OfficeRelationshipNs + "id", "rId1")),
                    new XElement(
                        SpreadsheetNs + "sheet",
                        new XAttribute("name", "Metadata"),
                        new XAttribute("sheetId", "2"),
                        new XAttribute("state", "hidden"),
                        new XAttribute(OfficeRelationshipNs + "id", "rId2")))));

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

    private static XDocument ResultsSheet(AssessmentResultsWorkspace workspace)
    {
        var rows = new List<XElement>();
        var header = new List<XElement>
        {
            InlineCell("A1", "StudentProfileId"),
            InlineCell("B1", "ResultRowVersion"),
            InlineCell("C1", "Student")
        };
        for (var index = 0; index < workspace.Questions.Count; index++)
        {
            var question = workspace.Questions[index];
            header.Add(InlineCell(
                $"{ColumnName(4 + index)}1",
                $"Q{question.Order} ({question.MaxScore.ToString("0.##", CultureInfo.InvariantCulture)} pts)"));
        }
        rows.Add(new XElement(SpreadsheetNs + "row", new XAttribute("r", "1"), header));

        for (var studentIndex = 0; studentIndex < workspace.Students.Count; studentIndex++)
        {
            var student = workspace.Students[studentIndex];
            var rowNumber = studentIndex + 2;
            var cells = new List<XElement>
            {
                InlineCell($"A{rowNumber}", student.StudentProfileId.ToString("D")),
                InlineCell(
                    $"B{rowNumber}",
                    student.RowVersion is null ? string.Empty : Convert.ToBase64String(student.RowVersion)),
                InlineCell($"C{rowNumber}", student.DisplayName)
            };

            for (var questionIndex = 0; questionIndex < workspace.Questions.Count; questionIndex++)
            {
                var question = workspace.Questions[questionIndex];
                if (student.QuestionScores.TryGetValue(question.Id, out var score))
                {
                    cells.Add(NumericCell(
                        $"{ColumnName(4 + questionIndex)}{rowNumber}",
                        score));
                }
                else
                {
                    cells.Add(InlineCell(
                        $"{ColumnName(4 + questionIndex)}{rowNumber}",
                        string.Empty));
                }
            }

            rows.Add(new XElement(
                SpreadsheetNs + "row",
                new XAttribute("r", rowNumber),
                cells));
        }

        return new XDocument(
            new XElement(
                SpreadsheetNs + "worksheet",
                new XElement(
                    SpreadsheetNs + "cols",
                    new XElement(
                        SpreadsheetNs + "col",
                        new XAttribute("min", "1"),
                        new XAttribute("max", "2"),
                        new XAttribute("hidden", "1"),
                        new XAttribute("width", "2"),
                        new XAttribute("customWidth", "1"))),
                new XElement(SpreadsheetNs + "sheetData", rows)));
    }

    private static XDocument MetadataSheet(AssessmentResultsWorkspace workspace)
    {
        var questionIds = new List<XElement> { InlineCell("A2", "QuestionIds") };
        var orders = new List<XElement> { InlineCell("A3", "QuestionOrders") };
        for (var index = 0; index < workspace.Questions.Count; index++)
        {
            var column = ColumnName(2 + index);
            questionIds.Add(InlineCell(
                $"{column}2",
                workspace.Questions[index].Id.ToString("D")));
            orders.Add(NumericCell(
                $"{column}3",
                workspace.Questions[index].Order));
        }

        return new XDocument(
            new XElement(
                SpreadsheetNs + "worksheet",
                new XElement(
                    SpreadsheetNs + "sheetData",
                    new XElement(
                        SpreadsheetNs + "row",
                        new XAttribute("r", "1"),
                        InlineCell("A1", "AssessmentId"),
                        InlineCell("B1", workspace.Assessment.Id.ToString("D"))),
                    new XElement(
                        SpreadsheetNs + "row",
                        new XAttribute("r", "2"),
                        questionIds),
                    new XElement(
                        SpreadsheetNs + "row",
                        new XAttribute("r", "3"),
                        orders),
                    new XElement(
                        SpreadsheetNs + "row",
                        new XAttribute("r", "4"),
                        InlineCell("A4", "TemplateVersion"),
                        InlineCell("B4", TemplateVersion)),
                    new XElement(
                        SpreadsheetNs + "row",
                        new XAttribute("r", "5"),
                        InlineCell("A5", "ClassGroupId"),
                        InlineCell("B5", workspace.Assessment.ClassGroupId.ToString("D"))))));
    }

    private static Dictionary<int, Dictionary<int, string>> ReadSheet(
        ZipArchive archive,
        string path,
        IReadOnlyList<string> sharedStrings)
    {
        var entry = archive.GetEntry(path)
            ?? throw new InvalidDataException($"Workbook is missing {path}.");
        using var stream = entry.Open();
        var document = XDocument.Load(stream);
        var result = new Dictionary<int, Dictionary<int, string>>();

        foreach (var row in document.Descendants(SpreadsheetNs + "row"))
        {
            if (!int.TryParse((string?)row.Attribute("r"), out var rowNumber))
                continue;

            var values = new Dictionary<int, string>();
            foreach (var cell in row.Elements(SpreadsheetNs + "c"))
            {
                var reference = (string?)cell.Attribute("r") ?? string.Empty;
                var column = ColumnIndex(reference);
                if (column <= 0)
                    continue;

                values[column] = ReadCellValue(cell, sharedStrings);
            }
            result[rowNumber] = values;
        }

        return result;
    }

    private static IReadOnlyList<string> ReadSharedStrings(ZipArchive archive)
    {
        var entry = archive.GetEntry("xl/sharedStrings.xml");
        if (entry is null)
            return [];

        using var stream = entry.Open();
        var document = XDocument.Load(stream);
        return document
            .Descendants(SpreadsheetNs + "si")
            .Select(si => string.Concat(si.Descendants(SpreadsheetNs + "t").Select(x => x.Value)))
            .ToArray();
    }

    private static string ReadCellValue(
        XElement cell,
        IReadOnlyList<string> sharedStrings)
    {
        var type = (string?)cell.Attribute("t");
        if (string.Equals(type, "inlineStr", StringComparison.Ordinal))
            return string.Concat(cell.Descendants(SpreadsheetNs + "t").Select(x => x.Value));

        var raw = cell.Element(SpreadsheetNs + "v")?.Value ?? string.Empty;
        if (string.Equals(type, "s", StringComparison.Ordinal) &&
            int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var sharedIndex) &&
            sharedIndex >= 0 &&
            sharedIndex < sharedStrings.Count)
        {
            return sharedStrings[sharedIndex];
        }

        return raw;
    }

    private static string Value(
        IReadOnlyDictionary<int, Dictionary<int, string>> sheet,
        int row,
        int column) =>
        sheet.TryGetValue(row, out var values) &&
        values.TryGetValue(column, out var value)
            ? value.Trim()
            : string.Empty;

    private static int ColumnIndex(string reference)
    {
        var value = 0;
        foreach (var ch in reference)
        {
            if (ch is < 'A' or > 'Z')
                break;
            value = checked(value * 26 + (ch - 'A' + 1));
        }
        return value;
    }

    private static string ColumnName(int oneBased)
    {
        if (oneBased <= 0)
            throw new ArgumentOutOfRangeException(nameof(oneBased));

        var value = oneBased;
        var chars = new Stack<char>();
        while (value > 0)
        {
            value--;
            chars.Push((char)('A' + value % 26));
            value /= 26;
        }
        return new string(chars.ToArray());
    }

    private static XElement InlineCell(string reference, string value) =>
        new(
            SpreadsheetNs + "c",
            new XAttribute("r", reference),
            new XAttribute("t", "inlineStr"),
            new XElement(
                SpreadsheetNs + "is",
                new XElement(
                    SpreadsheetNs + "t",
                    new XAttribute(XNamespace.Xml + "space", "preserve"),
                    value ?? string.Empty)));

    private static XElement NumericCell(string reference, decimal value) =>
        new(
            SpreadsheetNs + "c",
            new XAttribute("r", reference),
            new XElement(
                SpreadsheetNs + "v",
                value.ToString(CultureInfo.InvariantCulture)));

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
