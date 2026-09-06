using System.Security.Claims;
using Edulytics.Core.Constants;
using Edulytics.Core.Enums;
using Edulytics.Services.Assessments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

[Authorize(Roles = RoleNames.Teacher)]
[Route("school/assessments/{assessmentId:guid}/delivery")]
public sealed class AssessmentDeliveryCorrectionController(
    IAssessmentDeliverySettingsService deliverySettings) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(
        Guid assessmentId,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(
                User.FindFirstValue(ClaimTypes.NameIdentifier),
                out var actorUserId))
            return Forbid();

        var result = await deliverySettings.GetAsync(
            actorUserId,
            assessmentId,
            cancellationToken);
        if (result.Value is null)
            return result.Error == AssessmentErrorCode.AccessDenied
                ? Forbid()
                : NotFound();

        var assessment = result.Value.Assessment;
        if (assessment.Status != AssessmentStatus.Open ||
            assessment.DeliveryMode != AssessmentDeliveryMode.Offline)
        {
            return RedirectToAction(
                nameof(AssessmentBuilderController.Index),
                "AssessmentBuilder",
                new { assessmentId });
        }

        return View(result.Value);
    }
}
