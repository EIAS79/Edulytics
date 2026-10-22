using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Web.Controllers;
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u02-l06")]
public sealed class Grade2Lesson24ActivitiesController(
    IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit2SixthLessonCompareMethods.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson24Page(curriculumAdoptionId,lessonId,title!,
            false,new Grade2Lesson24Answers(),[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson24Answers answers,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null)return BadRequest("Missing answers");
        if(typeof(Grade2Lesson24Answers).GetProperties().Where(p=>p.PropertyType==typeof(string))
            .Select(p=>p.GetValue(answers) as string).Any(v=>v?.Length>2000))
            return BadRequest("Answer too long");
        var rows=new List<Grade2Lesson24Feedback>();
        void Add(string label,UsCcssG2Unit2SixthLessonCompareMethods.Review r)=>
            rows.Add(new(label,r.Correct,r.TeacherReviewNeeded,r.Feedback));
        Add("Three source place-value True/False expressions",
            UsCcssG2Unit2SixthLessonCompareMethods.Warmup(
                answers.True1,answers.True2,answers.True3));
        Add("82−9 source diagrams and equations",
            UsCcssG2Unit2SixthLessonCompareMethods.Diagram(
                answers.DiagramTens,answers.DiagramOnes,answers.DiagramAnswer));
        Add("Target Numbers Stage4 first-party numeric two-turn illustration",
            UsCcssG2Unit2SixthLessonCompareMethods.Target(
                answers.TargetStart,answers.TargetCard1,answers.TargetMode1,answers.TargetAfter1,
                answers.TargetCard2,answers.TargetMode2,answers.TargetAfter2));
        Add("Original Edulytics Cool-down",UsCcssG2Unit2SixthLessonCompareMethods.Cooldown(
            answers.Cool1,answers.Cool2,answers.Cool3));
        rows.Add(new("Actual 6-round center, authentic comparisons and teacher assessment",null,true,
            "A teacher verifies physical number cards, blocks, six legal turns, actual winner, MLR2 partner discussion and original diagrams."));
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson24Page(curriculumAdoptionId,lessonId,title!,true,answers,rows));
    }
    private async Task<(string? Title,IActionResult? Failure)> Access(Guid adoptionId,Guid lessonId,CancellationToken ct)
    {
        if(!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))
            return (null,Forbid());
        var w=await privatePractice.GetWorkspaceAsync(actor,adoptionId,ct);
        if(w.SelectedCurriculumAdoptionId!=adoptionId)return (null,NotFound());
        var row=w.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
        if(row is null||row.LessonCode!=Code)return (null,NotFound());
        var c=await lessonContent.GetPublishedForStudentAsync(
            actor,lessonId,System.Globalization.CultureInfo.CurrentUICulture.Name,ct);
        if(c.Value is null)return (null,c.Error==LessonContentErrorCode.AccessDenied?Forbid():NotFound());
        if(c.Value.LessonCode!=Code)return (null,NotFound());
        return (c.Value.Title,null);
    }
}
