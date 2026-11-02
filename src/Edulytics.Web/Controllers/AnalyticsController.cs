using System.Security.Claims;
using Edulytics.Core.Constants;
using Edulytics.Services.Analytics;
using Edulytics.Web.ViewModels.Analytics;
using Edulytics.Web.Printing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.RateLimiting;
using Edulytics.Web.Resilience;
using Microsoft.Extensions.Localization;

namespace Edulytics.Web.Controllers;

[Authorize(Policy = "AnalyticsRead")]
[Route("school/analytics")]
public sealed class AnalyticsController : Controller
{
    private readonly IAnalyticsService _analytics;
    private readonly IAnalyticsInterventionService _interventions;
    private readonly IStringLocalizer<AnalyticsResource> _text;

    public AnalyticsController(
        IAnalyticsService analytics,
        IAnalyticsInterventionService interventions,
        IStringLocalizer<AnalyticsResource> text)
    {
        _analytics = analytics;
        _interventions = interventions;
        _text = text;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(
        Guid? academicYearId,
        Guid? classGroupId,
        Guid? subjectId,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId))
            return Forbid();

        var result =
            await _analytics.GetDashboardAsync(
                actorId,
                academicYearId,
                classGroupId,
                subjectId,
                cancellationToken);

        if (result.Value is null)
            return HandleQueryError(result.Error);

        return View(
            new AnalyticsIndexViewModel(
                result.Value));
    }

    [HttpGet("students")]
    public async Task<IActionResult> Students(
        Guid academicYearId,
        Guid classGroupId,
        Guid subjectId,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId))
            return Forbid();

        if (academicYearId == Guid.Empty ||
            classGroupId == Guid.Empty ||
            subjectId == Guid.Empty)
        {
            return BadRequest();
        }

        var result = await _analytics.GetStudentsEvaluationAsync(
            actorId,
            academicYearId,
            classGroupId,
            subjectId,
            cancellationToken);

        return result.Value is null
            ? HandleQueryError(result.Error)
            : View(result.Value);
    }

    [HttpGet("student/{studentProfileId:guid}")]
    public async Task<IActionResult> Student(
        Guid studentProfileId,
        Guid academicYearId,
        Guid classGroupId,
        Guid subjectId,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId))
            return Forbid();

        if (studentProfileId == Guid.Empty ||
            academicYearId == Guid.Empty ||
            classGroupId == Guid.Empty ||
            subjectId == Guid.Empty)
        {
            return BadRequest();
        }

        var result = await _analytics.GetStudentEvaluationAsync(
            actorId,
            studentProfileId,
            academicYearId,
            classGroupId,
            subjectId,
            cancellationToken);

        return result.Value is null
            ? HandleQueryError(result.Error)
            : View(result.Value);
    }

    [HttpGet("student/{studentProfileId:guid}/report")]
    public async Task<IActionResult> StudentReport(
        Guid studentProfileId,
        Guid academicYearId,
        Guid classGroupId,
        Guid subjectId,
        Guid? termId,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId))
            return Forbid();

        if (studentProfileId == Guid.Empty ||
            academicYearId == Guid.Empty ||
            classGroupId == Guid.Empty ||
            subjectId == Guid.Empty)
        {
            return BadRequest();
        }

        var result = await _analytics.GetStudentEvaluationAsync(
            actorId,
            studentProfileId,
            academicYearId,
            classGroupId,
            subjectId,
            cancellationToken);

        if (result.Value is null)
            return HandleQueryError(result.Error);

        var report = BuildStudentReportPage(
            result.Value,
            termId);

        return report is null
            ? NotFound()
            : View(report);
    }

    [HttpGet("topics-skills")]
    public async Task<IActionResult> TopicsSkills(
        Guid academicYearId,
        Guid classGroupId,
        Guid subjectId,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId))
            return Forbid();

        if (academicYearId == Guid.Empty ||
            classGroupId == Guid.Empty ||
            subjectId == Guid.Empty)
        {
            return BadRequest();
        }

        var result = await _analytics.GetTopicSkillEvaluationAsync(
            actorId,
            academicYearId,
            classGroupId,
            subjectId,
            cancellationToken);

        return result.Value is null
            ? HandleQueryError(result.Error)
            : View(result.Value);
    }

    [Authorize(Roles = RoleNames.Teacher)]
    [HttpPost("student/{studentProfileId:guid}/intervention-check")]
    [ValidateAntiForgeryToken]
    [RequestTimeout(BackendResiliencePolicyNames.InteractiveWrite)]
    [EnableRateLimiting(BackendResiliencePolicyNames.HeavyWriteConcurrency)]
    public async Task<IActionResult> CreateInterventionCheck(
        Guid studentProfileId,
        Guid academicYearId,
        Guid classGroupId,
        Guid subjectId,
        Guid learningOutcomeId,
        string skillKey,
        int questionCount,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId))
            return Forbid();

        var result = await _interventions.CreateTargetedCheckAsync(
            actorId,
            new CreateInterventionCheckRequest(
                studentProfileId,
                academicYearId,
                classGroupId,
                subjectId,
                learningOutcomeId,
                skillKey,
                questionCount is >= 3 and <= 10
                    ? questionCount
                    : 5),
            cancellationToken);

        if (result.Value is not null)
        {
            TempData["Success"] =
                _text["SuccessInterventionDraftCreated"].Value;

            return Redirect(
                result.Value.ReviewRoute);
        }

        TempData["Error"] =
            result.Error switch
            {
                AnalyticsInterventionErrorCode.AccessDenied =>
                    _text["ErrorAccessDenied"].Value,
                AnalyticsInterventionErrorCode.SkillNotFound =>
                    _text["ErrorInterventionSkillUnavailable"].Value,
                AnalyticsInterventionErrorCode.TermNotFound =>
                    _text["ErrorInterventionTermUnavailable"].Value,
                AnalyticsInterventionErrorCode.QuestionGenerationFailed =>
                    _text["ErrorInterventionGenerationFailed"].Value,
                _ =>
                    _text["ErrorPersistence"].Value
            };

        return RedirectToAction(
            nameof(Student),
            new
            {
                studentProfileId,
                academicYearId,
                classGroupId,
                subjectId
            });
    }

    [Authorize(Roles = RoleNames.SubjectSupervisor + "," + RoleNames.SchoolAdmin)]
    [HttpGet("subject-overview")]
    public async Task<IActionResult> SubjectOverview(
        Guid academicYearId,
        Guid subjectId,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId))
            return Forbid();

        if (academicYearId == Guid.Empty ||
            subjectId == Guid.Empty)
        {
            return BadRequest();
        }

        var result = await _analytics.GetSupervisorSubjectOverviewAsync(
            actorId,
            academicYearId,
            subjectId,
            cancellationToken);

        return result.Value is null
            ? HandleQueryError(result.Error)
            : View(result.Value);
    }

    [HttpGet("class-report.pdf")]
    public async Task<IActionResult> ClassReportPdf(
        Guid? academicYearId,
        Guid? classGroupId,
        Guid? subjectId,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId))
            return Forbid();

        if (academicYearId.HasValue &&
            academicYearId.Value != Guid.Empty &&
            classGroupId.HasValue &&
            classGroupId.Value != Guid.Empty &&
            subjectId.HasValue &&
            subjectId.Value != Guid.Empty)
        {
            var students = await _analytics.GetStudentsEvaluationAsync(
                actorId,
                academicYearId.Value,
                classGroupId.Value,
                subjectId.Value,
                cancellationToken);
            var topics = await _analytics.GetTopicSkillEvaluationAsync(
                actorId,
                academicYearId.Value,
                classGroupId.Value,
                subjectId.Value,
                cancellationToken);

            if (students.Value is null)
                return HandleQueryError(students.Error);
            if (topics.Value is null)
                return HandleQueryError(topics.Error);

            var evaluationBytes =
                AnalyticsPdfRenderer.RenderClassEvaluationReport(
                    students.Value,
                    topics.Value);

            return File(
                evaluationBytes,
                "application/pdf",
                $"edulytics-class-evaluation-{DateTime.UtcNow:yyyyMMdd}.pdf");
        }

        var result = await _analytics.GetDashboardAsync(
            actorId,
            academicYearId,
            classGroupId,
            subjectId,
            cancellationToken);
        if (result.Value is null)
            return HandleQueryError(result.Error);

        var bytes = AnalyticsPdfRenderer.RenderClassReport(result.Value);
        return File(
            bytes,
            "application/pdf",
            $"edulytics-class-analytics-{DateTime.UtcNow:yyyyMMdd}.pdf");
    }

    [HttpGet("student-report.pdf")]
    public async Task<IActionResult> StudentReportPdf(
        Guid studentProfileId,
        Guid academicYearId,
        Guid classGroupId,
        Guid subjectId,
        Guid? termId,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId))
            return Forbid();

        if (studentProfileId == Guid.Empty ||
            academicYearId == Guid.Empty ||
            classGroupId == Guid.Empty ||
            subjectId == Guid.Empty)
        {
            return BadRequest();
        }

        var result = await _analytics.GetStudentEvaluationAsync(
            actorId,
            studentProfileId,
            academicYearId,
            classGroupId,
            subjectId,
            cancellationToken);
        if (result.Value is null)
            return HandleQueryError(result.Error);

        var report = BuildStudentReportPage(
            result.Value,
            termId);

        if (report is null)
            return NotFound();

        var bytes =
            AnalyticsPdfRenderer.RenderStudentEvaluationReport(
                report);
        return File(
            bytes,
            "application/pdf",
            $"edulytics-student-evaluation-{studentProfileId:N}-{DateTime.UtcNow:yyyyMMdd}.pdf");
    }

    [Authorize(Roles = RoleNames.SubjectSupervisor)]
    [HttpPost("recalculate")]
    [ValidateAntiForgeryToken]
    [RequestTimeout(BackendResiliencePolicyNames.Analytics)]
    [EnableRateLimiting(BackendResiliencePolicyNames.AnalyticsConcurrency)]
    public async Task<IActionResult> Recalculate(
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId))
            return Forbid();

        var result =
            await _analytics.RecalculateAsync(
                actorId,
                cancellationToken);

        TempData[
            result.Succeeded
                ? "Success"
                : "Error"] =
            result.Succeeded
                ? _text[
                    "SuccessRecalculated"].Value
                : _text[
                    ErrorKey(
                        result.Error)].Value;

        return RedirectToAction(nameof(Index));
    }

    private static AnalyticsStudentEvaluationReportPage?
        BuildStudentReportPage(
            AnalyticsStudentEvaluationPage source,
            Guid? termId)
    {
        AnalyticsTermEvaluationItem? selectedTerm = null;

        if (termId.HasValue)
        {
            selectedTerm = source.Terms.SingleOrDefault(
                x => x.TermId == termId.Value);

            if (selectedTerm is null)
                return null;
        }

        var assessments = termId.HasValue
            ? source.Assessments
                .Where(x => x.TermId == termId.Value)
                .ToArray()
            : source.Assessments;

        var evidence = termId.HasValue
            ? source.Evidence
                .Where(x => x.TermId == termId.Value)
                .ToArray()
            : source.Evidence;

        return new AnalyticsStudentEvaluationReportPage(
            source,
            termId,
            selectedTerm?.TermName,
            selectedTerm,
            assessments,
            evidence,
            DateTime.UtcNow);
    }

    private bool TryActor(out Guid id) =>
        Guid.TryParse(
            User.FindFirstValue(
                ClaimTypes.NameIdentifier),
            out id);

    private IActionResult HandleQueryError(
        AnalyticsErrorCode? error) =>
        error is AnalyticsErrorCode.AccessDenied
            or AnalyticsErrorCode.SchoolNotActive
                ? Forbid()
                : NotFound();

    private static string ErrorKey(
        AnalyticsErrorCode? error) =>
        error switch
        {
            AnalyticsErrorCode.AccessDenied =>
                "ErrorAccessDenied",

            AnalyticsErrorCode.SchoolNotActive =>
                "ErrorSchoolNotActive",

            AnalyticsErrorCode
                .RecalculationRequiresSchoolAdmin =>
                "ErrorRecalculateAdminOnly",

            AnalyticsErrorCode.InvalidSourceData =>
                "ErrorInvalidSourceData",

            _ =>
                "ErrorPersistence"
        };
}
