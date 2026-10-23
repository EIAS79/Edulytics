using System.Globalization;
using System.Security.Claims;
using Edulytics.Core.Mathematics.Curriculum;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

/// <summary>
/// Grade 3 U01 L01 student companion for the first-edition graph lesson.
/// Mathematics is aligned to the public 2021 source; diagrams, wording and
/// Cool-down are independently authored. Human reasoning is never auto-passed.
/// This view does not persist official scores or replace teacher-led work.
/// </summary>
[Authorize(Policy = "StudentPortal")]
[Route("student/practice/grade3-u01-l01")]
public sealed class Grade3Lesson1ActivitiesController(
    IStudentPrivatePracticeService privatePractice,
    ILessonContentService lessonContent) : Controller
{
    private const string Code = UsCcssG3Unit1Lesson1GraphCandidate.LessonCode;

    [HttpGet("")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(
        Guid curriculumAdoptionId,
        Guid lessonId,
        CancellationToken cancellationToken)
    {
        var access = await CheckAccess(curriculumAdoptionId, lessonId, cancellationToken);
        if (access.Failure is not null)
            return access.Failure;

        Response.Headers["X-Robots-Tag"] = "noindex, nofollow, noarchive";
        return View(new Grade3Lesson1PracticePage(
            curriculumAdoptionId, lessonId, access.Title!, false,
            new Grade3Lesson1Answers(), []));
    }

    [HttpPost("submit"), ValidateAntiForgeryToken]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(
        Guid curriculumAdoptionId,
        Guid lessonId,
        [FromForm] Grade3Lesson1Answers answers,
        CancellationToken cancellationToken)
    {
        var access = await CheckAccess(curriculumAdoptionId, lessonId, cancellationToken);
        if (access.Failure is not null)
            return access.Failure;
        if (answers is null)
            return BadRequest("Lesson answers are required.");

        if (typeof(Grade3Lesson1Answers).GetProperties()
            .Select(p => p.GetValue(answers) as string)
            .Any(value => value?.Length > 2000))
            return BadRequest("Lesson answer exceeds allowed length.");

        var feedback = new List<Grade3Lesson1Feedback>
        {
            CheckSource("Count the bus category", "named-category-count",
                "Bus", null, answers.BusCount),
            CheckSource("Compare bus and skateboard", "named-category-difference",
                "Bus", "Skateboard", answers.BusSkateboardDifference),
            CheckSource("Total represented learners", "all-category-total",
                null, null, answers.TotalStudents),
            CheckCoolDown("Edulytics Cool-down: read a category",
                "G3-U01-L01-EDU-CD-COUNT", answers.CoolDownCount),
            CheckCoolDown("Edulytics Cool-down: compare categories",
                "G3-U01-L01-EDU-CD-COMPARE", answers.CoolDownDifference),
            new("Warm-up, invented labels and graph comparison", null, true,
                "A teacher must evaluate observations, plausible labels, shared explanations and how the two graphs show the same dataset."),
            new("Student-authored graph questions", null, true,
                "A teacher must check that both questions can actually be answered from the displayed categories and values."),
            new("Edulytics Cool-down: author a new question", null, true,
                "A teacher must review the learner's original question and reasoning; an entered explanation is not an automatic pass.")
        };

        Response.Headers["X-Robots-Tag"] = "noindex, nofollow, noarchive";
        return View("Index", new Grade3Lesson1PracticePage(
            curriculumAdoptionId, lessonId, access.Title!, true, answers, feedback));
    }

    private static Grade3Lesson1Feedback CheckSource(
        string title, string taskKind, string? first, string? second, string? rawAnswer)
    {
        if (!TryAnswer(rawAnswer, out var answer))
            return new(title, false, false, "Enter a whole-number answer.");

        if (!UsCcssG3Unit1Lesson1GraphCandidate.TryGradeNumeric(
            taskKind, first, second, answer, out var correct))
            return new(title, false, false, "The source task cannot be auto-graded.");

        return new(title, correct, false, correct
            ? "The answer matches the source graph."
            : "Recount the pictured learners and compare the correct categories.");
    }

    private static Grade3Lesson1Feedback CheckCoolDown(
        string title, string questionId, string? rawAnswer)
    {
        if (!TryAnswer(rawAnswer, out var answer))
            return new(title, false, false, "Enter a whole-number answer.");

        if (!UsCcssG3OriginalCoolDownCandidates.TryGradeNumeric(
            Code, questionId, answer, out var correct))
            return new(title, false, false, "Only approved numeric items may be automatically scored.");

        return new(title, correct, false, correct
            ? "Correct for the independent Edulytics reading-corners graph."
            : "Use the library graph, not the original travel graph.");
    }

    private static bool TryAnswer(string? value, out int answer) =>
        int.TryParse(value, NumberStyles.Integer,
            CultureInfo.InvariantCulture, out answer);

    private async Task<(string? Title, IActionResult? Failure)> CheckAccess(
        Guid adoptionId, Guid lessonId, CancellationToken ct)
    {
        if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var actorId))
            return (null, Forbid());

        var workspace = await privatePractice.GetWorkspaceAsync(actorId, adoptionId, ct);
        if (workspace.SelectedCurriculumAdoptionId != adoptionId)
            return (null, NotFound());

        var lesson = workspace.Lessons.SingleOrDefault(x => x.LessonId == lessonId);
        if (lesson is null || lesson.LessonCode != Code)
            return (null, NotFound());

        var published = await lessonContent.GetPublishedForStudentAsync(
            actorId, lessonId, CultureInfo.CurrentUICulture.Name, ct);
        if (published.Value is null)
            return (null, published.Error == LessonContentErrorCode.AccessDenied
                ? Forbid() : NotFound());

        if (published.Value.LessonCode != Code)
            return (null, NotFound());

        return (published.Value.Title, null);
    }
}
