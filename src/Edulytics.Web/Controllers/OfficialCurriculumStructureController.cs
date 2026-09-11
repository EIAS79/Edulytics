using System.Globalization;
using System.Security.Claims;
using Edulytics.Core.Users;
using Edulytics.Services.Curriculum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

[Authorize(Roles = RoleNames.SubjectSupervisor)]
[Route("school/curriculum")]
public sealed class OfficialCurriculumStructureController(
    IOfficialCurriculumStructureService structure) : Controller
{
    [HttpPost("official-structure")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Initialize(
        Guid curriculumAdoptionId,
        Guid? academicYearId,
        Guid? academicProgramId,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var actorId))
            return Forbid();

        var result = await structure.InitializeAsync(
            actorId,
            curriculumAdoptionId,
            cancellationToken);

        var polish = string.Equals(
            CultureInfo.CurrentUICulture.TwoLetterISOLanguageName,
            "pl",
            StringComparison.OrdinalIgnoreCase);

        if (result.Succeeded)
        {
            TempData["Success"] = result.TopicsCreated == 0 && result.OutcomesCreated == 0
                ? (polish
                    ? "Oficjalna struktura programu jest już zsynchronizowana."
                    : "The official curriculum structure is already synchronized.")
                : (polish
                    ? $"Dodano oficjalną strukturę programu: {result.TopicsCreated} tematów i {result.OutcomesCreated} efektów uczenia się."
                    : $"Official curriculum structure added: {result.TopicsCreated} topics and {result.OutcomesCreated} learning outcomes.");
        }
        else
        {
            TempData["Error"] = polish
                ? "Nie udało się utworzyć struktury z oficjalnego programu."
                : "The official curriculum structure could not be created.";
        }

        return RedirectToAction(
            "Index",
            "Curriculum",
            new
            {
                academicYearId,
                academicProgramId,
                curriculumAdoptionId
            });
    }
}
