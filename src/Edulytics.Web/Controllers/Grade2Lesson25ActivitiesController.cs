using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Web.Controllers;
/// <summary>Protected source-math companion; authentic group cards remain teacher-reviewed.</summary>
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u02-l07")]
public sealed class Grade2Lesson25ActivitiesController(
    IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit2SeventhLessonSubtractTwoDigits.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson25Page(curriculumAdoptionId,lessonId,title!,false,new Grade2Lesson25Answers(),[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson25Answers answers,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null)return BadRequest("Missing answers");
        if(typeof(Grade2Lesson25Answers).GetProperties().Where(p=>p.PropertyType==typeof(string))
            .Select(p=>p.GetValue(answers) as string).Any(v=>v?.Length>2000))
            return BadRequest("Answer length exceeded");
        var results=new List<Grade2Lesson25Feedback>();
        void Add(string title,UsCcssG2Unit2SeventhLessonSubtractTwoDigits.Review r)=>
            results.Add(new(title,r.Correct,r.TeacherReviewNeeded,r.Feedback));
        Add("Original 46−28 and 93−54",UsCcssG2Unit2SeventhLessonSubtractTwoDigits.SourceProblems(
            answers.FirstDifference,answers.SecondDifference,answers.FirstTradedTens,
            answers.FirstTradedOnes,answers.SecondTradedTens,answers.SecondTradedOnes));
        Add("First-party examples, not source cards",UsCcssG2Unit2SeventhLessonSubtractTwoDigits.FirstParty(
            answers.FirstPartyA,answers.FirstPartyB));
        Add("Independently authored Edulytics exit",UsCcssG2Unit2SeventhLessonSubtractTwoDigits.Exit(
            answers.CoolA,answers.CoolB,answers.CoolC));
        results.Add(new("Authentic group-of-four source card game",null,true,
            "Teacher must inspect the real Diego, Lin, Jada, Han cards, actual two action cards for each, physical manipulations and group total. No card contents or gameplay are assumed."));
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson25Page(curriculumAdoptionId,lessonId,title!,true,answers,results));
    }
    private async Task<(string? Title,IActionResult? Failure)> Access(Guid adoptionId,Guid lessonId,CancellationToken ct)
    {
        if(!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))
            return (null,Forbid());
        var w=await privatePractice.GetWorkspaceAsync(actor,adoptionId,ct);
        if(w.SelectedCurriculumAdoptionId!=adoptionId)return (null,NotFound());
        var lesson=w.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
        if(lesson is null||lesson.LessonCode!=Code)return (null,NotFound());
        var content=await lessonContent.GetPublishedForStudentAsync(
            actor,lessonId,System.Globalization.CultureInfo.CurrentUICulture.Name,ct);
        if(content.Value is null)
            return (null,content.Error==LessonContentErrorCode.AccessDenied?Forbid():NotFound());
        if(content.Value.LessonCode!=Code)return (null,NotFound());
        return (content.Value.Title,null);
    }
}
