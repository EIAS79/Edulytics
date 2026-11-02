using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Web.Controllers;
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u03-l06")]
public sealed class Grade2Lesson40ActivitiesController(
    IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit3SixthLessonReptileStories.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson40Page(curriculumAdoptionId,lessonId,title!,false,new Grade2Lesson40Answers(),[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,[FromForm] Grade2Lesson40Answers answers,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null)return BadRequest("Missing answers");
        if(typeof(Grade2Lesson40Answers).GetProperties().Where(p=>p.PropertyType==typeof(string))
           .Select(p=>p.GetValue(answers) as string).Any(x=>x?.Length>2000))
            return BadRequest("Answer too long");
        var rows=new List<Grade2Lesson40Feedback>();
        void Add(string label,UsCcssG2Unit3SixthLessonReptileStories.Review r)=>
            rows.Add(new(label,r.Correct,r.TeacherReviewNeeded,r.Feedback));
        Add("Four original Fives and Tens Number Talk expressions",UsCcssG2Unit3SixthLessonReptileStories.NumberTalk(
            answers.TalkA,answers.TalkB,answers.TalkC,answers.TalkD));
        Add("Lin/Jada bigger unknown and Mai/Diego smaller unknown",UsCcssG2Unit3SixthLessonReptileStories.OppositeKeywordStories(
            answers.JadaLizard,answers.DiegoSnake));
        Add("Eight source A/B reptile lengths in centimeters",UsCcssG2Unit3SixthLessonReptileStories.SourceReptileLengths(
            answers.DayGecko,answers.Komodo,answers.BabyCobra,answers.Iguana,
            answers.Ribbon,answers.Gila,answers.Alligator,answers.Ringneck));
        Add("Independent Edulytics comparison exit",UsCcssG2Unit3SixthLessonReptileStories.Independent(
            answers.FirstPartyLonger,answers.FirstPartyShorter,answers.FirstPartyGap));
        rows.Add(new("Authentic MLR6 Three Reads and secret partner guessing activity",null,true,
            "Teacher must observe two real partners choosing one reptile each from original opposite A/B lists, hidden clues, correctly matched tape diagrams and MLR8 discussion. Typed lengths do NOT certify secret partner gameplay."));
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson40Page(curriculumAdoptionId,lessonId,title!,true,answers,rows));
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
        if(content.Value is null)return (null,content.Error==LessonContentErrorCode.AccessDenied?Forbid():NotFound());
        if(content.Value.LessonCode!=Code)return (null,NotFound());
        return (content.Value.Title,null);
    }
}
