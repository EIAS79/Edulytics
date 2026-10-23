using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Web.Controllers;
/// <summary>Numeric centimeter schematic. Real original reptile images/measurements checked by teacher.</summary>
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u03-l02")]
public sealed class Grade2Lesson36ActivitiesController(
    IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit3SecondLessonMeasureCm.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson36Page(curriculumAdoptionId,lessonId,title!,false,new Grade2Lesson36Answers(),[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson36Answers answers,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null)return BadRequest("Missing answers");
        if(typeof(Grade2Lesson36Answers).GetProperties().Where(p=>p.PropertyType==typeof(string))
            .Select(p=>p.GetValue(answers) as string).Any(v=>v?.Length>2000))
            return BadRequest("Answer too long");
        var rows=new List<Grade2Lesson36Feedback>();
        void Add(string section,UsCcssG2Unit3SecondLessonMeasureCm.Review r)=>
            rows.Add(new(section,r.Correct,r.TeacherReviewNeeded,r.Feedback));
        Add("Source bearded dragon 24cm and ten-cm rod plus extra centimeter edges",
            UsCcssG2Unit3SecondLessonMeasureCm.BeardedDragon(
                answers.SourceDragonCubes,answers.SourceDragonRods,
                answers.SourceDragonExtraCm,answers.SourceDragonLength));
        Add("Independent first-party cm rod/reptile comparison and exit",
            UsCcssG2Unit3SecondLessonMeasureCm.FirstParty(
                answers.FirstPartyRods,answers.FirstPartyExtra,answers.FirstPartyDifference,
                answers.ExitRods,answers.ExitExtra));
        rows.Add(new("Actual source reptile print sheet, partner measurement and MLR8",null,true,
            "Teacher must measure the ORIGINAL printed reptile illustrations with actual1cm cubes/10cm rods. Public source does not specify measured numbers for gecko and threadsnake; their actual difference cannot be judged digitally here."));
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson36Page(curriculumAdoptionId,lessonId,title!,true,answers,rows));
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
