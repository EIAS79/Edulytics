using System.Globalization;
using System.Security.Claims;
using Edulytics.Core.Mathematics.Curriculum;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

/// <summary>Two-step graphical problems with separate human assessed reasoning.</summary>
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade3-u01-l08")]
public sealed class Grade3Lesson8ActivitiesController(
    IStudentPrivatePracticeService practice,ILessonContentService lessons):Controller
{
    private const string Code=UsCcssG3Unit1Lesson8TwoStepDataProblems.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var error=await CheckAccess(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex, nofollow, noarchive";
        return View(new Grade3Lesson8PracticePage(curriculumAdoptionId,lessonId,
            false,new Grade3Lesson8Answers(),[]));
    }

    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade3Lesson8Answers answers,CancellationToken ct)
    {
        var error=await CheckAccess(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null || typeof(Grade3Lesson8Answers).GetProperties()
            .Select(p=>p.GetValue(answers) as string).Any(v=>v?.Length>2000))
            return BadRequest("Invalid or oversized response.");
        var questions=new[]{
            ("L08-FEELINGS-TWO-STEP","Excited minus Nervous and Curious",answers.FeelingsTwoStep),
            ("L08-TREES-PINE-FIR","Pine minus Fir",answers.PineFir),
            ("L08-TREES-PINE-VS-OAK-MAPLE","Pine minus Oak and Maple",answers.PineOakMaple),
            ("L08-TREES-PINE-OAK","Pine minus Oak",answers.PineOak),
            ("L08-TREES-FIR-VS-OAK-MAPLE","Fir minus Oak and Maple",answers.FirOakMaple),
            ("L08-EDU-CD-ADVENTURE-SCIENCE","Cool-down Adventure minus Science",answers.CoolAdventureScience),
            ("L08-EDU-CD-ADVENTURE-VS-SCIENCE-COMICS","Cool-down Adventure minus Science and Comics",answers.CoolAdventureVsTwo),
            ("L08-EDU-CD-TOTAL","Cool-down three categories total",answers.CoolTotal)
        };
        var feedback=questions.Select(item=>{
            if(!int.TryParse(item.Item3,NumberStyles.Integer,CultureInfo.InvariantCulture,out var n)
                || !UsCcssG3Unit1Lesson8TwoStepDataProblems.TryGradeNumeric(
                    item.Item1,n,out var good))
                return new Grade3Lesson8Feedback(item.Item2,false,false,"Enter a whole number.");
            return new Grade3Lesson8Feedback(item.Item2,good,false,
                good?"Correct source or Edulytics-authored arithmetic."
                :"Read all relevant categories: add the two comparison groups before subtracting.");
        }).ToList();
        feedback.Add(new("Three Reads problem comprehension",null,true,
            "Teacher checks first read context, second read quantities and third read chosen operations."));
        feedback.Add(new("Two valid solution methods and equations",null,true,
            "Teacher checks distinct correct methods, not just the final numerical answer."));
        feedback.Add(new("Drawing and explanation for original Edulytics Cool-down",null,true,
            "Teacher reviews labelled axes, exact values, equation and explanation."));
        Response.Headers["X-Robots-Tag"]="noindex, nofollow, noarchive";
        return View("Index",new Grade3Lesson8PracticePage(curriculumAdoptionId,lessonId,
            true,answers,feedback));
    }

    private async Task<IActionResult?> CheckAccess(Guid adoption,Guid lessonId,CancellationToken ct)
    {
        if(!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))
            return Forbid();
        var workspace=await practice.GetWorkspaceAsync(actor,adoption,ct);
        if(workspace.SelectedCurriculumAdoptionId!=adoption)return NotFound();
        var item=workspace.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
        if(item is null||item.LessonCode!=Code)return NotFound();
        var published=await lessons.GetPublishedForStudentAsync(
            actor,lessonId,CultureInfo.CurrentUICulture.Name,ct);
        if(published.Value is null)
            return published.Error==LessonContentErrorCode.AccessDenied?Forbid():NotFound();
        return published.Value.LessonCode==Code?null:NotFound();
    }
}
