using System.Security.Claims;
using Edulytics.Services.Practice;
using Edulytics.Web.Resilience;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Localization;

namespace Edulytics.Web.Controllers;

[Authorize(Policy = "StudentPortal")]
[Route("student/practice")]
public sealed class StudentPracticeController(
    IStudentPrivatePracticeService privatePractice,
    IPracticeService practice,
    IStringLocalizer<StudentResource> text) : Controller
{
    private const string LessonPracticePilotCode = "PED:CAMBRIDGE-INTL-MATH:S1:L10";
    private const string LessonGameMode = "lesson-game";
    private const int LessonPracticePilotQuestionCount = 10;

    [HttpGet("")]
    public async Task<IActionResult> Index(Guid? curriculumAdoptionId, CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        var workspace = await privatePractice.GetWorkspaceAsync(actorId, curriculumAdoptionId, cancellationToken);
        return View(workspace);
    }

    [HttpPost("generate"), ValidateAntiForgeryToken]
    [RequestTimeout(BackendResiliencePolicyNames.InteractiveWrite)]
    [EnableRateLimiting(BackendResiliencePolicyNames.HeavyWriteConcurrency)]
    public async Task<IActionResult> Generate(
        Guid curriculumAdoptionId,
        StudentPrivatePracticeScope scope,
        Guid? lessonId,
        string? unitKey,
        StudentPrivatePracticeDifficulty difficulty,
        int questionCount,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        var result = await privatePractice.GenerateAsync(actorId,
            new GenerateStudentPrivatePracticeRequest(
                curriculumAdoptionId, scope, lessonId, unitKey, difficulty, questionCount),
            cancellationToken);
        if (!result.Succeeded)
        {
            TempData["Error"] = PrivatePracticeErrorMessage(result.Error);
            return RedirectToAction(nameof(Index), new { curriculumAdoptionId });
        }
        return RedirectToAction(nameof(Attempt), new { id = result.AttemptId });
    }

    [HttpPost("lesson-pilot/start"), ValidateAntiForgeryToken]
    [RequestTimeout(BackendResiliencePolicyNames.InteractiveWrite)]
    [EnableRateLimiting(BackendResiliencePolicyNames.HeavyWriteConcurrency)]
    public async Task<IActionResult> StartLessonPilot(
        Guid curriculumAdoptionId,
        Guid lessonId,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();

        var workspace = await privatePractice.GetWorkspaceAsync(
            actorId, curriculumAdoptionId, cancellationToken);
        var pilotLesson = workspace.Lessons.SingleOrDefault(x =>
            x.LessonId == lessonId &&
            string.Equals(x.LessonCode, LessonPracticePilotCode, StringComparison.Ordinal));
        if (pilotLesson is null || workspace.SelectedCurriculumAdoptionId != curriculumAdoptionId)
            return NotFound();

        var result = await privatePractice.GenerateAsync(
            actorId,
            new GenerateStudentPrivatePracticeRequest(
                curriculumAdoptionId,
                StudentPrivatePracticeScope.Lesson,
                lessonId,
                null,
                StudentPrivatePracticeDifficulty.MyLevel,
                LessonPracticePilotQuestionCount),
            cancellationToken);

        if (!result.Succeeded)
        {
            TempData["Error"] = PrivatePracticeErrorMessage(result.Error);
            return RedirectToAction("Lesson", "StudentPortal", new { id = lessonId });
        }

        return RedirectToAction(nameof(Attempt), new
        {
            id = result.AttemptId,
            mode = LessonGameMode
        });
    }

    [HttpGet("attempt/{id:guid}")]
    public async Task<IActionResult> Attempt(Guid id, string? mode, CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        var result = await practice.GetAttemptAsync(actorId, id, cancellationToken);
        if (result.Value is null)
            return result.Error == PracticeErrorCode.AccessDenied ? Forbid() : NotFound();
        ViewData["LessonGameMode"] = string.Equals(
            mode, LessonGameMode, StringComparison.OrdinalIgnoreCase);
        return View(result.Value);
    }

    [HttpPost("attempt/{id:guid}/answer"), ValidateAntiForgeryToken]
    [RequestTimeout(BackendResiliencePolicyNames.InteractiveWrite)]
    [EnableRateLimiting(BackendResiliencePolicyNames.HeavyWriteConcurrency)]
    public async Task<IActionResult> Answer(
        Guid id,
        Guid attemptItemId,
        string answer,
        string? mode,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        var result = await practice.AnswerAsync(actorId, id, attemptItemId, answer, cancellationToken);
        if (result.Value is null)
        {
            TempData["Error"] = PracticeErrorMessage(result.Error);
        }
        else
        {
            TempData["PracticeFeedback"] = result.Value.IsCorrect ? "correct" : "incorrect";
            TempData["PracticeSolution"] = result.Value.Solution;
        }
        return RedirectToAction(nameof(Attempt), new
        {
            id,
            mode = NormalizeMode(mode)
        });
    }

    [HttpPost("attempt/{id:guid}/submit"), ValidateAntiForgeryToken]
    [RequestTimeout(BackendResiliencePolicyNames.InteractiveWrite)]
    [EnableRateLimiting(BackendResiliencePolicyNames.HeavyWriteConcurrency)]
    public async Task<IActionResult> Submit(Guid id, string? mode, CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        var result = await practice.SubmitAsync(actorId, id, cancellationToken);
        if (result.Value is null)
            TempData["Error"] = PracticeErrorMessage(result.Error);
        return RedirectToAction(nameof(Attempt), new
        {
            id,
            mode = NormalizeMode(mode)
        });
    }

    private static string? NormalizeMode(string? mode) =>
        string.Equals(mode, LessonGameMode, StringComparison.OrdinalIgnoreCase)
            ? LessonGameMode
            : null;

    private string PrivatePracticeErrorMessage(StudentPrivatePracticeError? error) => error switch
    {
        StudentPrivatePracticeError.NoSupportedOutcomes => text["PrivatePracticeNoSupportedOutcomes"].Value,
        StudentPrivatePracticeError.CurriculumNotAvailable => text["PrivatePracticeCurriculumUnavailable"].Value,
        StudentPrivatePracticeError.InvalidQuestionCount => text["PrivatePracticeInvalidQuestionCount"].Value,
        StudentPrivatePracticeError.InvalidScope => text["PrivatePracticeInvalidScope"].Value,
        StudentPrivatePracticeError.GenerationFailed => text["PrivatePracticeGenerationFailed"].Value,
        StudentPrivatePracticeError.AccessDenied => text["PrivatePracticeAccessDenied"].Value,
        _ => text["PrivatePracticeGenerationFailed"].Value
    };

    private string PracticeErrorMessage(PracticeErrorCode? error) => error switch
    {
        PracticeErrorCode.AttemptIncomplete => text["PracticeAnswerAll"].Value,
        PracticeErrorCode.InvalidAnswer => text["PracticeInvalidAnswer"].Value,
        PracticeErrorCode.AttemptNotInProgress => text["PracticeAttemptNotInProgress"].Value,
        _ => text["PracticeOperationFailed"].Value
    };

    private bool TryActor(out Guid actorUserId) =>
        Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out actorUserId);
}
