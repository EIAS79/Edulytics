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
/// Authorized pupil companion for 2021 Grade2 U01 L14 bar to tape diagrams.
/// Drawings, student stories and physical cut strips require teacher rubric.
/// The independent exit ticket and CSS visual do not copy publisher images.
/// </summary>
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u01-l14")]
public sealed class Grade2Lesson14ActivitiesController(
    IStudentPrivatePracticeService privatePractice,
    ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit1FourteenthLessonTapeDiagrams.LessonCode;

    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,failed)=await Access(curriculumAdoptionId,lessonId,ct);
        if(failed is not null)return failed;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson14Page(
            curriculumAdoptionId,lessonId,title!,false,new Grade2Lesson14Answers(),[]));
    }

    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson14Answers answers,CancellationToken ct)
    {
        var (title,failed)=await Access(curriculumAdoptionId,lessonId,ct);
        if(failed is not null)return failed;
        if(answers is null)return BadRequest("Tape diagram inputs are required.");
        if(typeof(Grade2Lesson14Answers).GetProperties()
           .Select(p=>p.GetValue(answers) as string).Any(x=>x?.Length>2000))
           return BadRequest("Tape diagram answer exceeded permitted length.");

        var warm=UsCcssG2Unit1FourteenthLessonTapeDiagrams.VerifyTapes(
            UsCcssG2Unit1FourteenthLessonTapeDiagrams.JuiceVotes,
            "Apple","Grape",answers.AppleGrapeGap,answers.AppleGrapeAdd,answers.AppleGrapeSub);
        var party=UsCcssG2Unit1FourteenthLessonTapeDiagrams.VerifyTapes(
            UsCcssG2Unit1FourteenthLessonTapeDiagrams.PartyVotes,
            answers.PartyMore,answers.PartyFewer,answers.PartyGap,
            answers.PartyAdd,answers.PartySub);
        var pizzaBurgers=UsCcssG2Unit1FourteenthLessonTapeDiagrams.VerifyTapes(
            UsCcssG2Unit1FourteenthLessonTapeDiagrams.PartyVotes,
            "Pizza","Burgers",answers.PizzaBurgerGap,
            answers.PizzaBurgerAdd,answers.PizzaBurgerSub);
        var pizzaHotdogs=UsCcssG2Unit1FourteenthLessonTapeDiagrams.VerifyTapes(
            UsCcssG2Unit1FourteenthLessonTapeDiagrams.PartyVotes,
            "Pizza","Hot dogs",answers.PizzaHotdogsGap,
            answers.PizzaHotdogsAdd,answers.PizzaHotdogsSub);
        var cool=UsCcssG2Unit1FourteenthLessonTapeDiagrams
            .CheckIndependentCoolDown(answers.CoolGap,answers.CoolAdd,answers.CoolSub);
        var feedback=new List<Grade2Lesson14Feedback>{
            new("Warm-up juice bar to tape diagram",warm.Correct,true,warm.Reason),
            new("Party Time Part 1: two authentic category comparisons",party.Correct,true,party.Reason),
            new("Party Time Part 2: pizza against burgers",pizzaBurgers.Correct,true,pizzaBurgers.Reason),
            new("Party Time Part 2: pizza against hot dogs",pizzaHotdogs.Correct,true,pizzaHotdogs.Reason),
            new("Independent Edulytics library tape Cool-down",cool.Correct,false,cool.Reason),
            new("Physical tape construction, comparison stories and partner talk",null,true,
                string.IsNullOrWhiteSpace(answers.TeacherExplanation)
                    ?"Show the physical/cut or hand-drawn tape to a teacher and explain the missing part."
                    :"A teacher separately reviews hand-drawn rectangular strips, accurate labels and student explanation.")
        };
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson14Page(
            curriculumAdoptionId,lessonId,title!,true,answers,feedback));
    }

    private async Task<(string? Title,IActionResult? Failure)> Access(
        Guid adoption,Guid lessonId,CancellationToken ct)
    {
        if(!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))
            return (null,Forbid());
        var workspace=await privatePractice.GetWorkspaceAsync(actor,adoption,ct);
        if(workspace.SelectedCurriculumAdoptionId!=adoption)return (null,NotFound());
        var lesson=workspace.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
        if(lesson is null||lesson.LessonCode!=Code)return (null,NotFound());
        var content=await lessonContent.GetPublishedForStudentAsync(
            actor,lessonId,CultureInfo.CurrentUICulture.Name,ct);
        if(content.Value is null)
            return (null,content.Error==LessonContentErrorCode.AccessDenied?Forbid():NotFound());
        if(content.Value.LessonCode!=Code)return (null,NotFound());
        return (content.Value.Title,null);
    }
}
