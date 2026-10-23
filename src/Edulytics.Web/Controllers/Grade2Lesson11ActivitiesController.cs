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
/// Learner-authorized original Grade2 U01 L11 graph question-writing task
/// based on Grade2 U01 L10 graph tables. Four student-authored questions
/// are teacher-reviewed; no official grades, partner names or publisher art.
/// </summary>
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u01-l11")]
public sealed class Grade2Lesson11ActivitiesController(
    IStudentPrivatePracticeService privatePractice,
    ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit1EleventhLessonGraphQuestions.LessonCode;

    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(
        Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson11PracticePage(
            curriculumAdoptionId,lessonId,title!,false,new Grade2Lesson11Answers(),[]));
    }

    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson11Answers answers,CancellationToken ct)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,ct);
        if(error is not null)return error;
        if(answers is null)return BadRequest("Lesson questions are required.");
        if(answers.Questions is null || answers.Questions.Count>4 ||
            answers.Questions.Any(q=>new[]{q.Question,q.Graph,q.Operation,
                q.CategoryOne,q.CategoryTwo,q.PartnerAnswer}
                .Any(x=>x?.Length>500)) ||
            new[]{answers.CoolPictureDifference,answers.CoolBarCombined,
                answers.CoolNumberTalk,answers.PartnerReflection}
                .Any(x=>x?.Length>2000))
            return BadRequest("An answer exceeded the allowed length or four-question limit.");

        var review=UsCcssG2Unit1EleventhLessonGraphQuestions.CheckAll(answers.Questions);
        var cooldown=UsCcssG2Unit1EleventhLessonGraphQuestions.CheckEdulyticsCoolDown(
            answers.CoolPictureDifference,answers.CoolBarCombined,answers.CoolNumberTalk);
        var messages=new List<Grade2Lesson11Feedback>{
            new("Four distinct source-graph questions (two for each)",review.Correct,true,review.Feedback),
            new("Independent Edulytics Cool-down",cooldown.Correct,false,cooldown.Feedback)
        };
        foreach(var (question,index) in answers.Questions.Select((q,i)=>(q,i)))
        {
            var numeric=UsCcssG2Unit1EleventhLessonGraphQuestions.CheckSourceQuestion(question);
            messages.Add(new($"Question {index+1}: proposed partner answer",numeric.Correct,
                true,numeric.Feedback));
        }
        messages.Add(new("Partner graph selection and mathematical explanation",
            null,true,string.IsNullOrWhiteSpace(answers.PartnerReflection)
                ?"Explain how your partner chose the correct graph and what you revised."
                :"Teacher reviews the genuinely student-written wording, partner response, reasoning and graph artifacts. No automatic classroom score is asserted."));

        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson11PracticePage(
            curriculumAdoptionId,lessonId,title!,true,answers,messages));
    }

    private async Task<(string? Title,IActionResult? Failure)> Access(
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
