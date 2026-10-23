using System.Globalization;
using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

/// <summary>Optional G2 L17 genuine-collection rubric and independently
/// authored digital count/center arithmetic. Never saves an official grade.</summary>
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u01-l17")]
public sealed class Grade2Lesson17ActivitiesController(
    IStudentPrivatePracticeService privatePractice, ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit1SeventeenthLessonCenters.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson17Page(curriculumAdoptionId,lessonId,title!,
            false,new Grade2Lesson17Answers(),[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson17Answers answers,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null)return BadRequest("Missing activity answers");
        if(typeof(Grade2Lesson17Answers).GetProperties()
            .Where(p=>p.PropertyType==typeof(string))
            .Select(p=>p.GetValue(answers) as string)
            .Any(s=>s?.Length>2000))
            return BadRequest("Answer input exceeds safe limit");
        var rows=new List<Grade2Lesson17Feedback>();
        void Add(string label,UsCcssG2Unit1SeventeenthLessonCenters.Review review)=>
            rows.Add(new(label,review.Correct,review.TeacherReviewNeeded,review.Explanation));
        Add("Number Talk",UsCcssG2Unit1SeventeenthLessonCenters.CheckWarmup(
            answers.WarmOne,answers.WarmTwo,answers.WarmThree,answers.WarmFour));
        Add("First-party virtual collection",UsCcssG2Unit1SeventeenthLessonCenters.CheckDigitalCollection(
            answers.DigitalGrouping,answers.DigitalGroups,answers.DigitalRemainder,answers.DigitalTotal));
        Add("Real counting equation, teacher observation still needed",UsCcssG2Unit1SeventeenthLessonCenters.CheckPhysicalConsistency(
            answers.ActualGrouping,answers.ActualGroups,answers.ActualRemainder,answers.ActualTotal));
        Add("Choose two different centers",UsCcssG2Unit1SeventeenthLessonCenters.CheckChoicePair(
            answers.FirstCenter,answers.SecondCenter));
        Add("Independent center example 1",UsCcssG2Unit1SeventeenthLessonCenters.CheckChoice(
            answers.FirstCenter,answers.FirstAnswer));
        Add("Independent center example 2",UsCcssG2Unit1SeventeenthLessonCenters.CheckChoice(
            answers.SecondCenter,answers.SecondAnswer));
        Add("Independent original Edulytics Cool-down",UsCcssG2Unit1SeventeenthLessonCenters.CheckCoolDown(
            answers.CoolCount,answers.CoolAdd,answers.CoolGraph));
        rows.Add(new("Original physical objects, estimate, representations and partner work",null,true,
            "Teacher must inspect the actual collection, written estimate, authentic graph or grouping drawing and two center sessions. The application does not know how many physical objects were really present."));
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson17Page(curriculumAdoptionId,lessonId,
            title!,true,answers,rows));
    }
    private async Task<(string? Title,IActionResult? Failure)> Access(
        Guid adoptionId,Guid lessonId,CancellationToken ct)
    {
        if(!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))
            return (null,Forbid());
        var workspace=await privatePractice.GetWorkspaceAsync(actor,adoptionId,ct);
        if(workspace.SelectedCurriculumAdoptionId!=adoptionId)return (null,NotFound());
        var current=workspace.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
        if(current is null||current.LessonCode!=Code)return (null,NotFound());
        var source=await lessonContent.GetPublishedForStudentAsync(
            actor,lessonId,CultureInfo.CurrentUICulture.Name,ct);
        if(source.Value is null)return (null,source.Error==LessonContentErrorCode.AccessDenied
            ?Forbid():NotFound());
        if(source.Value.LessonCode!=Code)return (null,NotFound());
        return (source.Value.Title,null);
    }
}
