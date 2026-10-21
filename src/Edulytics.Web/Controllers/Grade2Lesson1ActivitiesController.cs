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
/// Learner-authorized Grade 2 Unit 1 Lesson 1 source-goal activity, independently
/// authored for Edulytics. Does not claim to reproduce publisher Cool-down or
/// save graded answers as an official school assessment.
/// </summary>
[Authorize(Policy = "StudentPortal")]
[Route("student/practice/grade2-u01-l01")]
public sealed class Grade2Lesson1ActivitiesController(
    IStudentPrivatePracticeService privatePractice,
    ILessonContentService lessonContent) : Controller
{
    private const string Code = UsCcssG2Unit1FirstLessonEdulyticsCoolDown.LessonCode;

    [HttpGet("")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(
        Guid curriculumAdoptionId, Guid lessonId, CancellationToken cancellationToken)
    {
        var access = await CheckAccess(curriculumAdoptionId, lessonId, cancellationToken);
        if (access.Failure is not null) return access.Failure;

        Response.Headers["X-Robots-Tag"] = "noindex, nofollow, noarchive";
        return View(BuildPage(curriculumAdoptionId, lessonId, access.Title!, false,
            null, null, null, null, null, null, null, null, null, []));
    }

    [HttpPost("submit"), ValidateAntiForgeryToken]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(
        Guid curriculumAdoptionId, Guid lessonId,
        string? answerOne, string? answerTwo, string? expressionOne,
        string? expressionTwo, string? explanation,
        string? checkOffTarget, string? checkOffExpressionOne,
        string? checkOffExpressionTwo,
        CancellationToken cancellationToken)
    {
        var access = await CheckAccess(curriculumAdoptionId, lessonId, cancellationToken);
        if (access.Failure is not null) return access.Failure;

        if (new[] { answerOne, answerTwo, expressionOne, expressionTwo,
            explanation, checkOffTarget, checkOffExpressionOne,
            checkOffExpressionTwo }.Any(x => x?.Length > 2000))
            return BadRequest("An activity answer exceeded the accepted length.");

        var outcomes = new List<Grade2Lesson1Result>();
        var first = UsCcssG2Unit1FirstLessonEdulyticsCoolDown
            .AssessInteger("CD-1", answerOne);
        var second = UsCcssG2Unit1FirstLessonEdulyticsCoolDown
            .AssessInteger("CD-2", answerTwo);
        var third = UsCcssG2Unit1FirstLessonEdulyticsCoolDown
            .AssessTwoExpressions("CD-3", [expressionOne ?? "", expressionTwo ?? ""]);

        outcomes.Add(new("Cool-down 1: combine counters", first.Correct,
            false, first.Reason));
        outcomes.Add(new("Cool-down 2: counters remaining", second.Correct,
            false, second.Reason));
        outcomes.Add(new("Cool-down 3: two different expressions", third.Correct,
            true, third.Reason));
        outcomes.Add(new("Cool-down 4: ten-frame explanation", null,
            true, string.IsNullOrWhiteSpace(explanation)
                ? "Add your drawing or words and discuss the explanation with your teacher."
                : "Explanation recorded in this view for discussion. The teacher must review the reasoning; no automatic pass is claimed."));

        if (int.TryParse(checkOffTarget, NumberStyles.Integer,
                CultureInfo.InvariantCulture, out var target) &&
            target is >= 0 and <= 10)
        {
            var review = UsCcssG2Unit1FirstLessonExpressionRecord.CheckAll(
                target, [checkOffExpressionOne ?? "", checkOffExpressionTwo ?? ""]);
            var bothMatch = review.All(x => x.ExpressionValid && x.TargetMatched);
            var nonDuplicate = review.Select(x => x.NormalizedExpression)
                .Distinct(StringComparer.Ordinal).Count() == 2;
            outcomes.Add(new("Check It Off: independent record", bothMatch && nonDuplicate,
                true, bothMatch && nonDuplicate
                    ? "Both expressions match the checked number. Partner card draws and reasoning still need teacher observation."
                    : "Use two different valid two-card expressions equal to the checked number."));
        }
        else
        {
            outcomes.Add(new("Check It Off: independent record", false,
                true, "Choose a marked value from 0 through 10."));
        }

        Response.Headers["X-Robots-Tag"] = "noindex, nofollow, noarchive";
        return View("Index", BuildPage(curriculumAdoptionId, lessonId,
            access.Title!, true, answerOne, answerTwo, expressionOne,
            expressionTwo, explanation, checkOffTarget, checkOffExpressionOne,
            checkOffExpressionTwo, outcomes));
    }

    private static Grade2Lesson1PracticePage BuildPage(
        Guid adoption, Guid lesson, string title, bool submitted,
        string? one, string? two, string? exprOne, string? exprTwo,
        string? explanation, string? target, string? checkedOne,
        string? checkedTwo, IReadOnlyList<Grade2Lesson1Result> results) =>
        new(adoption, lesson, title,
            UsCcssG2Unit1FirstLessonEdulyticsCoolDown.Tasks,
            UsCcssG2Unit1FirstLessonEdulyticsCoolDown.ExplanationRubric,
            submitted, one, two, exprOne, exprTwo, explanation,
            target, checkedOne, checkedTwo, results);

    private async Task<(string? Title, IActionResult? Failure)> CheckAccess(
        Guid curriculumAdoptionId, Guid lessonId, CancellationToken ct)
    {
        var actorClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(actorClaim, out var actorId))
            return (null, Forbid());
        var workspace = await privatePractice.GetWorkspaceAsync(
            actorId, curriculumAdoptionId, ct);
        if (workspace.SelectedCurriculumAdoptionId != curriculumAdoptionId)
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
