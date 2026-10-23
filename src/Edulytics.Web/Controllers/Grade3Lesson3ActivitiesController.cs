using System.Globalization;
using System.Security.Claims;
using Edulytics.Core.Mathematics.Curriculum;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

/// <summary>G3 L03 independently authored scaled pictograph companion.</summary>
[Authorize(Policy = "StudentPortal")]
[Route("student/practice/grade3-u01-l03")]
public sealed class Grade3Lesson3ActivitiesController(
    IStudentPrivatePracticeService practice, ILessonContentService lessonContent) : Controller
{
    private const string Code = UsCcssG3Unit1Lesson3ScaledPictureGraph.LessonCode;

    [HttpGet("")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId, Guid lessonId, CancellationToken ct)
    {
        var fail = await CheckAccess(curriculumAdoptionId, lessonId, ct);
        if (fail is not null) return fail;
        Response.Headers["X-Robots-Tag"] = "noindex, nofollow, noarchive";
        return View(new Grade3Lesson3PracticePage(curriculumAdoptionId, lessonId,
            false, new Grade3Lesson3Answers(), []));
    }

    [HttpPost("submit"), ValidateAntiForgeryToken]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId, Guid lessonId,
        [FromForm] Grade3Lesson3Answers answers, CancellationToken ct)
    {
        var fail = await CheckAccess(curriculumAdoptionId, lessonId, ct);
        if (fail is not null) return fail;
        if (answers is null || typeof(Grade3Lesson3Answers).GetProperties()
                .Select(x => x.GetValue(answers) as string).Any(x => x?.Length > 2000))
            return BadRequest("Invalid answer payload.");

        var tasks = new (string Id, string? Value)[] {
            ("SPORTS-TOTAL", answers.SportsTotal),
            ("SPORTS-BASKETBALL", answers.SportsBasketball),
            ("SPORTS-COMPARE", answers.SportsCompare),
            ("FLOWERS-ROSES", answers.FlowersRoses),
            ("FLOWERS-TOTAL", answers.FlowersTotal),
            ("FLOWERS-COMPARE", answers.FlowersCompare),
            ("FLOWERS-VIOLETS", answers.FlowersViolets),
            ("BOOKS-COMICS", answers.BooksComics),
            ("BOOKS-TOTAL", answers.BooksTotal),
            ("COOL-NATURE", answers.CoolNature),
            ("COOL-DIFFERENCE", answers.CoolDifference)
        };
        var feedback = tasks.Select(item => {
            if (!int.TryParse(item.Value, NumberStyles.Integer, CultureInfo.InvariantCulture,
                out var number) ||
                !UsCcssG3Unit1Lesson3ScaledPictureGraph.TryGradeNumeric(
                    Code, item.Id, number, out var correct))
                return new Grade3Lesson3Feedback(item.Id, false, false,
                    "Enter a valid integer for the matching first-party graph.");
            return new Grade3Lesson3Feedback(item.Id, correct, false,
                correct ? "Correct for the independently authored diagram."
                    : "Multiply the symbols by the key before counting or comparing.");
        }).ToList();
        feedback.Add(new("Warm-up: tens and ones, questions and partner reasoning",
            null, true, "Teacher judges the strategy and conversation."));
        feedback.Add(new("Explain why both sports graphs represent the same 65 students",
            null, true, "Teacher reviews use of the key and explanation."));
        feedback.Add(new("Student-created flower and books questions", null, true,
            "Teacher checks two answerable questions about each dataset."));
        feedback.Add(new("Independent Cool-down invented question and explanation",
            null, true, "Teacher checks that the question is answerable, math is correct, and the key is applied."));
        Response.Headers["X-Robots-Tag"] = "noindex, nofollow, noarchive";
        return View("Index", new Grade3Lesson3PracticePage(curriculumAdoptionId, lessonId,
            true, answers, feedback));
    }

    private async Task<IActionResult?> CheckAccess(Guid adoptionId, Guid lessonId, CancellationToken ct)
    {
        if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            return Forbid();
        var workspace = await practice.GetWorkspaceAsync(userId, adoptionId, ct);
        if (workspace.SelectedCurriculumAdoptionId != adoptionId)
            return NotFound();
        var lesson = workspace.Lessons.SingleOrDefault(x => x.LessonId == lessonId);
        if (lesson is null || lesson.LessonCode != Code)
            return NotFound();
        var published = await lessonContent.GetPublishedForStudentAsync(
            userId, lessonId, CultureInfo.CurrentUICulture.Name, ct);
        if (published.Value is null)
            return published.Error == LessonContentErrorCode.AccessDenied
                ? Forbid() : NotFound();
        return published.Value.LessonCode == Code ? null : NotFound();
    }
}
