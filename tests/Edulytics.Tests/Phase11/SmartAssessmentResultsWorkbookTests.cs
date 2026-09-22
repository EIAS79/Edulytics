using System.IO.Compression;
using System.Xml.Linq;
using Edulytics.Core.Enums;
using Edulytics.Services.Assessments;
using Edulytics.Services.Imports;
using Edulytics.Web.Imports;

namespace Edulytics.Tests.Phase11;

public sealed class SmartAssessmentResultsWorkbookTests
{
    private static readonly XNamespace SpreadsheetNs =
        "http://schemas.openxmlformats.org/spreadsheetml/2006/main";

    [Fact]
    public void Workbook_UsesOneOfflineAssessmentAndHidesInternalStudentColumns()
    {
        var (workspace, classItem) = BuildWorkspace();

        var bytes = BulkAssessmentResultsWorkbook.Create(
            workspace,
            classItem);

        Assert.Equal(
            workspace.Assessment.Id,
            BulkAssessmentResultsWorkbook.ReadAssessmentId(bytes));

        using var stream = new MemoryStream(bytes);
        using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
        var sheet = LoadXml(archive, "xl/worksheets/sheet1.xml");

        var hidden = Assert.Single(
            sheet
                .Descendants(SpreadsheetNs + "col")
                .Where(x =>
                    (string?)x.Attribute("min") == "1" &&
                    (string?)x.Attribute("max") == "3"));

        Assert.Equal("1", (string?)hidden.Attribute("hidden"));
        Assert.Contains(
            sheet.Descendants(SpreadsheetNs + "t"),
            x => x.Value == workspace.Assessment.Title);
        Assert.Contains(
            sheet.Descendants(SpreadsheetNs + "t"),
            x => x.Value == classItem.Name);
        Assert.Contains(
            sheet.Descendants(SpreadsheetNs + "t"),
            x => x.Value == workspace.Students[0].DisplayName);
    }

    [Fact]
    public void OnlineAssessment_CannotGenerateBulkWorkbook()
    {
        var (workspace, classItem) = BuildWorkspace();
        workspace = workspace with
        {
            Assessment = workspace.Assessment with
            {
                DeliveryMode = AssessmentDeliveryMode.Online
            }
        };

        Assert.Throws<InvalidOperationException>(
            () => BulkAssessmentResultsWorkbook.Create(
                workspace,
                classItem));
    }

    [Fact]
    public void ValidWorkbook_NormalizesToExistingAssessmentResultsImportShape()
    {
        var (workspace, classItem) = BuildWorkspace();
        var bytes = BulkAssessmentResultsWorkbook.Create(
            workspace,
            classItem);

        bytes = SetInlineCell(bytes, "E7", "2");
        bytes = SetInlineCell(bytes, "F7", "3");

        var result = BulkAssessmentResultsWorkbook.ValidateAndNormalize(
            bytes,
            workspace,
            classItem);

        Assert.True(result.Succeeded, result.Error);
        Assert.NotNull(result.Upload);

        var parsed = new ImportFileParser().Parse(
            result.Upload!.FileName,
            result.Upload.Bytes);

        Assert.True(parsed.Succeeded);
        Assert.Equal(
            new[]
            {
                "AssessmentTitle",
                "AssessmentDate",
                "ClassName",
                "StudentNumber",
                "StudentName",
                "QuestionOrder",
                "Score",
                "ClassCode",
                "AssessmentId"
            },
            parsed.File!.Headers);

        Assert.Equal(2, parsed.File.Rows.Count);
        Assert.All(
            parsed.File.Rows,
            row =>
            {
                Assert.Equal(
                    workspace.Assessment.Id.ToString("D"),
                    row.Values["AssessmentId"]);
                Assert.Equal(
                    workspace.Students[0].StudentNumber,
                    row.Values["StudentNumber"]);
                Assert.Equal(
                    workspace.Students[0].DisplayName,
                    row.Values["StudentName"]);
                Assert.Equal(
                    classItem.Code,
                    row.Values["ClassCode"]);
            });
    }

    [Fact]
    public void EditedStudentName_IsRejectedWithOriginalValue()
    {
        var (workspace, classItem) = BuildWorkspace();
        var bytes = BulkAssessmentResultsWorkbook.Create(
            workspace,
            classItem);

        bytes = SetInlineCell(bytes, "D7", "Changed Student");

        var result = BulkAssessmentResultsWorkbook.ValidateAndNormalize(
            bytes,
            workspace,
            classItem);

        Assert.False(result.Succeeded);
        Assert.Contains("Student name on row 7 was changed", result.Error);
        Assert.Contains(
            workspace.Students[0].DisplayName,
            result.Error);
        Assert.Contains("Restore the original value", result.Error);
    }

    [Fact]
    public void ChangedAuthoritativeAssessment_ForcesFreshDownload()
    {
        var (workspace, classItem) = BuildWorkspace();
        var bytes = BulkAssessmentResultsWorkbook.Create(
            workspace,
            classItem);

        var current = workspace with
        {
            Assessment = workspace.Assessment with
            {
                Title = "Updated title"
            }
        };

        var result = BulkAssessmentResultsWorkbook.ValidateAndNormalize(
            bytes,
            current,
            classItem);

        Assert.False(result.Succeeded);
        Assert.Contains("out of date", result.Error);
        Assert.Contains("Download a fresh workbook", result.Error);
    }

    [Fact]
    public void ScoreOutsideQuestionMaximum_ReturnsPreciseError()
    {
        var (workspace, classItem) = BuildWorkspace();
        var bytes = BulkAssessmentResultsWorkbook.Create(
            workspace,
            classItem);

        bytes = SetInlineCell(bytes, "E7", "5");
        bytes = SetInlineCell(bytes, "F7", "3");

        var result = BulkAssessmentResultsWorkbook.ValidateAndNormalize(
            bytes,
            workspace,
            classItem);

        Assert.False(result.Succeeded);
        Assert.Contains("question 1", result.Error);
        Assert.Contains("Allowed range: 0–2", result.Error);
    }

    private static (AssessmentResultsWorkspace Workspace, AssessmentClassItem ClassItem)
        BuildWorkspace()
    {
        var yearId = Guid.NewGuid();
        var classId = Guid.NewGuid();
        var assessmentId = Guid.NewGuid();
        var q1 = Guid.NewGuid();
        var q2 = Guid.NewGuid();
        var student1 = Guid.NewGuid();
        var student2 = Guid.NewGuid();

        var assessment = new AssessmentListItem(
            assessmentId,
            Guid.NewGuid(),
            classId,
            yearId,
            Guid.NewGuid(),
            "Fractions checkpoint",
            new DateOnly(2026, 9, 22),
            5m,
            AssessmentStatus.Open,
            [1, 2, 3],
            AssessmentTargetType.Class,
            null,
            AssessmentDeliveryMode.Offline,
            AssessmentDifficultyBand.AtClassLevel);

        var questions = new[]
        {
            new AssessmentQuestionItem(
                q1,
                "Question one",
                2m,
                1,
                []),
            new AssessmentQuestionItem(
                q2,
                "Question two",
                3m,
                2,
                [])
        };

        var students = new[]
        {
            new AssessmentStudentResultItem(
                student1,
                "STU-001",
                "A Student",
                null,
                0m,
                0m,
                null,
                new Dictionary<Guid, decimal>()),
            new AssessmentStudentResultItem(
                student2,
                "STU-002",
                "B Student",
                null,
                0m,
                0m,
                null,
                new Dictionary<Guid, decimal>())
        };

        var classItem = new AssessmentClassItem(
            classId,
            yearId,
            Guid.NewGuid(),
            "Grade 6 A",
            "G6-A");

        return (
            new AssessmentResultsWorkspace(
                assessment,
                questions,
                students),
            classItem);
    }

    private static byte[] SetInlineCell(
        byte[] source,
        string reference,
        string value)
    {
        using var stream = new MemoryStream();
        stream.Write(source);
        stream.Position = 0;

        using (var archive = new ZipArchive(
                   stream,
                   ZipArchiveMode.Update,
                   leaveOpen: true))
        {
            var entry = archive.GetEntry("xl/worksheets/sheet1.xml");
            Assert.NotNull(entry);

            XDocument document;
            using (var entryStream = entry!.Open())
            {
                document = XDocument.Load(entryStream);
            }

            var cell = document
                .Descendants(SpreadsheetNs + "c")
                .Single(x =>
                    string.Equals(
                        (string?)x.Attribute("r"),
                        reference,
                        StringComparison.Ordinal));

            cell.SetAttributeValue("t", "inlineStr");
            cell.RemoveNodes();
            cell.Add(
                new XElement(
                    SpreadsheetNs + "is",
                    new XElement(
                        SpreadsheetNs + "t",
                        new XAttribute(XNamespace.Xml + "space", "preserve"),
                        value)));

            entry.Delete();
            var replacement = archive.CreateEntry(
                "xl/worksheets/sheet1.xml",
                CompressionLevel.Fastest);
            using var replacementStream = replacement.Open();
            document.Save(replacementStream);
        }

        return stream.ToArray();
    }

    private static XDocument LoadXml(
        ZipArchive archive,
        string path)
    {
        var entry = archive.GetEntry(path);
        Assert.NotNull(entry);

        using var stream = entry!.Open();
        return XDocument.Load(stream);
    }
}
