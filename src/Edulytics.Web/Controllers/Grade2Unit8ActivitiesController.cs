using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;
/// <summary>Private authentic Grade2 Unit8 source companion; original open responses never auto-certified.</summary>
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u08/l{lessonNumber:int}")]
public sealed class Grade2Unit8ActivitiesController(IStudentPrivatePracticeService privatePractice,
    ILessonContentService contentService):Controller
{
    private static bool Allowed(int n)=>n>=1&&n<=13;
    private static string Code(int n)=>$"PED:US-CCSS-MATH:G2:U08:L{n:D2}";
    private static Grade2Unit8Page Page(int n,Guid adoption,Guid lesson,string title,bool posted,
        Grade2Unit8Answers answers,IReadOnlyList<Grade2Unit8Feedback> feedback)=>
        new(adoption,lesson,title,n,posted,UsCcssG2Unit8OriginalActivities.AuthenticTasks[n],answers,feedback);
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(int lessonNumber,Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var(title,error)=await Access(lessonNumber,curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(Page(lessonNumber,curriculumAdoptionId,lessonId,title!,false,new(),[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(int lessonNumber,Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Unit8Answers a,CancellationToken ct)
    {
        var(title,error)=await Access(lessonNumber,curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(a is null)return BadRequest("Missing source answers");
        // Student-authored submissions must never impersonate authenticated teacher feedback.
        a.TeacherComments = null;
        if(typeof(Grade2Unit8Answers).GetProperties().Where(p=>p.PropertyType==typeof(string))
            .Select(p=>p.GetValue(a) as string).Any(x=>x?.Length>2000))
            return BadRequest("Input length exceeds source companion limit");
        var first=UsCcssG2Unit8OriginalActivities.Source(lessonNumber,a.SourceA,a.SourceB,a.SourceC,a.SourceD);
        var own=UsCcssG2Unit8OriginalActivities.NewMath(lessonNumber,a.OwnA,a.OwnB,a.OwnC,a.OwnD);
        var rows=new List<Grade2Unit8Feedback>{
            new("Source-specific numeric demonstrations",first.Correct,true,first.Feedback),
            new("New Edulytics independent numeric demonstrations",own.Correct,false,own.Feedback),
            new("Actual source number lines, source diagrams, block models, dice, methods and teacher-observed explanations",null,true,
                UsCcssG2Unit8OriginalActivities.AuthenticTasks[lessonNumber])
        };
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",Page(lessonNumber,curriculumAdoptionId,lessonId,title!,true,a,rows));
    }
    private async Task<(string? Title,IActionResult? Failure)> Access(int n,Guid adoption,Guid lessonId,CancellationToken ct)
    {
        if(!Allowed(n)||!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))
            return(null,Forbid());
        var workspace=await privatePractice.GetWorkspaceAsync(actor,adoption,ct);
        if(workspace.SelectedCurriculumAdoptionId!=adoption)return(null,NotFound());
        var lesson=workspace.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
        if(lesson is null||lesson.LessonCode!=Code(n))return(null,NotFound());
        var published=await contentService.GetPublishedForStudentAsync(actor,lessonId,
            System.Globalization.CultureInfo.CurrentUICulture.Name,ct);
        if(published.Value is null)return(null,published.Error==LessonContentErrorCode.AccessDenied?Forbid():NotFound());
        if(published.Value.LessonCode!=Code(n))return(null,NotFound());
        return(published.Value.Title,null);
    }
}
