using System.Globalization;
using System.Security.Claims;
using Edulytics.Core.Mathematics.Curriculum;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.ViewModels.LessonContent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;
/// <summary>G3 U01 L04 data-dependent student graph helper, not teacher survey verification.</summary>
[Authorize(Policy = "StudentPortal")]
[Route("student/practice/grade3-u01-l04")]
public sealed class Grade3Lesson4ActivitiesController(
    IStudentPrivatePracticeService practice,
    ILessonContentService lessonContent) : Controller
{
    private const string Code = UsCcssG3Unit1Lesson4ScaledTravelSurvey.LessonCode;

    [HttpGet("")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(Guid curriculumAdoptionId, Guid lessonId, CancellationToken ct)
    {
        var failure = await CheckAccess(curriculumAdoptionId, lessonId, ct);
        if (failure is not null) return failure;
        Response.Headers["X-Robots-Tag"] = "noindex, nofollow, noarchive";
        return View(new Grade3Lesson4PracticePage(curriculumAdoptionId, lessonId,
            false, new Grade3Lesson4Answers(), null, []));
    }

    [HttpPost("submit"), ValidateAntiForgeryToken]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Submit(Guid curriculumAdoptionId, Guid lessonId,
        [FromForm] Grade3Lesson4Answers answers, CancellationToken ct)
    {
        var failure = await CheckAccess(curriculumAdoptionId, lessonId, ct);
        if (failure is not null) return failure;
        if (answers is null || typeof(Grade3Lesson4Answers).GetProperties()
                .Select(p => p.GetValue(answers) as string).Any(x => x?.Length > 2000))
            return BadRequest("Response exceeds the allowed limit.");

        var data = Parse(answers);
        var valid = UsCcssG3Unit1Lesson4ScaledTravelSurvey.TryValidate(data, out var message);
        var feedback = new List<Grade3Lesson4Feedback>
        {
            new("Travel survey", null, true, message +
                " Only the teacher can verify real classmates' responses or participation.")
        };
        if (valid)
        {
            var car = data!.Single(x => x.Category == "Car").Students;
            var train = data.Single(x => x.Category == "Train").Students;
            feedback.Add(Conditional("Full symbols for Car", answers.CarWholeSymbols, car / 2));
            feedback.Add(Conditional("Half symbols for Train", answers.TrainHalfSymbol, train % 2));
        }
        feedback.Add(GradeSourcePlaneExtra(answers.SourcePlaneExtraWhole));
        feedback.Add(GradeSourcePlaneWholeAfter(answers.SourcePlaneWholeAfterFour));
        feedback.Add(new("Original source Cool-down graph drawing", null, true,
            string.IsNullOrWhiteSpace(answers.SourcePlaneDrawing)
                ? "Missing drawing explanation; teacher reviews student sketch of TWO added full Plane marks."
                : "Teacher checks that the student actually drew TWO new full Plane marks. Numeric correctness alone does not certify the drawing."));
        feedback.Add(CoolDown("G3-U01-L04-CD-TRAIN-WHOLE",
            "Cool-down: full train marks", answers.CoolDownTrainWhole));
        feedback.Add(CoolDown("G3-U01-L04-CD-TRAIN-HALF",
            "Cool-down: half train mark", answers.CoolDownTrainHalf));
        if (!string.IsNullOrWhiteSpace(answers.CoolDownPlanePlusTwoWhole))
            feedback.Add(CoolDown("G3-U01-L04-CD-PLANE-PLUS-TWO-WHOLE",
                "Optional Edulytics extension: Plane +2", answers.CoolDownPlanePlusTwoWhole));
        feedback.Add(CoolDown("G3-U01-L04-CD-TOTAL",
            "Cool-down: total survey respondents", answers.CoolDownTotal));
        feedback.Add(new("Student-drawn graph, evidence and own explanations", null, true,
            "Teacher checks authentic data collection, title, category labels, a symbol key worth two students, half-symbol for odd counts, and written reasoning."));
        feedback.Add(new("Source synthesis: two more Balloon students", null, true,
            string.IsNullOrWhiteSpace(answers.BalloonPlusTwoGraphChange)
                ? "Missing explanation: teacher checks one additional whole picture in the entered Balloon row."
                : "Teacher checks one new whole symbol, category count and an accurate drawing or explanation. No automatic certification."));
        feedback.Add(new("Source synthesis: one more Car student", null, true,
            string.IsNullOrWhiteSpace(answers.CarPlusOneGraphChange)
                ? "Missing explanation: teacher checks the initial Car parity and the half-picture change."
                : "Teacher checks one new half-picture when Car was even, or completion of an existing half-picture when Car was odd. No numeric pass."));
        feedback.Add(new("Class discussion and mathematical-community norms", null, true,
            "Teacher documents partner explanation, how questions were addressed and Math Community norms."));
        feedback.Add(new("Original Edulytics Cool-down drawing and reasoning", null, true,
            "The first-party numbers alone never certify an accurate student-created picture graph."));

        Response.Headers["X-Robots-Tag"] = "noindex, nofollow, noarchive";
        return View("Index", new Grade3Lesson4PracticePage(curriculumAdoptionId, lessonId,
            true, answers, valid ? data : null, feedback));
    }

    private static List<UsCcssG3Unit1Lesson4ScaledTravelSurvey.TravelCount>? Parse(
        Grade3Lesson4Answers answers)
    {
        var names = UsCcssG3Unit1Lesson4ScaledTravelSurvey.Categories;
        var values = new[] { answers.Car, answers.Train, answers.Boat,
            answers.Balloon, answers.Plane, answers.Helicopter };
        var rows = new List<UsCcssG3Unit1Lesson4ScaledTravelSurvey.TravelCount>();
        for (var i = 0; i < names.Count; i++)
        {
            if (!int.TryParse(values[i], NumberStyles.Integer,
                    CultureInfo.InvariantCulture, out var number))
                return null;
            rows.Add(new(names[i], number));
        }
        return rows;
    }

    private static Grade3Lesson4Feedback Conditional(string label, string? input, int expected)
    {
        if (!int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out var answer))
            return new(label, false, false, "Enter a whole number.");
        return new(label, answer == expected, true,
            answer == expected
                ? "Matches the supplied counts, but the class survey is not independently certified."
                : "For each two pupils draw one full mark; one left over requires a half mark.");
    }

    private static Grade3Lesson4Feedback GradeSourcePlaneExtra(string? input)
    {
        if (!int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out var n))
            return new("Original four Plane pupils: extra whole marks", false, false, "Enter an integer.");
        UsCcssG3Unit1Lesson4ScaledTravelSurvey.TryGradeSourcePlaneExtraWhole(n, out var correct);
        return new("Original four Plane pupils: extra whole marks", correct, false,
            correct ? "Two new whole marks represent four people." : "Four divided by key two gives two new symbols.");
    }

    private static Grade3Lesson4Feedback GradeSourcePlaneWholeAfter(string? input)
    {
        if (!int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out var n))
            return new("Fictional Plane after four: whole marks", false, false, "Enter an integer.");
        UsCcssG3Unit1Lesson4ScaledTravelSurvey.TryGradeSourcePlaneWholeAfter(3, n, out var correct);
        return new("Fictional Plane after four: whole marks", correct, false,
            correct ? "3 + 4 = 7 pupils, three full symbols and one half." : "Seven pupils require three whole marks plus one half.");
    }

    private static Grade3Lesson4Feedback CoolDown(string id, string label, string? input)
    {
        if (!int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out var answer))
            return new(label, false, false, "Enter a whole number.");
        var eligible = UsCcssG3Unit1Lesson4ScaledTravelSurvey.TryGradeCoolDown(
            id, answer, out var correct);
        return new(label, eligible ? correct : null, !eligible,
            !eligible ? "No machine scorer exists for this open question."
                : correct ? "Correct for the separate Edulytics fictional graph."
                : "Count full and half symbols according to the key of two.");
    }

    private async Task<IActionResult?> CheckAccess(Guid adoptionId, Guid lessonId, CancellationToken ct)
    {
        if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var user))
            return Forbid();
        var workspace = await practice.GetWorkspaceAsync(user, adoptionId, ct);
        if (workspace.SelectedCurriculumAdoptionId != adoptionId)
            return NotFound();
        var lesson = workspace.Lessons.SingleOrDefault(x => x.LessonId == lessonId);
        if (lesson is null || lesson.LessonCode != Code)
            return NotFound();
        var published = await lessonContent.GetPublishedForStudentAsync(
            user, lessonId, CultureInfo.CurrentUICulture.Name, ct);
        if (published.Value is null)
            return published.Error == LessonContentErrorCode.AccessDenied
                ? Forbid() : NotFound();
        return published.Value.LessonCode == Code ? null : NotFound();
    }
}
