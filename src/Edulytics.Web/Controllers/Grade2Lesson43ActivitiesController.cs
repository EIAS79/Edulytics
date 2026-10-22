using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Web.Controllers;
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u03-l09")]
public sealed class Grade2Lesson43ActivitiesController(
    IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit3NinthLessonFishFeet.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson43Page(curriculumAdoptionId,lessonId,title!,false,new Grade2Lesson43Answers(),[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,[FromForm] Grade2Lesson43Answers answers,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null)return BadRequest("Missing answers");
        if(typeof(Grade2Lesson43Answers).GetProperties().Where(p=>p.PropertyType==typeof(string))
           .Select(p=>p.GetValue(answers) as string).Any(s=>s?.Length>2000))
            return BadRequest("Answer too long");
        var rows=new List<Grade2Lesson43Feedback>();
        void Add(string label,UsCcssG2Unit3NinthLessonFishFeet.Review r)=>
            rows.Add(new(label,r.Correct,r.TeacherReviewNeeded,r.Feedback));
        Add("Source floor fish tape targets in feet",UsCcssG2Unit3NinthLessonFishFeet.SourceTapeFeet(
            answers.TapeAFeet,answers.TapeBFeet,answers.TapeCFeet,answers.TapeDFeet,answers.TapeDExtraInches));
        Add("Source fish tape single-inch segment targets",UsCcssG2Unit3NinthLessonFishFeet.SourceTapeInchIntervals(
            answers.TapeAInches,answers.TapeBInches,answers.TapeCInches,answers.TapeDInches));
        Add("Independent first-party physical ruler-span counting",UsCcssG2Unit3NinthLessonFishFeet.FirstParty(
            answers.FirstPartyOneSpan,answers.FirstPartyThreeSpans,answers.FirstPartyMixed));
        rows.Add(new("Real tape lengths, genuine warm-up fish estimates and choice of measuring tool",null,true,
            "Teacher checks true floor tape construction and ruler repetition, source fish picture reasoning using boy48in height, physical measurement of real classroom items, and real MLR2 discussion. No one can infer exact fish length from warm-up estimate."));
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson43Page(curriculumAdoptionId,lessonId,title!,true,answers,rows));
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
