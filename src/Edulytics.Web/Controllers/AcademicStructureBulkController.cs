using System.Globalization;
using System.Security.Claims;
using Edulytics.Core.Constants;
using Edulytics.Services.Curriculum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Edulytics.Web.Controllers;

[Authorize(Policy = "SchoolAccess")]
[Authorize(Roles = RoleNames.SubjectSupervisor)]
[Route("school/academic-structure")]
public sealed class AcademicStructureBulkController : Controller
{
    private readonly IExplicitCurriculumLevelService _levels;
    private readonly IStringLocalizer<AcademicResource> _text;

    public AcademicStructureBulkController(
        IExplicitCurriculumLevelService levels,
        IStringLocalizer<AcademicResource> text)
    {
        _levels = levels;
        _text = text;
    }

    [HttpPost("curriculum-levels/bulk")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AdoptCurriculumLevels(
        Guid academicYearId,
        Guid academicProgramId,
        string[]? curriculumLevelKeys,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(
                User.FindFirstValue(ClaimTypes.NameIdentifier),
                out var actorUserId))
        {
            return Forbid();
        }

        var keys = (curriculumLevelKeys ?? [])
            .Select(x => x?.Trim() ?? string.Empty)
            .Where(x => x.Length > 0)
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        if (academicYearId == Guid.Empty ||
            academicProgramId == Guid.Empty ||
            keys.Length == 0)
        {
            TempData["AcademicError"] = LocalizeError(
                ExplicitCurriculumLevelErrorCode.Required);
            return BackToCurriculumLevels();
        }

        var dashboardResult = await _levels.GetDashboardAsync(
            actorUserId,
            cancellationToken);
        if (dashboardResult.Value is null)
        {
            TempData["AcademicError"] = LocalizeError(
                dashboardResult.Error ?? ExplicitCurriculumLevelErrorCode.AccessDenied);
            return BackToCurriculumLevels();
        }

        var dashboard = dashboardResult.Value;
        var availableKeys = dashboard.AvailableLevels
            .Where(x => x.AcademicProgramId == academicProgramId)
            .Select(x => x.LevelKey)
            .ToHashSet(StringComparer.Ordinal);

        if (keys.Any(key => !availableKeys.Contains(key)))
        {
            TempData["AcademicError"] = LocalizeError(
                ExplicitCurriculumLevelErrorCode.CurriculumLevelProgramMismatch);
            return BackToCurriculumLevels();
        }

        var existingKeys = dashboard.Adoptions
            .Where(x =>
                x.AcademicYearId == academicYearId &&
                x.AcademicProgramId == academicProgramId)
            .Select(x => x.LevelKey)
            .ToHashSet(StringComparer.Ordinal);

        var pendingKeys = keys
            .Where(key => !existingKeys.Contains(key))
            .ToArray();

        var added = 0;
        foreach (var key in pendingKeys)
        {
            var result = await _levels.AdoptLevelAsync(
                actorUserId,
                new AdoptExplicitCurriculumLevelRequest(
                    academicYearId,
                    academicProgramId,
                    key),
                cancellationToken);

            if (!result.Succeeded)
            {
                TempData["AcademicError"] = LocalizeError(
                    result.Error ?? ExplicitCurriculumLevelErrorCode.PersistenceError);
                return BackToCurriculumLevels();
            }

            added++;
        }

        var polish = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName
            .Equals("pl", StringComparison.OrdinalIgnoreCase);
        TempData["AcademicSuccess"] = added == 0
            ? (polish
                ? "Wybrane poziomy programu nauczania są już dodane."
                : "The selected Curriculum Levels are already added.")
            : (polish
                ? $"Dodano poziomy programu nauczania: {added}."
                : $"Curriculum Levels added: {added}.");

        return BackToCurriculumLevels();
    }

    private string LocalizeError(ExplicitCurriculumLevelErrorCode code)
    {
        var localized = _text[$"ExplicitError{code}"];
        return localized.ResourceNotFound ? code.ToString() : localized.Value;
    }

    private RedirectResult BackToCurriculumLevels() =>
        Redirect("/school/academic-structure#grades");
}
