using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Web.Controllers;
/// <summary>Digital ruler illustrations are NOT student-observed classroom object measurement.</summary>
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u03-l04")]
public sealed class Grade2Lesson38ActivitiesController(
    IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit3FourthLessonEstimateCm.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson38Page(curriculumAdoptionId,lessonId,title!,false,new Grade2Lesson38Answers(),[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson38Answers answers,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null)return BadRequest("No answers");
        if(typeof(Grade2Lesson38Answers).GetProperties().Where(p=>p.PropertyType==typeof(string))
            .Select(p=>p.GetValue(answers) as string).Any(s=>s?.Length>2000))
            return BadRequest("Answer too long");
        var rows=new List<Grade2Lesson38Feedback>();
        void Add(string key,UsCcssG2Unit3FourthLessonEstimateCm.Review r)=>
            rows.Add(new(key,r.Correct,r.TeacherReviewNeeded,r.Feedback));
        Add("Original ruler images A–D reasoning",UsCcssG2Unit3FourthLessonEstimateCm.RulerImage(
            answers.ValidSourceRulers,answers.LengthFromOneToFive));
        Add("Independent Edulytics shifted-tick calibrated ruler",UsCcssG2Unit3FourthLessonEstimateCm.FirstPartyTicks(
            answers.VirtualStart,answers.VirtualEnd,answers.VirtualLength));
        Add("Original Edulytics 30-cm ruler iteration",UsCcssG2Unit3FourthLessonEstimateCm.IteratedRuler(
            answers.LongFullRulers,answers.LongRemaining,answers.LongTotal));
        Add("Order of too-low, about-right, too-high numerical estimates",UsCcssG2Unit3FourthLessonEstimateCm.EstimateOrder(
            answers.ThreeEstimatesLow,answers.ThreeEstimatesAbout,answers.ThreeEstimatesHigh));
        Add("Original independent shifted-tick Cool-down",UsCcssG2Unit3FourthLessonEstimateCm.FirstPartyTicks(
            answers.IndependentTickStart,answers.IndependentTickEnd,answers.IndependentTickAnswer));
        rows.Add(new("Genuine classroom measurement, real object lengths and MLR2 explanation",
            null,true,"The teacher must observe measuring ACTUAL objects with real centimeter rulers and true zero alignment. No typed number verifies a real object's length, ruler use or partner discussion."));
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson38Page(curriculumAdoptionId,lessonId,title!,true,answers,rows));
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
