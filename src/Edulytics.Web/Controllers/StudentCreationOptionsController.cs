using System.Security.Claims;
using Edulytics.Core.Constants;
using Edulytics.Services.StudentSetup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

[Authorize(Roles = RoleNames.SubjectSupervisor)]
public sealed class StudentCreationOptionsController : Controller
{
    private readonly IStudentCreationClassCatalog _classes;

    public StudentCreationOptionsController(
        IStudentCreationClassCatalog classes)
    {
        _classes = classes;
    }

    [HttpGet("/School/Users/Student-Classes")]
    public async Task<IActionResult> Classes(
        Guid schoolId,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(
                User.FindFirstValue(ClaimTypes.NameIdentifier),
                out var actorUserId) ||
            schoolId == Guid.Empty)
        {
            return Forbid();
        }

        var classes =
            await _classes.ListAsync(
                actorUserId,
                schoolId,
                cancellationToken);

        if (classes is null)
        {
            return Forbid();
        }

        return Json(
            classes.Select(
                x => new
                {
                    id = x.Id,
                    label = x.DisplayLabel,
                    academicYearName = x.AcademicYearName
                }));
    }
}