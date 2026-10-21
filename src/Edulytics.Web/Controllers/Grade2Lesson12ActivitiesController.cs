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
/// Original optional Grade2 U01 L12 mathematical authoring support:
/// genuine 20–30 object sorting, 3 categories, original graph, two authored
/// questions, make-ten subtraction and optional arithmetic center choice.
/// The form does NOT certify that physical sorting or partner activity occurred.
/// </summary>
[Authorize(Policy="StudentPortal")]
[Route("student/practice/grade2-u01-l12")]
public sealed class Grade2Lesson12ActivitiesController(
    IStudentPrivatePracticeService privatePractice,
    ILessonContentService lessonContent):Controller
{
    private const string Code=UsCcssG2Unit1TwelfthLessonSortAndDisplay.LessonCode;

    [HttpGet("")]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(
        Guid curriculumAdoptionId,Guid lessonId,CancellationToken cancellationToken)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,cancellationToken);
        if(error is not null)return error;
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View(new Grade2Lesson12PracticePage(
            curriculumAdoptionId,lessonId,title!,false,new Grade2Lesson12Answers(),null,false,[]));
    }

    [HttpPost("submit"),ValidateAntiForgeryToken]
    [ResponseCache(NoStore=true,Location=ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId,Guid lessonId,
        [FromForm] Grade2Lesson12Answers answers,CancellationToken cancellationToken)
    {
        var (title,error)=await Access(curriculumAdoptionId,lessonId,cancellationToken);
        if(error is not null)return error;
        if(answers is null)return BadRequest("Graph inputs are required.");
        if(typeof(Grade2Lesson12Answers).GetProperties()
            .Select(p=>p.GetValue(answers) as string).Any(x=>x?.Length>2000))
            return BadRequest("Object collection answer exceeded length.");

        var collection=UsCcssG2Unit1TwelfthLessonSortAndDisplay
            .CheckRealCollection(answers.CategoryA,answers.CategoryB,answers.CategoryC,
                answers.CountA,answers.CountB,answers.CountC);
        var graph=UsCcssG2Unit1TwelfthLessonSortAndDisplay
            .CheckRepresentation(collection.Data,answers.GraphTitle,answers.GraphKind,
                answers.PictureKey,answers.BarStart,answers.BarStep);
        var questions=UsCcssG2Unit1TwelfthLessonSortAndDisplay.CheckTwoOriginalQuestions(
            collection.Data,answers.QuestionOne,answers.QuestionTwo,
            answers.AnswerOne,answers.AnswerTwo);
        var warm=UsCcssG2Unit1TwelfthLessonSortAndDisplay.CheckNumberTalk(
            answers.WarmOne,answers.WarmTwo,answers.WarmThree,answers.WarmFour);
        var choiceCorrect=UsCcssG2Unit1TwelfthLessonSortAndDisplay.CenterChoices
            .Contains(answers.CenterChoice??"",StringComparer.Ordinal);
        var feedback=new List<Grade2Lesson12Feedback>{
            new("Original three-category 20–30 real objects",collection.Review.Correct,
                true,collection.Review.Feedback),
            new("Original picture or bar graph's title and key/scale",
                graph.MachineScorable?graph.Correct:null,true,graph.Feedback),
            new("Two student-generated graph questions and answers",
                questions.MachineScorable?questions.Correct:null,true,questions.Feedback),
            new("Make-ten Number Talk within twenty",warm.Correct,false,warm.Feedback),
            new("Previously introduced arithmetic center selected",
                choiceCorrect,true,choiceCorrect
                    ?"Center chosen. The teacher observes actual practice and collaboration."
                    :"Select one of the three previously introduced centers."),
            new("Physical sorting, graph drawing and partner explanation",
                null,true,string.IsNullOrWhiteSpace(answers.Explanation)
                    ?"Tell your teacher which objects you actually sorted, how you graphed them, and what your partner asked."
                    :"Teacher reviews actual collection, student-created graph and genuine partner exchange; no automatic participation or grade is claimed.")
        };
        Response.Headers["X-Robots-Tag"]="noindex,nofollow,noarchive";
        return View("Index",new Grade2Lesson12PracticePage(
            curriculumAdoptionId,lessonId,title!,true,answers,
            collection.Data,graph.Correct,feedback));
    }

    private async Task<(string? Title,IActionResult? Failure)> Access(
        Guid adoptionId,Guid lessonId,CancellationToken ct)
    {
        if(!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier),out var actor))
            return (null,Forbid());
        var workspace=await privatePractice.GetWorkspaceAsync(actor,adoptionId,ct);
        if(workspace.SelectedCurriculumAdoptionId!=adoptionId)
            return (null,NotFound());
        var l=workspace.Lessons.SingleOrDefault(x=>x.LessonId==lessonId);
        if(l is null || l.LessonCode!=Code)return (null,NotFound());
        var published=await lessonContent.GetPublishedForStudentAsync(
            actor,lessonId,CultureInfo.CurrentUICulture.Name,ct);
        if(published.Value is null)
            return (null,published.Error==LessonContentErrorCode.AccessDenied
                ?Forbid():NotFound());
        if(published.Value.LessonCode!=Code)return (null,NotFound());
        return (published.Value.Title,null);
    }
}
