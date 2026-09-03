using System.Security.Claims;
using Edulytics.Core.Constants;
using Edulytics.Core.Enums;
using Edulytics.Services.Curriculum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Edulytics.Web.Controllers;

[Authorize(Policy = "SchoolAccess")]
[Route("school/academic-structure/phase39")]
public sealed class Phase39AcademicRelationshipsController : Controller
{
    private readonly IExplicitCurriculumLevelService _curriculumLevels;
    private readonly IExplicitCurriculumLevelUiQuery _classQuery;
    private readonly IStringLocalizer<AcademicResource> _text;

    public Phase39AcademicRelationshipsController(
        IExplicitCurriculumLevelService curriculumLevels,
        IExplicitCurriculumLevelUiQuery classQuery,
        IStringLocalizer<AcademicResource> text)
    {
        _curriculumLevels = curriculumLevels;
        _classQuery = classQuery;
        _text = text;
    }

    [HttpGet("class-options")]
    public async Task<IActionResult> ClassOptions(CancellationToken cancellationToken)
    {
        if (!TryGetActorId(out var actorUserId))
            return Forbid();

        var classes = await _classQuery.ListClassesAsync(actorUserId, cancellationToken);

        return Json(classes.Select(item => new
        {
            id = item.ClassGroupId,
            label = item.DisplayLabel,
            academicYearName = item.AcademicYearName,
            academicProgramName = item.AcademicProgramName,
            curriculumLevelLabel = item.CurriculumLevelLabel,
            pathway = item.CurriculumPathway,
            status = (int)item.Status
        }));
    }

    [Authorize(Roles = RoleNames.SubjectSupervisor)]
    [HttpPost("teacher-assignments")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AssignTeacherToClasses(
        Guid teacherUserId,
        Guid[]? classGroupIds,
        CancellationToken cancellationToken)
    {
        if (!TryGetActorId(out var actorUserId))
            return Forbid();

        var selectedClassIds = (classGroupIds ?? [])
            .Where(id => id != Guid.Empty)
            .Distinct()
            .ToArray();

        if (selectedClassIds.Length == 0)
        {
            SetExplicitError(ExplicitCurriculumLevelErrorCode.Required);
            return RedirectToAcademicStructure();
        }

        var availableClasses = await _classQuery.ListClassesAsync(actorUserId, cancellationToken);
        var activeClassIds = availableClasses
            .Where(item => item.Status == AcademicStructureStatus.Active)
            .Select(item => item.ClassGroupId)
            .ToHashSet();

        if (selectedClassIds.Any(id => !activeClassIds.Contains(id)))
            return Forbid();

        foreach (var classGroupId in selectedClassIds)
        {
            var result = await _curriculumLevels.AssignTeacherAsync(
                actorUserId,
                new AssignTeacherToCurriculumClassRequest(
                    teacherUserId,
                    classGroupId),
                cancellationToken);

            if (result.Succeeded ||
                result.Error == ExplicitCurriculumLevelErrorCode.DuplicateTeacherAssignment)
            {
                continue;
            }

            SetExplicitError(
                result.Error ?? ExplicitCurriculumLevelErrorCode.PersistenceError);
            return RedirectToAcademicStructure();
        }

        var success = _text["SuccessTeacherAssigned"];
        TempData["AcademicSuccess"] = success.ResourceNotFound
            ? "Teacher classes updated."
            : success.Value;

        return RedirectToAcademicStructure();
    }

    private void SetExplicitError(ExplicitCurriculumLevelErrorCode code)
    {
        var localized = _text[$"ExplicitError{code}"];
        TempData["AcademicError"] = localized.ResourceNotFound
            ? code.ToString()
            : localized.Value;
    }

    private IActionResult RedirectToAcademicStructure() =>
        RedirectToAction(
            nameof(AcademicStructureController.Index),
            "AcademicStructure");

    private bool TryGetActorId(out Guid userId) =>
        Guid.TryParse(
            User.FindFirstValue(ClaimTypes.NameIdentifier),
            out userId);
}
