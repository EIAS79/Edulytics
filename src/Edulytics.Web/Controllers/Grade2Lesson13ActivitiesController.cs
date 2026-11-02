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
/// Authenticated Grade2 U01 L13 original source-aligned two-strategy
/// compare-bar learner companion, first-party bars and independent Cool-down.
/// Teacher assessment remains separate, no official school grade persisted.
/// </summary>
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u01-l13")]
public sealed class Grade2Lesson13ActivitiesController(
    IStudentPrivatePracticeService privatePractice,
    ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit1ThirteenthLessonBarCompare.LessonCode;

    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId,Guid lessonId,CancellationToken ct)
    {
        var (title,failed)=await Access(curriculumAdoptionId,lessonId,ct);
        if(failed is not null)return failed;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson13Page(curriculumAdoptionId,lessonId,title!,
            false,new Grade2Lesson13Answers(),[]));
    }

    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson13Answers answers,CancellationToken ct)
    {
        var (title,failed)=await Access(curriculumAdoptionId,lessonId,ct);
        if(failed is not null)return failed;
        if(answers is null)return BadRequest("Comparison data is required.");
        if(typeof(Grade2Lesson13Answers).GetProperties()
            .Select(p=>p.GetValue(answers) as string).Any(x=>x?.Length>2000))
            return BadRequest("A comparison response exceeded the input limit.");
        var warm=UsCcssG2Unit1ThirteenthLessonBarCompare
            .CheckTrueFalseWarmUp(answers.WarmFirst,answers.WarmSecond,answers.WarmThird);
        var pets=UsCcssG2Unit1ThirteenthLessonBarCompare
            .CheckSourcePetsDifference(answers.PetsDifference,
                answers.PetsCountingOn,answers.PetsCountingBack);
        var dogs=UsCcssG2Unit1ThirteenthLessonBarCompare
            .CheckDogComparison(answers.DogsMore,answers.DogsFewer,
                answers.DogsAddition,answers.DogsSubtraction);
        var cool=UsCcssG2Unit1ThirteenthLessonBarCompare
            .CheckEdulyticsCoolDown(answers.CoolDifference,answers.CoolAddition,
                answers.CoolSubtraction);
        var notes=new List<Grade2Lesson13Feedback>{
            new("Make-ten true and false number talk",warm.Correct,true,warm.Message),
            new("Pets: unknown comparison difference two ways",pets.Correct,true,pets.Message),
            new("Dog categories: MORE / FEWER and inverse equations",dogs.Correct,true,dogs.Message),
            new("First-party Edulytics Cool-down",cool.Correct,false,cool.Message),
            new("Learner comparison, counting methods and partner talk",
                null,true,string.IsNullOrWhiteSpace(answers.Explain)
                    ?"Show counting on and counting back in your own drawing or explain them to the teacher."
                    :"Teacher reviews original mathematical explanation and partner dialogue; no false automatic teacher approval is claimed.")
        };
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson13Page(
            curriculumAdoptionId,lessonId,title!,true,answers,notes));
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
        var publication=await lessonContent.GetPublishedForStudentAsync(
            actor,lessonId,CultureInfo.CurrentUICulture.Name,ct);
        if(publication.Value is null)
            return (null,publication.Error==LessonContentErrorCode.AccessDenied?Forbid():NotFound());
        if(publication.Value.LessonCode!=Code)return (null,NotFound());
        return (publication.Value.Title,null);
    }
}
