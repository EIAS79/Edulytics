using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Web.Controllers;
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u03-l13")]
public sealed class Grade2Lesson47ActivitiesController(
    IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit3ThirteenthLessonCenterDay2.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson47Page(curriculumAdoptionId,lessonId,title!,false,new Grade2Lesson47Answers(),[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson47Answers answers,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null)return BadRequest("Missing answers");
        if(typeof(Grade2Lesson47Answers).GetProperties().Where(p=>p.PropertyType==typeof(string))
            .Select(p=>p.GetValue(answers) as string).Any(v=>v?.Length>2000))return BadRequest("Answer too long");
        var feedback=new List<Grade2Lesson47Feedback>();
        void Add(string label,UsCcssG2Unit3ThirteenthLessonCenterDay2.Review r)=>
            feedback.Add(new(label,r.Correct,r.TeacherReviewNeeded,r.Feedback));
        Add("Original Number Talk make ten",UsCcssG2Unit3ThirteenthLessonCenterDay2.Warmup(
            answers.TalkA,answers.TalkB,answers.TalkC,answers.TalkD));
        Add("Independent measurement difference illustration",UsCcssG2Unit3ThirteenthLessonCenterDay2.ExampleMeasurement(
            answers.MeasurementUnit,answers.MeasurementTarget,answers.MeasurementActual,answers.MeasurementScore));
        Add("First-party Number Puzzles missing terms",UsCcssG2Unit3ThirteenthLessonCenterDay2.OriginalArithmetic(
            answers.PuzzleAddUnknown,answers.PuzzleSubtractUnknown));
        Add("First-party independent exit",UsCcssG2Unit3ThirteenthLessonCenterDay2.IndependentExit(
            answers.ExitCm,answers.ExitInch,answers.ExitAdd));
        feedback.Add(new("Real eight-round partner measurement and printed Number Puzzles",null,true,
            "Teacher checks physical zero-based ruler, stop caller's absolute difference, eight-round scores and printed stage4 cards. Online numbers never attest genuine participation or winners."));
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson47Page(curriculumAdoptionId,lessonId,title!,true,answers,feedback));
    }
    private async Task<(string? Title,IActionResult? Failure)> Access(Guid adoptionId,Guid lessonId,CancellationToken ct)
    {
        if(!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))return (null,Forbid());
        var w=await privatePractice.GetWorkspaceAsync(actor,adoptionId,ct);
        if(w.SelectedCurriculumAdoptionId!=adoptionId)return (null,NotFound());
        var row=w.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
        if(row is null||row.LessonCode!=Code)return (null,NotFound());
        var content=await lessonContent.GetPublishedForStudentAsync(
            actor,lessonId,System.Globalization.CultureInfo.CurrentUICulture.Name,ct);
        if(content.Value is null)return (null,content.Error==LessonContentErrorCode.AccessDenied?Forbid():NotFound());
        if(content.Value.LessonCode!=Code)return (null,NotFound());
        return (content.Value.Title,null);
    }
}
