using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Web.Controllers;
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u03-l18")]
public sealed class Grade2Lesson52ActivitiesController(
    IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit3EighteenthLessonYardstick.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson52Page(curriculumAdoptionId,lessonId,title!,false,new Grade2Lesson52Answers(),[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson52Answers answers,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null)return BadRequest("Missing answers");
        if(typeof(Grade2Lesson52Answers).GetProperties().Where(p=>p.PropertyType==typeof(string))
            .Select(p=>p.GetValue(answers) as string).Any(x=>x?.Length>2000))
            return BadRequest("Input too long");
        var rows=new List<Grade2Lesson52Feedback>();
        void Add(string label,UsCcssG2Unit3EighteenthLessonYardstick.Review v)=>
            rows.Add(new(label,v.Correct,v.TeacherReviewNeeded,v.Feedback));
        Add("Original source Number Talk 3 and5",UsCcssG2Unit3EighteenthLessonYardstick.Warmup(
            answers.TalkA,answers.TalkB,answers.TalkC,answers.TalkD));
        Add("3×5 card inch mark arithmetic (physical precision teacher only)",UsCcssG2Unit3EighteenthLessonYardstick.CardArithmetic(
            answers.OneInch,answers.TwoInches,answers.ElevenInches,answers.ThirtySixInches));
        Add("First-party fictional desk and arm examples",UsCcssG2Unit3EighteenthLessonYardstick.Fictional(
            answers.FictionDeskA,answers.FictionDeskB,answers.FictionDeskDifference,
            answers.FictionRemaining,answers.FictionForearmMinusHand));
        rows.Add(new("Real 36in yardstick, class desk line plot, peer revision and optional actual body measures",null,true,
            "Teacher must physically inspect correct 0..36 inch markers, equal intervals, two-person roles, real class desk measurements and plot, actual ruler revisions, and OPTIONAL hand/forearm measurements and TWO contextual equations. Fictional values do not prove any actual measurement."));
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson52Page(curriculumAdoptionId,lessonId,title!,true,answers,rows));
    }
    private async Task<(string? Title,IActionResult? Failure)> Access(Guid adoptionId,Guid lessonId,CancellationToken ct)
    {
        if(!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))return (null,Forbid());
        var w=await privatePractice.GetWorkspaceAsync(actor,adoptionId,ct);
        if(w.SelectedCurriculumAdoptionId!=adoptionId)return (null,NotFound());
        var lesson=w.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
        if(lesson is null||lesson.LessonCode!=Code)return (null,NotFound());
        var content=await lessonContent.GetPublishedForStudentAsync(
            actor,lessonId,System.Globalization.CultureInfo.CurrentUICulture.Name,ct);
        if(content.Value is null)return (null,content.Error==LessonContentErrorCode.AccessDenied?Forbid():NotFound());
        if(content.Value.LessonCode!=Code)return (null,NotFound());
        return (content.Value.Title,null);
    }
}
