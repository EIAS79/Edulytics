using System.Globalization;
using System.Security.Claims;
using Edulytics.Core.Constants;
using Edulytics.Services.LessonContent;
using Edulytics.Web.ViewModels.LessonContent;
using Edulytics.Web.Resilience;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Web.Controllers;

[Authorize(Roles=RoleNames.SchoolAdmin+","+RoleNames.SubjectSupervisor+","+RoleNames.Teacher)]
[Route("lesson-content")]
public sealed class LessonContentController : Controller
{
    private readonly ILessonContentService _lessons;
    private readonly IYouTubeLessonDiscoveryService _youTubeLessons;

    public LessonContentController(
        ILessonContentService lessons,
        IYouTubeLessonDiscoveryService youTubeLessons)
    {
        _lessons = lessons;
        _youTubeLessons = youTubeLessons;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(
        Guid? academicYearId = null,
        Guid? academicProgramId = null,
        Guid? curriculumAdoptionId = null,
        CancellationToken cancellationToken = default)
    {
        if(!TryActor(out var actorId))return Forbid();
        var r=await _lessons.GetDashboardAsync(
            actorId,
            new LessonContentSelection(
                academicYearId,
                academicProgramId,
                curriculumAdoptionId),
            cancellationToken);
        return r.Value is null?HandleError(r.Error):View(new LessonContentIndexViewModel(r.Value));
    }

    [NonAction]
    public Task<IActionResult> Index(CancellationToken cancellationToken) =>
        Index(null, null, null, cancellationToken);

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Detail(
        Guid id,
        Guid? academicYearId,
        Guid? academicProgramId,
        Guid? curriculumAdoptionId,
        CancellationToken cancellationToken)
    {
        if(!TryActor(out var actorId))return Forbid();
        var result=await _lessons.GetStaffLessonAsync(actorId,id,CultureInfo.CurrentUICulture.Name,cancellationToken);
        if (result.Value is null)
            return HandleError(result.Error);

        ViewData["BackAcademicYearId"] = academicYearId;
        ViewData["BackAcademicProgramId"] = academicProgramId;
        ViewData["BackCurriculumAdoptionId"] = curriculumAdoptionId;

        ViewData["BackAcademicYearId"] = academicYearId;
        ViewData["BackAcademicProgramId"] = academicProgramId;
        ViewData["BackCurriculumAdoptionId"] = curriculumAdoptionId;

        return View(
            new LessonContentDetailViewModel(
                result.Value));
    }

    [HttpGet("{id:guid}/youtube")]
    [EnableRateLimiting(BackendResiliencePolicyNames.YouTubeLessonSearch)]
    public async Task<IActionResult> LessonYouTube(
        Guid id,
        string? q,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId))
            return Forbid();

        var lesson = await _lessons.GetStaffLessonAsync(
            actorId,
            id,
            CultureInfo.CurrentUICulture.Name,
            cancellationToken);

        if (lesson.Value is null)
            return HandleError(lesson.Error);

        var result = await _youTubeLessons.DiscoverAsync(
            new YouTubeLessonDiscoveryRequest(
                lesson.Value.LessonCode,
                lesson.Value.LessonTitle,
                lesson.Value.GradeName,
                lesson.Value.FrameworkName,
                CultureInfo.CurrentUICulture.Name,
                q,
                lesson.Value.Outcomes
                    .Select(x => x.Description)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .ToArray()),
            cancellationToken);

        return Json(result);
    }

    private IActionResult HandleError(LessonContentErrorCode? error)=>
        error is LessonContentErrorCode.AccessDenied or LessonContentErrorCode.SchoolNotActive?Forbid():NotFound();

    private bool TryActor(out Guid actorUserId)=>Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out actorUserId);
}
