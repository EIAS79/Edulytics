using System.Security.Claims;
using Edulytics.Core.Constants;
using Edulytics.Services.Curriculum;
using Edulytics.Web.ViewModels.Curriculum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Edulytics.Web.Controllers;

[Authorize(Policy = "SchoolAccess")]
[Route("school/curriculum")]
public sealed class CurriculumController : Controller
{
    private readonly ICurriculumService _curriculum;
    private readonly IStringLocalizer<CurriculumResource> _text;

    // Historical constructor intentionally preserved for Phase07 regression tests.
    public CurriculumController(
        ICurriculumService curriculum,
        IStringLocalizer<CurriculumResource> text)
    {
        _curriculum = curriculum;
        _text = text;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(
        Guid? academicYearId = null,
        Guid? academicProgramId = null,
        Guid? curriculumAdoptionId = null,
        CancellationToken cancellationToken = default)
    {
        if (!TryActor(out var actorId))
            return Forbid();

        var levels = ExplicitLevels;
        var contentQuery = ExplicitContentQuery;

        // Compatibility fallback for direct controller tests / legacy hosts that
        // have not registered Phase29 explicit services. Production registers both.
        if (levels is null || contentQuery is null)
        {
            var legacy = await _curriculum.GetDashboardAsync(
                actorId,
                cancellationToken);

            if (legacy.Value is null)
                return HandleQueryError(legacy.Error);

            return View(
                new CurriculumIndexViewModel(
                    legacy.Value.GradeLevels,
                    legacy.Value.Subjects,
                    legacy.Value.Topics)
                {
                    AcademicPrograms = legacy.Value.AcademicPrograms,
                    Frameworks = legacy.Value.Frameworks,
                    Adoptions = legacy.Value.Adoptions
                });
        }

        if (!User.IsInRole(RoleNames.SchoolAdmin) &&
            !User.IsInRole(RoleNames.SubjectSupervisor) &&
            !User.IsInRole(RoleNames.Teacher))
        {
            return Forbid();
        }

        var levelResult = await levels.GetDashboardAsync(
            actorId,
            cancellationToken);
        if (levelResult.Value is null)
            return Forbid();

        var allAdoptions = levelResult.Value.Adoptions;
        var allTopics = await contentQuery.ListTopicsAsync(
            actorId,
            cancellationToken);

        var hasContext =
            academicYearId.HasValue ||
            academicProgramId.HasValue ||
            curriculumAdoptionId.HasValue;

        var selectedAdoptions = hasContext
            ? allAdoptions
                .Where(x =>
                    (!academicYearId.HasValue || x.AcademicYearId == academicYearId.Value) &&
                    (!academicProgramId.HasValue || x.AcademicProgramId == academicProgramId.Value) &&
                    (!curriculumAdoptionId.HasValue || x.Id == curriculumAdoptionId.Value))
                .ToArray()
            : [];

        var selectedAdoptionIds = selectedAdoptions
            .Select(x => x.Id)
            .ToHashSet();

        var selectedTopics = hasContext
            ? allTopics
                .Where(x => selectedAdoptionIds.Contains(x.CurriculumAdoptionId))
                .ToArray()
            : [];

        ViewData["ExplicitCurriculum"] = new ExplicitCurriculumLevelDashboard(
            levelResult.Value.AvailableLevels,
            selectedAdoptions);
        ViewData["AllCurriculumAdoptions"] = allAdoptions;
        ViewData["ExplicitTopics"] = selectedTopics;
        ViewData["SelectedAcademicYearId"] = academicYearId;
        ViewData["SelectedAcademicProgramId"] = academicProgramId;
        ViewData["SelectedCurriculumAdoptionId"] = curriculumAdoptionId;

        // The Razor contract type is retained so older compiled callers do not
        // break. Normal Phase29 rendering reads explicit ViewData only.
        return View(new CurriculumIndexViewModel([], [], []));
    }

    [NonAction]
    public Task<IActionResult> Index(CancellationToken cancellationToken) =>
        Index(null, null, null, cancellationToken);

    [Authorize(Roles = RoleNames.SubjectSupervisor)]
    [HttpPost("curriculum-topics")]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> CreateCurriculumTopic(
        Guid curriculumAdoptionId,
        string name,
        int order,
        CancellationToken cancellationToken,
        Guid? academicYearId = null,
        Guid? academicProgramId = null)
    {
        var levels = ExplicitLevels;
        if (levels is null)
            return Task.FromResult<IActionResult>(StatusCode(500));

        return ExecuteExplicitAsync(
            id => levels.CreateTopicAsync(
                id,
                new CreateTopicForCurriculumLevelRequest(
                    curriculumAdoptionId,
                    name,
                    order),
                cancellationToken),
            "SuccessTopicCreated",
            CurriculumContextRouteValues(
                academicYearId,
                academicProgramId,
                curriculumAdoptionId));
    }

    [Authorize(Roles = RoleNames.SubjectSupervisor)]
    [HttpPost("curriculum-outcomes/official")]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> CreateCurriculumOfficialOutcome(
        Guid topicId,
        string selectionKey,
        int order,
        CancellationToken cancellationToken,
        Guid? academicYearId = null,
        Guid? academicProgramId = null,
        Guid? curriculumAdoptionId = null)
    {
        var levels = ExplicitLevels;
        if (levels is null)
            return Task.FromResult<IActionResult>(StatusCode(500));

        var routeValues = CurriculumContextRouteValues(
            academicYearId,
            academicProgramId,
            curriculumAdoptionId);

        var selection = ParseOfficialSelection(selectionKey);
        if (selection is null)
        {
            TempData["Error"] = _text["ErrorOfficialOutcomeNotFound"].Value;
            return Task.FromResult<IActionResult>(
                RedirectToAction(nameof(Index), routeValues));
        }

        return ExecuteExplicitAsync(
            id => levels.CreateOfficialOutcomeAsync(
                id,
                new CreateOfficialOutcomeForCurriculumLevelRequest(
                    topicId,
                    selection.Value.ContentNodeId,
                    selection.Value.LessonNodeId,
                    order),
                cancellationToken),
            "SuccessOfficialOutcomeAdded",
            routeValues);
    }

    // -------- Legacy compatibility API below. --------

    [Authorize(Roles = RoleNames.SubjectSupervisor)]
    [HttpPost("framework")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SelectFramework(
        Guid subjectId,
        Guid gradeLevelId,
        Guid academicProgramId,
        string frameworkCode,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId))
            return Forbid();

        var result = await _curriculum.SelectFrameworkAsync(
            actorId,
            new SelectCurriculumFrameworkRequest(
                subjectId,
                gradeLevelId,
                frameworkCode,
                academicProgramId),
            cancellationToken);

        SetFeedback(result, "SuccessFrameworkSelected");
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = RoleNames.SubjectSupervisor)]
    [HttpPost("topics")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateTopic(
        Guid subjectId,
        Guid gradeLevelId,
        Guid academicProgramId,
        string name,
        int order,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId))
            return Forbid();

        var result = await _curriculum.CreateTopicAsync(
            actorId,
            new CreateCurriculumTopicRequest(
                subjectId,
                gradeLevelId,
                name,
                order,
                academicProgramId),
            cancellationToken);

        SetFeedback(result, "SuccessTopicCreated");
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = RoleNames.SubjectSupervisor)]
    [HttpGet("topics/{id:guid}/edit")]
    public async Task<IActionResult> EditTopic(
        Guid id,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId))
            return Forbid();

        var result = await _curriculum.GetTopicAsync(
            actorId,
            id,
            cancellationToken);

        if (result.Value is null)
            return HandleQueryError(result.Error);

        return View(
            new CurriculumTopicEditViewModel(result.Value));
    }

    [Authorize(Roles = RoleNames.SubjectSupervisor)]
    [HttpPost("topics/{id:guid}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditTopic(
        Guid id,
        string name,
        int order,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId))
            return Forbid();

        var result = await _curriculum.UpdateTopicAsync(
            actorId,
            new UpdateCurriculumTopicRequest(id, name, order),
            cancellationToken);

        SetFeedback(result, "SuccessTopicUpdated");

        return result.Succeeded
            ? RedirectToAction(nameof(Index))
            : RedirectToAction(nameof(EditTopic), new { id });
    }

    [Authorize(Roles = RoleNames.SubjectSupervisor)]
    [HttpPost("outcomes/official")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateOfficialOutcome(
        Guid topicId,
        string selectionKey,
        int order,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId))
            return Forbid();

        var selection = ParseOfficialSelection(selectionKey);
        var result = selection is null
            ? CurriculumCommandResult.Failure(
                "ContentNodeId",
                CurriculumErrorCode.OfficialOutcomeNotFound)
            : await _curriculum.CreateOfficialOutcomeAsync(
                actorId,
                new CreateOfficialLearningOutcomeRequest(
                    topicId,
                    selection.Value.ContentNodeId,
                    selection.Value.LessonNodeId,
                    order),
                cancellationToken);

        SetFeedback(result, "SuccessOfficialOutcomeAdded");
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = RoleNames.SubjectSupervisor)]
    [HttpGet("outcomes/{id:guid}/edit")]
    public async Task<IActionResult> EditOutcome(
        Guid id,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId))
            return Forbid();

        var result = await _curriculum.GetOutcomeAsync(
            actorId,
            id,
            cancellationToken);

        if (result.Value is null)
            return HandleQueryError(result.Error);

        return View(
            new LearningOutcomeEditViewModel(result.Value));
    }

    [Authorize(Roles = RoleNames.SubjectSupervisor)]
    [HttpPost("outcomes/{id:guid}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditOutcome(
        Guid id,
        string code,
        string description,
        int order,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId))
            return Forbid();

        var result = await _curriculum.UpdateOutcomeAsync(
            actorId,
            new UpdateLearningOutcomeRequest(
                id,
                code,
                description,
                order),
            cancellationToken);

        SetFeedback(result, "SuccessOutcomeUpdated");

        return result.Succeeded
            ? RedirectToAction(nameof(Index))
            : RedirectToAction(nameof(EditOutcome), new { id });
    }

    private async Task<IActionResult> ExecuteExplicitAsync(
        Func<Guid, Task<ExplicitCurriculumLevelCommandResult>> action,
        string successKey,
        object? routeValues = null)
    {
        if (!TryActor(out var actorId))
            return Forbid();

        var result = await action(actorId);
        if (result.Succeeded)
        {
            var success = _text[successKey];
            TempData["Success"] = success.ResourceNotFound
                ? "Curriculum updated."
                : success.Value;
        }
        else
        {
            var code = result.Error ?? ExplicitCurriculumLevelErrorCode.PersistenceError;
            var localized = _text[$"ExplicitError{code}"];
            TempData["Error"] = localized.ResourceNotFound
                ? code.ToString()
                : localized.Value;
        }

        return RedirectToAction(nameof(Index), routeValues);
    }

    private static object? CurriculumContextRouteValues(
        Guid? academicYearId,
        Guid? academicProgramId,
        Guid? curriculumAdoptionId)
    {
        if (!academicYearId.HasValue ||
            !academicProgramId.HasValue ||
            !curriculumAdoptionId.HasValue)
        {
            return null;
        }

        return new
        {
            academicYearId = academicYearId.Value,
            academicProgramId = academicProgramId.Value,
            curriculumAdoptionId = curriculumAdoptionId.Value
        };
    }

    private IExplicitCurriculumLevelService? ExplicitLevels =>
        HttpContext?.RequestServices.GetService<IExplicitCurriculumLevelService>();

    private IExplicitCurriculumContentUiQuery? ExplicitContentQuery =>
        HttpContext?.RequestServices.GetService<IExplicitCurriculumContentUiQuery>();

    private bool TryActor(out Guid id) =>
        Guid.TryParse(
            User.FindFirstValue(ClaimTypes.NameIdentifier),
            out id);

    private IActionResult HandleQueryError(
        CurriculumErrorCode? error) =>
        error == CurriculumErrorCode.AccessDenied
            ? Forbid()
            : NotFound();

    private void SetFeedback(
        CurriculumCommandResult result,
        string successKey)
    {
        if (result.Succeeded)
        {
            TempData["Success"] = _text[successKey].Value;
            return;
        }

        TempData["Error"] = _text[ErrorKey(result.Error)].Value;
    }

    private static string ErrorKey(CurriculumErrorCode? code) =>
        code switch
        {
            CurriculumErrorCode.AccessDenied => "ErrorAccessDenied",
            CurriculumErrorCode.SchoolNotActive => "ErrorSchoolNotActive",
            CurriculumErrorCode.Required => "ErrorRequired",
            CurriculumErrorCode.InvalidName => "ErrorInvalidName",
            CurriculumErrorCode.InvalidOrder => "ErrorInvalidOrder",
            CurriculumErrorCode.InvalidCode => "ErrorInvalidCode",
            CurriculumErrorCode.InvalidWeight => "ErrorInvalidWeight",
            CurriculumErrorCode.SubjectNotFound => "ErrorSubjectNotFound",
            CurriculumErrorCode.GradeLevelNotFound => "ErrorGradeLevelNotFound",
            CurriculumErrorCode.TopicNotFound => "ErrorTopicNotFound",
            CurriculumErrorCode.OutcomeNotFound => "ErrorOutcomeNotFound",
            CurriculumErrorCode.DuplicateTopicName => "ErrorDuplicateTopicName",
            CurriculumErrorCode.DuplicateTopicOrder => "ErrorDuplicateTopicOrder",
            CurriculumErrorCode.DuplicateOutcomeCode => "ErrorDuplicateOutcomeCode",
            CurriculumErrorCode.DuplicateOutcomeOrder => "ErrorDuplicateOutcomeOrder",
            CurriculumErrorCode.FrameworkNotFound => "ErrorFrameworkNotFound",
            CurriculumErrorCode.CurriculumNotSelected => "ErrorCurriculumNotSelected",
            CurriculumErrorCode.CurriculumFrameworkInUse => "ErrorCurriculumFrameworkInUse",
            CurriculumErrorCode.OfficialOutcomeNotFound => "ErrorOfficialOutcomeNotFound",
            CurriculumErrorCode.OfficialOutcomeReadOnly => "ErrorOfficialOutcomeReadOnly",
            CurriculumErrorCode.AcademicProgramNotFound => "ErrorAcademicProgramNotFound",
            _ => "ErrorPersistence"
        };

    private static (Guid ContentNodeId, Guid? LessonNodeId)?
        ParseOfficialSelection(string? value)
    {
        var parts = (value ?? string.Empty).Split('|');
        if (parts.Length is < 1 or > 2 ||
            !Guid.TryParse(parts[0], out var contentNodeId))
        {
            return null;
        }

        if (parts.Length == 1 || string.IsNullOrWhiteSpace(parts[1]))
            return (contentNodeId, null);

        return Guid.TryParse(parts[1], out var lessonNodeId)
            ? (contentNodeId, lessonNodeId)
            : null;
    }
}
