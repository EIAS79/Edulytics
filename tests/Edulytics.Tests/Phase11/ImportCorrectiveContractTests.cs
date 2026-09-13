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
