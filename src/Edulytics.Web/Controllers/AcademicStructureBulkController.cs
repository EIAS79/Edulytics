using System.Globalization;
using System.Security.Claims;
using Edulytics.Core.Constants;
using Edulytics.Services.Academics;
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
    private readonly IStudentPlacementService _placements;
    private readonly IStringLocalizer<AcademicResource> _text;

    public AcademicStructureBulkController(
        IExplicitCurriculumLevelService levels,
        IStudentPlacementService placements,
        IStringLocalizer<AcademicResource> text)
    {
        _levels = levels;
        _placements = placements;
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
        if (!TryGetActorId(out var actorUserId))
            return Forbid();

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

        var polish = IsPolish();
        TempData["AcademicSuccess"] = added == 0
            ? (polish
                ? "Wybrane poziomy programu nauczania są już dodane."
                : "The selected Curriculum Levels are already added.")
            : (polish
                ? $"Dodano poziomy programu nauczania: {added}."
                : $"Curriculum Levels added: {added}.");

        return BackToCurriculumLevels();
    }

    [HttpPost("student-placements/bulk")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PlaceStudents(
        Guid sourceClassGroupId,
        Guid classGroupId,
        Guid[]? studentProfileIds,
        CancellationToken cancellationToken)
    {
        if (!TryGetActorId(out var actorUserId))
            return Forbid();

        var ids = (studentProfileIds ?? [])
            .Where(x => x != Guid.Empty)
            .Distinct()
            .ToArray();

        if (sourceClassGroupId == Guid.Empty ||
            classGroupId == Guid.Empty ||
            sourceClassGroupId == classGroupId ||
            ids.Length == 0)
        {
            TempData["AcademicError"] = IsPolish()
                ? "Wybierz klasę i co najmniej jednego ucznia."
                : "Select a class and at least one student.";
            return BackToStudents();
        }

        var result = await _placements.MoveStudentsAsync(
            actorUserId,
            sourceClassGroupId,
            classGroupId,
            ids,
            cancellationToken);

        if (result.Succeeded && result.Failures.Count == 0)
        {
            TempData["AcademicSuccess"] = IsPolish()
                ? $"Przeniesiono uczniów: {result.Moved}."
                : $"Students moved: {result.Moved}.";
            return BackToStudents();
        }

        TempData["AcademicError"] = BuildMoveFailureMessage(result.Failures);
        return BackToStudents();
    }

    private string BuildMoveFailureMessage(
        IReadOnlyList<StudentPlacementFailure> failures)
    {
        var codes = failures
            .Select(x => x.Code)
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        if (codes.Any(code =>
                code is "CrossAcademicYearMoveNotAllowed" or
                    "CrossGradeMoveNotAllowed" or
                    "CrossCurriculumMoveNotAllowed" or
                    "InvalidClassMove"))
        {
            return IsPolish()
                ? "Wybierz inną klasę w tym samym roku akademickim, programie i poziomie programu nauczania."
                : "Choose a different destination class in the same academic year, program, and curriculum level.";
        }

        if (codes.Contains("StudentNotInSourceClass", StringComparer.Ordinal) ||
            codes.Contains("StudentNotEnrolledForAcademicYear", StringComparer.Ordinal))
        {
            return IsPolish()
                ? "Co najmniej jeden wybrany uczeń nie należy już do klasy źródłowej. Odśwież stronę i spróbuj ponownie."
                : "At least one selected student is no longer enrolled in the source class. Refresh the page and try again.";
        }

        if (codes.Contains("StudentInactive", StringComparer.Ordinal) ||
            codes.Contains("StudentProfileNotFound", StringComparer.Ordinal))
        {
            return IsPolish()
                ? "Nie można przenieść co najmniej jednego wybranego ucznia, ponieważ jego profil jest nieaktywny lub niedostępny."
                : "At least one selected student cannot be moved because the profile is inactive or unavailable.";
        }

        if (codes.Contains("PersistenceError", StringComparer.Ordinal))
        {
            return IsPolish()
                ? "Nie udało się zapisać zmiany klasy. Żaden uczeń nie został przeniesiony."
                : "The class change could not be saved. No students were moved.";
        }

        return IsPolish()
            ? "Nie udało się przenieść wybranych uczniów. Żaden uczeń nie został przeniesiony."
            : "The selected students could not be moved. No students were moved.";
    }

    private bool TryGetActorId(out Guid actorUserId) =>
        Guid.TryParse(
            User.FindFirstValue(ClaimTypes.NameIdentifier),
            out actorUserId);

    private bool IsPolish() =>
        CultureInfo.CurrentUICulture.TwoLetterISOLanguageName
            .Equals("pl", StringComparison.OrdinalIgnoreCase);

    private string LocalizeError(ExplicitCurriculumLevelErrorCode code)
    {
        var localized = _text[$"ExplicitError{code}"];
        return localized.ResourceNotFound ? code.ToString() : localized.Value;
    }

    private RedirectResult BackToCurriculumLevels() =>
        Redirect("/school/academic-structure#grades");

    private RedirectResult BackToStudents() =>
        Redirect("/school/academic-structure#students");
}
