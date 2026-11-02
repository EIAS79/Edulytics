using Edulytics.Core.Constants;
using Edulytics.Core.Enums;
using Edulytics.Services.Imports;
using Edulytics.Web.Imports;

namespace Edulytics.Tests.Acceptance;

public sealed class AssessmentResultsBulkImportRestorationTests
{
    [Fact]
    public void Teacher_bulk_import_restores_assessment_results_only()
    {
        Assert.True(DataImportService.CanImportType(
            RoleNames.Teacher,
            ImportType.AssessmentResults));

        Assert.False(DataImportService.CanImportType(
            RoleNames.Teacher,
            ImportType.Students));
        Assert.False(DataImportService.CanImportType(
            RoleNames.Teacher,
            ImportType.Teachers));
        Assert.False(DataImportService.CanImportType(
            RoleNames.Teacher,
            ImportType.Classes));
        Assert.False(DataImportService.CanImportType(
            RoleNames.Teacher,
            ImportType.SubjectSupervisors));
    }

    [Fact]
    public void Subject_supervisor_bulk_import_permissions_are_unchanged()
    {
        Assert.True(DataImportService.CanImportType(
            RoleNames.SubjectSupervisor,
            ImportType.Students));
        Assert.True(DataImportService.CanImportType(
            RoleNames.SubjectSupervisor,
            ImportType.Teachers));
        Assert.True(DataImportService.CanImportType(
            RoleNames.SubjectSupervisor,
            ImportType.Classes));

        Assert.False(DataImportService.CanImportType(
            RoleNames.SubjectSupervisor,
            ImportType.AssessmentResults));
        Assert.False(DataImportService.CanImportType(
            RoleNames.SubjectSupervisor,
            ImportType.SubjectSupervisors));
    }

    [Fact]
    public void Generic_bulk_import_exposes_the_pre_247_assessment_results_template()
    {
        Assert.True(MathOnlyImportAdapter.IsSupported(
            ImportType.AssessmentResults));

        var options = MathOnlyImportAdapter.FilterOptions(
            [new ImportTypeOption(
                ImportType.AssessmentResults,
                [
                    "AssessmentId",
                    "ClassCode",
                    "StudentNumber",
                    "QuestionOrder",
                    "Score"
                ])]);

        var option = Assert.Single(options);
        Assert.Equal(ImportType.AssessmentResults, option.Type);
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
            option.RequiredHeaders);
    }
}
