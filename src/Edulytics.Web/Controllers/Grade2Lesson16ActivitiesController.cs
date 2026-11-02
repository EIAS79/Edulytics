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
/// Grade2 U01 L16 original source numerical stories with three reads,
/// multiple valid representation choices and independent Cool-down.
/// No copied publisher protected visual or teacher credit inferred.
/// </summary>
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u01-l16")]
public sealed class Grade2Lesson16ActivitiesController(
    IStudentPrivatePracticeService privatePractice,
    ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit1SixteenthLessonComparePractice.LessonCode;

    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson16Page(curriculumAdoptionId,lessonId,
            title!,false,new Grade2Lesson16Answers(),[]));
    }

    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson16Answers answers,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null || answers.SourceAnswers is null ||
            answers.SourceAnswers.Count!=6)
            return BadRequest("The six original source problems are required.");
        if(typeof(Grade2Lesson16Answers).GetProperties()
            .Where(p=>p.PropertyType==typeof(string))
            .Select(p=>p.GetValue(answers) as string).Any(x=>x?.Length>2000) ||
            answers.SourceAnswers.Any(x=>new[]{x.SourceId,x.UnknownPosition,
                x.LargerPerson,x.NumericalAnswer,x.Equation}
                .Any(t=>t?.Length>200)))
            return BadRequest("One or more answers exceed the safe limit.");
        var warm=UsCcssG2Unit1SixteenthLessonComparePractice
            .CheckWarmup(answers.TrueOne,answers.TrueTwo,answers.FalseThree);
        var feedback=new List<Grade2Lesson16Feedback>{
            new("Original make-ten true or false warm-up",warm.Correct,true,warm.Explanation)
        };
        foreach(var (item,index) in answers.SourceAnswers.Select((x,i)=>(x,i)))
        {
            var review=UsCcssG2Unit1SixteenthLessonComparePractice
                .CheckOriginalProblem(item.SourceId,item.UnknownPosition,
                    item.LargerPerson,item.NumericalAnswer,item.Equation);
            feedback.Add(new($"Source compare story {index+1}",
                review.MachineScorable?review.Correct:null,true,review.Explanation));
        }
        var exit=UsCcssG2Unit1SixteenthLessonComparePractice
            .CheckNewEdulyticsCoolDown(answers.CoolLarger,
                answers.CoolSmaller,answers.CoolDifference);
        feedback.Add(new("Independent first-party three-unknown Cool-down",
            exit.Correct,false,exit.Explanation));
        feedback.Add(new("Three Reads, original diagrams and partner mathematical explanations",
            null,true,string.IsNullOrWhiteSpace(answers.RepresentationExplanation)
                ?"Explain what the story is about, which quantities matter, and what the question asks in THREE separate reads."
                :"Teacher checks your original tape or other correct drawings, real partner discourse and place-value strategy."));
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson16Page(curriculumAdoptionId,lessonId,
            title!,true,answers,feedback));
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
        if(source.Value is null)
            return (null,source.Error==LessonContentErrorCode.AccessDenied
                ?Forbid():NotFound());
        if(source.Value.LessonCode!=Code)return (null,NotFound());
        return (source.Value.Title,null);
    }
}
