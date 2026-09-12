using System.Security.Claims;
using Edulytics.Core.Assessments;
using Edulytics.Core.Constants;
using Edulytics.Services.Assessments;
using Edulytics.Web.Resilience;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Edulytics.Web.Controllers;

[Authorize(Roles = RoleNames.Teacher)]
[Route("school/assessments/{assessmentId:guid}/builder/approval")]
public sealed class AssessmentApprovalRecoveryController(
    IAssessmentBuilderService service,
    IAssessmentBuilderBulkApprovalService bulkApproval) : Controller
{
    [HttpPost("question/{questionId:guid}"), ValidateAntiForgeryToken]
    [RequestTimeout(BackendResiliencePolicyNames.InteractiveWrite)]
    [EnableRateLimiting(BackendResiliencePolicyNames.HeavyWriteConcurrency)]
    public async Task<IActionResult> ApproveQuestion(
        Guid assessmentId,
        Guid questionId,
        string rowVersion,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId))
            return Forbid();
        if (!TryDecode(rowVersion, out var version))
            return Back(assessmentId, "The assessment changed. Reload and try again.");

        var workspace = await service.GetWorkspaceAsync(actorId, assessmentId, cancellationToken);
        if (workspace.Value is null)
            return Back(assessmentId, "The assessment builder could not be loaded.");
        if (!workspace.Value.Details.Assessment.RowVersion.SequenceEqual(version))
            return Back(assessmentId, "The assessment changed. Reload and try again.");

        var question = workspace.Value.Questions.FirstOrDefault(x => x.Id == questionId);
        if (question is null || question.Status != AssessmentBuilderQuestionStatus.Draft)
            return Back(assessmentId, "This question is no longer a draft.");

        var result = await service.ApproveQuestionAsync(
            actorId,
            assessmentId,
            questionId,
            workspace.Value.Details.Assessment.RowVersion,
            cancellationToken);

        if (result.Succeeded)
            TempData["Success"] = "Question approved.";
        else
            TempData["Error"] = "This question could not be approved. Review the question and try again.";

        return Back(assessmentId);
    }

    // Compatibility endpoint for older builder pages that posted to /approval/drafts.
    // GET never changes state; it only recovers stale browser navigation back to Builder.
    [HttpGet("drafts")]
    public IActionResult ApproveDraftsLegacyGet(Guid assessmentId) => Back(assessmentId);

    [HttpPost("drafts"), ValidateAntiForgeryToken]
    [RequestTimeout(BackendResiliencePolicyNames.InteractiveWrite)]
    [EnableRateLimiting(BackendResiliencePolicyNames.HeavyWriteConcurrency)]
    public async Task<IActionResult> ApproveDrafts(
        Guid assessmentId,
        string rowVersion,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId))
            return Forbid();
        if (!TryDecode(rowVersion, out var version))
            return Back(assessmentId, "The assessment changed. Reload and try again.");

        var result = await bulkApproval.ApproveAllDraftQuestionsAsync(
            actorId,
            assessmentId,
            version,
            cancellationToken);

        if (result.Succeeded)
        {
            TempData["Success"] = "All draft questions approved.";
        }
        else
        {
            TempData["Error"] = result.Error == AssessmentErrorCode.ConcurrencyConflict
                ? "The assessment changed. Reload and try again. No partial bulk approval was saved."
                : "Draft questions could not be approved. No partial bulk approval was saved.";
        }

        return Back(assessmentId);
    }

    private RedirectResult Back(Guid assessmentId, string? error = null)
    {
        if (!string.IsNullOrWhiteSpace(error))
            TempData["Error"] = error;
        return Redirect($"/school/assessments/{assessmentId:D}/builder");
    }

    private bool TryActor(out Guid actorUserId) =>
        Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out actorUserId);

    private static bool TryDecode(string? value, out byte[] bytes)
    {
        bytes = [];
        if (string.IsNullOrWhiteSpace(value))
            return false;
        try
        {
            bytes = Convert.FromBase64String(value);
            return bytes.Length > 0;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
