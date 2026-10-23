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
/// Independently authored G3 U01 L02 class-survey companion.
/// Supplied category counts are not certified to be authentic class responses.
/// No official grades are persisted; graph drawing and explanation need a teacher.
/// </summary>
[Authorize(Policy = "StudentPortal")]
[Route("student/practice/grade3-u01-l02")]
public sealed class Grade3Lesson2ActivitiesController(
    IStudentPrivatePracticeService privatePractice,
    ILessonContentService lessonContent) : Controller
{
    private const string Code = UsCcssG3Unit1Lesson2ClassSurveyCandidate.LessonCode;
    private static readonly string[] Categories = ["Bike", "Walk", "Van", "Bus", "Car", "Train"];

    [HttpGet("")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(
        Guid curriculumAdoptionId, Guid lessonId, CancellationToken ct)
    {
        var (title, failure) = await CheckAccess(curriculumAdoptionId, lessonId, ct);
        if (failure is not null) return failure;
        Response.Headers["X-Robots-Tag"] = "noindex, nofollow, noarchive";
        return View(new Grade3Lesson2PracticePage(curriculumAdoptionId, lessonId, title!,
            false, new Grade3Lesson2Answers(), null, []));
    }

    [HttpPost("submit"), ValidateAntiForgeryToken]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(
        Guid curriculumAdoptionId, Guid lessonId,
        [FromForm] Grade3Lesson2Answers answers, CancellationToken ct)
    {
        var (title, failure) = await CheckAccess(curriculumAdoptionId, lessonId, ct);
        if (failure is not null) return failure;
        if (answers is null || typeof(Grade3Lesson2Answers).GetProperties()
                .Select(p => p.GetValue(answers) as string)
                .Any(x => x?.Length > 2000))
            return BadRequest("A lesson response is invalid or too long.");

        var survey = ParseCounts(answers);
        var valid = UsCcssG3Unit1Lesson2ClassSurveyCandidate.TryGetValidatedCounts(
            survey, out var counts) && counts is not null;
        var feedback = new List<Grade3Lesson2Feedback>
        {
            new("Actual classroom survey", null, true,
                valid
                    ? "The entered numeric dataset is well formed; only your teacher can establish whether it was collected in class."
                    : "Supply six nonnegative counts, at most 40 in total, and at least one recorded response. Numeric comparisons are not graded for invalid class data.")
        };

        if (valid)
        {
            feedback.Add(CompareDifference(survey, answers.BusCarDifference));
            feedback.Add(CompareTwoStep(survey, answers.WalkBikeVanDifference));
            feedback.Add(CheckStatement("Walk exceeds each other category",
                answers.WalkMost,
                counts!["Walk"] > counts.Where(x => x.Key != "Walk").Max(x => x.Value)));
            feedback.Add(CheckStatement("Bus exceeds Car", answers.BusMoreThanCar,
                counts!["Bus"] > counts["Car"]));
            feedback.Add(CheckStatement("Walk is fewer than Bike", answers.WalkFewerThanBike,
                counts!["Walk"] < counts["Bike"]));
            feedback.Add(CheckStatement("Walk plus Bike exceeds Van",
                answers.WalkBikeMoreThanVan,
                counts!["Walk"] + counts["Bike"] > counts["Van"]));
        }

        feedback.Add(CheckCoolDown("G3-U01-L02-EDU-CD-ONE-STEP",
            "Fictional club survey: one-step comparison", answers.CoolDownOne));
        feedback.Add(CheckCoolDown("G3-U01-L02-EDU-CD-TWO-STEP",
            "Fictional club survey: two-step comparison", answers.CoolDownTwo));
        feedback.Add(new("Draw matching picture and bar graphs", null, true,
            "Teacher checks title, six categories, unit key, axis, matching plotted counts and evidence of a real survey."));
        feedback.Add(new("Explain statements, discuss with partner, revise norms", null, true,
            "Explanation, discussion and mathematical community reflection are teacher-reviewed, not numeric passes."));
        feedback.Add(new("Draw and explain the Edulytics Cool-down bar graph", null, true,
            "Teacher evaluates the independently authored graph drawing and two-step equation."));

        Response.Headers["X-Robots-Tag"] = "noindex, nofollow, noarchive";
        return View("Index", new Grade3Lesson2PracticePage(curriculumAdoptionId,
            lessonId, title!, true, answers, valid ? survey : null, feedback));
    }

    private static List<UsCcssG3Unit1Lesson2ClassSurveyCandidate.ClassroomCount>? ParseCounts(
        Grade3Lesson2Answers a)
    {
        var values = new[] { a.Bike, a.Walk, a.Van, a.Bus, a.Car, a.Train };
        var result = new List<UsCcssG3Unit1Lesson2ClassSurveyCandidate.ClassroomCount>();
        for (var i = 0; i < Categories.Length; ++i)
        {
            if (!int.TryParse(values[i], NumberStyles.Integer,
                    CultureInfo.InvariantCulture, out var count))
                return null;
            result.Add(new(Categories[i], count));
        }
        return result;
    }

    private static Grade3Lesson2Feedback CompareDifference(
        IReadOnlyList<UsCcssG3Unit1Lesson2ClassSurveyCandidate.ClassroomCount>? data, string? submitted)
    {
        if (!int.TryParse(submitted, NumberStyles.Integer,
                CultureInfo.InvariantCulture, out var response))
            return new("Bus/Car absolute difference", false, false, "Enter a whole number.");
        if (!UsCcssG3Unit1Lesson2ClassSurveyCandidate.TryGradeDifference(
                data, "Bus", "Car", response, out var correct))
            return new("Bus/Car absolute difference", null, true,
                "Invalid class data: teacher must choose an in-scope comparison.");
        return new("Bus/Car absolute difference", correct, false,
            correct ? "Matches the supplied class counts." : "Subtract the smaller count from the larger count.");
    }

    private static Grade3Lesson2Feedback CompareTwoStep(
        IReadOnlyList<UsCcssG3Unit1Lesson2ClassSurveyCandidate.ClassroomCount>? data, string? submitted)
    {
        if (!int.TryParse(submitted, NumberStyles.Integer,
                CultureInfo.InvariantCulture, out var response))
            return new("Walk + Bike versus Van", false, false, "Enter a whole number.");
        if (!UsCcssG3Unit1Lesson2ClassSurveyCandidate.TryGradeTwoStep(
                data, "Walk", "Bike", "Van", response, out var correct))
            return new("Walk + Bike versus Van", null, true,
                "The selected comparison exceeds the current within-20 family or its data are invalid. Teacher review needed.");
        return new("Walk + Bike versus Van", correct, false,
            correct ? "Sum and absolute difference match the input." : "Add Walk and Bike, then compare with Van.");
    }

    private static Grade3Lesson2Feedback CheckStatement(string label, string? submitted, bool expected)
    {
        if (!bool.TryParse(submitted, out var selected))
            return new(label, false, true, "Choose True or False and explain to your teacher.");
        return new(label, selected == expected, true,
            selected == expected ? "Selection matches these counts; explanation needs teacher review."
                : "Compare the category numbers and explain a corrected selection.");
    }

    private static Grade3Lesson2Feedback CheckCoolDown(
        string questionId, string label, string? submitted)
    {
        if (!int.TryParse(submitted, NumberStyles.Integer,
                CultureInfo.InvariantCulture, out var response))
            return new(label, false, false, "Enter a whole number.");
        if (!UsCcssG3OriginalCoolDownCandidates.TryGradeNumeric(
                Code, questionId, response, out var correct))
            return new(label, null, true, "This task is not eligible for numeric grading.");
        return new(label, correct, false,
            correct ? "Correct for the separate fictional club dataset."
                : "Use the fictional club graph rather than the real class data.");
    }

    private async Task<(string? Title, IActionResult? Failure)> CheckAccess(
        Guid adoptionId, Guid lessonId, CancellationToken ct)
    {
        if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var actor))
            return (null, Forbid());
        var workspace = await privatePractice.GetWorkspaceAsync(actor, adoptionId, ct);
        if (workspace.SelectedCurriculumAdoptionId != adoptionId)
            return (null, NotFound());
        var lesson = workspace.Lessons.SingleOrDefault(x => x.LessonId == lessonId);
        if (lesson is null || lesson.LessonCode != Code)
            return (null, NotFound());
        var published = await lessonContent.GetPublishedForStudentAsync(
            actor, lessonId, CultureInfo.CurrentUICulture.Name, ct);
        if (published.Value is null)
            return (null, published.Error == LessonContentErrorCode.AccessDenied
                ? Forbid() : NotFound());
        if (published.Value.LessonCode != Code)
            return (null, NotFound());
        return (published.Value.Title, null);
    }
}
