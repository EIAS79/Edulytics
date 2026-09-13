using System.Text;
using Edulytics.Core.Enums;
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
                "ClassCode"
            },
            MathOnlyImportAdapter.TemplateHeaders(
                ImportType.Students,
                []));

        Assert.Equal(
            new[]
            {
                "Email",
                "AcademicYear",
                "ClassCode"
            },
            MathOnlyImportAdapter.TemplateHeaders(
                ImportType.Teachers,
                []));

        Assert.Equal(
            new[]
            {
                "AssessmentTitle",
                "AssessmentDate",
                "ClassCode",
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
    public void TeacherAssignmentUpload_AddsInternalMathematicsSubjectCode()
    {
        var upload = MathOnlyImportAdapter.NormalizeUpload(
            ImportType.Teachers,
            "teachers.csv",
            Encoding.UTF8.GetBytes(
                "Email,AcademicYear,ClassCode\nteacher@example.test,2026/2027,G1-A\n"));

        var parsed = new ImportFileParser().Parse(
            upload.FileName,
            upload.Bytes);

        Assert.True(parsed.Succeeded);
        Assert.Contains("SubjectCode", parsed.File!.Headers);
        Assert.Single(parsed.File.Rows);
        Assert.Equal(
            "MATH",
            parsed.File.Rows[0].Values["SubjectCode"]);
    }
}
