using System.Text;
using Edulytics.Core.Enums;
using Edulytics.Services.Assessments;
using Edulytics.Services.Imports;
using Edulytics.Web.Imports;

namespace Edulytics.Tests.Phase11;

public sealed class ImportCorrectiveContractTests
{
    [Fact]
    public void ProductTemplates_UseHumanReadableCorrectiveSchemas()
    {
        Assert.Equal(
            new[]
            {
                "StudentNumber",
                "FirstName",
                "LastName",
                "Email",
                "AcademicYear",
                "ClassName"
            },
            MathOnlyImportAdapter.TemplateHeaders(
                ImportType.Students,
                []));

        Assert.Equal(
            new[]
            {
                "Email",
                "AcademicYear",
                "ClassName"
            },
            MathOnlyImportAdapter.TemplateHeaders(
                ImportType.Teachers,
                []));

        Assert.Equal(
            new[]
            {
                "Email"
            },
            MathOnlyImportAdapter.TemplateHeaders(
                ImportType.SubjectSupervisors,
                []));

        Assert.Equal(
            new[]
            {
                "AssessmentTitle",
                "AssessmentDate",
                "ClassName",
                "StudentNumber",
                "StudentName",
                "QuestionOrder",
                "Score"
            },
            MathOnlyImportAdapter.TemplateHeaders(
                ImportType.AssessmentResults,
                []));
    }


    [Fact]
    public void AssessmentResultsAdapter_ResolvesOnlyOpenOfflineAssessments()
    {
        var yearId = Guid.NewGuid();
        var classItem = new AssessmentClassItem(
            Guid.NewGuid(),
            yearId,
            Guid.NewGuid(),
            "Grade 6 A",
            "G6-A");
        var offline = new AssessmentListItem(
            Guid.NewGuid(),
            Guid.NewGuid(),
            classItem.Id,
            yearId,
            Guid.NewGuid(),
            "Fractions test",
            new DateOnly(2026, 9, 22),
            10m,
            AssessmentStatus.Open,
            [1],
            AssessmentTargetType.Class,
            null,
            AssessmentDeliveryMode.Offline);
        var online = offline with
        {
            Id = Guid.NewGuid(),
            DeliveryMode = AssessmentDeliveryMode.Online
        };
        var workspace = new AssessmentWorkspace(
            [online, offline],
            [],
            [classItem],
            []);

        var upload = MathOnlyImportAdapter.NormalizeAssessmentResults(
            "results.csv",
            Encoding.UTF8.GetBytes(
                "AssessmentTitle,AssessmentDate,ClassName,StudentNumber,StudentName,QuestionOrder,Score\n"
                + "Fractions test,2026-09-22,Grade 6 A,STU-1,Student One,1,2\n"),
            workspace);

        var parsed = new ImportFileParser().Parse(
            upload.FileName,
            upload.Bytes);

        Assert.True(parsed.Succeeded);
        var row = Assert.Single(parsed.File!.Rows);
        Assert.Equal(
            offline.Id.ToString("D"),
            row.Values["AssessmentId"]);

        var onlineOnly = workspace with
        {
            Assessments = [online]
        };
        var unresolved = MathOnlyImportAdapter.NormalizeAssessmentResults(
            "results.csv",
            Encoding.UTF8.GetBytes(
                "AssessmentTitle,AssessmentDate,ClassName,StudentNumber,StudentName,QuestionOrder,Score\n"
                + "Fractions test,2026-09-22,Grade 6 A,STU-1,Student One,1,2\n"),
            onlineOnly);
        var unresolvedParsed = new ImportFileParser().Parse(
            unresolved.FileName,
            unresolved.Bytes);

        Assert.True(unresolvedParsed.Succeeded);
        Assert.StartsWith(
            "UNRESOLVED:",
            Assert.Single(unresolvedParsed.File!.Rows).Values["AssessmentId"]);
    }

    [Fact]
    public void TeacherUpload_ResolvesClassNameAndAddsInternalMathematicsSubjectCode()
    {
        var workspace = new AssessmentWorkspace(
            [],
            [],
            [new AssessmentClassItem(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Grade 1 A",
                "G1-A")],
            []);

        var upload = MathOnlyImportAdapter.NormalizeUpload(
            ImportType.Teachers,
            "teachers.csv",
            Encoding.UTF8.GetBytes(
                "Email,AcademicYear,ClassName\nteacher@example.test,2026/2027,Grade 1 A\n"),
            workspace);

        var parsed = new ImportFileParser().Parse(
            upload.FileName,
            upload.Bytes);

        Assert.True(parsed.Succeeded);
        Assert.Contains("ClassCode", parsed.File!.Headers);
        Assert.Contains("SubjectCode", parsed.File.Headers);
        Assert.Single(parsed.File.Rows);
        Assert.Equal(
            "G1-A",
            parsed.File.Rows[0].Values["ClassCode"]);
        Assert.Equal(
            "MATH",
            parsed.File.Rows[0].Values["SubjectCode"]);
    }
}
