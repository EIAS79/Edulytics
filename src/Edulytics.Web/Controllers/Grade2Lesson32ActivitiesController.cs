using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Web.Controllers;
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u02-l14")]
public sealed class Grade2Lesson32ActivitiesController(
    IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit2FourteenthLessonTwoStepSeeds.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson32Page(curriculumAdoptionId,lessonId,title!,false,new Grade2Lesson32Answers(),[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson32Answers answers,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null)return BadRequest("Missing answers");
        if(typeof(Grade2Lesson32Answers).GetProperties().Where(p=>p.PropertyType==typeof(string))
           .Select(p=>p.GetValue(answers) as string).Any(v=>v?.Length>2000))
            return BadRequest("Answer too long");
        var rows=new List<Grade2Lesson32Feedback>();
        void Add(string name,UsCcssG2Unit2FourteenthLessonTwoStepSeeds.Review r)=>
            rows.Add(new(name,r.Correct,r.TeacherReviewNeeded,r.Feedback));
        Add("Four original Number Talk sums",UsCcssG2Unit2FourteenthLessonTwoStepSeeds.Warmup(
            answers.WarmupOne,answers.WarmupTwo,answers.WarmupThree,answers.WarmupFour));
        Add("Andre's TWO successive seed questions",UsCcssG2Unit2FourteenthLessonTwoStepSeeds.SourceAndre(
            answers.AndreFirst,answers.AndreTotal));
        Add("Original Priya, Mai and Kiran three-person group and total",UsCcssG2Unit2FourteenthLessonTwoStepSeeds.GroupStories(
            answers.PriyaSmaller,answers.MaiMissing,answers.KiranResult,answers.GroupTotal));
        Add("Source student section summary",UsCcssG2Unit2FourteenthLessonTwoStepSeeds.SectionSummary(answers.JadaStart));
        Add("Independently authored Edulytics numeric exit",UsCcssG2Unit2FourteenthLessonTwoStepSeeds.FirstParty(
            answers.FirstPartyTriple,answers.FirstPartySmaller,answers.FirstPartyPart,answers.FirstPartyResult));
        rows.Add(new("Original three-person work assignment, models and MLR7 gallery discussion",null,true,
            "Teacher must observe each group member solving separate true stories, final combined answer and authentic explanations. Numeric inputs cannot certify participation."));
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson32Page(curriculumAdoptionId,lessonId,title!,true,answers,rows));
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
