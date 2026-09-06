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
    IAssessmentBuilderService service) : Controller
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
        if (!ReadyForApproval(question))
            return Back(assessmentId, "This question still needs teacher review before it can be approved.");

        var result = await service.ApproveQuestionAsync(
            actorId,
            assessmentId,
            questionId,
            version,
            cancellationToken);

        if (result.Succeeded)
            TempData["Success"] = "Question approved.";
        else
            TempData["Error"] = "This question could not be approved. Review the question and try again.";

        return Back(assessmentId);
    }

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

        var workspace = await service.GetWorkspaceAsync(actorId, assessmentId, cancellationToken);
        if (workspace.Value is null)
            return Back(assessmentId, "The assessment builder could not be loaded.");
        if (!workspace.Value.Details.Assessment.RowVersion.SequenceEqual(version))
            return Back(assessmentId, "The assessment changed. Reload and try again.");

        var drafts = workspace.Value.Questions
            .Where(x => x.Status == AssessmentBuilderQuestionStatus.Draft)
            .ToArray();

        var approved = 0;
        var review = 0;
        foreach (var question in drafts)
        {
            if (!ReadyForApproval(question))
            {
                review++;
                continue;
            }

            var result = await service.ApproveQuestionAsync(
                actorId,
                assessmentId,
                question.Id,
                version,
                cancellationToken);

            if (result.Succeeded)
                approved++;
            else
                review++;
        }

        if (review == 0)
            TempData["Success"] = approved == 1
                ? "1 draft question approved."
                : $"{approved} draft questions approved.";
        else if (approved > 0)
            TempData["Success"] = $"Approved {approved} draft question(s). {review} remain for teacher review.";
        else
            TempData["Error"] = $"No draft questions were approved. {review} remain for teacher review.";

        return Back(assessmentId);
    }

    private static bool ReadyForApproval(AssessmentBuilderQuestion question) =>
        !string.IsNullOrWhiteSpace(question.Prompt) &&
        !string.IsNullOrWhiteSpace(question.CorrectAnswer) &&
        question.MaxScore > 0m &&
        question.OutcomeIds.Count > 0;

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
