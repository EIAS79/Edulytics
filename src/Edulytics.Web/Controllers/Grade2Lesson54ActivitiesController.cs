using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Web.Controllers;

[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u04-l02")]
public sealed class Grade2Lesson54ActivitiesController(
    IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit4SecondLessonNumberLineFeatures.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,failure)=await Access(curriculumAdoptionId,lessonId,ct);
        if(failure is not null)return failure;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson54Page(curriculumAdoptionId,lessonId,title!,false,new Grade2Lesson54Answers(),[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,[FromForm] Grade2Lesson54Answers answers,CancellationToken ct)
    {
        var (title,failure)=await Access(curriculumAdoptionId,lessonId,ct);
        if(failure is not null)return failure;
        if(answers is null)return BadRequest("Missing answers");
        if(typeof(Grade2Lesson54Answers).GetProperties().Where(p=>p.PropertyType==typeof(string))
            .Select(p=>p.GetValue(answers) as string).Any(x=>x?.Length>2000))
            return BadRequest("Input too long");
        var feedback=new List<Grade2Lesson54Feedback>();
        void Add(string section,UsCcssG2Unit4SecondLessonNumberLineFeatures.Review item)=>
            feedback.Add(new(section,item.Correct,item.TeacherReviewNeeded,item.Feedback));
        Add("Original choral count landmarks within0..100",UsCcssG2Unit4SecondLessonNumberLineFeatures.ChoralCount(
            answers.FiveFirst,answers.FiveNext,answers.FiveFifty,answers.FiveLast));
        Add("Original source eleven-tick number line from0 to10",UsCcssG2Unit4SecondLessonNumberLineFeatures.SourceBlankLine(
            answers.MissingAfterZero,answers.MissingFive,answers.MissingTen,answers.FullTickCount));
        Add("Edulytics independent exact numerical companion",UsCcssG2Unit4SecondLessonNumberLineFeatures.FirstPartyNumbers(
            answers.FictionCardEighteen,answers.FictionDistanceSix,answers.FictionFiveCountTotal));
        feedback.Add(new("ACTUAL class yarn, card arrangement, publisher diagram revisions, real peer discussion",null,true,
            "Only a teacher with REAL source images and physical class evidence can verify Jada/Andre/Elena-specific errors, 0..30 yarn/card physical unit spacing, revisions, oral count and MLR8 dialogue. No guessed image data nor original publisher assessment grade."));
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson54Page(curriculumAdoptionId,lessonId,title!,true,answers,feedback));
    }
    private async Task<(string? Title,IActionResult? Failure)> Access(Guid adoptionId,Guid lessonId,CancellationToken ct)
    {
        if(!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))return(null,Forbid());
        var workspace=await privatePractice.GetWorkspaceAsync(actor,adoptionId,ct);
        if(workspace.SelectedCurriculumAdoptionId!=adoptionId)return(null,NotFound());
        var selected=workspace.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
        if(selected is null||selected.LessonCode!=Code)return(null,NotFound());
        var content=await lessonContent.GetPublishedForStudentAsync(
            actor,lessonId,System.Globalization.CultureInfo.CurrentUICulture.Name,ct);
        if(content.Value is null)return(null,content.Error==LessonContentErrorCode.AccessDenied?Forbid():NotFound());
        if(content.Value.LessonCode!=Code)return(null,NotFound());
        return(content.Value.Title,null);
    }
}
