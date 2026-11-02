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
/// First-party original 2021 G2 U01 L09 bar-graph source math companion.
/// Graphs independently rendered. Teacher grades student-created statements;
/// the numeric answer checker does not persist a school assessment grade.
/// </summary>
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u01-l09")]
public sealed class Grade2Lesson9ActivitiesController(
    IStudentPrivatePracticeService privatePractice,
    ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit1NinthLessonEdulyticsCoolDown.LessonCode;

    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(
        Guid curriculumAdoptionId,Guid lessonId,CancellationToken cancellationToken)
    {
        var (title,failure)=await Access(curriculumAdoptionId,lessonId,cancellationToken);
        if(failure is not null)return failure;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson9PracticePage(
            curriculumAdoptionId,lessonId,title!,false,new Grade2Lesson9Answers(),[]));
    }

    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(
        Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson9Answers answers,CancellationToken cancellationToken)
    {
        var (title,failure)=await Access(curriculumAdoptionId,lessonId,cancellationToken);
        if(failure is not null)return failure;
        if(answers is null)return BadRequest("Answer data required.");
        if(typeof(Grade2Lesson9Answers).GetProperties()
            .Select(p=>p.GetValue(answers) as string).Any(x=>x?.Length>2000))
            return BadRequest("Answer exceeds safe limit.");

        var season=UsCcssG2Unit1NinthLessonEdulyticsCoolDown
            .CheckSeasonGraph(answers.Summer,answers.SpringAndFall,
                answers.PairOfSeasonsMakingTen,answers.AllSeasonVotes);
        var cool=UsCcssG2Unit1NinthLessonEdulyticsCoolDown
            .CheckIndependentSchoolClubCoolDown(answers.ClubFootball,
                answers.ClubTotal,answers.ClubFootballMinusArt);
        var feedback=new List<Grade2Lesson9Feedback>{
            new("Read and interpret four season bar heights",season.Correct,true,season.Reason),
            new("Independent school-clubs bar graph Cool-down",cool.Correct,false,cool.Reason),
            new("Write multiple original true field-trip statements",null,true,
                string.IsNullOrWhiteSpace(answers.FirstTrueFieldTripStatement) ||
                string.IsNullOrWhiteSpace(answers.SecondTrueFieldTripStatement)
                    ?"Describe at least two source-goal true mathematical statements, with their class and category labels."
                    :"Teacher review is required to verify both original graph statements and their source class."),
            new("Compare the picture-graph key and bar-graph scale",null,true,
                string.IsNullOrWhiteSpace(answers.GraphComparisonExplanation)
                    ?"Explain how one image per pet differs from a numbered vertical scale."
                    :"Teacher reviews the explanation, drawing and classroom discussion.")
        };
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson9PracticePage(
            curriculumAdoptionId,lessonId,title!,true,answers,feedback));
    }

    private async Task<(string? Title,IActionResult? Failure)> Access(
        Guid adoptionId,Guid lessonId,CancellationToken ct)
    {
        if(!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))
            return (null,Forbid());
        var workspace=await privatePractice.GetWorkspaceAsync(actor,adoptionId,ct);
        if(workspace.SelectedCurriculumAdoptionId!=adoptionId)
            return (null,NotFound());
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
