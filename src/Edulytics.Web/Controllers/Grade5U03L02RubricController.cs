using System.Security.Claims;
using Edulytics.Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

[Authorize(Policy = "StudentPortal")]
[Route("student/g5/u03/l02/rubric")]
public sealed class Grade5U03L02RubricStudentController(Grade5U03L02RubricService rubric) : Controller
{
    private bool Actor(out Guid id) =>
        Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out id);



    [HttpGet("")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(
        Guid lessonId, Guid curriculumAdoptionId, CancellationToken ct)
    {
        if (!Actor(out var actor)) return Forbid();
        var scope = await rubric.StudentScopeAsync(actor, lessonId, curriculumAdoptionId, ct);
        if (scope is null) return NotFound();
        var existing = await rubric.FindStudentAsync( scope.Value.School, scope.Value.Class, scope.Value.Student, lessonId, ct);
        return View(new G5L02RubricStudentPage(lessonId, curriculumAdoptionId, existing));
    }

    [HttpPost("submit"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(
        Guid lessonId, Guid curriculumAdoptionId,
        [FromForm] string? explanation, CancellationToken ct)
    {
        if (!Actor(out var actor)) return Forbid();
        var scope = await rubric.StudentScopeAsync(actor, lessonId, curriculumAdoptionId, ct);
        if (scope is null) return NotFound();
        if (string.IsNullOrWhiteSpace(explanation) ||
            explanation.Trim().Length is < 20 or > 8000)
            return BadRequest("An explanation between 20 and 8000 characters is required.");
        var inserted = await rubric.InsertAsync( scope.Value.School, scope.Value.Class, scope.Value.Student,
            lessonId, explanation.Trim(), ct);
        if (!inserted) return Conflict("Already submitted; teacher reviews the original answer.");
        return RedirectToAction(nameof(Index), new { lessonId, curriculumAdoptionId });
    }
}

[Authorize(Roles = RoleNames.Teacher)]
[Route("school/g5/u03/l02/rubric")]
public sealed class Grade5U03L02RubricTeacherController(Grade5U03L02RubricService rubric) : Controller
{
    private bool Actor(out Guid id) =>
        Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out id);



    [HttpGet("")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        if (!Actor(out var actor)) return Forbid();
        var groups = await rubric.TeacherGroupsAsync(actor, ct);
        var rows = new List<G5L02RubricRow>();
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
        return View(new G5L02RubricTeacherPage(row, G5L02RubricStorage.Criteria));
    }

    [HttpPost("{id:guid}"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Grade(Guid id, [FromForm] int[]? marks,
        [FromForm] string? teacherFeedback, CancellationToken ct)
    {
        if (!Actor(out var actor)) return Forbid();
        var row = await rubric.FindAsync( id, ct);
        if (row is null) return NotFound();
        if (!await rubric.MayReviewAsync(actor, row, ct)) return NotFound();
        if (marks is null || marks.Length != 6 || marks.Any(x => x is not 0 and not 1) ||
            string.IsNullOrWhiteSpace(teacherFeedback) ||
            teacherFeedback.Trim().Length is < 10 or > 2000)
            return BadRequest("All six rubric criteria and teacher feedback are required.");
        if (!await rubric.GradeAsync( row, actor, marks, teacherFeedback.Trim(), ct))
            return Conflict("This submission has already been reviewed.");
        return RedirectToAction(nameof(Review), new { id });
    }
}
