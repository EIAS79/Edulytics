using System.Security.Claims;
using System.Globalization;
using Edulytics.Services.Assessments;
using Edulytics.Services.Notifications;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Services.StudentPortal;
using Edulytics.Web.GameRouting;
using Edulytics.Web.ViewModels.StudentPortal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

[Authorize(Policy = "StudentPortal")]
[Route("student")]
public sealed class StudentPortalController : Controller
{
    private const string LessonPracticePilotCode = "PED:CAMBRIDGE-INTL-MATH:S1:L10";

    private readonly IStudentPortalService _portal;
    private readonly INotificationService _notifications;
    private readonly ILessonContentService _lessonContent;
    private readonly IStudentAssessmentDeliveryService _assessmentDelivery;
    private readonly IStudentPrivatePracticeService _privatePractice;

    public StudentPortalController(
        IStudentPortalService portal,
        INotificationService notifications,
        ILessonContentService lessonContent,
        IStudentAssessmentDeliveryService assessmentDelivery,
        IStudentPrivatePracticeService privatePractice)
    {
        _portal = portal;
        _notifications = notifications;
        _lessonContent = lessonContent;
        _assessmentDelivery = assessmentDelivery;
        _privatePractice = privatePractice;
    }

    [HttpGet("")]
    [HttpGet("dashboard")]
    public async Task<IActionResult> Dashboard(CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        var workspace = await _portal.GetWorkspaceAsync(actorId, cancellationToken);
        if (workspace.Value is null) return HandlePortalError(workspace.Error);
        var notifications = await _notifications.ListInboxAsync(actorId, cancellationToken);
        return View(new StudentDashboardViewModel(workspace.Value, notifications.Value ?? []));
    }

    [HttpGet("learning")]
    public async Task<IActionResult> Learning(
        Guid? curriculumAdoptionId = null,
        Guid? classGroupId = null,
        Guid[]? focusNodeIds = null,
        CancellationToken cancellationToken = default)
    {
        var workspace = await WorkspaceAsync(cancellationToken);
        if (workspace.Result is not null) return workspace.Result;
        if (!TryActor(out var actorId)) return Forbid();
        var lessons = await _lessonContent.ListPublishedForStudentAsync(
            actorId, CultureInfo.CurrentUICulture.Name, cancellationToken);
        if (lessons.Value is null)
            return lessons.Error == LessonContentErrorCode.AccessDenied ? Forbid() : NotFound();

        var selectedContext = curriculumAdoptionId.HasValue && classGroupId.HasValue
            ? workspace.Workspace!.Learning.FirstOrDefault(x =>
                x.CurriculumAdoptionId == curriculumAdoptionId.Value &&
                x.ClassGroupId == classGroupId.Value)
            : null;

        var selectedNodeIds = selectedContext is null
            ? Array.Empty<Guid>()
            : (focusNodeIds ?? [])
                .Distinct()
                .Where(id => selectedContext.Nodes.Any(node => node.Id == id))
                .Take(100)
                .ToArray();

        return View(
            nameof(Learning),
            new StudentLearningViewModel(workspace.Workspace!, lessons.Value)
            {
                SelectedCurriculumAdoptionId = selectedContext?.CurriculumAdoptionId,
                SelectedClassGroupId = selectedContext?.ClassGroupId,
                SelectedLearningNodeIds = selectedNodeIds
            });
    }

    [NonAction]
    public Task<IActionResult> Learning(CancellationToken cancellationToken) =>
        Learning(null, null, null, cancellationToken);

    [HttpGet("learning/lesson/{id:guid}")]
    public async Task<IActionResult> Lesson(Guid id, CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        var workspace = await _portal.GetWorkspaceAsync(actorId, cancellationToken);
        if (workspace.Value is null) return HandlePortalError(workspace.Error);
        var lesson = await _lessonContent.GetPublishedForStudentAsync(
            actorId, id, CultureInfo.CurrentUICulture.Name, cancellationToken);
        if (lesson.Value is null)
            return lesson.Error == LessonContentErrorCode.AccessDenied ? Forbid() : NotFound();

        var availability = await FindLessonPracticeAvailabilityAsync(actorId, id, cancellationToken);
        ViewData["GameAdoptionId"] = availability.GameAdoptionId;
        ViewData["LessonPracticePilotAdoptionId"] = availability.PilotAdoptionId;
        return View(nameof(Lesson), lesson.Value);
    }

    [HttpGet("assessments")]
    public async Task<IActionResult> Assessments(CancellationToken cancellationToken)
    {
        var workspace = await WorkspaceAsync(cancellationToken);
        return workspace.Result ?? View(nameof(Assessments), new StudentAssessmentsViewModel(workspace.Workspace!));
    }

    [HttpGet("assessments/{id:guid}")]
    public async Task<IActionResult> TakeAssessment(Guid id, CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        var attempt = await _assessmentDelivery.GetAttemptAsync(actorId, id, cancellationToken);
        if (attempt.Value is not null) return View(nameof(TakeAssessment), attempt.Value);
        return attempt.Error switch
        {
            StudentAssessmentDeliveryErrorCode.AlreadySubmitted => RedirectToAction(nameof(Assessments)),
            StudentAssessmentDeliveryErrorCode.AccessDenied or
            StudentAssessmentDeliveryErrorCode.SchoolNotActive or
            StudentAssessmentDeliveryErrorCode.ProfileNotLinked or
            StudentAssessmentDeliveryErrorCode.NotTargeted => Forbid(),
            _ => NotFound()
        };
    }

    [HttpPost("assessments/{id:guid}/submit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubmitAssessment(
        Guid id,
        Guid[]? questionIds,
        string[]? responses,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        questionIds ??= [];
        responses ??= [];
        if (questionIds.Length != responses.Length) return BadRequest();
        var payload = questionIds
            .Select((questionId, index) => new StudentAssessmentResponse(questionId, responses[index]))
            .ToArray();
        var submitted = await _assessmentDelivery.SubmitAsync(actorId, id, payload, cancellationToken);
        if (submitted.Value is not null) return View("AssessmentSubmitted", submitted.Value);
        return submitted.Error switch
        {
            StudentAssessmentDeliveryErrorCode.AlreadySubmitted => RedirectToAction(nameof(Assessments)),
            StudentAssessmentDeliveryErrorCode.InvalidSubmission => BadRequest(),
            StudentAssessmentDeliveryErrorCode.AccessDenied or
            StudentAssessmentDeliveryErrorCode.SchoolNotActive or
            StudentAssessmentDeliveryErrorCode.ProfileNotLinked or
            StudentAssessmentDeliveryErrorCode.NotTargeted => Forbid(),
            _ => NotFound()
        };
    }

    [HttpGet("results")]
    public async Task<IActionResult> Results(CancellationToken cancellationToken)
    {
        var workspace = await WorkspaceAsync(cancellationToken);
        return workspace.Result ?? View(nameof(Results), new StudentResultsViewModel(workspace.Workspace!));
    }

    [HttpGet("notifications")]
    public async Task<IActionResult> Notifications(CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        var workspace = await _portal.GetWorkspaceAsync(actorId, cancellationToken);
        if (workspace.Value is null) return HandlePortalError(workspace.Error);
        var notifications = await _notifications.ListInboxAsync(actorId, cancellationToken);
        if (notifications.Value is null) return Forbid();
        return View(new StudentNotificationsViewModel(workspace.Value, notifications.Value));
    }

    [HttpPost("notifications/{id:guid}/read")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetNotificationReadState(
        Guid id, bool isRead, CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        var workspace = await _portal.GetWorkspaceAsync(actorId, cancellationToken);
        if (workspace.Value is null) return HandlePortalError(workspace.Error);
        var result = await _notifications.SetReadStateAsync(actorId, id, isRead, cancellationToken);
        if (result.Value is null) return Forbid();
        return RedirectToAction(nameof(Notifications));
    }

    private async Task<(Guid? GameAdoptionId, Guid? PilotAdoptionId)>
        FindLessonPracticeAvailabilityAsync(
            Guid actorId,
            Guid lessonId,
            CancellationToken cancellationToken)
    {
        Guid? gameAdoptionId = null;
        Guid? pilotAdoptionId = null;

        var initial = await _privatePractice.GetWorkspaceAsync(actorId, null, cancellationToken);
        InspectPracticeWorkspace(initial, lessonId, ref gameAdoptionId, ref pilotAdoptionId);

        foreach (var curriculum in initial.Curricula)
        {
            if (gameAdoptionId.HasValue && pilotAdoptionId.HasValue)
                break;
            if (curriculum.CurriculumAdoptionId == initial.SelectedCurriculumAdoptionId)
                continue;

            var candidate = await _privatePractice.GetWorkspaceAsync(
                actorId,
                curriculum.CurriculumAdoptionId,
                cancellationToken);
            InspectPracticeWorkspace(candidate, lessonId, ref gameAdoptionId, ref pilotAdoptionId);
        }

        return (gameAdoptionId, pilotAdoptionId);
    }

    private static void InspectPracticeWorkspace(
        StudentPrivatePracticeWorkspace workspace,
        Guid lessonId,
        ref Guid? gameAdoptionId,
        ref Guid? pilotAdoptionId)
    {
        var lesson = workspace.Lessons.FirstOrDefault(x => x.LessonId == lessonId);
        if (lesson is null || !workspace.SelectedCurriculumAdoptionId.HasValue)
            return;

        if (!gameAdoptionId.HasValue &&
            GameLessonRouteResolver.Resolve(lesson.LessonCode, lesson.UnitTitle, lesson.LessonTitle).IsPlayable)
        {
            gameAdoptionId = workspace.SelectedCurriculumAdoptionId;
        }

        if (!pilotAdoptionId.HasValue &&
            string.Equals(lesson.LessonCode, LessonPracticePilotCode, StringComparison.Ordinal))
        {
            pilotAdoptionId = workspace.SelectedCurriculumAdoptionId;
        }
    }

    private async Task<(StudentPortalWorkspace? Workspace, IActionResult? Result)> WorkspaceAsync(
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return (null, Forbid());
        var workspace = await _portal.GetWorkspaceAsync(actorId, cancellationToken);
        return workspace.Value is null
            ? (null, HandlePortalError(workspace.Error))
            : (workspace.Value, null);
    }

    private IActionResult HandlePortalError(StudentPortalErrorCode? error) =>
        error switch
        {
            StudentPortalErrorCode.AccessDenied => Forbid(),
            StudentPortalErrorCode.ProfileNotLinked => Forbid(),
            StudentPortalErrorCode.SchoolNotActive => Forbid(),
            _ => NotFound()
        };

    private bool TryActor(out Guid actorUserId) =>
        Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out actorUserId);
}
