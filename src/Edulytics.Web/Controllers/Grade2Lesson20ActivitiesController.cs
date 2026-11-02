using System.Globalization;
using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;
/// <summary>G2 U02 L02 source methods, independently authored exit and student-only access.
/// No official grade or evidence of actual physical manipulation inferred.</summary>
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u02-l02")]
public sealed class Grade2Lesson20ActivitiesController(
    IStudentPrivatePracticeService privatePractice,
    ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit2SecondLessonUnknownAddends.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson20Page(curriculumAdoptionId,lessonId,title!,
            false,new Grade2Lesson20Answers(),[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson20Answers answers,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null)return BadRequest("Missing learner answers");
        if(typeof(Grade2Lesson20Answers).GetProperties()
            .Where(p=>p.PropertyType==typeof(string))
            .Select(p=>p.GetValue(answers) as string).Any(s=>s?.Length>2000))
            return BadRequest("The answer exceeds the safe length limit");
        var rows=new List<Grade2Lesson20Feedback>();
        void Add(string activity,UsCcssG2Unit2SecondLessonUnknownAddends.Review review)=>
            rows.Add(new(activity,review.Correct,review.TeacherReviewNeeded,review.Feedback));
        Add("Original 97 to 7 choral count by tens",
            UsCcssG2Unit2SecondLessonUnknownAddends.CheckWarmup(
                answers.Warm77,answers.Warm37,answers.Warm7));
        Add("Source exact three unknown addends and 67−55 inverse",
            UsCcssG2Unit2SecondLessonUnknownAddends.CheckSourceUnknowns(
                answers.Source41,answers.Source17,answers.Source21,answers.SourceInverse));
        Add("Han and Mai: inverse addition/subtraction equations",
            UsCcssG2Unit2SecondLessonUnknownAddends.CheckTwoMethods(
                answers.HanAddition,answers.MaiSubtraction));
        Add("Independent Edulytics no-regroup Cool-down",
            UsCcssG2Unit2SecondLessonUnknownAddends.CheckIndependentCoolDown(
                answers.CoolFirst,answers.CoolSecond,answers.CoolThird));
        rows.Add(new("Real base-ten blocks, connecting cubes, student-selected tool and authentic peer explanation",
            null,true,"Teacher must directly examine actual blocks and students' own drawings/strategies; numerical correct submissions alone do not prove physical work."));
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson20Page(curriculumAdoptionId,lessonId,
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
        var source=await lessonContent.GetPublishedForStudentAsync(
            actor,lessonId,CultureInfo.CurrentUICulture.Name,ct);
        if(source.Value is null)return (null,
            source.Error==LessonContentErrorCode.AccessDenied?Forbid():NotFound());
        if(source.Value.LessonCode!=Code)return (null,NotFound());
        return (source.Value.Title,null);
    }
}
