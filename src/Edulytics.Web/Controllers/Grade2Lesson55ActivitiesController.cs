using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u04-l03")]
public sealed class Grade2Lesson55ActivitiesController(
    IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit4ThirdLessonUnlabeledTicks.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson55Page(curriculumAdoptionId,lessonId,title!,false,new Grade2Lesson55Answers(),[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson55Answers answers,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null)return BadRequest("Missing answers");
        if(typeof(Grade2Lesson55Answers).GetProperties().Where(p=>p.PropertyType==typeof(string))
            .Select(p=>p.GetValue(answers) as string).Any(x=>x?.Length>2000))
            return BadRequest("Input too long");
        var rows=new List<Grade2Lesson55Feedback>();
        void Add(string heading,UsCcssG2Unit4ThirdLessonUnlabeledTicks.Review r)=>
            rows.Add(new(heading,r.Correct,r.TeacherReviewNeeded,r.Feedback));
        Add("Original Activity1 number targets, teacher checks image point placement",
            UsCcssG2Unit4ThirdLessonUnlabeledTicks.SourceActivity1(
                answers.Original24,answers.Original37,answers.Original48,answers.Original83));
        Add("Original number-line landmark offset arithmetic, images teacher only",
            UsCcssG2Unit4ThirdLessonUnlabeledTicks.SourceLandmarkOffsets(
                answers.OffsetBefore25,answers.OffsetBefore40,answers.OffsetBefore50,answers.OffsetAfter80));
        Add("Independent Edulytics numerical checks",
            UsCcssG2Unit4ThirdLessonUnlabeledTicks.FirstPartyNumeric(
                answers.FirstParty83,answers.FirstPartyGap40to60,answers.FirstPartyGap17to43));
        rows.Add(new("Teacher-only original figures for17/59/43/35 and student dots for24/37/48/83",
            null,true,"Only the teacher can check original published diagram images, correct one-unit point positions and peer explanations. The plain-text source does not expose full second-activity figures for59 or35; no image coordinates or publisher cool-down marks invented."));
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson55Page(curriculumAdoptionId,lessonId,title!,true,answers,rows));
    }
    private async Task<(string? Title,IActionResult? Failure)> Access(Guid adoptionId,Guid lessonId,CancellationToken ct)
    {
        if(!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))return(null,Forbid());
        var w=await privatePractice.GetWorkspaceAsync(actor,adoptionId,ct);
        if(w.SelectedCurriculumAdoptionId!=adoptionId)return(null,NotFound());
        var lesson=w.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
        if(lesson is null||lesson.LessonCode!=Code)return(null,NotFound());
        var content=await lessonContent.GetPublishedForStudentAsync(actor,lessonId,
            System.Globalization.CultureInfo.CurrentUICulture.Name,ct);
        if(content.Value is null)return(null,content.Error==LessonContentErrorCode.AccessDenied?Forbid():NotFound());
        if(content.Value.LessonCode!=Code)return(null,NotFound());
        return(content.Value.Title,null);
    }
}
