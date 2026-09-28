using System.Security.Claims;
using Edulytics.Services.AdaptivePractice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

[Authorize(Policy = "StudentPortal")]
[Route("student/adaptive-intelligence")]
public sealed class StudentAdaptiveIntelligenceController(
    IAdaptiveIntelligenceV2Service intelligence) : Controller
{
    [HttpGet("next-steps")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> NextSteps(
        Guid academicYearId,
        Guid classGroupId,
        Guid subjectId,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId))
            return Forbid();

        var result = await intelligence.GetNextStepsAsync(
            actorId,
            academicYearId,
            classGroupId,
            subjectId,
            cancellationToken);

        return Result(result);
    }

    [HttpGet("question-log")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> QuestionLog(
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId))
            return Forbid();

        var result = await intelligence.GetQuestionLogAsync(
            actorId,
            200,
            cancellationToken);

        if (result.Value is null)
        {
            return result.Error == AdaptiveIntelligenceV2Error.AccessDenied
                ? Forbid()
                : NotFound();
        }

        Response.Headers["X-Robots-Tag"] =
            "noindex, nofollow, noarchive";

        return View(result.Value);
    }

    [HttpGet("diagnostic")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Diagnostic(
        Guid curriculumAdoptionId,
        Guid lessonId,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId))
            return Forbid();

        var result = await intelligence.GetDiagnosticPreviewAsync(
            actorId,
            curriculumAdoptionId,
            lessonId,
            cancellationToken);

        return Result(result);
    }

    private IActionResult Result<T>(
        AdaptiveIntelligenceV2Result<T> result)
        where T : class
    {
        if (result.Value is not null)
            return Json(result.Value);

        return result.Error switch
        {
            AdaptiveIntelligenceV2Error.AccessDenied => Forbid(),
            AdaptiveIntelligenceV2Error.FeatureDisabled => NotFound(),
            AdaptiveIntelligenceV2Error.ScopeNotAvailable => NotFound(),
            AdaptiveIntelligenceV2Error.NotEnoughEvidence =>
                UnprocessableEntity(
                    new { error = "adaptive_intelligence_not_enough_evidence" }),
            _ => NotFound()
        };
    }

    private bool TryActor(out Guid actorUserId) =>
        Guid.TryParse(
            User.FindFirstValue(ClaimTypes.NameIdentifier),
            out actorUserId);
}

[Authorize(Policy = "AnalyticsRead")]
[Route("school/analytics/adaptive")]
public sealed class AdaptiveClassroomIntelligenceController(
    IAdaptiveIntelligenceV2Service intelligence) : Controller
{
    [HttpGet("live")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Live(
        Guid academicYearId,
        Guid classGroupId,
        Guid subjectId,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(
                User.FindFirstValue(ClaimTypes.NameIdentifier),
                out var actorId))
        {
            return Forbid();
        }

        var result = await intelligence.GetLiveClassroomAsync(
            actorId,
            academicYearId,
            classGroupId,
            subjectId,
            cancellationToken);

        if (result.Value is not null)
            return Json(result.Value);

        return result.Error == AdaptiveIntelligenceV2Error.AccessDenied
            ? Forbid()
            : NotFound();
    }
}
