using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Web.Controllers;
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u03-l05")]
public sealed class Grade2Lesson39ActivitiesController(
    IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit3FifthLessonMeters.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson39Page(curriculumAdoptionId,lessonId,title!,false,new Grade2Lesson39Answers(),[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,[FromForm] Grade2Lesson39Answers answers,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null)return BadRequest("Missing answers");
        if(typeof(Grade2Lesson39Answers).GetProperties().Where(p=>p.PropertyType==typeof(string))
           .Select(p=>p.GetValue(answers) as string).Any(s=>s?.Length>2000))
            return BadRequest("Answer too long");
        var rows=new List<Grade2Lesson39Feedback>();
        void Add(string label,UsCcssG2Unit3FifthLessonMeters.Review r)=>
            rows.Add(new(label,r.Correct,r.TeacherReviewNeeded,r.Feedback));
        Add("Source floor tape strips A to D",UsCcssG2Unit3FifthLessonMeters.SourceTapeABCD(
            answers.TapeA,answers.TapeB,answers.TapeC,answers.TapeD));
        Add("Source long floor strips E to G",UsCcssG2Unit3FifthLessonMeters.SourceTapeEFG(
            answers.TapeE,answers.TapeF,answers.TapeG));
        Add("Komodo D in meterstick units",UsCcssG2Unit3FifthLessonMeters.DInMeters(
            answers.KomodoMeters,answers.KomodoExtraCm));
        Add("Independent first-party meter conversion",UsCcssG2Unit3FifthLessonMeters.Independent(
            answers.ExitTotal247,answers.ExitRemainder365,answers.ExitFull412));
        rows.Add(new("Authentic teacher-built reptile tape measurement and MLR2 discussion",null,true,
            "A teacher must observe true floor tape placement, ruler/meterstick zero and repeats, students' tool choices, peer discussion and recorded units. Source tape lengths are target quantities, not certified student measurements."));
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson39Page(curriculumAdoptionId,lessonId,title!,true,answers,rows));
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
