using System.Reflection;
using System.Text;
using Edulytics.Core.Analytics;
using Edulytics.Services.Analytics;
using Edulytics.Web.Controllers;
using Edulytics.Web.Printing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Tests.Phase44;

public sealed class EvaluationVisualReportingTests
{
    [Fact]
    public void ChartSystem_IsDependencyFreeAnimatedAndReducedMotionAware()
    {
        var root = FindRoot();
        var script = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/js/evaluation-charts.js"));
        var css = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/css/site.css"));

        Assert.Contains("data-evaluation-chart", script);
        Assert.Contains("IntersectionObserver", script);
        Assert.Contains("prefers-reduced-motion", script);
        Assert.Contains("case \"donut\"", script);
        Assert.Contains("case \"line\"", script);
        Assert.Contains("case \"waterfall\"", script);

        Assert.DoesNotContain("chart.js", script, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("apexcharts", script, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("echarts", script, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("https://", script, StringComparison.OrdinalIgnoreCase);

        Assert.Contains(".evaluation-donut", css);
        Assert.Contains(".evaluation-lollipop-list", css);
        Assert.Contains(".evaluation-line-chart", css);
        Assert.Contains(".evaluation-waterfall", css);
        Assert.Contains("@media (prefers-reduced-motion: reduce)", css);
    }

    [Fact]
    public void StaffAnalytics_ExposeVisualsAndReportActions()
    {
        var root = FindRoot();
        var dashboard = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Analytics/_AnalyticsDashboardResults.cshtml"));
        var student = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Analytics/Student.cshtml"));
        var students = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Analytics/Students.cshtml"));
        var topics = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Analytics/TopicsSkills.cshtml"));
        var supervisor = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Analytics/SubjectOverview.cshtml"));

        Assert.Contains("analytics-overview-layout", dashboard);
        Assert.Contains("data-evaluation-chart=\"donut\"", dashboard);
        Assert.Contains("data-evaluation-chart=\"lollipop\"", dashboard);
        Assert.Contains("data-evaluation-chart=\"line\"", dashboard);
        Assert.DoesNotContain("analytics-metrics", dashboard);

        Assert.Contains("data-evaluation-chart=\"donut\"", student);
        Assert.Contains("data-evaluation-chart=\"line\"", student);
        Assert.Contains("data-evaluation-chart=\"waterfall\"", student);
        Assert.Contains("asp-action=\"StudentReport\"", student);

        Assert.Contains("MasteryDistribution", students);
        Assert.Contains("evaluation-student-list", students);
        Assert.Contains("asp-action=\"StudentReport\"", students);
        Assert.Contains("asp-action=\"StudentReportPdf\"", students);
        Assert.Contains("asp-action=\"ClassReportPdf\"", students);

        Assert.Contains("data-evaluation-chart=\"bars\"", topics);
        Assert.Contains("ClassComparisonChart", supervisor);
        Assert.Contains("PriorityGapChart", supervisor);
        Assert.Contains("asp-action=\"ClassReportPdf\"", supervisor);
    }

    [Fact]
    public void StaffStudentReport_UsesExistingAnalyticsAuthorizationAndTermFocus()
    {
        var controllerType = typeof(AnalyticsController);
        var controllerAuthorization = controllerType
            .GetCustomAttributes<AuthorizeAttribute>()
            .Single();

        Assert.Equal("AnalyticsRead", controllerAuthorization.Policy);

        var method = controllerType.GetMethod(
            nameof(AnalyticsController.StudentReport));

        Assert.NotNull(method);
        Assert.Contains(
            method!.GetCustomAttributes<HttpGetAttribute>(),
            x => x.Template == "student/{studentProfileId:guid}/report");

        var parameters = method.GetParameters()
            .Select(x => x.Name)
            .ToArray();

        Assert.Contains("studentProfileId", parameters);
        Assert.Contains("academicYearId", parameters);
        Assert.Contains("classGroupId", parameters);
        Assert.Contains("subjectId", parameters);
        Assert.Contains("termId", parameters);

        var root = FindRoot();
        var registration = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Extensions/AnalyticsRegistrationExtensions.cs"));

        Assert.Contains("RoleNames.SchoolAdmin", registration);
        Assert.Contains("RoleNames.SubjectSupervisor", registration);
        Assert.Contains("RoleNames.Teacher", registration);
    }

    [Fact]
    public void StudentSelfReport_HasNoStudentSelectorAndKeepsPrivatePracticeSeparate()
    {
        var report = typeof(StudentPortalController)
            .GetMethod(nameof(StudentPortalController.ProgressReport));
        var pdf = typeof(StudentPortalController)
            .GetMethod(nameof(StudentPortalController.ProgressReportPdf));

        Assert.NotNull(report);
        Assert.NotNull(pdf);

        foreach (var method in new[] { report!, pdf! })
        {
            Assert.DoesNotContain(
                method.GetParameters(),
                parameter => string.Equals(
                    parameter.Name,
                    "studentProfileId",
                    StringComparison.OrdinalIgnoreCase));
        }

        var root = FindRoot();
        var controller = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Controllers/StudentPortalController.cs"));
        var view = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/StudentPortal/ProgressReport.cshtml"));

        Assert.Contains("ResolveProgressContextAsync(", controller);
        Assert.Contains("_selfEvaluation.GetAsync(", controller);
        Assert.Contains("PrivatePractice", view);
        Assert.Contains("AssessmentMastery", view);
        Assert.Contains("PracticeAssessmentGap", view);
        Assert.Contains("StudentReflection", view);
        Assert.Contains("ParentReview", view);
    }

    [Fact]
    public void ReportViews_AreBrandedPrintableAndContainReviewPlaceholders()
    {
        var root = FindRoot();
        var staff = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Analytics/StudentReport.cshtml"));
        var student = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/StudentPortal/ProgressReport.cshtml"));
        var css = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/css/site.css"));

        Assert.Contains("_LocalizedBrand", staff);
        Assert.Contains("data-report-print", staff);
        Assert.Contains("TeacherComments", staff);
        Assert.Contains("ParentStudentReview", staff);
        Assert.Contains("asp-route-termId", staff);

        Assert.Contains("_LocalizedBrand", student);
        Assert.Contains("data-report-print", student);
        Assert.Contains("ProgressReportPdf", student);
        Assert.Contains("asp-route-termId", student);

        var script = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/js/evaluation-charts.js"));

        Assert.Contains("window.print()", script);
        Assert.Contains("data-report-autosubmit", script);

        Assert.Contains("@media print", css);
        Assert.Contains(".evaluation-report-sheet", css);
        Assert.Contains(".evaluation-report-writing-space", css);
    }

    [Fact]
    public void TermIdentity_IsCarriedIntoAssessmentHistory()
    {
        var root = FindRoot();
        var contracts = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/Analytics/LearningEvaluationViewContracts.cs"));
        var staff = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/Analytics/AnalyticsService.cs"));
        var self = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/Analytics/StudentSelfEvaluationService.cs"));

        Assert.Contains("Guid? TermId = null", contracts);
        Assert.Contains("current.Assessment.TermId", staff);
        Assert.Contains("current.Assessment.TermId", self);
    }

    [Fact]
    public void BrandedStaffReportPdf_RendersFromTermAwareReportModel()
    {
        var evaluation = MinimalEvaluation();
        var source = new AnalyticsStudentEvaluationPage(
            evaluation,
            [],
            [],
            [],
            new AnalyticsPracticeEvaluationSummary(
                0,
                0,
                0,
                0,
                null,
                EvaluationTrendBand.InsufficientEvidence,
                null),
            [],
            []);

        var page = new AnalyticsStudentEvaluationReportPage(
            source,
            null,
            null,
            null,
            [],
            [],
            DateTime.UtcNow);

        var bytes = AnalyticsPdfRenderer.RenderStudentEvaluationReport(page);

        Assert.True(bytes.Length > 1000);
        Assert.Equal(
            "%PDF",
            Encoding.ASCII.GetString(bytes, 0, 4));
    }

    [Fact]
    public void BrandedStudentSelfReportPdf_RendersWithoutAnotherStudentId()
    {
        var evaluation = MinimalEvaluation();
        var source = new StudentSelfEvaluationPage(
            evaluation,
            new StudentSelfPracticeSummary(
                2,
                10,
                2,
                1,
                58m,
                EvaluationTrendBand.Stable,
                DateTime.UtcNow),
            -7m,
            [],
            [],
            [],
            []);

        var page = new StudentSelfEvaluationReportPage(
            source,
            null,
            null,
            null,
            [],
            DateTime.UtcNow);

        var bytes =
            AnalyticsPdfRenderer.RenderStudentSelfEvaluationReport(page);

        Assert.True(bytes.Length > 1000);
        Assert.Equal(
            "%PDF",
            Encoding.ASCII.GetString(bytes, 0, 4));
    }

    private static StudentSubjectEvaluation MinimalEvaluation() =>
        new(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "ST-001",
            "Student One",
            Guid.NewGuid(),
            "2026/2027",
            Guid.NewGuid(),
            "4A",
            Guid.NewGuid(),
            "Mathematics",
            64m,
            58m,
            73m,
            -15m,
            72m,
            10,
            7,
            80m,
            EvaluationConfidenceBand.Strong,
            EvaluationTrendBand.Improving,
            EvaluationTrendBand.Improving,
            4,
            2,
            1,
            0,
            0,
            [],
            "evaluation-v1");

    private static string FindRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null)
        {
            if (File.Exists(Path.Combine(
                    directory.FullName,
                    "Edulytics.sln")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException();
    }
}
