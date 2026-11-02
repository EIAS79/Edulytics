using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Web.Controllers;
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u02-l10")]
public sealed class Grade2Lesson28ActivitiesController(
    IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit2TenthLessonCenterDay2.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson28Page(curriculumAdoptionId,lessonId,title!,false,new Grade2Lesson28Answers(),[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson28Answers answers,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null)return BadRequest("Missing answers");
        if(typeof(Grade2Lesson28Answers).GetProperties().Where(p=>p.PropertyType==typeof(string))
           .Select(p=>p.GetValue(answers) as string).Any(v=>v?.Length>2000))
            return BadRequest("Answer too long");
        var rows=new List<Grade2Lesson28Feedback>();
        void Add(string name,UsCcssG2Unit2TenthLessonCenterDay2.Review v)=>
            rows.Add(new(name,v.Correct,v.TeacherReviewNeeded,v.Feedback));
        Add("Source Notice and Wonder66−28",UsCcssG2Unit2TenthLessonCenterDay2.Warmup(
            answers.SourceWarmup,answers.WarmupTens,answers.WarmupOnes));
        Add("Source three-dice example73 with faces4,2,5",UsCcssG2Unit2TenthLessonCenterDay2.DiceMath(
            answers.SourceDiceCurrent,answers.SourceDiceBorrowChoice,answers.SourceDiceBorrowAnswer,
            answers.SourceDiceSimpleChoice,answers.SourceDiceSimpleAnswer));
        Add("Independent Edulytics numeric exit",UsCcssG2Unit2TenthLessonCenterDay2.Independent(
            answers.IndependentA,answers.IndependentB,answers.IndependentC));
        rows.Add(new("Authentic Stage5 dice game, choice center and disputed end condition",null,true,
            "Teacher confirms exact selected rules for winner: public source says both nearest-to-zero-after-six and first-below-ten. Teacher reviews real cube rolls, partners, cards and center choice; numeric examples never assert a win."));
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson28Page(curriculumAdoptionId,lessonId,title!,true,answers,rows));
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
