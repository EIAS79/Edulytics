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
/// Authenticated student original Number Puzzles center and reflection.
/// No publisher-sourced Cool-down is claimed; none is present in that
/// original optional center day. No official grade is stored by this form.
/// </summary>
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u01-l06")]
public sealed class Grade2Lesson6ActivitiesController(
    IStudentPrivatePracticeService privatePractice,
    ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit1SixthLessonEdulyticsCenterCheck.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(
        Guid curriculumAdoptionId,Guid lessonId,CancellationToken cancellationToken)
    {
        var (title,failure)=await CheckAccess(curriculumAdoptionId,lessonId,cancellationToken);
        if(failure is not null)return failure;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson6PracticePage(
            curriculumAdoptionId,lessonId,title!,false,null,null,null,null,null,null,null,[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(
        Guid curriculumAdoptionId,Guid lessonId,string? first,string? second,
        string? third,string? fourth,string? warmUp,
        string? centerChoice,string? explanation,CancellationToken cancellationToken)
    {
        var (title,failure)=await CheckAccess(curriculumAdoptionId,lessonId,cancellationToken);
        if(failure is not null)return failure;
        if(new[]{first,second,third,fourth,warmUp,centerChoice,explanation}
           .Any(x=>x?.Length>2000))return BadRequest("Activity answer exceeded length.");
        var board=UsCcssG2Unit1SixthLessonEdulyticsCenterCheck
            .CheckDigitBoard(first,second,third,fourth);
        var warm=UsCcssG2Unit1SixthLessonEdulyticsCenterCheck.CheckWarmup(warmUp);
        var validChoice=UsCcssG2Unit1SixthLessonEdulyticsCenterCheck
            .SourceAlignedCenterChoices.Contains(centerChoice??"",StringComparer.Ordinal);
        var reviews=new List<Grade2Lesson6Feedback>{
            new("Mental addition 14 + 22",warm.Correct,false,warm.Reason),
            new("Digit-card puzzle board",board.Correct,true,board.Reason),
            new("Previously introduced center choice",validChoice,true,
                validChoice?"You selected one of the previously learned centers."
                           :"Select a center listed in the lesson."),
            new("Center learning reflection",null,true,
                string.IsNullOrWhiteSpace(explanation)
                  ?"Describe what skill improved and talk about it with your teacher."
                  :"Teacher review is needed to assess your authentic activity and explanation.")
        };
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson6PracticePage(
            curriculumAdoptionId,lessonId,title!,true,first,second,third,
            fourth,warmUp,centerChoice,explanation,reviews));
    }
    private async Task<(string? Title,IActionResult? Failure)> CheckAccess(
        Guid adoptionId,Guid lessonId,CancellationToken ct)
    {
        if(!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))
            return (null,Forbid());
        var workspace=await privatePractice.GetWorkspaceAsync(actor,adoptionId,ct);
        if(workspace.SelectedCurriculumAdoptionId!=adoptionId)return (null,NotFound());
        var lesson=workspace.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
        if(lesson is null || lesson.LessonCode!=Code)return (null,NotFound());
        var detail=await lessonContent.GetPublishedForStudentAsync(
            actor,lessonId,CultureInfo.CurrentUICulture.Name,ct);
        if(detail.Value is null)
            return (null,detail.Error==LessonContentErrorCode.AccessDenied?Forbid():NotFound());
        if(detail.Value.LessonCode!=Code)return (null,NotFound());
        return (detail.Value.Title,null);
    }
}
