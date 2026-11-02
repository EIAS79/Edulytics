using System.Globalization;
using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

/// <summary>Source-faithful optional REAL classroom survey + independently
/// authored numeric cooldown. Does not store names, grades or responses.</summary>
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u01-l18")]
public sealed class Grade2Lesson18ActivitiesController(
    IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit1EighteenthLessonClassSurveys.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,failure)=await Access(curriculumAdoptionId,lessonId,ct);
        if(failure is not null)return failure;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson18Page(curriculumAdoptionId,lessonId,title!,
            false,new Grade2Lesson18Answers(),[],null));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson18Answers answers,CancellationToken ct)
    {
        var (title,failure)=await Access(curriculumAdoptionId,lessonId,ct);
        if(failure is not null)return failure;
        if(answers is null||answers.Categories is null||answers.Counts is null||
           answers.Categories.Count!=4||answers.Counts.Count!=4)
            return BadRequest("Exactly four category input slots are required");
        if(typeof(Grade2Lesson18Answers).GetProperties()
           .Where(p=>p.PropertyType==typeof(string))
           .Select(p=>p.GetValue(answers) as string).Any(x=>x?.Length>2000) ||
           answers.Categories.Any(x=>x?.Length>80)||
           answers.Counts.Any(x=>x?.Length>8))
            return BadRequest("Survey input exceeds safe limits");
        var good=UsCcssG2Unit1EighteenthLessonClassSurveys.TryReadSurvey(
            answers.SurveyQuestion,answers.GraphType,answers.GraphTitle,
            answers.NumberCategories,answers.Categories,answers.Counts,
            out var survey);
        var feedback=new List<Grade2Lesson18Feedback>();
        void Add(string activity,UsCcssG2Unit1EighteenthLessonClassSurveys.Review r)=>
            feedback.Add(new(activity,r.Correct,r.TeacherReviewNeeded,r.Feedback));
        Add("Original self-created classroom survey structure",
            new(good,true,good
                ?"You submitted 3 or 4 distinct named categories and consistent counts. A teacher must confirm classmates were ACTUALLY surveyed anonymously."
                :"Create three or four distinct categories and count one response per person. This cannot establish real survey participation."));
        Add("Original picture or bar graph scale and labels",
            UsCcssG2Unit1EighteenthLessonClassSurveys.CheckGraph(
                survey,answers.PictureKey,answers.AxisStart,answers.AxisStep));
        Add("Peer-created combine/compare questions and two-fact tape equations",
            UsCcssG2Unit1EighteenthLessonClassSurveys.CheckPair(
                survey,answers.CategoryOne,answers.CategoryTwo,
                answers.CombineAnswer,answers.CombineEquation,
                answers.CompareAnswer,answers.CompareEquation,
                answers.TapeDifference,answers.TapeEquation));
        Add("Independent first-party fictional data Cool-down",
            UsCcssG2Unit1EighteenthLessonClassSurveys.CheckCoolDown(
                answers.CoolTotal,answers.CoolCombine,
                answers.CoolCompare,answers.CoolTape));
        feedback.Add(new("Real peer survey, personally authored questions, graph drawing and partner explanation",
            null,true,"Teacher evaluates actual classmates' anonymous responses, partner exchange, student-authored questions and drawing. A correct entered number cannot prove any of those happened."));
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson18Page(curriculumAdoptionId,lessonId,title!,
            true,answers,feedback,survey));
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
