using Edulytics.Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

[Authorize(Roles = RoleNames.SchoolAdmin)]
[Route("school/subject-supervisors")]
public sealed class SubjectSupervisorAssignmentsController
    : Controller
{
    // Compatibility controller retained so old route references fail closed.
    // The approved workflow no longer assigns a Subject Supervisor to a
    // subject from the School Administrator workspace.

    [HttpGet("")]
    public IActionResult Index() => NotFound();

    [HttpPost("assign")]
    [ValidateAntiForgeryToken]
    public IActionResult Assign() => NotFound();

    [HttpPost("{assignmentId:guid}/remove")]
    [ValidateAntiForgeryToken]
    public IActionResult Remove(Guid assignmentId) => NotFound();
}
