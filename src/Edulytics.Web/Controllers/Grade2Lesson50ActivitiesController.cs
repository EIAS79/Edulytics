using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Web.Controllers;
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u03-l16")]
public sealed class Grade2Lesson50ActivitiesController(
    IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit3SixteenthLessonPlantPlot.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson50Page(curriculumAdoptionId,lessonId,title!,
            false,new Grade2Lesson50Answers(),[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson50Answers answers,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null)return BadRequest("Missing answers");
        if(typeof(Grade2Lesson50Answers).GetProperties().Where(p=>p.PropertyType==typeof(string))
            .Select(p=>p.GetValue(answers) as string).Any(x=>x?.Length>2000))
            return BadRequest("Input too long");
        var rows=new List<Grade2Lesson50Feedback>();
        void Add(string label,UsCcssG2Unit3SixteenthLessonPlantPlot.Review v)=>
            rows.Add(new(label,v.Correct,v.TeacherReviewNeeded,v.Feedback));
        Add("Original four-expression addition Number Talk",UsCcssG2Unit3SixteenthLessonPlantPlot.Warmup(
            answers.TalkA,answers.TalkB,answers.TalkC,answers.TalkD));
        Add("Published Group B plant data (centimeters)",UsCcssG2Unit3SixteenthLessonPlantPlot.Source(
            answers.SourceMin,answers.SourceMax,answers.SourceDifference,
            answers.SourceAbove30,answers.SourceTotal,answers.SourceMode));
        Add("Equal-spaced source plot ticks and stack heights",UsCcssG2Unit3SixteenthLessonPlantPlot.TickCounts(
            answers.At25,answers.At33,answers.Empty28,answers.Empty34));
        Add("First-party fictional plant data, not source Group B",UsCcssG2Unit3SixteenthLessonPlantPlot.Fictional(
            answers.FictionMin,answers.FictionMax,answers.FictionMode,answers.FictionAbove26));
        rows.Add(new("Actual Han line-plot IMAGE and authentic classroom work",null,true,
            "Original Han plot figure is necessary to answer whether29cm is tallest, its true total, and a valid statement. Teacher checks that IMAGE, real paper plot, student partner comparison and MLR8 speech; none of these is inferred from numeric Group B answers."));
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson50Page(curriculumAdoptionId,lessonId,title!,true,answers,rows));
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
