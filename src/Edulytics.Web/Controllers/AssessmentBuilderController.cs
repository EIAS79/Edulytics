using System.Security.Claims;
using Edulytics.Core.Assessments;
using Edulytics.Core.Constants;
using Edulytics.Core.Enums;
using Edulytics.Services.Assessments;
using Edulytics.Web.Resilience;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Localization;

namespace Edulytics.Web.Controllers;

[Authorize(Roles = RoleNames.Teacher)]
[Route("school/assessments/{assessmentId:guid}/builder")]
public sealed class AssessmentBuilderController(
    IAssessmentBuilderService service,
    IAssessmentDeliverySettingsService deliverySettings,
    IStringLocalizer<AssessmentBuilderResource> text) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(Guid assessmentId, CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        var result = await service.GetWorkspaceAsync(actorId, assessmentId, cancellationToken);
        if (result.Value is null) return Handle(result.Error);

        var delivery = await deliverySettings.GetAsync(actorId, assessmentId, cancellationToken);
        if (delivery.Value is null) return Handle(delivery.Error);

        return View(result.Value with { TargetStudents = delivery.Value.TargetStudents });
    }

    [HttpPost("settings"), ValidateAntiForgeryToken]
    [RequestTimeout(BackendResiliencePolicyNames.InteractiveWrite)]
    [EnableRateLimiting(BackendResiliencePolicyNames.HeavyWriteConcurrency)]
    public async Task<IActionResult> UpdateDeliverySettings(
        Guid assessmentId,
        AssessmentTargetType targetType,
        Guid? targetStudentProfileId,
        AssessmentDeliveryMode deliveryMode,
        AssessmentDifficultyBand difficultyBand,
        string rowVersion,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        if (!TryDecode(rowVersion, out var version)) return ConcurrencyRedirect(assessmentId);

        var result = await deliverySettings.UpdateAsync(
            actorId,
            new UpdateAssessmentDeliverySettingsRequest(
                assessmentId,
                targetType,
                targetStudentProfileId,
                deliveryMode,
                difficultyBand,
                version),
            cancellationToken);

        Feedback(result, "DeliverySettingsSaved");
        return RedirectToAction(nameof(Index), new { assessmentId });
    }

    [HttpPost("manual"), ValidateAntiForgeryToken]
    [RequestTimeout(BackendResiliencePolicyNames.InteractiveWrite)]
    [EnableRateLimiting(BackendResiliencePolicyNames.HeavyWriteConcurrency)]
    public async Task<IActionResult> CreateManual(Guid assessmentId, string prompt, string correctAnswer, string solution,
        decimal maxScore, int order, AssessmentItemDifficulty difficulty, Guid[]? outcomeIds, string rowVersion, CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        if (!TryDecode(rowVersion, out var version)) return ConcurrencyRedirect(assessmentId);
        var result = await service.CreateManualQuestionAsync(actorId,
            new CreateManualBuilderQuestionRequest(assessmentId, prompt, correctAnswer, solution, maxScore, order, difficulty, outcomeIds ?? [], version), cancellationToken);
        Feedback(result, "SuccessQuestionCreated");
        if (result.Succeeded)
            TempData["ManualQuestionSaved"] = true;
        return RedirectToAction(nameof(Index), new { assessmentId });
    }

    [HttpPost("questions/{questionId:guid}/edit"), ValidateAntiForgeryToken]
    [RequestTimeout(BackendResiliencePolicyNames.InteractiveWrite)]
    [EnableRateLimiting(BackendResiliencePolicyNames.HeavyWriteConcurrency)]
    public async Task<IActionResult> Edit(Guid assessmentId, Guid questionId, string prompt, string correctAnswer, string solution,
        decimal maxScore, AssessmentItemDifficulty difficulty, string rowVersion, CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        if (!TryDecode(rowVersion, out var version)) return ConcurrencyRedirect(assessmentId);
        var result = await service.EditQuestionAsync(actorId,
            new EditBuilderQuestionRequest(assessmentId, questionId, prompt, correctAnswer, solution, maxScore, difficulty, version), cancellationToken);
        Feedback(result, "BuilderQuestionUpdated");
        return RedirectToAction(nameof(Index), new { assessmentId });
    }

    [HttpPost("generate"), ValidateAntiForgeryToken]
    [RequestTimeout(BackendResiliencePolicyNames.InteractiveWrite)]
    [EnableRateLimiting(BackendResiliencePolicyNames.HeavyWriteConcurrency)]
    public async Task<IActionResult> Generate(Guid assessmentId, int questionCount, decimal maxScorePerQuestion,
        AssessmentBuilderDifficulty difficulty, Guid[]? outcomeIds, int seed, string rowVersion, CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        if (!TryDecode(rowVersion, out var version)) return ConcurrencyRedirect(assessmentId);
        var result = await service.GenerateQuestionsAsync(actorId,
            new GenerateBuilderQuestionsRequest(assessmentId, questionCount, maxScorePerQuestion, difficulty, outcomeIds ?? [], version, seed), cancellationToken);
        Feedback(result, "BuilderGenerated");
        return RedirectToAction(nameof(Index), new { assessmentId });
    }

    [HttpPost("questions/{questionId:guid}/regenerate"), ValidateAntiForgeryToken]
    [RequestTimeout(BackendResiliencePolicyNames.InteractiveWrite)]
    [EnableRateLimiting(BackendResiliencePolicyNames.HeavyWriteConcurrency)]
    public async Task<IActionResult> Regenerate(Guid assessmentId, Guid questionId, int seed, string rowVersion, CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        if (!TryDecode(rowVersion, out var version)) return ConcurrencyRedirect(assessmentId);
        var result = await service.RegenerateQuestionAsync(actorId, assessmentId, questionId, seed, version, cancellationToken);
        Feedback(result, "BuilderQuestionRegenerated");
        return RedirectToAction(nameof(Index), new { assessmentId });
    }

    [HttpPost("questions/{questionId:guid}/approve"), ValidateAntiForgeryToken]
    [RequestTimeout(BackendResiliencePolicyNames.InteractiveWrite)]
    [EnableRateLimiting(BackendResiliencePolicyNames.HeavyWriteConcurrency)]
    public async Task<IActionResult> Approve(Guid assessmentId, Guid questionId, string rowVersion, CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        if (!TryDecode(rowVersion, out var version)) return ConcurrencyRedirect(assessmentId);
        var result = await service.ApproveQuestionAsync(actorId, assessmentId, questionId, version, cancellationToken);
        Feedback(result, "BuilderQuestionApproved");
        return RedirectToAction(nameof(Index), new { assessmentId });
    }

    [HttpPost("questions/{questionId:guid}/delete"), ValidateAntiForgeryToken]
    [RequestTimeout(BackendResiliencePolicyNames.InteractiveWrite)]
    [EnableRateLimiting(BackendResiliencePolicyNames.HeavyWriteConcurrency)]
    public async Task<IActionResult> Delete(Guid assessmentId, Guid questionId, string rowVersion, CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        if (!TryDecode(rowVersion, out var version)) return ConcurrencyRedirect(assessmentId);
        var result = await service.DeleteQuestionAsync(actorId, assessmentId, questionId, version, cancellationToken);
        Feedback(result, "SuccessQuestionDeleted");
        return RedirectToAction(nameof(Index), new { assessmentId });
    }

    [HttpPost("publish"), ValidateAntiForgeryToken]
    [RequestTimeout(BackendResiliencePolicyNames.InteractiveWrite)]
    [EnableRateLimiting(BackendResiliencePolicyNames.HeavyWriteConcurrency)]
    public async Task<IActionResult> Publish(Guid assessmentId, string rowVersion, CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        if (!TryDecode(rowVersion, out var version)) return ConcurrencyRedirect(assessmentId);
        var result = await service.PublishAsync(actorId, assessmentId, version, cancellationToken);
        Feedback(result, "SuccessAssessmentOpened");
        return result.Succeeded
            ? RedirectToAction("Details", "Assessments", new { id = assessmentId })
            : RedirectToAction(nameof(Index), new { assessmentId });
    }

    private bool TryActor(out Guid id) => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out id);
    private IActionResult Handle(AssessmentErrorCode? error) => error == AssessmentErrorCode.AccessDenied ? Forbid() : NotFound();
    private IActionResult ConcurrencyRedirect(Guid assessmentId)
    {
        TempData["Error"] = text["ErrorConcurrencyConflict"].Value;
        return RedirectToAction(nameof(Index), new { assessmentId });
    }
    private void Feedback(AssessmentCommandResult result, string successKey)
    {
        TempData[result.Succeeded ? "Success" : "Error"] = result.Succeeded
            ? text[successKey].Value
            : text["BuilderOperationFailed", result.Error?.ToString() ?? "Unknown"].Value;
    }
    private static bool TryDecode(string? value, out byte[] bytes)
    {
        bytes = [];
        if (string.IsNullOrWhiteSpace(value)) return false;
        try { bytes = Convert.FromBase64String(value); return bytes.Length > 0; }
        catch (FormatException) { return false; }
    }
}