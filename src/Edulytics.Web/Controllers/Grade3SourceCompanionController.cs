using System.Globalization;
using System.Security.Claims;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.Curriculum.Grade3;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

/// <summary>
/// Optional learner companion for Grade 3 L09–L143 only. The separate original
/// questions are not certified publisher duplicates, official marks, teacher
/// observations or completed Rich V2 source parity.
/// </summary>
[Authorize(Policy = "StudentPortal")]
[Route("student/practice/grade3-source-companion")]
public sealed class Grade3SourceCompanionController(
    IStudentPrivatePracticeService practice,
    ILessonContentService lessonContent) : Controller
{
    [HttpGet("")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(
        Guid curriculumAdoptionId, Guid lessonId, CancellationToken ct)
    {
        var (lesson, failure) = await AuthorizeLesson(curriculumAdoptionId, lessonId, ct);
        if (failure is not null) return failure;
        Response.Headers["X-Robots-Tag"] = "noindex, nofollow, noarchive";
        return View(new Grade3SourceCompanionPage(
            curriculumAdoptionId, lessonId, lesson!, new(), false, null, null));
    }

    [HttpPost("submit"), ValidateAntiForgeryToken]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(
        Guid curriculumAdoptionId, Guid lessonId,
        [FromForm] Grade3SourceCompanionAnswers answers, CancellationToken ct)
    {
        var (lesson, failure) = await AuthorizeLesson(curriculumAdoptionId, lessonId, ct);
        if (failure is not null) return failure;
        if (answers is null ||
            typeof(Grade3SourceCompanionAnswers).GetProperties()
                .Select(p => p.GetValue(answers) as string)
                .Any(x => x?.Length > 2000))
            return BadRequest("Invalid lesson answer.");

        answers.SourceSubtaskAnswers ??= [];
        answers.SourceSubtaskReasoning ??= [];
        if (lesson!.SourceSubtasks.Count > 30 ||
            answers.SourceSubtaskAnswers.Count > lesson.SourceSubtasks.Count ||
            answers.SourceSubtaskReasoning.Count > lesson.SourceSubtasks.Count ||
            answers.SourceSubtaskAnswers.Any(x => x?.Length > 100) ||
            answers.SourceSubtaskReasoning.Any(x => x?.Length > 2000))
            return BadRequest("Invalid lesson subtask answers.");
        var sourceSubtaskFeedback = lesson.SourceSubtasks.Count == 0 ? null :
            lesson.SourceSubtasks.Select((q, i) => Grade3SourceCompanionCatalogue.Grade(
                q, answers.SourceSubtaskAnswers.ElementAtOrDefault(i))).ToArray();
        // L10/L11 source diagrams and L16 physical arrays require teacher review.
        // Numeric supplemental checks are optional: numeric correctness is not
        // evidence that an L16 array was constructed or explained.
        var optionalNumericSupplement = lesson.UnitNumber == 1 &&
            (lesson.LessonNumber == 10 || lesson.LessonNumber == 11 || lesson.LessonNumber == 16);
        var sourceCheck = lesson.SourceBasedCheck is null ? null :
            optionalNumericSupplement && string.IsNullOrWhiteSpace(answers.SourceCheckAnswer)
                ? new Grade3CompanionFeedback(null, true, "Optional numeric supplement not attempted; teacher checks the physical model.")
                : Grade3SourceCompanionCatalogue.Grade(lesson.SourceBasedCheck, answers.SourceCheckAnswer);
        var p = optionalNumericSupplement && string.IsNullOrWhiteSpace(answers.PracticeAnswer)
            ? new Grade3CompanionFeedback(null, true, "Optional independent practice not attempted.")
            : Grade3SourceCompanionCatalogue.Grade(lesson!.IndependentPractice!, answers.PracticeAnswer);
        var cool = optionalNumericSupplement && string.IsNullOrWhiteSpace(answers.CoolDownAnswer)
            ? new Grade3CompanionFeedback(null, true, "Optional extra Edulytics Cool-down not attempted.")
            : Grade3SourceCompanionCatalogue.Grade(lesson.OriginalEdulyticsCoolDown!, answers.CoolDownAnswer);
        Response.Headers["X-Robots-Tag"] = "noindex, nofollow, noarchive";
        return View("Index", new Grade3SourceCompanionPage(
            curriculumAdoptionId, lessonId, lesson, answers, true, p, cool,
            sourceCheck, sourceSubtaskFeedback));
    }

    private async Task<(Grade3CompanionLesson? Lesson, IActionResult? Failure)> AuthorizeLesson(
        Guid adoption, Guid lessonId, CancellationToken ct)
    {
        if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var user))
            return (null, Forbid());
        var workspace = await practice.GetWorkspaceAsync(user, adoption, ct);
        if (workspace.SelectedCurriculumAdoptionId != adoption)
            return (null, NotFound());
        var assigned = workspace.Lessons.SingleOrDefault(x => x.LessonId == lessonId);
        if (assigned is null || !Grade3SourceCompanionCatalogue.TryGet(
                assigned.LessonCode, out var companion))
            return (null, NotFound());
        var published = await lessonContent.GetPublishedForStudentAsync(
            user, lessonId, CultureInfo.CurrentUICulture.Name, ct);
        if (published.Value is null)
            return (null, published.Error == LessonContentErrorCode.AccessDenied
                ? Forbid() : NotFound());
        return published.Value.LessonCode == assigned.LessonCode
            ? (companion, null) : (null, NotFound());
    }
}
