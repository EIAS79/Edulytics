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
/// Student-authorized original seven-card activity and independently authored
/// grade2 U01 L05 Cool-down, not an official persisted school assessment.
/// </summary>
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u01-l05")]
public sealed class Grade2Lesson5ActivitiesController(
    IStudentPrivatePracticeService privatePractice,
    ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit1FifthLessonEdulyticsCoolDown.LessonCode;
    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(
        Guid curriculumAdoptionId,Guid lessonId,CancellationToken cancellationToken)
    {
        var (title,error)=await ValidateLesson(curriculumAdoptionId,lessonId,cancellationToken);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson5PracticePage(curriculumAdoptionId,
            lessonId,title!,false,null,null,null,null,null,[]));
    }
    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        string? twoDigitEquation,string? mental,string? regroup,
        string? toFifty,string? explanation,CancellationToken cancellationToken)
    {
        var (title,error)=await ValidateLesson(curriculumAdoptionId,lessonId,cancellationToken);
        if(error is not null)return error;
        if(new[]{twoDigitEquation,mental,regroup,toFifty,explanation}
            .Any(x=>x?.Length>2000))return BadRequest("Answer exceeds the accepted limit.");

        var eq=UsCcssG2Unit1FifthLessonEdulyticsCoolDown
            .CheckFourCardTwoDigitExpression(twoDigitEquation);
        var mentalResult=UsCcssG2Unit1FifthLessonEdulyticsCoolDown.CheckMentalTen(mental);
        var regroupResult=UsCcssG2Unit1FifthLessonEdulyticsCoolDown.CheckRegrouping(regroup);
        var fiftyResult=UsCcssG2Unit1FifthLessonEdulyticsCoolDown.CheckFiftySum(toFifty);
        var feedback=new List<Grade2Lesson5Feedback>{
            new("How Close with seven physical cards",eq.Correct,eq.TeacherReviewNeeded,eq.Feedback),
            new("Add 38 + 6",mentalResult.Correct,false,mentalResult.Feedback),
            new("Add 27 + 15",regroupResult.Correct,false,regroupResult.Feedback),
            new("Add 39 + 11",fiftyResult.Correct,false,fiftyResult.Feedback),
            new("Explain your place-value strategy",null,true,
                string.IsNullOrWhiteSpace(explanation)
                    ?"Explain how tens and ones were combined, or demonstrate with cubes."
                    :"A teacher must check the strategy and/or drawing; no automatic mark for reasoning is claimed.")
        };
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson5PracticePage(
            curriculumAdoptionId,lessonId,title!,true,twoDigitEquation,mental,
            regroup,toFifty,explanation,feedback));
    }

    private async Task<(string? Title,IActionResult? Failure)> ValidateLesson(
        Guid adoption,Guid lessonId,CancellationToken ct)
    {
        if(!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))
            return (null,Forbid());
        var workspace=await privatePractice.GetWorkspaceAsync(actor,adoption,ct);
        if(workspace.SelectedCurriculumAdoptionId!=adoption)return (null,NotFound());
        var lesson=workspace.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
        if(lesson is null || lesson.LessonCode!=Code)return (null,NotFound());
        var publication=await lessonContent.GetPublishedForStudentAsync(
            actor,lessonId,CultureInfo.CurrentUICulture.Name,ct);
        if(publication.Value is null)
            return (null,publication.Error==LessonContentErrorCode.AccessDenied
                ?Forbid():NotFound());
        if(publication.Value.LessonCode!=Code)return (null,NotFound());
        return (publication.Value.Title,null);
    }
}
