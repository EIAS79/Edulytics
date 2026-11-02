using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Web.Controllers;
/// <summary>Independent numeral practice, not a replica of physical source rectangle A–F lengths.</summary>
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u03-l03")]
public sealed class Grade2Lesson37ActivitiesController(
    IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit3ThirdLessonBuildRuler.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson37Page(curriculumAdoptionId,lessonId,title!,false,new Grade2Lesson37Answers(),[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson37Answers answers,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null)return BadRequest("Missing answers");
        if(typeof(Grade2Lesson37Answers).GetProperties().Where(p=>p.PropertyType==typeof(string))
           .Select(p=>p.GetValue(answers) as string).Any(s=>s?.Length>2000))
            return BadRequest("Answer too long");
        var rows=new List<Grade2Lesson37Feedback>();
        void Add(string label,UsCcssG2Unit3ThirdLessonBuildRuler.Review r)=>
            rows.Add(new(label,r.Correct,r.TeacherReviewNeeded,r.Feedback));
        Add("Original mental subtraction warm-up",UsCcssG2Unit3ThirdLessonBuildRuler.NumberTalk(
            answers.TalkA,answers.TalkB,answers.TalkC,answers.TalkD));
        Add("First-party ruler tick and span arithmetic",UsCcssG2Unit3ThirdLessonBuildRuler.RulerTickMeaning(
            answers.RulerTickZero,answers.RulerTickOne,answers.RulerTickTen,
            answers.RulerEnd,answers.RulerStart,answers.RulerLength));
        Add("Independent invented rectangle gap and joining",UsCcssG2Unit3ThirdLessonBuildRuler.FirstParty(
            answers.FirstPartyLong,answers.FirstPartyShort,answers.FirstPartyGap,answers.FirstPartyJoin));
        Add("Independent Edulytics ruler exit",UsCcssG2Unit3ThirdLessonBuildRuler.Exit(answers.ExitLength));
        rows.Add(new("Authentic source rectangles A–F and real student ruler",null,true,
            "The teacher verifies real 0–25cm drawn ruler tick spacing, authentic printed rectangle lengths and their comparisons. Public text does not supply rectangle A–F measurements."));
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson37Page(curriculumAdoptionId,lessonId,title!,true,answers,rows));
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
