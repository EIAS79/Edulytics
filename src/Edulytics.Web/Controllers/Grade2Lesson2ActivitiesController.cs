using System.Globalization;
using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

/// <summary>
/// Authenticated student task for 2021 G2 U01 L02 objectives. Our own figures
/// and Cool-down replace login-restricted publisher content, not copying it.
/// </summary>
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u01-l02")]
public sealed class Grade2Lesson2ActivitiesController(
    IStudentPrivatePracticeService privatePractice,
    ILessonContentService lessonContent) : Controller
{
    private const string Code=UsCcssG2Unit1SecondLessonEdulyticsCoolDown.LessonCode;

    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(
        Guid curriculumAdoptionId, Guid lessonId, CancellationToken ct)
    {
        var (title,failure)=await CheckAccess(curriculumAdoptionId,lessonId,ct);
        if(failure is not null)return failure;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson2PracticePage(curriculumAdoptionId,lessonId,title!,
            false,null,null,null,null,null,null,[]));
    }

    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(
        Guid curriculumAdoptionId, Guid lessonId,
        string? hidden,string? removed,string? addition,string? subtraction,
        string? tower,string? reasoning,CancellationToken ct)
    {
        var (title,failure)=await CheckAccess(curriculumAdoptionId,lessonId,ct);
        if(failure is not null)return failure;
        if(new[]{hidden,removed,addition,subtraction,tower,reasoning}
            .Any(x=>x?.Length>2000))
            return BadRequest("An activity answer exceeded the limit.");

        var tasks=new[]{
            ("Hidden ten-cubes",UsCcssG2Unit1SecondLessonEdulyticsCoolDown.VerifyHiddenTen(hidden)),
            ("Removed cubes",UsCcssG2Unit1SecondLessonEdulyticsCoolDown.VerifySubtractionBlank(removed)),
            ("Two related equations",UsCcssG2Unit1SecondLessonEdulyticsCoolDown.VerifyRelatedEquations(addition,subtraction)),
            ("Matching two-color tower",UsCcssG2Unit1SecondLessonEdulyticsCoolDown.VerifyTowerForTenMinusSeven(tower))
        };
        var feedback=tasks.Select(x=>new Grade2Lesson2Feedback(
            x.Item1,x.Item2.Correct,x.Item2.TeacherReviewNeeded,x.Item2.Message))
            .ToList();
        feedback.Add(new("Explain the ten-cube model",null,true,
            string.IsNullOrWhiteSpace(reasoning)
                ?"Your teacher will discuss how visible and hidden cubes relate."
                :"Explain your idea and visual model to a teacher; no automatic reasoning mark is awarded."));

        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson2PracticePage(
            curriculumAdoptionId,lessonId,title!,true,hidden,removed,addition,
            subtraction,tower,reasoning,feedback));
    }

    private async Task<(string? Title,IActionResult? Failure)> CheckAccess(
        Guid adoptionId,Guid lessonId,CancellationToken ct)
    {
        if(!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))
            return (null,Forbid());
        var workspace=await privatePractice.GetWorkspaceAsync(actor,adoptionId,ct);
        if(workspace.SelectedCurriculumAdoptionId!=adoptionId)
            return (null,NotFound());
        var option=workspace.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
        if(option is null || option.LessonCode!=Code)
            return (null,NotFound());
        var published=await lessonContent.GetPublishedForStudentAsync(
            actor,lessonId,CultureInfo.CurrentUICulture.Name,ct);
        if(published.Value is null)
            return (null,published.Error==LessonContentErrorCode.AccessDenied
                ?Forbid():NotFound());
        if(published.Value.LessonCode!=Code)
            return (null,NotFound());
        return (published.Value.Title,null);
    }
}
