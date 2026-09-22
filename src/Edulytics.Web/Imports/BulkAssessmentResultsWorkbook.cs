using System.Globalization;
using System.IO.Compression;
using System.Text;
using System.Xml.Linq;
using Edulytics.Core.Enums;
using Edulytics.Services.Assessments;

namespace Edulytics.Web.Imports;

public sealed record BulkAssessmentResultsWorkbookValidation(
    bool Succeeded,
    string? Error,
    AdaptedImportUpload? Upload)
{
    public static BulkAssessmentResultsWorkbookValidation Success(AdaptedImportUpload upload) =>
        new(true, null, upload);

    public static BulkAssessmentResultsWorkbookValidation Failure(string error) =>
        new(false, error, null);
}

public static class BulkAssessmentResultsWorkbook
{
    public const string ContentType =
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    private const string TemplateVersion = "bulk-assessment-results-v1";
    private const int HeaderRow = 6;
    private const int FirstStudentRow = 7;
    private const int FirstScoreColumn = 5;

    private static readonly XNamespace SpreadsheetNs =
        "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
    private static readonly XNamespace OfficeRelationshipNs =
        "http://schemas.openxmlformats.org/officeDocument/2006/relationships";
    private static readonly XNamespace PackageRelationshipNs =
        "http://schemas.openxmlformats.org/package/2006/relationships";
    private static readonly XNamespace ContentTypeNs =
        "http://schemas.openxmlformats.org/package/2006/content-types";

    public static byte[] Create(
        AssessmentResultsWorkspace workspace,
        AssessmentClassItem classItem)
    {
        ArgumentNullException.ThrowIfNull(workspace);
        ArgumentNullException.ThrowIfNull(classItem);

        if (workspace.Assessment.DeliveryMode != AssessmentDeliveryMode.Offline)
            throw new InvalidOperationException("Bulk results workbooks are only available for Offline assessments.");

        if (workspace.Assessment.Status != AssessmentStatus.Open)
            throw new InvalidOperationException("Bulk results workbooks require an Open assessment.");

        if (workspace.Questions.Count == 0)
            throw new InvalidOperationException("Assessment must contain questions.");

        if (classItem.Id != workspace.Assessment.ClassGroupId)
            throw new InvalidOperationException("Assessment class does not match workbook class.");

        using var stream = new MemoryStream();
        using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true))
        {
            WriteXml(archive, "[Content_Types].xml", ContentTypes());
            WriteXml(archive, "_rels/.rels", RootRelationships());
            WriteXml(archive, "xl/workbook.xml", Workbook());
            WriteXml(archive, "xl/_rels/workbook.xml.rels", WorkbookRelationships());
            WriteXml(archive, "xl/worksheets/sheet1.xml", ResultsSheet(workspace, classItem));
            WriteXml(archive, "xl/worksheets/sheet2.xml", MetadataSheet(workspace, classItem));
        }

        return stream.ToArray();
    }

    public static Guid? ReadAssessmentId(byte[] bytes)
    {
        try
        {
            using var stream = new MemoryStream(bytes, writable: false);
            using var archive = new ZipArchive(stream, ZipArchiveMode.Read, leaveOpen: false);
            var sharedStrings = ReadSharedStrings(archive);
            var metadata = ReadSheet(archive, "xl/worksheets/sheet2.xml", sharedStrings);
            var snapshot = ReadMetadata(metadata);

            return string.Equals(snapshot.TemplateVersion, TemplateVersion, StringComparison.Ordinal) &&
                   snapshot.AssessmentId != Guid.Empty
                ? snapshot.AssessmentId
                : null;
        }
        catch (InvalidDataException)
        {
            return null;
        }
        catch (IOException)
        {
            return null;
        }
        catch (FormatException)
        {
            return null;
        }
        catch (ArgumentException)
        {
            return null;
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }

    public static BulkAssessmentResultsWorkbookValidation ValidateAndNormalize(
        byte[] bytes,
        AssessmentResultsWorkspace current,
        AssessmentClassItem currentClass)
    {
        ArgumentNullException.ThrowIfNull(bytes);
        ArgumentNullException.ThrowIfNull(current);
        ArgumentNullException.ThrowIfNull(currentClass);

        WorkbookMetadata downloaded;
        Dictionary<int, Dictionary<int, string>> results;

        try
        {
            using var stream = new MemoryStream(bytes, writable: false);
            using var archive = new ZipArchive(stream, ZipArchiveMode.Read, leaveOpen: false);
            var sharedStrings = ReadSharedStrings(archive);
            results = ReadSheet(archive, "xl/worksheets/sheet1.xml", sharedStrings);
            var metadata = ReadSheet(archive, "xl/worksheets/sheet2.xml", sharedStrings);
            downloaded = ReadMetadata(metadata);
        }
        catch (Exception ex) when (ex is InvalidDataException or IOException or FormatException or ArgumentException or InvalidOperationException)
        {
            return BulkAssessmentResultsWorkbookValidation.Failure(
                "This is not a valid Edulytics Assessment Results workbook. Download a fresh workbook and try again.");
        }

        if (!string.Equals(downloaded.TemplateVersion, TemplateVersion, StringComparison.Ordinal))
        {
            return BulkAssessmentResultsWorkbookValidation.Failure(
                "This Assessment Results workbook uses an unsupported template version. Download a fresh workbook.");
        }

        if (current.Assessment.DeliveryMode != AssessmentDeliveryMode.Offline)
        {
            return BulkAssessmentResultsWorkbookValidation.Failure(
                "This assessment is Online. Online assessment results are recorded directly in Edulytics and cannot be imported here.");
        }

        if (current.Assessment.Status != AssessmentStatus.Open)
        {
            return BulkAssessmentResultsWorkbookValidation.Failure(
                "This assessment is no longer open for results import. Review the assessment status before continuing.");
        }

        if (downloaded.AssessmentId != current.Assessment.Id)
        {
            return BulkAssessmentResultsWorkbookValidation.Failure(
                "This workbook does not belong to the selected assessment. Download a fresh workbook.");
        }

        var stale = DetectStaleWorkbook(downloaded, current, currentClass);
        if (stale is not null)
            return BulkAssessmentResultsWorkbookValidation.Failure(stale);

        var edited = DetectEditedImmutableContent(downloaded, results, current);
        if (edited is not null)
            return BulkAssessmentResultsWorkbookValidation.Failure(edited);

        return NormalizeScores(downloaded, results, current, currentClass);
    }

    private static string? DetectStaleWorkbook(
        WorkbookMetadata downloaded,
        AssessmentResultsWorkspace current,
        AssessmentClassItem currentClass)
    {
        if (!string.Equals(downloaded.AssessmentTitle, current.Assessment.Title, StringComparison.Ordinal) ||
            downloaded.AssessmentDate != current.Assessment.AssessmentDate ||
            downloaded.AcademicYearId != current.Assessment.AcademicYearId ||
            downloaded.ClassGroupId != current.Assessment.ClassGroupId ||
            !string.Equals(downloaded.ClassName, currentClass.Name, StringComparison.Ordinal) ||
            !ByteArraysEqual(downloaded.AssessmentRowVersion, current.Assessment.RowVersion))
        {
            return "This workbook is out of date because the assessment details changed after it was downloaded. Download a fresh workbook before importing results.";
        }

        var questions = current.Questions.OrderBy(x => x.Order).ToArray();
        if (downloaded.Questions.Count != questions.Length)
        {
            return "This workbook is out of date because the assessment question set changed. Download a fresh workbook before importing results.";
        }

        for (var index = 0; index < questions.Length; index++)
        {
            var old = downloaded.Questions[index];
            var now = questions[index];
            if (old.Id != now.Id ||
                old.Order != now.Order ||
                old.MaxScore != now.MaxScore ||
                !string.Equals(old.Prompt, now.Prompt, StringComparison.Ordinal))
            {
                return "This workbook is out of date because an assessment question, its order, or its maximum score changed. Download a fresh workbook before importing results.";
            }
        }

        var students = current.Students.OrderBy(x => x.DisplayName).ThenBy(x => x.StudentProfileId).ToArray();
        if (downloaded.Students.Count != students.Length)
        {
            return "This workbook is out of date because the assessment class roster changed. Download a fresh workbook before importing results.";
        }

        for (var index = 0; index < students.Length; index++)
        {
            var old = downloaded.Students[index];
            var now = students[index];
            if (old.StudentProfileId != now.StudentProfileId ||
                !string.Equals(old.StudentNumber, now.StudentNumber, StringComparison.Ordinal) ||
                !string.Equals(old.StudentName, now.DisplayName, StringComparison.Ordinal) ||
                old.ResultId != now.ResultId ||
                !ByteArraysEqual(old.ResultRowVersion, now.RowVersion))
            {
                return "This workbook is out of date because the student roster or an existing result changed. Download a fresh workbook before importing results.";
            }
        }

        return null;
    }

    private static string? DetectEditedImmutableContent(
        WorkbookMetadata downloaded,
        IReadOnlyDictionary<int, Dictionary<int, string>> results,
        AssessmentResultsWorkspace current)
    {
        var visibleTitle = Value(results, 1, 5);
        if (!string.Equals(visibleTitle, downloaded.AssessmentTitle, StringComparison.Ordinal))
        {
            return ChangedValue(
                "Assessment name",
                visibleTitle,
                downloaded.AssessmentTitle);
        }

        var visibleDate = Value(results, 2, 5);
        var originalDate = downloaded.AssessmentDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        if (!string.Equals(visibleDate, originalDate, StringComparison.Ordinal))
        {
            return ChangedValue(
                "Assessment date",
                visibleDate,
                originalDate);
        }

        var visibleClass = Value(results, 3, 5);
        if (!string.Equals(visibleClass, downloaded.ClassName, StringComparison.Ordinal))
        {
            return ChangedValue(
                "Class",
                visibleClass,
                downloaded.ClassName);
        }

        if (!string.Equals(Value(results, HeaderRow, 1), "StudentProfileId", StringComparison.Ordinal) ||
            !string.Equals(Value(results, HeaderRow, 2), "StudentNumber", StringComparison.Ordinal) ||
            !string.Equals(Value(results, HeaderRow, 3), "ResultRowVersion", StringComparison.Ordinal) ||
            !string.Equals(Value(results, HeaderRow, 4), "Student", StringComparison.Ordinal))
        {
            return "The workbook structure was changed. Restore the original headers or download a fresh workbook.";
        }

        var questions = current.Questions.OrderBy(x => x.Order).ToArray();
        for (var index = 0; index < questions.Length; index++)
        {
            var expected = QuestionHeader(questions[index]);
            var actual = Value(results, HeaderRow, FirstScoreColumn + index);
            if (!string.Equals(actual, expected, StringComparison.Ordinal))
            {
                return ChangedValue(
                    $"Question {questions[index].Order} header",
                    actual,
                    expected);
            }
        }

        for (var index = 0; index < downloaded.Students.Count; index++)
        {
            var row = FirstStudentRow + index;
            var original = downloaded.Students[index];

            var submittedId = Value(results, row, 1);
            var originalId = original.StudentProfileId.ToString("D");
            if (!string.Equals(submittedId, originalId, StringComparison.OrdinalIgnoreCase))
            {
                return $"Student identity on row {row} was changed. Restore the original row exactly or download a fresh workbook.";
            }

            var submittedNumber = Value(results, row, 2);
            if (!string.Equals(submittedNumber, original.StudentNumber, StringComparison.Ordinal))
            {
                return ChangedValue(
                    $"Student number on row {row}",
                    submittedNumber,
                    original.StudentNumber);
            }

            var submittedVersion = Value(results, row, 3);
            var originalVersion = original.ResultRowVersion is null
                ? string.Empty
                : Convert.ToBase64String(original.ResultRowVersion);
            if (!string.Equals(submittedVersion, originalVersion, StringComparison.Ordinal))
            {
                return $"Protected result metadata on row {row} was changed. Restore the original row exactly or download a fresh workbook.";
            }

            var submittedName = Value(results, row, 4);
            if (!string.Equals(submittedName, original.StudentName, StringComparison.Ordinal))
            {
                return ChangedValue(
                    $"Student name on row {row}",
                    submittedName,
                    original.StudentName);
            }
        }

        var expectedLastRow = FirstStudentRow + downloaded.Students.Count - 1;
        foreach (var row in results.Keys.Where(x => x > expectedLastRow))
        {
            if (results[row].Values.Any(x => !string.IsNullOrWhiteSpace(x)))
            {
                return $"Unexpected data was added on row {row}. Remove the added row or download a fresh workbook.";
            }
        }

        return null;
    }

    private static BulkAssessmentResultsWorkbookValidation NormalizeScores(
        WorkbookMetadata downloaded,
        IReadOnlyDictionary<int, Dictionary<int, string>> results,
        AssessmentResultsWorkspace current,
        AssessmentClassItem currentClass)
    {
        var questions = current.Questions.OrderBy(x => x.Order).ToArray();
        var students = current.Students.OrderBy(x => x.DisplayName).ThenBy(x => x.StudentProfileId).ToArray();
        var output = new StringBuilder();
        output.AppendLine(
            "AssessmentTitle,AssessmentDate,ClassName,StudentNumber,StudentName,QuestionOrder,Score,ClassCode,AssessmentId");

        var scoreRowCount = 0;
        for (var studentIndex = 0; studentIndex < students.Length; studentIndex++)
        {
            var row = FirstStudentRow + studentIndex;
            var scoreTexts = Enumerable.Range(0, questions.Length)
                .Select(index => Value(results, row, FirstScoreColumn + index))
                .ToArray();

            if (scoreTexts.All(string.IsNullOrWhiteSpace))
                continue;

            if (scoreTexts.Any(string.IsNullOrWhiteSpace))
            {
                return BulkAssessmentResultsWorkbookValidation.Failure(
                    $"All question scores are required for {students[studentIndex].DisplayName} (row {row}). Complete the missing score cells and upload the workbook again.");
            }

            for (var questionIndex = 0; questionIndex < questions.Length; questionIndex++)
            {
                var scoreText = scoreTexts[questionIndex];
                if (!decimal.TryParse(
                        scoreText,
                        NumberStyles.Number,
                        CultureInfo.InvariantCulture,
                        out var score))
                {
                    return BulkAssessmentResultsWorkbookValidation.Failure(
                        $"Invalid score for {students[studentIndex].DisplayName}, question {questions[questionIndex].Order}. Enter a numeric value between 0 and {questions[questionIndex].MaxScore.ToString("0.##", CultureInfo.InvariantCulture)}.");
                }

                var question = questions[questionIndex];
                if (score < 0m || score > question.MaxScore)
                {
                    return BulkAssessmentResultsWorkbookValidation.Failure(
                        $"Invalid score for {students[studentIndex].DisplayName}, question {question.Order}. Entered: {scoreText}. Allowed range: 0–{question.MaxScore.ToString("0.##", CultureInfo.InvariantCulture)}.");
                }

                var values = new[]
                {
                    current.Assessment.Title,
                    current.Assessment.AssessmentDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    currentClass.Name,
                    students[studentIndex].StudentNumber,
                    students[studentIndex].DisplayName,
                    question.Order.ToString(CultureInfo.InvariantCulture),
                    score.ToString(CultureInfo.InvariantCulture),
                    currentClass.Code,
                    current.Assessment.Id.ToString("D")
                };

                output.AppendLine(string.Join(",", values.Select(EscapeCsv)));
                scoreRowCount++;
            }
        }

        if (scoreRowCount == 0)
        {
            return BulkAssessmentResultsWorkbookValidation.Failure(
                "Enter scores for at least one student before uploading the workbook.");
        }

        return BulkAssessmentResultsWorkbookValidation.Success(
            new AdaptedImportUpload(
                $"edulytics-AssessmentResults-{current.Assessment.Id:N}.csv",
                Encoding.UTF8.GetBytes(output.ToString())));
    }

    private static string ChangedValue(
        string field,
        string submitted,
        string expected) =>
        $"{field} was changed. Current file: \"{submitted}\". Original value: \"{expected}\". Restore the original value exactly or download a fresh workbook.";

    private static bool ByteArraysEqual(byte[]? left, byte[]? right)
    {
        left ??= [];
        right ??= [];
        return left.AsSpan().SequenceEqual(right);
    }

    private static XDocument ResultsSheet(
        AssessmentResultsWorkspace workspace,
        AssessmentClassItem classItem)
    {
        var rows = new List<XElement>
        {
            Row(1,
                InlineCell("D1", "Assessment"),
                InlineCell("E1", workspace.Assessment.Title)),
            Row(2,
                InlineCell("D2", "Assessment date"),
                InlineCell("E2", workspace.Assessment.AssessmentDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))),
            Row(3,
                InlineCell("D3", "Class"),
                InlineCell("E3", classItem.Name)),
            Row(4,
                InlineCell("D4", "Instructions"),
                InlineCell("E4", "Enter scores only. Do not change assessment, class, student, or question information."))
        };

        var header = new List<XElement>
        {
            InlineCell("A6", "StudentProfileId"),
            InlineCell("B6", "StudentNumber"),
            InlineCell("C6", "ResultRowVersion"),
            InlineCell("D6", "Student")
        };

        var questions = workspace.Questions.OrderBy(x => x.Order).ToArray();
        for (var index = 0; index < questions.Length; index++)
        {
            header.Add(
                InlineCell(
                    $"{ColumnName(FirstScoreColumn + index)}{HeaderRow}",
                    QuestionHeader(questions[index])));
        }
        rows.Add(new XElement(SpreadsheetNs + "row", new XAttribute("r", HeaderRow), header));

        var students = workspace.Students
            .OrderBy(x => x.DisplayName)
            .ThenBy(x => x.StudentProfileId)
            .ToArray();

        for (var studentIndex = 0; studentIndex < students.Length; studentIndex++)
        {
            var student = students[studentIndex];
            var rowNumber = FirstStudentRow + studentIndex;
            var cells = new List<XElement>
            {
                InlineCell($"A{rowNumber}", student.StudentProfileId.ToString("D")),
                InlineCell($"B{rowNumber}", student.StudentNumber),
                InlineCell(
                    $"C{rowNumber}",
                    student.RowVersion is null ? string.Empty : Convert.ToBase64String(student.RowVersion)),
                InlineCell($"D{rowNumber}", student.DisplayName)
            };

            for (var questionIndex = 0; questionIndex < questions.Length; questionIndex++)
            {
                cells.Add(
                    InlineCell(
                        $"{ColumnName(FirstScoreColumn + questionIndex)}{rowNumber}",
                        string.Empty));
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
                        new XAttribute("max", "3"),
                        new XAttribute("hidden", "1"),
                        new XAttribute("width", "2"),
                        new XAttribute("customWidth", "1")),
                    new XElement(
                        SpreadsheetNs + "col",
                        new XAttribute("min", "4"),
                        new XAttribute("max", "4"),
                        new XAttribute("width", "28"),
                        new XAttribute("customWidth", "1")),
                    new XElement(
                        SpreadsheetNs + "col",
                        new XAttribute("min", "5"),
                        new XAttribute("max", Math.Max(5, FirstScoreColumn + questions.Length - 1)),
                        new XAttribute("width", "18"),
                        new XAttribute("customWidth", "1"))),
                new XElement(SpreadsheetNs + "sheetData", rows)));
    }

    private static XDocument MetadataSheet(
        AssessmentResultsWorkspace workspace,
        AssessmentClassItem classItem)
    {
        var rows = new List<XElement>
        {
            MetadataRow(1, "TemplateVersion", TemplateVersion),
            MetadataRow(2, "AssessmentId", workspace.Assessment.Id.ToString("D")),
            MetadataRow(3, "AssessmentTitle", workspace.Assessment.Title),
            MetadataRow(4, "AssessmentDate", workspace.Assessment.AssessmentDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)),
            MetadataRow(5, "AcademicYearId", workspace.Assessment.AcademicYearId.ToString("D")),
            MetadataRow(6, "ClassGroupId", workspace.Assessment.ClassGroupId.ToString("D")),
            MetadataRow(7, "ClassName", classItem.Name),
            MetadataRow(8, "AssessmentRowVersion", Convert.ToBase64String(workspace.Assessment.RowVersion))
        };

        var rowNumber = 10;
        foreach (var question in workspace.Questions.OrderBy(x => x.Order))
        {
            rows.Add(
                Row(
                    rowNumber++,
                    InlineCell($"A{rowNumber - 1}", "Question"),
                    InlineCell($"B{rowNumber - 1}", question.Id.ToString("D")),
                    NumericCell($"C{rowNumber - 1}", question.Order),
                    NumericCell($"D{rowNumber - 1}", question.MaxScore),
                    InlineCell($"E{rowNumber - 1}", question.Prompt)));
        }

        rowNumber += 2;
        foreach (var student in workspace.Students.OrderBy(x => x.DisplayName).ThenBy(x => x.StudentProfileId))
        {
            var resultId = student.ResultId?.ToString("D") ?? string.Empty;
            var resultVersion = student.RowVersion is null
                ? string.Empty
                : Convert.ToBase64String(student.RowVersion);

            rows.Add(
                Row(
                    rowNumber,
                    InlineCell($"A{rowNumber}", "Student"),
                    InlineCell($"B{rowNumber}", student.StudentProfileId.ToString("D")),
                    InlineCell($"C{rowNumber}", student.StudentNumber),
                    InlineCell($"D{rowNumber}", student.DisplayName),
                    InlineCell($"E{rowNumber}", resultId),
                    InlineCell($"F{rowNumber}", resultVersion)));
            rowNumber++;
        }

        return new XDocument(
            new XElement(
                SpreadsheetNs + "worksheet",
                new XElement(SpreadsheetNs + "sheetData", rows)));
    }

    private static WorkbookMetadata ReadMetadata(
        IReadOnlyDictionary<int, Dictionary<int, string>> sheet)
    {
        var values = sheet
            .Where(x => !string.IsNullOrWhiteSpace(Value(sheet, x.Key, 1)) &&
                        !string.Equals(Value(sheet, x.Key, 1), "Question", StringComparison.Ordinal) &&
                        !string.Equals(Value(sheet, x.Key, 1), "Student", StringComparison.Ordinal))
            .ToDictionary(
                x => Value(sheet, x.Key, 1),
                x => Value(sheet, x.Key, 2),
                StringComparer.Ordinal);

        var templateVersion = values.GetValueOrDefault("TemplateVersion") ?? string.Empty;

        if (!Guid.TryParse(values.GetValueOrDefault("AssessmentId"), out var assessmentId))
            assessmentId = Guid.Empty;

        var title = values.GetValueOrDefault("AssessmentTitle") ?? string.Empty;

        if (!DateOnly.TryParseExact(
                values.GetValueOrDefault("AssessmentDate"),
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var date))
        {
            date = default;
        }

        if (!Guid.TryParse(values.GetValueOrDefault("AcademicYearId"), out var academicYearId))
            academicYearId = Guid.Empty;

        if (!Guid.TryParse(values.GetValueOrDefault("ClassGroupId"), out var classGroupId))
            classGroupId = Guid.Empty;

        var className = values.GetValueOrDefault("ClassName") ?? string.Empty;
        var assessmentVersion = ParseBase64(values.GetValueOrDefault("AssessmentRowVersion"));

        var questions = new List<QuestionSnapshot>();
        var students = new List<StudentSnapshot>();

        foreach (var row in sheet.Keys.OrderBy(x => x))
        {
            var kind = Value(sheet, row, 1);
            if (string.Equals(kind, "Question", StringComparison.Ordinal))
            {
                if (!Guid.TryParse(Value(sheet, row, 2), out var questionId) ||
                    !int.TryParse(Value(sheet, row, 3), NumberStyles.Integer, CultureInfo.InvariantCulture, out var order) ||
                    !decimal.TryParse(Value(sheet, row, 4), NumberStyles.Number, CultureInfo.InvariantCulture, out var maxScore))
                {
                    throw new InvalidDataException("Invalid question metadata.");
                }

                questions.Add(new QuestionSnapshot(
                    questionId,
                    order,
                    maxScore,
                    Value(sheet, row, 5)));
            }
            else if (string.Equals(kind, "Student", StringComparison.Ordinal))
            {
                if (!Guid.TryParse(Value(sheet, row, 2), out var studentId))
                    throw new InvalidDataException("Invalid student metadata.");

                Guid? resultId = null;
                var resultIdText = Value(sheet, row, 5);
                if (!string.IsNullOrWhiteSpace(resultIdText))
                {
                    if (!Guid.TryParse(resultIdText, out var parsedResultId))
                        throw new InvalidDataException("Invalid result metadata.");
                    resultId = parsedResultId;
                }

                students.Add(
                    new StudentSnapshot(
                        studentId,
                        Value(sheet, row, 3),
                        Value(sheet, row, 4),
                        resultId,
                        ParseBase64(Value(sheet, row, 6))));
            }
        }

        return new WorkbookMetadata(
            templateVersion,
            assessmentId,
            title,
            date,
            academicYearId,
            classGroupId,
            className,
            assessmentVersion,
            questions,
            students);
    }

    private static byte[]? ParseBase64(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        try
        {
            return Convert.FromBase64String(value);
        }
        catch (FormatException)
        {
            throw new InvalidDataException("Workbook version metadata is invalid.");
        }
    }

    private static string QuestionHeader(AssessmentQuestionItem question) =>
        $"Q{question.Order} ({question.MaxScore.ToString("0.##", CultureInfo.InvariantCulture)} pts)";

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

    private static XElement Row(int rowNumber, params XElement[] cells) =>
        new(
            SpreadsheetNs + "row",
            new XAttribute("r", rowNumber),
            cells);

    private static XElement MetadataRow(int rowNumber, string key, string value) =>
        Row(
            rowNumber,
            InlineCell($"A{rowNumber}", key),
            InlineCell($"B{rowNumber}", value));

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

    private static XElement NumericCell(string reference, int value) =>
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

    private static string EscapeCsv(string value)
    {
        value ??= string.Empty;
        if (!value.Contains(',') &&
            !value.Contains('"') &&
            !value.Contains('\r') &&
            !value.Contains('\n'))
        {
            return value;
        }

        return $"\"{value.Replace("\"", "\"\"", StringComparison.Ordinal)}\"";
    }

    private sealed record QuestionSnapshot(
        Guid Id,
        int Order,
        decimal MaxScore,
        string Prompt);

    private sealed record StudentSnapshot(
        Guid StudentProfileId,
        string StudentNumber,
        string StudentName,
        Guid? ResultId,
        byte[]? ResultRowVersion);

    private sealed record WorkbookMetadata(
        string TemplateVersion,
        Guid AssessmentId,
        string AssessmentTitle,
        DateOnly AssessmentDate,
        Guid AcademicYearId,
        Guid ClassGroupId,
        string ClassName,
        byte[]? AssessmentRowVersion,
        IReadOnlyList<QuestionSnapshot> Questions,
        IReadOnlyList<StudentSnapshot> Students);
}
