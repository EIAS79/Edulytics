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

        var draftIds = workspace.Value.Questions
            .Where(x => x.Status == AssessmentBuilderQuestionStatus.Draft)
            .Select(x => x.Id)
            .ToArray();

        var approved = 0;
        var review = 0;
        foreach (var questionId in draftIds)
        {
            var result = await service.ApproveQuestionAsync(
                actorId,
                assessmentId,
                questionId,
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
