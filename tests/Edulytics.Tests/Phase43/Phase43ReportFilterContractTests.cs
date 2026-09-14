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

        Assert.Contains("BuildSchoolDocument", service);
        Assert.Contains("BuildClassDocument", service);
        Assert.Contains("BuildSubjectDocument", service);
        Assert.Contains("BuildStudentDocument", service);
        Assert.Contains("BuildLearningOutcomeDocument", service);
    }

    [Fact]
    public void Phase43_ReportView_UsesDynamicFilterContract()
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

        var siteJs = File.ReadAllText(Path.Combine(
            root,
            "src",
            "Edulytics.Web",
            "wwwroot",
            "js",
            "site.js"));

        Assert.Contains("wireReportKindFilters", siteJs);
        Assert.Contains("form.requestSubmit()", siteJs);
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
    public void Phase43_CountingGrove_UsesDedicatedEddyHintArtwork()
    {
        var root = FindRoot();
        var hintJs = File.ReadAllText(Path.Combine(
            root,
            "src",
            "Edulytics.Web",
            "wwwroot",
            "js",
            "count-touch-eddy-hint-v1.js"));
        var gameView = File.ReadAllText(Path.Combine(
            root,
            "src",
            "Edulytics.Web",
            "Views",
            "StudentPractice",
            "Game.cshtml"));

        Assert.Contains("/images/game/v9/eddy-hint.webp", hintJs);
        Assert.Contains("count-touch-eddy-hint-v1.js", gameView);
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
