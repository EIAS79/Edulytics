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
/// Authenticated first-party Grade 2 U01 L04 How Close/within20 learner activity.
/// Publisher Cool-down is replaced, not copied. Official grading/persistence
/// and teacher observations remain in the school assessment workflow.
/// </summary>
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u01-l04")]
public sealed class Grade2Lesson4ActivitiesController(
    IStudentPrivatePracticeService privatePractice,
    ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit1FourthLessonEdulyticsCoolDown.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(
        Guid curriculumAdoptionId,Guid lessonId,CancellationToken cancellationToken)
    {
        var (title,failed)=await Allow(curriculumAdoptionId,lessonId,cancellationToken);
        if(failed is not null)return failed;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson4PracticePage(
            curriculumAdoptionId,lessonId,title!,false,null,null,null,null,[]));
    }

    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(
        Guid curriculumAdoptionId,Guid lessonId,
        string? cardEquation,string? seventeenMinusNine,
        string? sevenPlusSix,string? explanation,CancellationToken cancellationToken)
    {
        var (title,failed)=await Allow(curriculumAdoptionId,lessonId,cancellationToken);
        if(failed is not null)return failed;
        if(new[]{cardEquation,seventeenMinusNine,sevenPlusSix,explanation}
            .Any(x=>x?.Length>2000))
            return BadRequest("Activity response too long.");
        var cards=UsCcssG2Unit1FourthLessonEdulyticsCoolDown
            .VerifyOriginalThreeCardEquation(cardEquation);
        var subtract=UsCcssG2Unit1FourthLessonEdulyticsCoolDown
            .VerifySubtractSeventeenNine(seventeenMinusNine);
        var add=UsCcssG2Unit1FourthLessonEdulyticsCoolDown
            .VerifySevenPlusSix(sevenPlusSix);
        var results=new List<Grade2Lesson4Feedback>{
            new("How Close: original three-card sum",cards.Correct,true,cards.Reason),
            new("Subtract 17 − 9",subtract.Correct,false,subtract.Reason),
            new("Add 7 + 6",add.Correct,false,add.Reason),
            new("Explain your chosen strategy",null,true,
                string.IsNullOrWhiteSpace(explanation)
                    ?"Discuss the number relationships, drawings or decomposition with your teacher."
                    :"Your teacher reviews this open strategy; no automatic explanation mark is awarded.")
        };
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson4PracticePage(
            curriculumAdoptionId,lessonId,title!,true,cardEquation,seventeenMinusNine,
            sevenPlusSix,explanation,results));
    }

    private async Task<(string? Title,IActionResult? Failure)> Allow(
        Guid adoptionId,Guid lessonId,CancellationToken ct)
    {
        if(!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))
            return (null,Forbid());
        var workspace=await privatePractice.GetWorkspaceAsync(actor,adoptionId,ct);
        if(workspace.SelectedCurriculumAdoptionId!=adoptionId)
            return (null,NotFound());
        var lesson=workspace.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
        if(lesson is null || lesson.LessonCode!=Code)return (null,NotFound());
        var published=await lessonContent.GetPublishedForStudentAsync(
            actor,lessonId,CultureInfo.CurrentUICulture.Name,ct);
        if(published.Value is null)
            return (null,published.Error==LessonContentErrorCode.AccessDenied
                ?Forbid():NotFound());
        if(published.Value.LessonCode!=Code)return (null,NotFound());
        return (published.Value.Title,null);
    }
}
