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

    [HttpGet("attempt/{id:guid}")]
    public async Task<IActionResult> Attempt(Guid id, CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        var result = await practice.GetAttemptAsync(actorId, id, cancellationToken);
        if (result.Value is null)
            return result.Error == PracticeErrorCode.AccessDenied ? Forbid() : NotFound();
        return View(result.Value);
    }

    [HttpPost("attempt/{id:guid}/answer"), ValidateAntiForgeryToken]
    [RequestTimeout(BackendResiliencePolicyNames.InteractiveWrite)]
    [EnableRateLimiting(BackendResiliencePolicyNames.HeavyWriteConcurrency)]
    public async Task<IActionResult> Answer(
        Guid id,
        Guid attemptItemId,
        string answer,
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
        return RedirectToAction(nameof(Attempt), new { id });
    }

    [HttpPost("attempt/{id:guid}/submit"), ValidateAntiForgeryToken]
    [RequestTimeout(BackendResiliencePolicyNames.InteractiveWrite)]
    [EnableRateLimiting(BackendResiliencePolicyNames.HeavyWriteConcurrency)]
    public async Task<IActionResult> Submit(Guid id, CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        var result = await practice.SubmitAsync(actorId, id, cancellationToken);
        if (result.Value is null)
            TempData["Error"] = PracticeErrorMessage(result.Error);
        return RedirectToAction(nameof(Attempt), new { id });
    }

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
