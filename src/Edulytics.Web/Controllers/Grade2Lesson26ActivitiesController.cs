using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Web.Controllers;
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u02-l08")]
public sealed class Grade2Lesson26ActivitiesController(
    IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit2EighthLessonDifferentWays.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson26Page(curriculumAdoptionId,lessonId,title!,false,new Grade2Lesson26Answers(),[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson26Answers answers,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null)return BadRequest("Missing answers");
        if(typeof(Grade2Lesson26Answers).GetProperties().Where(p=>p.PropertyType==typeof(string))
            .Select(p=>p.GetValue(answers) as string).Any(s=>s?.Length>2000))
            return BadRequest("Answer too long");
        var rows=new List<Grade2Lesson26Feedback>();
        void Add(string label,UsCcssG2Unit2EighthLessonDifferentWays.Review r)=>
            rows.Add(new(label,r.Correct,r.TeacherReviewNeeded,r.Feedback));
        Add("Original four-expression Number Talk",UsCcssG2Unit2EighthLessonDifferentWays.NumberTalk(
            answers.TalkA,answers.TalkB,answers.TalkC,answers.TalkD));
        Add("Original Lin/Clare71−56 numeric steps",UsCcssG2Unit2EighthLessonDifferentWays.LinClare(
            answers.LinClareDifference,answers.ClareResidual,answers.ClareAfterExchangeOnes));
        Add("Andre and source three two-digit differences",UsCcssG2Unit2EighthLessonDifferentWays.AndreAndPractice(
            answers.Andre,answers.Source34,answers.Source82,answers.Source71));
        Add("First-party independent exit",UsCcssG2Unit2EighthLessonDifferentWays.Exit(
            answers.ExitA,answers.ExitB,answers.ExitC));
        rows.Add(new("Actual Lin/Clare/Andre diagrams and genuine discussion",null,true,
            "Teacher must review learner-authored diagrams, why Lin and Clare differ in sequence, MLR8 peer discussion and genuine base-ten blocks."));
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson26Page(curriculumAdoptionId,lessonId,title!,true,answers,rows));
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
