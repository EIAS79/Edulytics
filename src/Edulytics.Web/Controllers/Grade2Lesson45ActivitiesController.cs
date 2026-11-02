using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Web.Controllers;
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u03-l11")]
public sealed class Grade2Lesson45ActivitiesController(
    IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit3EleventhLessonSareeRibbon.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson45Page(curriculumAdoptionId,lessonId,title!,false,new Grade2Lesson45Answers(),[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson45Answers answers,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null)return BadRequest("Missing answers");
        if(typeof(Grade2Lesson45Answers).GetProperties().Where(p=>p.PropertyType==typeof(string))
            .Select(p=>p.GetValue(answers) as string).Any(x=>x?.Length>2000))
            return BadRequest("Answer too long");
        var rows=new List<Grade2Lesson45Feedback>();
        void Add(string name,UsCcssG2Unit3EleventhLessonSareeRibbon.Review r)=>
            rows.Add(new(name,r.Correct,r.TeacherReviewNeeded,r.Feedback));
        Add("Source 54−16 ribbon warm-up",UsCcssG2Unit3EleventhLessonSareeRibbon.Warmup(answers.WarmupRemain));
        Add("Four original ribbon and start-unknown results",
            UsCcssG2Unit3EleventhLessonSareeRibbon.SourceFour(
                answers.SourcePriyaAfter,answers.SourceElenaAfter,
                answers.SourceHanAfter,answers.SourcePriyaOriginal));
        Add("Independent Edulytics ribbon stories",UsCcssG2Unit3EleventhLessonSareeRibbon.FirstParty(
            answers.FirstPartyRemaining,answers.FirstPartyStarting,answers.FirstPartyExtra));
        rows.Add(new("Original dotted ribbon tape and real group discussion",null,true,
            "Teacher must observe source image, tape diagram labels, real base-ten blocks and MLR8 partner explanation. Numeric correctness cannot certify classroom performance."));
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson45Page(curriculumAdoptionId,lessonId,title!,true,answers,rows));
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
