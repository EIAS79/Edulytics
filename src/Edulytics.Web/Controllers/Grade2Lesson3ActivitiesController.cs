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
/// First-party Grade 2 U01 L03 20-cube learning and exit ticket, fully
/// scoped to authorized enrolled students of the published exact lesson.
/// No publisher-only Cool-down reproduction, automated classroom sign-off
/// or official persisted school mark is implied.
/// </summary>
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u01-l03")]
public sealed class Grade2Lesson3ActivitiesController(
    IStudentPrivatePracticeService privatePractice,
    ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit1ThirdLessonEdulyticsCoolDown.LessonCode;

    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(
        Guid curriculumAdoptionId,Guid lessonId,CancellationToken cancellationToken)
    {
        var (title,failure)=await CheckAccess(curriculumAdoptionId,lessonId,cancellationToken);
        if(failure is not null)return failure;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson3PracticePage(curriculumAdoptionId,lessonId,title!,
            false,null,null,null,null,null,null,[]));
    }

    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(
        Guid curriculumAdoptionId,Guid lessonId,string? hidden,string? removed,
        string? extension,string? statementOne,string? statementTwo,
        string? explanation,CancellationToken cancellationToken)
    {
        var (title,failure)=await CheckAccess(curriculumAdoptionId,lessonId,cancellationToken);
        if(failure is not null)return failure;
        if(new[]{hidden,removed,extension,statementOne,statementTwo,explanation}
            .Any(x=>x?.Length>2000))
            return BadRequest("An activity response exceeded its maximum length.");

        var numeric=UsCcssG2Unit1ThirdLessonEdulyticsCoolDown.VerifyHiddenPart(hidden);
        var removedReview=UsCcssG2Unit1ThirdLessonEdulyticsCoolDown.VerifyRemovedPart(removed);
        var extensionReview=UsCcssG2Unit1ThirdLessonEdulyticsCoolDown.VerifyChallengeStartingAmount(extension);
        var truth=UsCcssG2Unit1ThirdLessonEdulyticsCoolDown.VerifyEquationsTruth(statementOne,statementTwo);
        var results=new List<Grade2Lesson3Feedback>{
            new("Twenty-cube hidden part",numeric.Correct,false,numeric.Message),
            new("Unknown number removed from twenty",removedReview.Correct,false,removedReview.Message),
            new("Two statements: true or false",truth.Correct,false,truth.Message)
        };
        if(!string.IsNullOrWhiteSpace(extension))
            results.Add(new("Optional greater-than-twenty challenge",
                extensionReview.Correct,false,extensionReview.Message));
        results.Add(new("Explain your ten-and-twenty model",null,true,
            string.IsNullOrWhiteSpace(explanation)
                ?"Draw or describe two ten-cube towers and discuss them with your teacher."
                :"Your explanation is ready for teacher review; an automatic numerical score does not check your reasoning."));

        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson3PracticePage(
            curriculumAdoptionId,lessonId,title!,true,hidden,removed,extension,
            statementOne,statementTwo,explanation,results));
    }

    private async Task<(string? Title,IActionResult? Failure)> CheckAccess(
        Guid adoptionId,Guid lessonId,CancellationToken ct)
    {
        if(!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))
            return (null,Forbid());
        var workspace=await privatePractice.GetWorkspaceAsync(actor,adoptionId,ct);
        if(workspace.SelectedCurriculumAdoptionId!=adoptionId)
            return (null,NotFound());
        var option=workspace.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
        if(option is null || option.LessonCode!=Code)
            return (null,NotFound());
        var source=await lessonContent.GetPublishedForStudentAsync(
            actor,lessonId,CultureInfo.CurrentUICulture.Name,ct);
        if(source.Value is null)
            return (null,source.Error==LessonContentErrorCode.AccessDenied
                ?Forbid():NotFound());
        if(source.Value.LessonCode!=Code)
            return (null,NotFound());
        return (source.Value.Title,null);
    }
}
