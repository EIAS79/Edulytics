using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Web.Controllers;
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u03-l08")]
public sealed class Grade2Lesson42ActivitiesController(
    IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit3EighthLessonWhatIsAnInch.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson42Page(curriculumAdoptionId,lessonId,title!,false,new Grade2Lesson42Answers(),[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,[FromForm] Grade2Lesson42Answers answers,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null)return BadRequest("Missing answers");
        if(typeof(Grade2Lesson42Answers).GetProperties().Where(p=>p.PropertyType==typeof(string))
           .Select(p=>p.GetValue(answers) as string).Any(x=>x?.Length>2000))
            return BadRequest("Answer too long");
        var rows=new List<Grade2Lesson42Feedback>();
        void Add(string label,UsCcssG2Unit3EighthLessonWhatIsAnInch.Review r)=>
            rows.Add(new(label,r.Correct,r.TeacherReviewNeeded,r.Feedback));
        Add("Source one-inch tile versus inch-ruler0–1 interval",
            UsCcssG2Unit3EighthLessonWhatIsAnInch.InchTile(
                answers.InchTileSide,answers.InchRulerZeroOne));
        Add("Original Edulytics invented complete inch tiles and ruler intervals",
            UsCcssG2Unit3EighthLessonWhatIsAnInch.OriginalVirtual(
                answers.VirtualTiles,answers.VirtualTicks));
        Add("Independent Edulytics approximate whole-inch virtual endpoints",
            UsCcssG2Unit3EighthLessonWhatIsAnInch.NearestWholeVirtual(
                answers.VirtualBelow,answers.VirtualAbove));
        rows.Add(new("Real ruler and source geometric-shape measurements in inches",null,true,
            "Teacher checks physical inch tile and ruler, two real objects about1 inch, real marker, pencil,11 cubes, book; authentic rectangle/square/triangle printed shape estimates BEFORE measuring, actual zero-aligned readings and MLR8 discussion. No specific actual object size is asserted."));
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson42Page(curriculumAdoptionId,lessonId,title!,true,answers,rows));
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
