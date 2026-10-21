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
/// Original Edulytics representation of REAL travel-to-school responses.
/// Authenticated student lesson only. No names, generated classmates,
/// fake survey data, persisted official grades, or publisher art.
/// </summary>
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u01-l07")]
public sealed class Grade2Lesson7ActivitiesController(
    IStudentPrivatePracticeService privatePractice,
    ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit1SeventhLessonClassData.LessonCode;

    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(
        Guid curriculumAdoptionId,Guid lessonId,CancellationToken cancellationToken)
    {
        var (title,failed)=await CheckAccess(curriculumAdoptionId,lessonId,cancellationToken);
        if(failed is not null)return failed;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson7PracticePage(
            curriculumAdoptionId,lessonId,title!,false,false,
            null,null,null,null,null,null,null,null,null,
            [],new Dictionary<string,int>(StringComparer.Ordinal),[]));
    }

    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(
        Guid curriculumAdoptionId,Guid lessonId,
        string? rawResponses,string? walkCount,string? carCount,
        string? trainCount,string? busCount,string? totalAnswer,
        string? busAnswer,string? comparisonAnswer,string? explanation,
        CancellationToken cancellationToken)
    {
        var (title,failed)=await CheckAccess(curriculumAdoptionId,lessonId,cancellationToken);
        if(failed is not null)return failed;
        if(new[]{rawResponses,walkCount,carCount,trainCount,busCount,
            totalAnswer,busAnswer,comparisonAnswer,explanation}
            .Any(x=>x?.Length>2000))
            return BadRequest("Classroom data response exceeded the safe input limit.");

        var survey=UsCcssG2Unit1SeventhLessonClassData
            .ParseActualResponses(rawResponses);
        var grouped=UsCcssG2Unit1SeventhLessonClassData
            .VerifyFourCategoryRepresentation(survey,walkCount,carCount,
                trainCount,busCount);
        var coolDown=UsCcssG2Unit1SeventhLessonClassData
            .VerifyOriginalCoolDown(survey,totalAnswer,busAnswer,comparisonAnswer);
        var feedback=new List<Grade2Lesson7Feedback>{
            new("Real anonymous class responses",survey.Accepted,true,survey.Feedback),
            new("My organized category representation",grouped.MachineScorable
                ? grouped.Correct : null,true,grouped.Reason),
            new("Edulytics Cool-down: reading the same real data",
                coolDown.MachineScorable?coolDown.Correct:null,false,coolDown.Reason),
            new("Explain and compare your two representations",null,true,
                string.IsNullOrWhiteSpace(explanation)
                    ? "Explain why a new display made your genuine data easier to read."
                    : "Your teacher must review your explanation, drawing, gallery walk and partner conversation.")
        };
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson7PracticePage(
            curriculumAdoptionId,lessonId,title!,true,survey.Accepted,
            rawResponses,walkCount,carCount,trainCount,busCount,totalAnswer,
            busAnswer,comparisonAnswer,explanation,
            survey.OriginalResponses,survey.Counts,feedback));
    }

    private async Task<(string? Title,IActionResult? Failure)> CheckAccess(
        Guid adoption,Guid lessonId,CancellationToken ct)
    {
        if(!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))
            return (null,Forbid());
        var workspace=await privatePractice.GetWorkspaceAsync(actor,adoption,ct);
        if(workspace.SelectedCurriculumAdoptionId!=adoption)
            return (null,NotFound());
        var lesson=workspace.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
        if(lesson is null||lesson.LessonCode!=Code)return (null,NotFound());
        var source=await lessonContent.GetPublishedForStudentAsync(
            actor,lessonId,CultureInfo.CurrentUICulture.Name,ct);
        if(source.Value is null)
            return (null,source.Error==LessonContentErrorCode.AccessDenied
                ?Forbid():NotFound());
        if(source.Value.LessonCode!=Code)return (null,NotFound());
        return (source.Value.Title,null);
    }
}
