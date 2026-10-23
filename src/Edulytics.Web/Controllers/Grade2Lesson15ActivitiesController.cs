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
/// Student-owned source Grade2 U01 L15 shell collection and original
/// compare word-story / tape-diagram / equivalent-equation card sort.
/// Only authorized lesson & published curriculum can open/submit.
/// Genuine partner explanations and diagram drawing teacher-reviewed.
/// </summary>
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u01-l15")]
public sealed class Grade2Lesson15ActivitiesController(
    IStudentPrivatePracticeService privatePractice,
    ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit1FifteenthLessonCompareCards.LessonCode;

    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,failure)=await Access(curriculumAdoptionId,lessonId,ct);
        if(failure is not null)return failure;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson15PracticePage(curriculumAdoptionId,lessonId,
            title!,false,new Grade2Lesson15Answers(),[]));
    }

    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson15Answers answers,CancellationToken ct)
    {
        var (title,failure)=await Access(curriculumAdoptionId,lessonId,ct);
        if(failure is not null)return failure;
        if(answers is null || answers.CardMatches is null || answers.CardMatches.Count!=3)
            return BadRequest("Three original story cards must be matched.");
        if(typeof(Grade2Lesson15Answers).GetProperties()
            .Where(x=>x.PropertyType==typeof(string))
            .Select(x=>x.GetValue(answers) as string).Any(x=>x?.Length>2000) ||
            answers.CardMatches.Any(x=>new[]{x.StoryId,x.DiagramKey,
                x.UnknownPosition,x.Answer,x.EquationA,x.EquationB}
                .Any(v=>v?.Length>200)))
            return BadRequest("One or more question answers exceeded the safe length.");
        var warm=UsCcssG2Unit1FifteenthLessonCompareCards
            .CheckNumberTalk(answers.WarmA,answers.WarmB,answers.WarmC,answers.WarmD);
        var feedback=new List<Grade2Lesson15Feedback>{
            new("Original make-ten Number Talk",warm.Correct,true,warm.Feedback)
        };
        foreach(var (question,index) in answers.CardMatches.Select((value,i)=>(value,i)))
        {
            var card=UsCcssG2Unit1FifteenthLessonCompareCards.CheckCard(
                question.StoryId,question.DiagramKey,question.UnknownPosition,
                question.Answer,question.EquationA,question.EquationB);
            feedback.Add(new($"Original card-sort match {index+1}",
                card.Correct,true,card.Feedback));
        }
        var cooldown=UsCcssG2Unit1FifteenthLessonCompareCards
            .CheckIndependentCoolDown(answers.CoolSmaller,
                answers.CoolLarger,answers.CoolDifference);
        feedback.Add(new("Edulytics independent all-unknown-positions Cool-down",
            cooldown.Correct,false,cooldown.Feedback));
        feedback.Add(new("Explain and defend tape diagram and partner card matches",
            null,true,string.IsNullOrWhiteSpace(answers.ExplainUnknownPositions)
                ?"Explain how each unknown position controls the tape and why multiple equations may match."
                :"Teacher reviews original strip drawings, selected card meanings, genuine pair collaboration and justification. The numeric checker never awards that credit."));
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson15PracticePage(curriculumAdoptionId,
            lessonId,title!,true,answers,feedback));
    }

    private async Task<(string? Title,IActionResult? Failure)> Access(
        Guid adoptionId,Guid lessonId,CancellationToken ct)
    {
        if(!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))
            return (null,Forbid());
        var workspace=await privatePractice.GetWorkspaceAsync(actor,adoptionId,ct);
        if(workspace.SelectedCurriculumAdoptionId!=adoptionId)return (null,NotFound());
        var lesson=workspace.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
        if(lesson is null || lesson.LessonCode!=Code)return (null,NotFound());
        var content=await lessonContent.GetPublishedForStudentAsync(
            actor,lessonId,CultureInfo.CurrentUICulture.Name,ct);
        if(content.Value is null)
            return (null,content.Error==LessonContentErrorCode.AccessDenied
                ?Forbid():NotFound());
        if(content.Value.LessonCode!=Code)return (null,NotFound());
        return (content.Value.Title,null);
    }
}
