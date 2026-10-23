using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Web.Controllers;
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u03-l15")]
public sealed class Grade2Lesson49ActivitiesController(
    IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit3FifteenthLessonPencilPlot.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson49Page(curriculumAdoptionId,lessonId,title!,
            false,new Grade2Lesson49Answers(),[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson49Answers answers,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null)return BadRequest("Missing answers");
        if(typeof(Grade2Lesson49Answers).GetProperties().Where(p=>p.PropertyType==typeof(string))
            .Select(p=>p.GetValue(answers) as string).Any(s=>s?.Length>2000))return BadRequest("Input too long");
        var rows=new List<Grade2Lesson49Feedback>();
        void Add(string name,UsCcssG2Unit3FifteenthLessonPencilPlot.Review r)=>
            rows.Add(new(name,r.Correct,r.TeacherReviewNeeded,r.Feedback));
        Add("Original subtraction Number Talk",UsCcssG2Unit3FifteenthLessonPencilPlot.Warmup(
            answers.TalkA,answers.TalkB,answers.TalkC,answers.TalkD));
        Add("Original Group C pencil data",UsCcssG2Unit3FifteenthLessonPencilPlot.SourceDataset(
            answers.SourceTotal,answers.SourceMode,answers.SourceShortest,
            answers.SourceLongest,answers.SourceLongerThan12));
        Add("Consecutive nonzero-start centimeter scale",UsCcssG2Unit3FifteenthLessonPencilPlot.SourceTicks(
            answers.EmptyAt11,answers.EmptyAt15,answers.CountAt10,answers.CountAt16));
        Add("Independent Edulytics fictional plot",UsCcssG2Unit3FifteenthLessonPencilPlot.Independent(
            answers.IndependentMode,answers.IndependentTotal,answers.IndependentRange));
        rows.Add(new("Real group of four pencil measurements and drawn line plot",null,true,
            "Teacher checks original 10–12 pencils each measured by two people with actual cm rulers, honest recorded pencil lengths, correct plot title/axis and MLR8 partner comparison. Public Group C data are NOT authentic measured group pencil values."));
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson49Page(curriculumAdoptionId,lessonId,title!,true,answers,rows));
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
