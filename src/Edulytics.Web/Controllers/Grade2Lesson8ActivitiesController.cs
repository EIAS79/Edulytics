using System.Globalization;
using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

/// <summary>
/// Original Edulytics Grade 2 picture-graph reader and Cool-down,
/// authorized for current lesson. No publisher imagery or restricted exam
/// reproduction, no official grade persistence, teacher explanations
/// deliberately excluded from automatic scoring.
/// </summary>
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u01-l08")]
public sealed class Grade2Lesson8ActivitiesController(
    IStudentPrivatePracticeService privatePractice,
    ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit1EighthLessonEdulyticsCoolDown.LessonCode;

    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(
        Guid curriculumAdoptionId,Guid lessonId,CancellationToken cancellationToken)
    {
        var (title,error)=await CheckAccess(curriculumAdoptionId,lessonId,cancellationToken);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson8PracticePage(
            curriculumAdoptionId,lessonId,title!,false,new Grade2Lesson8Answers(),[]));
    }

    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson8Answers answers,CancellationToken cancellationToken)
    {
        var (title,error)=await CheckAccess(curriculumAdoptionId,lessonId,cancellationToken);
        if(error is not null)return error;
        if(answers is null)return BadRequest("Lesson answers are required.");
        if(typeof(Grade2Lesson8Answers).GetProperties()
            .Select(p=>p.GetValue(answers) as string)
            .Any(x=>x?.Length>2000))
            return BadRequest("Answer exceeded allowed length.");

        var sports=UsCcssG2Unit1EighthLessonEdulyticsCoolDown
            .CheckSportsReading(answers.AdultsBasketball,answers.ChildrenAdultsSoccerGap);
        var art=UsCcssG2Unit1EighthLessonEdulyticsCoolDown
            .CheckArtQuantities(answers.Markers,answers.CrayonsAndClay,
                answers.MoreMarkersThanCrayons,answers.PaintAndMarkers);
        var questionScope=UsCcssG2Unit1EighthLessonEdulyticsCoolDown
            .CheckWhichQuestionsGraphCanAnswer(
                answers.CanAskOne,answers.CanAskTwo,answers.CanAskThree,
                answers.CanAskFour,answers.CanAskFive);
        var coolDown=UsCcssG2Unit1EighthLessonEdulyticsCoolDown
            .CheckOriginalCoolDown(answers.CoolTotal,answers.CoolDifference);
        var feedback=new List<Grade2Lesson8Feedback>{
            new("Children versus adults sports charts",sports.Correct,false,sports.Reason),
            new("Category, combined, compare questions",art.Correct,false,art.Reason),
            new("Does the graph have enough information?",questionScope.Correct,false,questionScope.Reason),
            new("Edulytics independent Cool-down",coolDown.Correct,false,coolDown.Reason),
            new("Explain why a graph can/cannot answer a question",null,true,
                string.IsNullOrWhiteSpace(answers.Explanation)
                    ?"Explain at least one answerable and one unanswerable question to your teacher."
                    :"Your teacher reviews source-appropriate mathematical justification; no automatic explanation grade is awarded.")
        };
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson8PracticePage(
            curriculumAdoptionId,lessonId,title!,true,answers,feedback));
    }

    private async Task<(string? Title,IActionResult? Failure)> CheckAccess(
        Guid adoptionId,Guid lessonId,CancellationToken ct)
    {
        if(!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))
            return (null,Forbid());
        var workspace=await privatePractice.GetWorkspaceAsync(actor,adoptionId,ct);
        if(workspace.SelectedCurriculumAdoptionId!=adoptionId)return (null,NotFound());
        var lesson=workspace.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
        if(lesson is null||lesson.LessonCode!=Code)return (null,NotFound());
        var published=await lessonContent.GetPublishedForStudentAsync(
            actor,lessonId,CultureInfo.CurrentUICulture.Name,ct);
        if(published.Value is null)
            return (null,published.Error==LessonContentErrorCode.AccessDenied
                ?Forbid():NotFound());
        if(published.Value.LessonCode!=Code)return (null,NotFound());
        return (published.Value.Title,null);
    }
}
