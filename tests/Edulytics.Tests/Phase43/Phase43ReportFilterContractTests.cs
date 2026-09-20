using Edulytics.Core.Reports;
using Edulytics.Services.Reports;

namespace Edulytics.Tests.Phase43;

public sealed class Phase43ReportFilterContractTests
{
    [Fact]
    public void Phase43_ReportRequestPolicy_NormalizesFiltersByReportKind()
    {
        var yearId = Guid.NewGuid();
        var classId = Guid.NewGuid();
        var subjectId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var outcomeId = Guid.NewGuid();

        var classRequest = ReportRequestPolicy.Normalize(
            new ReportRequest(
                ReportKind.Class,
                yearId,
                classId,
                subjectId,
                studentId,
                outcomeId));

        Assert.Equal(yearId, classRequest.AcademicYearId);
        Assert.Equal(classId, classRequest.ClassGroupId);
        Assert.Null(classRequest.SubjectId);
        Assert.Null(classRequest.StudentProfileId);
        Assert.Null(classRequest.LearningOutcomeId);

        var subjectRequest = ReportRequestPolicy.Normalize(
            new ReportRequest(
                ReportKind.Subject,
                yearId,
                classId,
                subjectId,
                studentId,
                outcomeId));

        Assert.Equal(yearId, subjectRequest.AcademicYearId);
        Assert.Equal(classId, subjectRequest.ClassGroupId);
        Assert.Equal(subjectId, subjectRequest.SubjectId);
        Assert.Null(subjectRequest.StudentProfileId);
        Assert.Null(subjectRequest.LearningOutcomeId);

        var studentRequest = ReportRequestPolicy.Normalize(
            new ReportRequest(
                ReportKind.Student,
                yearId,
                classId,
                subjectId,
                studentId,
                outcomeId));

        Assert.Equal(yearId, studentRequest.AcademicYearId);
        Assert.Equal(classId, studentRequest.ClassGroupId);
        Assert.Null(studentRequest.SubjectId);
        Assert.Equal(studentId, studentRequest.StudentProfileId);
        Assert.Null(studentRequest.LearningOutcomeId);

        var outcomeRequest = ReportRequestPolicy.Normalize(
            new ReportRequest(
                ReportKind.LearningOutcome,
                yearId,
                classId,
                subjectId,
                studentId,
                outcomeId));

        Assert.Equal(yearId, outcomeRequest.AcademicYearId);
        Assert.Equal(classId, outcomeRequest.ClassGroupId);
        Assert.Equal(subjectId, outcomeRequest.SubjectId);
        Assert.Null(outcomeRequest.StudentProfileId);
        Assert.Equal(outcomeId, outcomeRequest.LearningOutcomeId);
    }

    [Fact]
    public void Phase43_ReportRequestPolicy_RequiresRelationalHierarchy()
    {
        var yearId = Guid.NewGuid();
        var classId = Guid.NewGuid();
        var subjectId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var outcomeId = Guid.NewGuid();

        Assert.False(ReportRequestPolicy.HasRequiredSelection(
            new ReportRequest(ReportKind.Class, ClassGroupId: classId)));
        Assert.True(ReportRequestPolicy.HasRequiredSelection(
            new ReportRequest(ReportKind.Class, yearId, classId)));

        Assert.False(ReportRequestPolicy.HasRequiredSelection(
            new ReportRequest(ReportKind.Subject, yearId, classId)));
        Assert.True(ReportRequestPolicy.HasRequiredSelection(
            new ReportRequest(
                ReportKind.Subject,
                yearId,
                classId,
                subjectId)));

        Assert.False(ReportRequestPolicy.HasRequiredSelection(
            new ReportRequest(ReportKind.Student, yearId, classId)));
        Assert.True(ReportRequestPolicy.HasRequiredSelection(
            new ReportRequest(
                ReportKind.Student,
                yearId,
                classId,
                StudentProfileId: studentId)));

        Assert.False(ReportRequestPolicy.HasRequiredSelection(
            new ReportRequest(
                ReportKind.LearningOutcome,
                yearId,
                classId,
                LearningOutcomeId: outcomeId)));
        Assert.True(ReportRequestPolicy.HasRequiredSelection(
            new ReportRequest(
                ReportKind.LearningOutcome,
                yearId,
                classId,
                subjectId,
                LearningOutcomeId: outcomeId)));
    }

    [Fact]
    public void Phase43_ReportRequestPolicy_ExposesCascadingFilters()
    {
        Assert.True(ReportRequestPolicy.UsesAcademicYear(ReportKind.Subject));
        Assert.True(ReportRequestPolicy.UsesClass(ReportKind.Subject));
        Assert.True(ReportRequestPolicy.UsesSubject(ReportKind.Subject));

        Assert.True(ReportRequestPolicy.UsesAcademicYear(ReportKind.LearningOutcome));
        Assert.True(ReportRequestPolicy.UsesClass(ReportKind.LearningOutcome));
        Assert.True(ReportRequestPolicy.UsesSubject(ReportKind.LearningOutcome));
        Assert.True(ReportRequestPolicy.UsesLearningOutcome(ReportKind.LearningOutcome));
    }

    [Fact]
    public void Phase43_ReportService_UsesAssignmentsAndEnrollmentAsScopeSources()
    {
        var root = FindRoot();
        var service = File.ReadAllText(Path.Combine(
            root,
            "src",
            "Edulytics.Services",
            "Reports",
            "Phase43ReportQueryService.cs"));

        Assert.Contains("ListActiveBySupervisorAsync", service);
        Assert.Contains("projection.TeacherAssignments", service);
        Assert.Contains("context.Source.StudentEnrollments", service);
        Assert.Contains("context.Source.StudentProfiles", service);
        Assert.Contains("BuildScopedCatalog", service);
    }

    [Fact]
    public void Phase43_ReportService_BuildsDistinctReportKinds()
    {
        var root = FindRoot();
        var service = File.ReadAllText(Path.Combine(
            root,
            "src",
            "Edulytics.Services",
            "Reports",
            "Phase43ReportQueryService.cs"));

        Assert.Contains("BuildPerformanceOverview", service);
        Assert.Contains("BuildClassReport", service);
        Assert.Contains("BuildSubjectReport", service);
        Assert.Contains("BuildStudentReport", service);
        Assert.Contains("BuildLearningOutcomeReport", service);
    }

    [Fact]
    public void Phase43_ReportView_UsesDynamicCascadingFilterContract()
    {
        var root = FindRoot();
        var view = File.ReadAllText(Path.Combine(
            root,
            "src",
            "Edulytics.Web",
            "Views",
            "Reports",
            "Index.cshtml"));

        Assert.Contains("ShowAcademicYear(Model.Request.Kind)", view);
        Assert.Contains("ShowClass(Model.Request.Kind)", view);
        Assert.Contains("ShowSubject(Model.Request.Kind)", view);
        Assert.Contains("ShowStudent(Model.Request.Kind)", view);
        Assert.Contains("ShowLearningOutcome(Model.Request.Kind)", view);
        Assert.Contains("data-report-kind-filter", view);
        Assert.Contains("data-report-cascade=\"year\"", view);
        Assert.Contains("data-report-cascade=\"class\"", view);
        Assert.Contains("data-report-cascade=\"subject\"", view);
        Assert.Contains("clear(classGroup)", view);
        Assert.Contains("clear(subject)", view);
        Assert.Contains("clear(student)", view);
        Assert.Contains("clear(outcome)", view);
        Assert.Contains("form.requestSubmit()", view);

        var siteJs = File.ReadAllText(Path.Combine(
            root,
            "src",
            "Edulytics.Web",
            "wwwroot",
            "js",
            "site.js"));

        Assert.Contains("wireReportKindFilters", siteJs);
    }

    [Fact]
    public void Phase43_StudentMasteryView_UsesCompactOutcomeScorePresentation()
    {
        var root = FindRoot();
        var view = File.ReadAllText(Path.Combine(
            root,
            "src",
            "Edulytics.Web",
            "Views",
            "Reports",
            "Index.cshtml"));

        Assert.Contains("report-student-mastery-table", view);
        Assert.Contains("selectedStudentName", view);
        Assert.Contains("fullCode.LastIndexOf(':')", view);
        Assert.Contains("row.Cells[3]", view);
        Assert.Contains("row.Cells[4]", view);
        Assert.Contains("row.Cells[5]", view);
        Assert.Contains("Learning-outcome codes are shown only as curriculum references", view);
    }

    [Fact]
    public void Phase43_PendingExports_ShowGeneratingStateAndRefresh()
    {
        var root = FindRoot();
        var view = File.ReadAllText(Path.Combine(
            root,
            "src",
            "Edulytics.Web",
            "Views",
            "Reports",
            "Index.cshtml"));

        Assert.Contains("hasPendingExports", view);
        Assert.Contains("ReportExportJobStatus.Pending", view);
        Assert.Contains("Generating…", view);
        Assert.Contains("window.location.reload()", view);
    }

    [Fact]
    public void Phase43_LegacySubjectExportCompatibility_RemainsScopeBound()
    {
        var root = FindRoot();
        var service = File.ReadAllText(Path.Combine(
            root,
            "src",
            "Edulytics.Services",
            "Reports",
            "ReportExportService.cs"));

        Assert.Contains("IsAuthorizedLegacySubjectExport", service);
        Assert.Contains("job.ReportKind != ReportKind.Subject", service);
        Assert.Contains("job.ClassGroupId.HasValue", service);
        Assert.Contains("job.AcademicYearId.HasValue", service);
        Assert.Contains("job.SubjectId.HasValue", service);
        Assert.Contains("catalog.AllowedKinds.Contains(ReportKind.Subject)", service);
        Assert.Contains("catalog.AcademicYears.Any", service);
        Assert.Contains("catalog.Subjects.Any", service);
    }

    [Fact]
    public void Phase43_PublicTrust_UsesApprovedTechnicalAssuranceCopy()
    {
        var root = FindRoot();
        var trustJs = File.ReadAllText(Path.Combine(
            root,
            "src",
            "Edulytics.Web",
            "wwwroot",
            "js",
            "public-trust-v1.js"));

        Assert.Contains("Technical assurance by OUR-CS Software", trustJs);
        Assert.Contains(
            "Edulytics has undergone structured testing and technical review by OUR-CS Software, covering security, performance, reliability, and operational stability.",
            trustJs);
        Assert.DoesNotContain("Certification", trustJs, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Independent Audit", trustJs, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Phase43_CountingGrove_UsesCurrentApprovedCharacterDirectly()
    {
        var root = FindRoot();
        var runtime = File.ReadAllText(Path.Combine(
            root,
            "src",
            "Edulytics.Web",
            "wwwroot",
            "js",
            "count-touch-preview.js"));
        var gameView = File.ReadAllText(Path.Combine(
            root,
            "src",
            "Edulytics.Web",
            "Views",
            "StudentPractice",
            "Game.cshtml"));
        var obsoleteOverride = Path.Combine(
            root,
            "src",
            "Edulytics.Web",
            "wwwroot",
            "js",
            "count-touch-eddy-hint-v1.js");

        Assert.Contains("guide: '/images/public/edulaytiks-character.png?v=43'", runtime);
        Assert.DoesNotContain("/images/game/v9/eddy-hint.webp", runtime);\n        Assert.DoesNotContain("count-touch-eddy-hint-v1.js", gameView);
        Assert.False(File.Exists(obsoleteOverride));
    }

    private static string FindRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null &&
               !File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
        {
            directory = directory.Parent;
        }

        return directory?.FullName
            ?? throw new InvalidOperationException("Repository root not found.");
    }
}
