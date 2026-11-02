using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Web.Controllers;
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u02-l13")]
public sealed class Grade2Lesson31ActivitiesController(
    IStudentPrivatePracticeService privatePractice,ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit2ThirteenthLessonStoryEquations.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson31Page(curriculumAdoptionId,lessonId,title!,false,new Grade2Lesson31Answers(),[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson31Answers answers,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null)return BadRequest("Missing answers");
        if(typeof(Grade2Lesson31Answers).GetProperties().Where(p=>p.PropertyType==typeof(string))
          .Select(p=>p.GetValue(answers) as string).Any(x=>x?.Length>2000))
            return BadRequest("Answer too long");
        var rows=new List<Grade2Lesson31Feedback>();
        void Add(string label,UsCcssG2Unit2ThirteenthLessonStoryEquations.Review result)=>
            rows.Add(new(label,result.Correct,result.TeacherReviewNeeded,result.Feedback));
        Add("Original four story numeric unknowns",UsCcssG2Unit2ThirteenthLessonStoryEquations.SourceAnswers(
            answers.LinStart,answers.AndreGained,answers.DiegoTotal,answers.KiranPart));
        Add("Four original question-mark positions",UsCcssG2Unit2ThirteenthLessonStoryEquations.SourceQuestionMarks(
            answers.LinEquation,answers.AndreEquation,answers.DiegoEquation,answers.KiranEquation));
        Add("Independently authored Edulytics exit",UsCcssG2Unit2ThirteenthLessonStoryEquations.Independent(
            answers.FirstPartyStart,answers.FirstPartyChange,answers.FirstPartyTotal,answers.FirstPartyPart));
        rows.Add(new("Actual source diagrams, original Cards A–I/F–I and peer explanation",null,true,
            "Teacher checks genuine previous-lesson story cards, equation-card matches, TWO real solved F–I problems and self-created question-mark diagrams. These are not verified by a correct numeric result."));
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson31Page(curriculumAdoptionId,lessonId,title!,true,answers,rows));
    }
    private async Task<(string? Title,IActionResult? Failure)> Access(Guid adoptionId,Guid lessonId,CancellationToken ct)
    {
        if(!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))
            return (null,Forbid());
        var w=await privatePractice.GetWorkspaceAsync(actor,adoptionId,ct);
        if(w.SelectedCurriculumAdoptionId!=adoptionId)return (null,NotFound());
        var row=w.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
        if(row is null||row.LessonCode!=Code)return (null,NotFound());
        var c=await lessonContent.GetPublishedForStudentAsync(
            actor,lessonId,System.Globalization.CultureInfo.CurrentUICulture.Name,ct);
        if(c.Value is null)return (null,c.Error==LessonContentErrorCode.AccessDenied?Forbid():NotFound());
        if(c.Value.LessonCode!=Code)return (null,NotFound());
        return (c.Value.Title,null);
    }
}
