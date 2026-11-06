using System.Security.Claims;
using Edulytics.Core.Constants;
using Edulytics.Web.Presentation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

[Authorize(Policy = "StudentPortal")]
[Route("student/g12/precalculus/rubric")]
public sealed class Grade12PrecalculusRubricStudentController(Grade12PrecalculusRubricService rubric) : Controller
{
    private bool Actor(out Guid id) =>
        Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out id);



    [HttpGet("")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(
        Guid lessonId, int exerciseNumber, CancellationToken ct)
    {
        if (!Actor(out var actor)) return Forbid();
        var task = Grade12PrecalculusSection11Exercises.Find(exerciseNumber);
        if (task is null) return NotFound();
        var scope = await rubric.StudentScopeAsync(actor, lessonId, Guid.Empty, ct);
        if (scope is null) return NotFound();
        var existing = await rubric.FindStudentAsync(scope.Value.School, scope.Value.Class, scope.Value.Student, lessonId, exerciseNumber, ct);
        return View(new G12PrecalcRubricStudentPage(lessonId, exerciseNumber, task.Prompt, existing));
    }

    [HttpPost("submit"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(
        Guid lessonId, int exerciseNumber,
        [FromForm] string? explanation,
        [FromForm] string? graphEvidence, CancellationToken ct)
    {
        if (!Actor(out var actor)) return Forbid();
        if (Grade12PrecalculusSection11Exercises.Find(exerciseNumber) is null)
            return NotFound();
        var scope = await rubric.StudentScopeAsync(actor, lessonId, Guid.Empty, ct);
        if (scope is null) return NotFound();
        var studentResponse = Grade12PrecalculusSection11Exercises.ComposeTeacherResponse(
            exerciseNumber, explanation, graphEvidence);
        if (studentResponse is null)
            return BadRequest("Provide complete written reasoning and, for graph-window " +
                "tasks 76-87, a labelled graph/coordinate description. " +
                "Maximum combined length is 8000 characters.");
        var inserted = await rubric.InsertAsync(scope.Value.School, scope.Value.Class, scope.Value.Student,
            lessonId, exerciseNumber, studentResponse, ct);
        if (!inserted) return Conflict("Already submitted; teacher reviews the original answer.");
        return RedirectToAction(nameof(Index), new { lessonId, exerciseNumber });
    }
}

[Authorize(Roles = RoleNames.Teacher)]
[Route("school/g12/precalculus/rubric")]
public sealed class Grade12PrecalculusRubricTeacherController(Grade12PrecalculusRubricService rubric) : Controller
{
    private bool Actor(out Guid id) =>
        Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out id);



    [HttpGet("")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        if (!Actor(out var actor)) return Forbid();
        var groups = await rubric.TeacherGroupsAsync(actor, ct);
        var rows = new List<G12PrecalcRubricRow>();
        foreach (var assignedGroup in groups)
        {
            var candidates = await rubric.QueueAsync(
                assignedGroup.SchoolId, assignedGroup.ClassGroupId, ct);
            foreach (var candidate in candidates)
                if (await rubric.MayReviewAsync(actor, candidate, ct))
                    rows.Add(candidate);
        }
        return View(rows.OrderByDescending(x => x.SubmittedAtUtc).ToArray());
    }

    [HttpGet("{id:guid}")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Review(Guid id, CancellationToken ct)
    {
        if (!Actor(out var actor)) return Forbid();
        var row = await rubric.FindAsync( id, ct);
        if (row is null) return NotFound();
        if (!await rubric.MayReviewAsync(actor, row, ct)) return NotFound();
        return View(new G12PrecalcRubricTeacherPage(row, G12PrecalcRubricStorage.Criteria));
    }

    [HttpPost("{id:guid}"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Grade(Guid id, [FromForm] int[]? marks,
        [FromForm] string? teacherFeedback, CancellationToken ct)
    {
        if (!Actor(out var actor)) return Forbid();
        var row = await rubric.FindAsync( id, ct);
        if (row is null) return NotFound();
        if (!await rubric.MayReviewAsync(actor, row, ct)) return NotFound();
        if (marks is null || marks.Length != 4 || marks.Any(x => x is not 0 and not 1) ||
            string.IsNullOrWhiteSpace(teacherFeedback) ||
            teacherFeedback.Trim().Length is < 10 or > 2000)
            return BadRequest("All four rubric criteria and teacher feedback are required.");
        if (!await rubric.GradeAsync( row, actor, marks, teacherFeedback.Trim(), ct))
            return Conflict("This submission has already been reviewed.");
        return RedirectToAction(nameof(Review), new { id });
    }
}
