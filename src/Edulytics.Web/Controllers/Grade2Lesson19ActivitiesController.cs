using System.Globalization;
using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;
/// <summary>Original source snack and real cube tasks with guarded student
/// view. Numerical practice has no official grade or proof of real partner work.</summary>
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u02-l01")]
public sealed class Grade2Lesson19ActivitiesController(
    IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit2FirstLessonCompareActivities.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson19Page(curriculumAdoptionId,lessonId,title!,
            false,new Grade2Lesson19Answers(),[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson19Answers answers,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null)return BadRequest("Missing learner answers");
        if(typeof(Grade2Lesson19Answers).GetProperties()
           .Where(p=>p.PropertyType==typeof(string))
           .Select(p=>p.GetValue(answers) as string).Any(x=>x?.Length>2000))
            return BadRequest("Answer length exceeds safe limit");
        var rows=new List<Grade2Lesson19Feedback>();
        void Add(string activity,UsCcssG2Unit2FirstLessonCompareActivities.Review r)=>
            rows.Add(new(activity,r.Correct,r.TeacherReviewNeeded,r.Feedback));
        Add("Movie Snacks: source graph category arithmetic",
            UsCcssG2Unit2FirstLessonCompareActivities.CheckMovieSnacks(
                answers.SnackSum,answers.SnackDifference,
                answers.SnackAdditionEquation,answers.SnackDifferenceEquation));
        Add("Partner physical cube trains: exact arithmetic only",
            UsCcssG2Unit2FirstLessonCompareActivities.CheckBuildTrains(
                answers.CubeTotal,answers.CubeDifference,answers.CubeInverseEquation));
        Add("Independent Edulytics no-regroup Cool-down",
            UsCcssG2Unit2FirstLessonCompareActivities.CheckIndependentCoolDown(
                answers.CoolAdd,answers.CoolSubtract,answers.CoolCompare));
        rows.Add(new("Which One Doesn't Belong, original drawings, place value and MLR7 gallery",
            null,true,"The teacher must evaluate mathematically justified reasoning, actual cube trains, drawings and genuine partner participation. The form checks numbers, not classroom evidence."));
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson19Page(curriculumAdoptionId,lessonId,
            title!,true,answers,rows));
    }
    private async Task<(string? Title,IActionResult? Failure)> Access(
        Guid adoptionId,Guid lessonId,CancellationToken ct)
    {
        if(!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))
            return (null,Forbid());
        var workspace=await privatePractice.GetWorkspaceAsync(actor,adoptionId,ct);
        if(workspace.SelectedCurriculumAdoptionId!=adoptionId)return (null,NotFound());
        var lesson=workspace.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
        if(lesson is null||lesson.LessonCode!=Code)return (null,NotFound());
        var content=await lessonContent.GetPublishedForStudentAsync(
            actor,lessonId,CultureInfo.CurrentUICulture.Name,ct);
        if(content.Value is null)return (null,
            content.Error==LessonContentErrorCode.AccessDenied?Forbid():NotFound());
        if(content.Value.LessonCode!=Code)return (null,NotFound());
        return (content.Value.Title,null);
    }
}
