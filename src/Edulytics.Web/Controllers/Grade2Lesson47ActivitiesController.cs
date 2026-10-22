using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Web.Controllers;
/// <summary>Optional Grade2 center's mathematical companion, NOT actual eight-round winner.</summary>
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u03-l13")]
public sealed class Grade2Lesson47ActivitiesController(
    IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit3ThirteenthLessonTargetMeasurement.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson47Page(curriculumAdoptionId,lessonId,title!,
            false,new Grade2Lesson47Answers(),[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson47Answers answers,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null)return BadRequest("Missing answers");
        if(typeof(Grade2Lesson47Answers).GetProperties()
            .Where(p=>p.PropertyType==typeof(string))
            .Select(p=>p.GetValue(answers) as string).Any(x=>x?.Length>2000))
            return BadRequest("Answer field too long");
        var rows=new List<Grade2Lesson47Feedback>();
        void Add(string label,UsCcssG2Unit3ThirteenthLessonTargetMeasurement.Review review)=>
            rows.Add(new(label,review.Correct,review.TeacherReviewNeeded,review.Feedback));
        Add("Original four Number Talk expressions",UsCcssG2Unit3ThirteenthLessonTargetMeasurement.Warmup(
            answers.TalkA,answers.TalkB,answers.TalkC,answers.TalkD));
        Add("First-party line-segment absolute error",UsCcssG2Unit3ThirteenthLessonTargetMeasurement.CheckTarget(
            answers.Unit,answers.TargetLength,answers.ActualLength,answers.AbsoluteError));
        Add("First-party Number Puzzles composing and decomposing",UsCcssG2Unit3ThirteenthLessonTargetMeasurement.Puzzle(
            answers.PuzzleSum,answers.PuzzleUnknownAddend,answers.PuzzleDifference,answers.PuzzleUnknownSubtrahend));
        Add("Original Edulytics independent check",UsCcssG2Unit3ThirteenthLessonTargetMeasurement.Exit(
            answers.ExitA,answers.ExitB,answers.ExitC));
        rows.Add(new("Authentic ruler, target and eight-round center work",null,true,
            "Teacher must observe drawing, ruler zero alignment, original gameboards and digits, partner STOP signals, eight rounds and the LOWEST actual score winning. Entering a digital result is not evidence."));
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson47Page(curriculumAdoptionId,lessonId,title!,
            true,answers,rows));
    }
    private async Task<(string? Title,IActionResult? Failure)> Access(Guid adoptionId,Guid lessonId,CancellationToken ct)
    {
        if(!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))
            return (null,Forbid());
        var w=await privatePractice.GetWorkspaceAsync(actor,adoptionId,ct);
        if(w.SelectedCurriculumAdoptionId!=adoptionId)return (null,NotFound());
        var row=w.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
        if(row is null||row.LessonCode!=Code)return (null,NotFound());
        var content=await lessonContent.GetPublishedForStudentAsync(
            actor,lessonId,System.Globalization.CultureInfo.CurrentUICulture.Name,ct);
        if(content.Value is null)
            return (null,content.Error==LessonContentErrorCode.AccessDenied?Forbid():NotFound());
        if(content.Value.LessonCode!=Code)return (null,NotFound());
        return (content.Value.Title,null);
    }
}
