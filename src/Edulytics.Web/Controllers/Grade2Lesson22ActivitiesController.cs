using System.Globalization;
using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

/// <summary>Optional 2021 games' arithmetic companion, real paper-clip/
/// counter play and winners need teacher observation. No official grade.</summary>
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u02-l04")]
public sealed class Grade2Lesson22ActivitiesController(
    IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit2FourthLessonOptionalCenters.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson22Page(curriculumAdoptionId,lessonId,title!,
            false,new Grade2Lesson22Answers(),[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson22Answers answers,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null)return BadRequest("No answers");
        if(typeof(Grade2Lesson22Answers).GetProperties()
            .Where(p=>p.PropertyType==typeof(string))
            .Select(p=>p.GetValue(answers) as string).Any(v=>v?.Length>2000))
            return BadRequest("Answer length exceeds limit");
        var rows=new List<Grade2Lesson22Feedback>();
        void Add(string label,UsCcssG2Unit2FourthLessonOptionalCenters.Review result)=>
            rows.Add(new(label,result.Correct,result.TeacherReviewNeeded,result.Feedback));
        Add("Number Talk — four subtraction facts",UsCcssG2Unit2FourthLessonOptionalCenters.CheckNumberTalk(
            answers.TalkOne,answers.TalkTwo,answers.TalkThree,answers.TalkFour));
        Add("Independent Capture Squares spinner/card arithmetic",UsCcssG2Unit2FourthLessonOptionalCenters.CheckCapture(
            answers.CaptureSpinner,answers.CaptureCard,answers.CaptureSum,answers.CaptureWild));
        Add("Independent Five in a Row two-turn sums and one-clip change",UsCcssG2Unit2FourthLessonOptionalCenters.CheckFiveTwoTurns(
            answers.FiveTopFirst,answers.FiveBottomFirst,answers.FiveSumFirst,
            answers.FiveTopSecond,answers.FiveBottomSecond,answers.FiveSumSecond));
        Add("Independent Edulytics center Cool-down",UsCcssG2Unit2FourthLessonOptionalCenters.CheckIndependentCoolDown(
            answers.CoolCapture,answers.CoolCompose,answers.CoolSubtract));
        rows.Add(new("Real gameboards, captured squares and first five-counter row",null,true,
            "Teacher checks actual legal edges, four-sided closure, partner turns and five counters in a row. Entered sums are NOT evidence of a win."));
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson22Page(curriculumAdoptionId,lessonId,
            title!,true,answers,rows));
    }
    private async Task<(string? Title,IActionResult? Failure)> Access(Guid adoptionId,Guid lessonId,CancellationToken ct)
    {
        if(!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))
            return (null,Forbid());
        var w=await privatePractice.GetWorkspaceAsync(actor,adoptionId,ct);
        if(w.SelectedCurriculumAdoptionId!=adoptionId)return (null,NotFound());
        var lesson=w.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
        if(lesson is null||lesson.LessonCode!=Code)return (null,NotFound());
        var v=await lessonContent.GetPublishedForStudentAsync(
            actor,lessonId,CultureInfo.CurrentUICulture.Name,ct);
        if(v.Value is null)return (null,v.Error==LessonContentErrorCode.AccessDenied?Forbid():NotFound());
        if(v.Value.LessonCode!=Code)return (null,NotFound());
        return (v.Value.Title,null);
    }
}
