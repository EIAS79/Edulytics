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
/// Grade 2 U01 L10 original Edulytics learner-created pictograph/bar graph
/// and independent exit ticket, only for authorized published source lesson.
/// No official grade or publisher worksheet/image reproduction is claimed.
/// </summary>
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u01-l10")]
public sealed class Grade2Lesson10ActivitiesController(
    IStudentPrivatePracticeService privatePractice,
    ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit1TenthLessonGraphConstruction.LessonCode;

    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(
        Guid curriculumAdoptionId,Guid lessonId,CancellationToken cancellationToken)
    {
        var (title,error)=await AuthorizeLesson(curriculumAdoptionId,lessonId,cancellationToken);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson10PracticePage(
            curriculumAdoptionId,lessonId,title!,false,new Grade2Lesson10Answers(),
            [],false,false,false));
    }

    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(
        Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson10Answers answers,CancellationToken cancellationToken)
    {
        var (title,error)=await AuthorizeLesson(curriculumAdoptionId,lessonId,cancellationToken);
        if(error is not null)return error;
        if(answers is null)return BadRequest("Graph responses are required.");
        if(typeof(Grade2Lesson10Answers).GetProperties()
           .Where(x=>x.PropertyType==typeof(string))
           .Select(x=>x.GetValue(answers) as string).Any(x=>x?.Length>2000) ||
           new[]{answers.PictureCounts,answers.BarCounts,answers.ExitPictureCounts,
               answers.ExitBarCounts}.Any(a=>a?.Length>4 || a?.Any(x=>x?.Length>100)==true))
            return BadRequest("Graph response exceeded accepted length.");

        var first=UsCcssG2Unit1TenthLessonGraphConstruction.VerifyPictureGraph(
            1,answers.PictureTitle,answers.PictureSymbol,answers.PictureKey,
            answers.PictureCounts);
        var second=UsCcssG2Unit1TenthLessonGraphConstruction.VerifyBarGraph(
            2,answers.BarTitle,answers.BarStart,answers.BarStep,
            answers.BarCounts);
        var exitPicture=UsCcssG2Unit1TenthLessonGraphConstruction.VerifyPictureGraph(
            3,answers.ExitTitle,answers.ExitSymbol,answers.ExitKey,
            answers.ExitPictureCounts);
        var exitBar=UsCcssG2Unit1TenthLessonGraphConstruction.VerifyBarGraph(
            3,answers.ExitTitle,"0","1",answers.ExitBarCounts);
        var readyExit=exitPicture.Correct && exitBar.Correct;
        var feedback=new List<Grade2Lesson10Feedback>{
            new("Activity 1: my picture graph from Table A",first.Correct,true,first.Reason),
            new("Activity 2: my bar graph from partner Table B",second.Correct,true,second.Reason),
            new("Edulytics Cool-down: same Table C in both graphs",
                readyExit,true,readyExit
                    ?"Both graphs match each other and every category in the assigned independent data table."
                    :"Check the picture key and each bar height: two representations of the SAME four categories must match."),
            new("Which One Doesn't Belong? multiple valid explanations",
                null,true,string.IsNullOrWhiteSpace(answers.WhichDoesNotBelong)
                    ?"Explain at least one mathematical distinction among four data display types."
                    :"Teacher review is required: a different mathematical reason can also be valid."),
            new("Partner feedback and display revision",
                null,true,string.IsNullOrWhiteSpace(answers.Reflection)
                    ?"Describe what your partner noticed about your graph and what you revised."
                    :"Teacher checks explanation, category labels, hand-created graphs and genuine partner participation.")
        };
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson10PracticePage(
            curriculumAdoptionId,lessonId,title!,true,answers,feedback,
            first.Correct,second.Correct,readyExit));
    }

    private async Task<(string? Title,IActionResult? Failure)> AuthorizeLesson(
        Guid adoptionId,Guid lessonId,CancellationToken ct)
    {
        if(!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))
            return (null,Forbid());
        var workspace=await privatePractice.GetWorkspaceAsync(actor,adoptionId,ct);
        if(workspace.SelectedCurriculumAdoptionId!=adoptionId)return (null,NotFound());
        var item=workspace.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
        if(item is null||item.LessonCode!=Code)return (null,NotFound());
        var published=await lessonContent.GetPublishedForStudentAsync(
            actor,lessonId,CultureInfo.CurrentUICulture.Name,ct);
        if(published.Value is null)
            return (null,published.Error==LessonContentErrorCode.AccessDenied
                ?Forbid():NotFound());
        if(published.Value.LessonCode!=Code)return (null,NotFound());
        return (published.Value.Title,null);
    }
}
