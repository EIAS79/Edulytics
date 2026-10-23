using System.Globalization;
using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

/// <summary>Student-only source exact zoo story arithmetic; no official
/// grades, no assertion that a child actually completed MLR6 or manipulatives.</summary>
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u02-l03")]
public sealed class Grade2Lesson21ActivitiesController(
    IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit2ThirdLessonZooStories.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson21Page(curriculumAdoptionId,lessonId,title!,
            false,new Grade2Lesson21Answers(),[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson21Answers answers,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null)return BadRequest("Missing answers");
        if(typeof(Grade2Lesson21Answers).GetProperties()
            .Where(p=>p.PropertyType==typeof(string))
            .Select(p=>p.GetValue(answers) as string).Any(v=>v?.Length>2000))
            return BadRequest("Answers exceed length limit");
        var feedback=new List<Grade2Lesson21Feedback>();
        void Add(string label,UsCcssG2Unit2ThirdLessonZooStories.Review v)=>
            feedback.Add(new(label,v.Correct,v.TeacherReviewNeeded,v.Feedback));
        Add("Original zoo bus, start unknown",UsCcssG2Unit2ThirdLessonZooStories.CheckBus(
            answers.BusStart,answers.BusEquation));
        Add("Original other three zoo stories",UsCcssG2Unit2ThirdLessonZooStories.CheckOtherStories(
            answers.Monkey,answers.BearsGap,answers.BirdsOutside));
        Add("First-party independent story Cool-down",UsCcssG2Unit2ThirdLessonZooStories.CheckCoolDown(
            answers.CoolTake,answers.CoolCompare,answers.CoolPart));
        feedback.Add(new("Original tens/ones drawing, MLR6 Three Reads, blocks and genuine partner discussion",
            null,true,"Teacher must inspect student-chosen diagrams, the three reading stages and real classroom discussion; numeric answers never certify those."));
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson21Page(curriculumAdoptionId,lessonId,
            title!,true,answers,feedback));
    }
    private async Task<(string? Title,IActionResult? Failure)> Access(Guid adoptionId,Guid lessonId,CancellationToken ct)
    {
        if(!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))
            return (null,Forbid());
        var workspace=await privatePractice.GetWorkspaceAsync(actor,adoptionId,ct);
        if(workspace.SelectedCurriculumAdoptionId!=adoptionId)return (null,NotFound());
        var row=workspace.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
        if(row is null||row.LessonCode!=Code)return (null,NotFound());
        var published=await lessonContent.GetPublishedForStudentAsync(
            actor,lessonId,CultureInfo.CurrentUICulture.Name,ct);
        if(published.Value is null)return (null,
            published.Error==LessonContentErrorCode.AccessDenied?Forbid():NotFound());
        if(published.Value.LessonCode!=Code)return (null,NotFound());
        return (published.Value.Title,null);
    }
}
