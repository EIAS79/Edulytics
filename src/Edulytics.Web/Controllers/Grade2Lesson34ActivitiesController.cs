using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Web.Controllers;
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u03-l01")]
public sealed class Grade2Lesson34ActivitiesController(
    IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit3FirstLessonStandardUnits.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson34Page(curriculumAdoptionId,lessonId,title!,false,new Grade2Lesson34Answers(),[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson34Answers answers,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null)return BadRequest("Missing answers");
        if(typeof(Grade2Lesson34Answers).GetProperties().Where(p=>p.PropertyType==typeof(string))
           .Select(p=>p.GetValue(answers) as string).Any(x=>x?.Length>2000))
            return BadRequest("Answer too long");
        var rows=new List<Grade2Lesson34Feedback>();
        void Add(string name,UsCcssG2Unit3FirstLessonStandardUnits.Review r)=>
            rows.Add(new(name,r.Correct,r.TeacherReviewNeeded,r.Feedback));
        Add("Source prepared 40cm ribbon with different straws and 1cm cubes",
            UsCcssG2Unit3FirstLessonStandardUnits.SourceUnits(
                answers.RealLongStraws,answers.RealShortStraws,answers.RealCentimeterCubes));
        Add("Independent first-party 30cm numeric model",
            UsCcssG2Unit3FirstLessonStandardUnits.FirstParty(
                answers.FirstPartyLong,answers.FirstPartyShort,answers.FirstPartyCubes));
        rows.Add(new("Authentic physical string measurement, cube size, and peer comparison",null,true,
            "Teacher checks same prepared string, true straw lengths, identical1-cm cube edges, end-to-end alignment with NO gaps/overlaps, and real discussion. Entered numbers do not certify physical measuring."));
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson34Page(curriculumAdoptionId,lessonId,title!,true,answers,rows));
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
        if(content.Value is null)return (null,content.Error==LessonContentErrorCode.AccessDenied?Forbid():NotFound());
        if(content.Value.LessonCode!=Code)return (null,NotFound());
        return (content.Value.Title,null);
    }
}
