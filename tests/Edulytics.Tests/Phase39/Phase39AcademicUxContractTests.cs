using System.Text;
using Edulytics.Core.Enums;
using Edulytics.Core.Curriculum;
using Edulytics.Services.Assessments;
using Edulytics.Services.Imports;
using Edulytics.Web.Imports;

namespace Edulytics.Tests.Phase39;

public sealed class Phase39AcademicUxContractTests
{
    [Fact]
    public void ClassLabels_AreCurriculumAwareAndSharedAcrossAcademicUx()
    {
        var root = FindRepositoryRoot();
        var query = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/Curriculum/ExplicitCurriculumLevelUiQuery.cs"));
        var studentCatalog = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/StudentSetup/StudentCreationClassCatalog.cs"));
        var studentOptions = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Controllers/StudentCreationOptionsController.cs"));

        Assert.Contains("public string DisplayLabel", query, StringComparison.Ordinal);
        Assert.Contains("AcademicProgramName", query, StringComparison.Ordinal);
        Assert.Contains("CurriculumLevelLabel", query, StringComparison.Ordinal);
        Assert.Contains("CurriculumPathway", query, StringComparison.Ordinal);
        Assert.Contains("CurriculumAdoptionId", studentCatalog, StringComparison.Ordinal);
        Assert.Contains("GetAdoptedCurriculumContextsAsync", studentCatalog, StringComparison.Ordinal);
        Assert.Contains("CurriculumDisplayLabel", studentCatalog, StringComparison.Ordinal);
        Assert.Contains("label = x.DisplayLabel", studentOptions, StringComparison.Ordinal);
    }


    [Fact]
    public void CambridgeCoreAndExtended_AreDistinctTracksWithExplicitUiLabels()
    {
        var level10 = CurriculumLevelIdentityRegistry
            .ForPack(MathematicsCurriculumPackRegistry.CambridgeCode)
            .Where(x => x.LogicalLevel == 10)
            .ToArray();

        var core = Assert.Single(level10, x => x.Pathway == "Core");
        var extended = Assert.Single(level10, x => x.Pathway == "Extended");

        Assert.NotEqual(core.Key, extended.Key);
        Assert.NotEqual(core.DisplayLabel, extended.DisplayLabel);
        Assert.Contains("Core", core.DisplayLabel, StringComparison.Ordinal);
        Assert.Contains("Extended", extended.DisplayLabel, StringComparison.Ordinal);
        Assert.Contains("level 10", core.DisplayLabel, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("level 10", extended.DisplayLabel, StringComparison.OrdinalIgnoreCase);

        var root = FindRepositoryRoot();
        var view = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/AcademicStructure/Index.cshtml"));

        Assert.Contains("@level.DisplayLabel", view, StringComparison.Ordinal);
        Assert.DoesNotContain(
            "@level.Label@(string.IsNullOrWhiteSpace(level.Pathway)",
            view,
            StringComparison.Ordinal);
    }


    [Fact]
    public void TeacherAssignment_EnforcesSupervisorSubjectBoundaryServerSide()
    {
        var root = FindRepositoryRoot();
        var service = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/Curriculum/ExplicitCurriculumLevelService.cs"));

        Assert.Contains(
            "ISubjectSupervisorAssignmentRepository?",
            service,
            StringComparison.Ordinal);
        Assert.Contains(
            "ListActiveBySupervisorAsync",
            service,
            StringComparison.Ordinal);
        Assert.Contains(
            "x.SubjectId == adoption.SubjectId",
            service,
            StringComparison.Ordinal);
        Assert.Contains(
            "ExplicitCurriculumLevelErrorCode.AccessDenied",
            service,
            StringComparison.Ordinal);
    }

    [Fact]
    public void TeacherAssignments_SupportMultipleClassesWithoutExposingSubjectChoice()
    {
        var root = FindRepositoryRoot();
        var controller = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Controllers/Phase39AcademicRelationshipsController.cs"));
        var javascript = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/js/site.js"));

        Assert.Contains("Guid[]? classGroupIds", controller, StringComparison.Ordinal);
        Assert.Contains("selectedClassIds", controller, StringComparison.Ordinal);
        Assert.Contains("activeClassIds", controller, StringComparison.Ordinal);
        Assert.Contains("AssignTeacherAsync", controller, StringComparison.Ordinal);
        Assert.Contains("teacherClass.multiple = true", javascript, StringComparison.Ordinal);
        Assert.Contains("teacherClass.name = \"classGroupIds\"", javascript, StringComparison.Ordinal);
        Assert.Contains("subjectCell.hidden = true", javascript, StringComparison.Ordinal);
    }

    [Fact]
    public void TeacherAssignmentDirectory_UsesServerSearchAndStableCurriculumLevelKeys()
    {
        var root = FindRepositoryRoot();
        var controller = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Controllers/SchoolUsersController.cs"));
        var view = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/AcademicStructure/Index.cshtml"));
        var javascript = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/js/site.js"));

        Assert.Contains("options/teachers", controller, StringComparison.Ordinal);
        Assert.Contains("Role: RoleNames.Teacher", controller, StringComparison.Ordinal);
        Assert.Contains("data-level-key", view, StringComparison.Ordinal);
        Assert.Contains("CurriculumLevelKey", view, StringComparison.Ordinal);
        Assert.Contains("/School/Users/options/teachers", javascript, StringComparison.Ordinal);
        Assert.Contains("wireTeacherAssignmentDirectory", javascript, StringComparison.Ordinal);
        Assert.Contains("option.dataset.levelKey", javascript, StringComparison.Ordinal);
    }

    [Fact]
    public void StudentCreation_UsesUserManagementWithoutLegacyEnrollmentForms()
    {
        var root = FindRepositoryRoot();
        var view = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/AcademicStructure/Index.cshtml"));
        var javascript = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/js/site.js"));
        var filter = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Filters/DirectStudentCreationFilter.cs"));

        Assert.Contains("ManageStudentAccounts", view, StringComparison.Ordinal);
        Assert.DoesNotContain("asp-action=\"CreateStudentEnrollment\"", view, StringComparison.Ordinal);
        Assert.DoesNotContain("id=\"enroll-student\"", view, StringComparison.Ordinal);
        Assert.DoesNotContain("id=\"enroll-class\"", view, StringComparison.Ordinal);
        Assert.DoesNotContain("Change student class enrollment", javascript, StringComparison.Ordinal);
        Assert.DoesNotContain("enrollmentClass", javascript, StringComparison.Ordinal);
        Assert.Contains("_transactions.BeginAsync", filter, StringComparison.Ordinal);
        Assert.Contains("ConvertToStudentAsync", filter, StringComparison.Ordinal);
        Assert.Contains("RollbackAsync", filter, StringComparison.Ordinal);
        Assert.Contains("CommitAsync", filter, StringComparison.Ordinal);
    }

    [Fact]
    public void ClassOverview_UsesCompactResponsiveCardsAndModalDetails()
    {
        var root = FindRepositoryRoot();
        var view = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/AcademicStructure/Index.cshtml"));
        var javascript = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/js/site.js"));
        var css = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/css/site.css"));

        Assert.Contains("academic-class-browser-card-modern", view, StringComparison.Ordinal);
        Assert.Contains("data-class-details-open", view, StringComparison.Ordinal);
        Assert.Contains("academic-class-details-dialog", view, StringComparison.Ordinal);
        Assert.Contains("mailto:@primaryTeacher", view, StringComparison.Ordinal);
        Assert.Contains("mailto:@teacherEmail", view, StringComparison.Ordinal);
        Assert.DoesNotContain("<details class=\"academic-class-browser-card\"", view, StringComparison.Ordinal);

        Assert.Contains("wireClassOverviewDialogs", javascript, StringComparison.Ordinal);
        Assert.Contains("showModal()", javascript, StringComparison.Ordinal);

        Assert.Contains("repeat(auto-fit, minmax(min(100%, 18.75rem), 1fr))", css, StringComparison.Ordinal);
        Assert.Contains("text-overflow: ellipsis", css, StringComparison.Ordinal);
        Assert.Contains(".academic-class-details-dialog", css, StringComparison.Ordinal);
    }

    [Fact]
    public void StudentMovement_RequiresExplicitSourceSelectionDestinationAndReview()
    {
        var root = FindRepositoryRoot();
        var view = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/AcademicStructure/Index.cshtml"));
        var controller = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Controllers/AcademicStructureBulkController.cs"));
        var service = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/Academics/StudentPlacementService.cs"));

        Assert.Contains("id=\"move-source-class\"", view, StringComparison.Ordinal);
        Assert.Contains("name=\"sourceClassGroupId\"", view, StringComparison.Ordinal);
        Assert.Contains("name=\"studentProfileIds\"", view, StringComparison.Ordinal);
        Assert.Contains("id=\"move-target-class\"", view, StringComparison.Ordinal);
        Assert.Contains("id=\"student-move-review\"", view, StringComparison.Ordinal);
        Assert.Contains("student-move-dialog", view, StringComparison.Ordinal);
        Assert.Contains("academic-student-move-modern", view, StringComparison.Ordinal);
        Assert.Contains("student-move-review-students", view, StringComparison.Ordinal);
        Assert.Contains("data-level-key", view, StringComparison.Ordinal);
        Assert.Contains("@A[\"ConfirmMove\"]", view, StringComparison.Ordinal);
        Assert.Contains("SelectAllVisible", view, StringComparison.Ordinal);
        Assert.Contains("ManageStudentAccounts", view, StringComparison.Ordinal);
        Assert.DoesNotContain("asp-action=\"CreateStudentEnrollment\"", view, StringComparison.Ordinal);

        Assert.Contains("Guid sourceClassGroupId", controller, StringComparison.Ordinal);
        Assert.Contains("MoveStudentsAsync(", controller, StringComparison.Ordinal);

        Assert.Contains("sourceClassGroupId == targetClassGroupId", service, StringComparison.Ordinal);
        Assert.Contains("CrossAcademicYearMoveNotAllowed", service, StringComparison.Ordinal);
        Assert.Contains("CrossGradeMoveNotAllowed", service, StringComparison.Ordinal);
        Assert.Contains("CrossCurriculumMoveNotAllowed", service, StringComparison.Ordinal);
        Assert.Contains("StudentNotInSourceClass", service, StringComparison.Ordinal);
        Assert.Contains("if (failures.Count > 0)", service, StringComparison.Ordinal);
        Assert.Contains("foreach (var enrollment in enrollments)", service, StringComparison.Ordinal);
        Assert.Contains("A reviewed bulk move is atomic", service, StringComparison.Ordinal);
        Assert.DoesNotContain(
            "AddEnrollmentAsync(\n                    new StudentEnrollment",
            service[
                service.IndexOf(
                    "public async Task<StudentPlacementResult> MoveStudentsAsync(",
                    StringComparison.Ordinal)..],
            StringComparison.Ordinal);
    }

    [Fact]
    public void DataImport_ExposesOnlyProductImportTypes()
    {
        Assert.True(MathOnlyImportAdapter.IsSupported(ImportType.Students));
        Assert.True(MathOnlyImportAdapter.IsSupported(ImportType.Teachers));
        Assert.True(MathOnlyImportAdapter.IsSupported(ImportType.SubjectSupervisors));
        Assert.True(MathOnlyImportAdapter.IsSupported(ImportType.Classes));
        Assert.True(MathOnlyImportAdapter.IsSupported(ImportType.AssessmentResults));
        Assert.False(MathOnlyImportAdapter.IsSupported(ImportType.Subjects));
        Assert.False(MathOnlyImportAdapter.IsSupported(ImportType.CurriculumMappings));

        var teacherHeaders = MathOnlyImportAdapter.TemplateHeaders(
            ImportType.Teachers,
            ["Email", "AcademicYear", "ClassCode", "SubjectCode"]);

        Assert.Equal(
            ["Email", "AcademicYear", "ClassName"],
            teacherHeaders);
    }

    [Fact]
    public void TeacherImport_ResolvesVisibleClassNameAndForcesMathematicsInternally()
    {
        const string csv =
            "Email,AcademicYear,ClassName\n" +
            "teacher@example.com,2026-2027,BG1\n";

        var workspace = new AssessmentWorkspace(
            [],
            [],
            [new AssessmentClassItem(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                "BG1",
                "B-1")],
            []);

        var adapted = MathOnlyImportAdapter.NormalizeUpload(
            ImportType.Teachers,
            "teachers.csv",
            Encoding.UTF8.GetBytes(csv),
            workspace);

        var parsed = new ImportFileParser().Parse(
            adapted.FileName,
            adapted.Bytes);

        Assert.True(parsed.Succeeded);
        Assert.Contains("ClassCode", parsed.File!.Headers);
        Assert.Contains("SubjectCode", parsed.File.Headers);
        Assert.Equal("B-1", parsed.File.Rows[0].Values["ClassCode"]);
        Assert.Equal("MATH", parsed.File.Rows[0].Values["SubjectCode"]);
    }

    [Fact]
    public void LegacyImportHistory_IsReadOnlyInTheDetailsUx()
    {
        var root = FindRepositoryRoot();
        var viewModel = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/ViewModels/Imports/ImportViewModels.cs"));
        var view = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Imports/Details.cshtml"));

        Assert.Contains("IsLegacyReadOnly", viewModel, StringComparison.Ordinal);
        Assert.Contains("MathOnlyImportAdapter.IsSupported", viewModel, StringComparison.Ordinal);
        Assert.Contains("public bool CanConfirm", viewModel, StringComparison.Ordinal);
        Assert.Contains("Model.CanConfirm", view, StringComparison.Ordinal);
        Assert.Contains("Model.IsLegacyReadOnly", view, StringComparison.Ordinal);
        Assert.Contains("ImportReadOnlyNotice", view, StringComparison.Ordinal);
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
                return directory.FullName;

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException(
            "Edulytics repository root not found.");
    }
}
