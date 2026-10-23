using System.Globalization;
using System.Security.Claims;
using Edulytics.Core.Mathematics.Curriculum;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edulytics.Web.Controllers;

[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade3-u01-l07")]
public sealed class Grade3Lesson7ActivitiesController(
    IStudentPrivatePracticeService practice,ILessonContentService lessons):Controller
{
    private const string Code=UsCcssG3Unit1Lesson7GraphDifferences.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var error=await CheckAccess(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex, nofollow, noarchive";
        return View(new Grade3Lesson7PracticePage(curriculumAdoptionId,lessonId,
            false,new Grade3Lesson7Answers(),[]));
    }

    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade3Lesson7Answers answers,CancellationToken ct)
    {
        var error=await CheckAccess(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null || typeof(Grade3Lesson7Answers).GetProperties()
            .Select(x=>x.GetValue(answers) as string).Any(x=>x?.Length>2000))
            return BadRequest("Lesson answer is missing or too long.");
        var exact=new[]{
            ("L07-SEASONS-TOTAL","Seasons total",answers.SeasonsTotal),
            ("L07-SEASONS-SPRING-OR-FALL","Spring or Fall",answers.SpringOrFall),
            ("L07-SEASONS-SUMMER-MINUS-WINTER","Summer more than Winter",answers.SummerWinter),
            ("L07-SEASONS-FALL-MINUS-SPRING","Spring fewer than Fall",answers.FallSpring),
            ("L07-EDU-CD-SOCCER-MINUS-TENNIS","New Cool-down Soccer versus Tennis",answers.CoolSoccerTennis),
            ("L07-EDU-CD-SWIMMING-PLUS-BASKETBALL","New Cool-down Swim and Basketball",answers.CoolSwimBasket),
            ("L07-EDU-CD-TOTAL","New Cool-down total",answers.CoolTotal)
        };
        var feedback=exact.Select(item=>{
            if(!int.TryParse(item.Item3,NumberStyles.Integer,CultureInfo.InvariantCulture,out var n)
                || !UsCcssG3Unit1Lesson7GraphDifferences.TryGradeExact(item.Item1,n,out var correct))
                return new Grade3Lesson7Feedback(item.Item2,false,false,"Enter a whole number.");
            return new Grade3Lesson7Feedback(item.Item2,correct,false,
                correct?"Exact source table/first-party assessment arithmetic is correct."
                    :"Read the correct data categories and redo the calculation.");
        }).ToList();

        var estimates=new[]{
            ("L07-BUGS-TOTAL","Estimated bug count",answers.BugTotalEstimate),
            ("L07-BUGS-ANTS-MINUS-SPIDERS","Estimated Ant–Spider difference",answers.AntSpiderEstimate),
            ("L07-BUGS-ANTS-MINUS-LADYBUGS","Estimated Ant–Ladybug difference",answers.AntLadybugEstimate)
        };
        foreach(var entry in estimates){
            if(!int.TryParse(entry.Item3,NumberStyles.Integer,CultureInfo.InvariantCulture,out var n))
            {feedback.Add(new(entry.Item2,null,true,"Teacher checks estimate and reasoning: no valid number supplied."));continue;}
            var allowed=UsCcssG3Unit1Lesson7GraphDifferences.TryCheckApproximateBugAnswer(
                entry.Item1,n,out var plausible);
            feedback.Add(new(entry.Item2,null,true,allowed&&plausible
                ? "This falls within a provisional near-value band. Teacher must decide if the estimate is reasonable given the scale of ten."
                : "This may be outside a simple near-value band. Ask the teacher to inspect graph reading and justification before assessing."));
        }
        feedback.Add(new("Source Activity 2, part 5: exchange questions with another group",null,true,
            string.IsNullOrWhiteSpace(answers.PartnerQuestionExchange)
                ? "No peer-question answer recorded; actual exchange and reasoning require teacher observation."
                : "Teacher checks both exchanged questions, answers and the observed partner conversation; text alone cannot certify participation."));
        feedback.Add(new("Question writing, explanation, Math Community",null,true,
            "A teacher must review two learner-authored bug questions, one-step equations, the number talk and partner reasoning."));
        Response.Headers["X-Robots-Tag"]="noindex, nofollow, noarchive";
        return View("Index",new Grade3Lesson7PracticePage(curriculumAdoptionId,
            lessonId,true,answers,feedback));
    }

    private async Task<IActionResult?> CheckAccess(Guid adoption,Guid lessonId,CancellationToken ct)
    {
        if(!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))
            return Forbid();
        var w=await practice.GetWorkspaceAsync(actor,adoption,ct);
        if(w.SelectedCurriculumAdoptionId!=adoption)return NotFound();
        var l=w.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
        if(l is null||l.LessonCode!=Code)return NotFound();
        var published=await lessons.GetPublishedForStudentAsync(
            actor,lessonId,CultureInfo.CurrentUICulture.Name,ct);
        if(published.Value is null)
            return published.Error==LessonContentErrorCode.AccessDenied?Forbid():NotFound();
        return published.Value.LessonCode==Code?null:NotFound();
    }
}
