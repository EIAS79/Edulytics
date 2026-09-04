from pathlib import Path
from xml.sax.saxutils import escape


def replace(path: str, old: str, new: str) -> None:
    p = Path(path)
    text = p.read_text(encoding="utf-8")
    if old not in text:
        raise SystemExit(f"PATCH_PATTERN_MISSING: {path}: {old[:120]!r}")
    p.write_text(text.replace(old, new, 1), encoding="utf-8")


def append_resx(path: str, entries: list[tuple[str, str]]) -> None:
    p = Path(path)
    text = p.read_text(encoding="utf-8")
    additions: list[str] = []
    for key, value in entries:
        if f'name="{key}"' in text:
            continue
        additions.append(
            f'  <data name="{key}" xml:space="preserve"><value>{escape(value)}</value></data>'
        )
    if not additions:
        return
    if "</root>" not in text:
        raise SystemExit(f"INVALID_RESX: {path}")
    p.write_text(text.replace("</root>", "\n".join(additions) + "\n</root>"), encoding="utf-8")


# Fix the fully-qualified enum reference from the repository edit already on the branch.
replace(
    "src/Edulytics.Data/Repositories/AssessmentBuilderRepository.cs",
    "x.Status == Core.Enums.AcademicStructureStatus.Active)",
    "x.Status == Edulytics.Core.Enums.AcademicStructureStatus.Active)",
)

# Keep AssessmentBuilderService focused on question composition. Delivery settings use a separate service.
replace(
    "src/Edulytics.Services/Assessments/IAssessmentBuilderService.cs",
    "    Task<AssessmentCommandResult> UpdateDeliverySettingsAsync(Guid actorUserId, UpdateAssessmentDeliverySettingsRequest request, CancellationToken cancellationToken = default);\n",
    "",
)

Path("src/Edulytics.Services/Assessments/AssessmentDeliverySettingsService.cs").write_text(
    r'''using Edulytics.Core.Constants;
using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;

namespace Edulytics.Services.Assessments;

public sealed record AssessmentDeliverySettingsWorkspace(
    AssessmentListItem Assessment,
    IReadOnlyList<AssessmentTargetStudentOption> TargetStudents);

public interface IAssessmentDeliverySettingsService
{
    Task<AssessmentQueryResult<AssessmentDeliverySettingsWorkspace>> GetAsync(
        Guid actorUserId,
        Guid assessmentId,
        CancellationToken cancellationToken = default);

    Task<AssessmentCommandResult> UpdateAsync(
        Guid actorUserId,
        UpdateAssessmentDeliverySettingsRequest request,
        CancellationToken cancellationToken = default);
}

public sealed class AssessmentDeliverySettingsService(
    IAssessmentService assessments,
    IAssessmentBuilderRepository repository,
    ISchoolUserRepository users) : IAssessmentDeliverySettingsService
{
    public async Task<AssessmentQueryResult<AssessmentDeliverySettingsWorkspace>> GetAsync(
        Guid actorUserId,
        Guid assessmentId,
        CancellationToken cancellationToken = default)
    {
        var actor = await users.GetActorAsync(actorUserId, cancellationToken);
        if (actor is null || !actor.IsActive || actor.IsLocked || !actor.SchoolId.HasValue ||
            actor.Roles.Count != 1 || actor.Roles[0] != RoleNames.Teacher)
            return AssessmentQueryResult<AssessmentDeliverySettingsWorkspace>.Failure(
                AssessmentErrorCode.AccessDenied);

        var details = await assessments.GetDetailsAsync(actorUserId, assessmentId, cancellationToken);
        if (details.Value is null)
            return AssessmentQueryResult<AssessmentDeliverySettingsWorkspace>.Failure(
                details.Error ?? AssessmentErrorCode.AccessDenied);

        var students = await repository.ListTargetStudentsAsync(
            actor.SchoolId.Value, assessmentId, cancellationToken);

        return AssessmentQueryResult<AssessmentDeliverySettingsWorkspace>.Success(
            new AssessmentDeliverySettingsWorkspace(
                details.Value.Assessment,
                students
                    .Select(x => new AssessmentTargetStudentOption(x.Id, x.StudentNumber, x.DisplayName))
                    .ToArray()));
    }

    public async Task<AssessmentCommandResult> UpdateAsync(
        Guid actorUserId,
        UpdateAssessmentDeliverySettingsRequest request,
        CancellationToken cancellationToken = default)
    {
        var actor = await users.GetActorAsync(actorUserId, cancellationToken);
        if (actor is null || !actor.IsActive || actor.IsLocked || !actor.SchoolId.HasValue ||
            actor.Roles.Count != 1 || actor.Roles[0] != RoleNames.Teacher)
            return AssessmentCommandResult.Failure(string.Empty, AssessmentErrorCode.AccessDenied);

        var details = await assessments.GetDetailsAsync(actorUserId, request.AssessmentId, cancellationToken);
        if (details.Value is null)
            return AssessmentCommandResult.Failure(
                string.Empty, details.Error ?? AssessmentErrorCode.AccessDenied);
        if (details.Value.Assessment.Status != AssessmentStatus.Draft)
            return AssessmentCommandResult.Failure(string.Empty, AssessmentErrorCode.AssessmentNotDraft);

        if (!Enum.IsDefined(request.TargetType) ||
            !Enum.IsDefined(request.DeliveryMode) ||
            !Enum.IsDefined(request.DifficultyBand))
            return AssessmentCommandResult.Failure(string.Empty, AssessmentErrorCode.Required);

        var context = await repository.GetContextAsync(
            actor.SchoolId.Value, request.AssessmentId, cancellationToken);
        if (context is null)
            return AssessmentCommandResult.Failure(string.Empty, AssessmentErrorCode.AssessmentNotFound);

        Guid? targetStudentId = null;
        if (request.TargetType == AssessmentTargetType.Student)
        {
            if (!request.TargetStudentProfileId.HasValue || request.TargetStudentProfileId.Value == Guid.Empty)
                return AssessmentCommandResult.Failure(string.Empty, AssessmentErrorCode.StudentNotFound);

            var eligible = await repository.ListTargetStudentsAsync(
                actor.SchoolId.Value, request.AssessmentId, cancellationToken);
            if (eligible.All(x => x.Id != request.TargetStudentProfileId.Value))
                return AssessmentCommandResult.Failure(string.Empty, AssessmentErrorCode.StudentNotEnrolled);

            targetStudentId = request.TargetStudentProfileId.Value;
        }

        context.Assessment.TargetType = request.TargetType;
        context.Assessment.TargetStudentProfileId = targetStudentId;
        context.Assessment.DeliveryMode = request.DeliveryMode;
        context.Assessment.DifficultyBand = request.DifficultyBand;
        context.Assessment.UpdatedAtUtc = DateTime.UtcNow;

        var saved = await repository.SaveAsync(
            context.Assessment, request.AssessmentRowVersion, cancellationToken);

        return saved.Succeeded
            ? AssessmentCommandResult.Success(context.Assessment.Id)
            : AssessmentCommandResult.Failure(
                string.Empty,
                saved.Error == Core.Assessments.AssessmentPersistenceError.Conflict
                    ? AssessmentErrorCode.ConcurrencyConflict
                    : AssessmentErrorCode.PersistenceError);
    }
}
''',
    encoding="utf-8",
)

# Register both delivery-setting and official student-delivery services.
replace(
    "src/Edulytics.Web/Extensions/AssessmentRegistrationExtensions.cs",
    "        services.AddScoped<IAssessmentBuilderService, AssessmentBuilderService>();\n",
    "        services.AddScoped<IAssessmentBuilderService, AssessmentBuilderService>();\n"
    "        services.AddScoped<IAssessmentDeliverySettingsService, AssessmentDeliverySettingsService>();\n"
    "        services.AddScoped<IStudentAssessmentDeliveryService, StudentAssessmentDeliveryService>();\n",
)

# Builder controller: combine question workspace with target-student options and persist settings separately.
Path("src/Edulytics.Web/Controllers/AssessmentBuilderController.cs").write_text(
    r'''using System.Security.Claims;
using Edulytics.Core.Assessments;
using Edulytics.Core.Constants;
using Edulytics.Core.Enums;
using Edulytics.Services.Assessments;
using Edulytics.Web.Resilience;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Localization;

namespace Edulytics.Web.Controllers;

[Authorize(Roles = RoleNames.Teacher)]
[Route("school/assessments/{assessmentId:guid}/builder")]
public sealed class AssessmentBuilderController(
    IAssessmentBuilderService service,
    IAssessmentDeliverySettingsService deliverySettings,
    IStringLocalizer<AssessmentBuilderResource> text) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(Guid assessmentId, CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        var result = await service.GetWorkspaceAsync(actorId, assessmentId, cancellationToken);
        if (result.Value is null) return Handle(result.Error);

        var delivery = await deliverySettings.GetAsync(actorId, assessmentId, cancellationToken);
        if (delivery.Value is null) return Handle(delivery.Error);

        return View(result.Value with { TargetStudents = delivery.Value.TargetStudents });
    }

    [HttpPost("settings"), ValidateAntiForgeryToken]
    [RequestTimeout(BackendResiliencePolicyNames.InteractiveWrite)]
    [EnableRateLimiting(BackendResiliencePolicyNames.HeavyWriteConcurrency)]
    public async Task<IActionResult> UpdateDeliverySettings(
        Guid assessmentId,
        AssessmentTargetType targetType,
        Guid? targetStudentProfileId,
        AssessmentDeliveryMode deliveryMode,
        AssessmentDifficultyBand difficultyBand,
        string rowVersion,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        if (!TryDecode(rowVersion, out var version)) return ConcurrencyRedirect(assessmentId);

        var result = await deliverySettings.UpdateAsync(
            actorId,
            new UpdateAssessmentDeliverySettingsRequest(
                assessmentId,
                targetType,
                targetStudentProfileId,
                deliveryMode,
                difficultyBand,
                version),
            cancellationToken);

        Feedback(result, "DeliverySettingsSaved");
        return RedirectToAction(nameof(Index), new { assessmentId });
    }

    [HttpPost("manual"), ValidateAntiForgeryToken]
    [RequestTimeout(BackendResiliencePolicyNames.InteractiveWrite)]
    [EnableRateLimiting(BackendResiliencePolicyNames.HeavyWriteConcurrency)]
    public async Task<IActionResult> CreateManual(Guid assessmentId, string prompt, string correctAnswer, string solution,
        decimal maxScore, int order, AssessmentItemDifficulty difficulty, Guid[]? outcomeIds, string rowVersion, CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        if (!TryDecode(rowVersion, out var version)) return ConcurrencyRedirect(assessmentId);
        var result = await service.CreateManualQuestionAsync(actorId,
            new CreateManualBuilderQuestionRequest(assessmentId, prompt, correctAnswer, solution, maxScore, order, difficulty, outcomeIds ?? [], version), cancellationToken);
        Feedback(result, "SuccessQuestionCreated");
        return RedirectToAction(nameof(Index), new { assessmentId });
    }

    [HttpPost("questions/{questionId:guid}/edit"), ValidateAntiForgeryToken]
    [RequestTimeout(BackendResiliencePolicyNames.InteractiveWrite)]
    [EnableRateLimiting(BackendResiliencePolicyNames.HeavyWriteConcurrency)]
    public async Task<IActionResult> Edit(Guid assessmentId, Guid questionId, string prompt, string correctAnswer, string solution,
        decimal maxScore, AssessmentItemDifficulty difficulty, string rowVersion, CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        if (!TryDecode(rowVersion, out var version)) return ConcurrencyRedirect(assessmentId);
        var result = await service.EditQuestionAsync(actorId,
            new EditBuilderQuestionRequest(assessmentId, questionId, prompt, correctAnswer, solution, maxScore, difficulty, version), cancellationToken);
        Feedback(result, "BuilderQuestionUpdated");
        return RedirectToAction(nameof(Index), new { assessmentId });
    }

    [HttpPost("generate"), ValidateAntiForgeryToken]
    [RequestTimeout(BackendResiliencePolicyNames.InteractiveWrite)]
    [EnableRateLimiting(BackendResiliencePolicyNames.HeavyWriteConcurrency)]
    public async Task<IActionResult> Generate(Guid assessmentId, int questionCount, decimal maxScorePerQuestion,
        AssessmentBuilderDifficulty difficulty, Guid[]? outcomeIds, int seed, string rowVersion, CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        if (!TryDecode(rowVersion, out var version)) return ConcurrencyRedirect(assessmentId);
        var result = await service.GenerateQuestionsAsync(actorId,
            new GenerateBuilderQuestionsRequest(assessmentId, questionCount, maxScorePerQuestion, difficulty, outcomeIds ?? [], version, seed), cancellationToken);
        Feedback(result, "BuilderGenerated");
        return RedirectToAction(nameof(Index), new { assessmentId });
    }

    [HttpPost("questions/{questionId:guid}/regenerate"), ValidateAntiForgeryToken]
    [RequestTimeout(BackendResiliencePolicyNames.InteractiveWrite)]
    [EnableRateLimiting(BackendResiliencePolicyNames.HeavyWriteConcurrency)]
    public async Task<IActionResult> Regenerate(Guid assessmentId, Guid questionId, int seed, string rowVersion, CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        if (!TryDecode(rowVersion, out var version)) return ConcurrencyRedirect(assessmentId);
        var result = await service.RegenerateQuestionAsync(actorId, assessmentId, questionId, seed, version, cancellationToken);
        Feedback(result, "BuilderQuestionRegenerated");
        return RedirectToAction(nameof(Index), new { assessmentId });
    }

    [HttpPost("questions/{questionId:guid}/approve"), ValidateAntiForgeryToken]
    [RequestTimeout(BackendResiliencePolicyNames.InteractiveWrite)]
    [EnableRateLimiting(BackendResiliencePolicyNames.HeavyWriteConcurrency)]
    public async Task<IActionResult> Approve(Guid assessmentId, Guid questionId, string rowVersion, CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        if (!TryDecode(rowVersion, out var version)) return ConcurrencyRedirect(assessmentId);
        var result = await service.ApproveQuestionAsync(actorId, assessmentId, questionId, version, cancellationToken);
        Feedback(result, "BuilderQuestionApproved");
        return RedirectToAction(nameof(Index), new { assessmentId });
    }

    [HttpPost("questions/{questionId:guid}/delete"), ValidateAntiForgeryToken]
    [RequestTimeout(BackendResiliencePolicyNames.InteractiveWrite)]
    [EnableRateLimiting(BackendResiliencePolicyNames.HeavyWriteConcurrency)]
    public async Task<IActionResult> Delete(Guid assessmentId, Guid questionId, string rowVersion, CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        if (!TryDecode(rowVersion, out var version)) return ConcurrencyRedirect(assessmentId);
        var result = await service.DeleteQuestionAsync(actorId, assessmentId, questionId, version, cancellationToken);
        Feedback(result, "SuccessQuestionDeleted");
        return RedirectToAction(nameof(Index), new { assessmentId });
    }

    [HttpPost("publish"), ValidateAntiForgeryToken]
    [RequestTimeout(BackendResiliencePolicyNames.InteractiveWrite)]
    [EnableRateLimiting(BackendResiliencePolicyNames.HeavyWriteConcurrency)]
    public async Task<IActionResult> Publish(Guid assessmentId, string rowVersion, CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        if (!TryDecode(rowVersion, out var version)) return ConcurrencyRedirect(assessmentId);
        var result = await service.PublishAsync(actorId, assessmentId, version, cancellationToken);
        Feedback(result, "SuccessAssessmentOpened");
        return result.Succeeded
            ? RedirectToAction("Details", "Assessments", new { id = assessmentId })
            : RedirectToAction(nameof(Index), new { assessmentId });
    }

    private bool TryActor(out Guid id) => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out id);
    private IActionResult Handle(AssessmentErrorCode? error) => error == AssessmentErrorCode.AccessDenied ? Forbid() : NotFound();
    private IActionResult ConcurrencyRedirect(Guid assessmentId)
    {
        TempData["Error"] = text["ErrorConcurrencyConflict"].Value;
        return RedirectToAction(nameof(Index), new { assessmentId });
    }
    private void Feedback(AssessmentCommandResult result, string successKey)
    {
        TempData[result.Succeeded ? "Success" : "Error"] = result.Succeeded
            ? text[successKey].Value
            : text["BuilderOperationFailed", result.Error?.ToString() ?? "Unknown"].Value;
    }
    private static bool TryDecode(string? value, out byte[] bytes)
    {
        bytes = [];
        if (string.IsNullOrWhiteSpace(value)) return false;
        try { bytes = Convert.FromBase64String(value); return bytes.Length > 0; }
        catch (FormatException) { return false; }
    }
}
''',
    encoding="utf-8",
)

# Builder view rewritten cleanly with delivery settings first.
Path("src/Edulytics.Web/Views/AssessmentBuilder/Index.cshtml").write_text(
    r'''@using Edulytics.Core.Assessments
@using Edulytics.Core.Enums
@model Edulytics.Services.Assessments.AssessmentBuilderWorkspace
@inject Microsoft.Extensions.Localization.IStringLocalizer<Edulytics.Web.AssessmentBuilderResource> A
@{
    ViewData["Title"] = A["AssessmentBuilder"];
    var assessment = Model.Details.Assessment;
    var rowVersion = Convert.ToBase64String(assessment.RowVersion);
    var targetStudents = Model.TargetStudents ?? Array.Empty<Edulytics.Services.Assessments.AssessmentTargetStudentOption>();
}
<section class="assessment-page">
    <header class="assessment-header">
        <div>
            <span class="assessment-context">@A["AssessmentBuilder"]</span>
            <h1>@assessment.Title</h1>
            <p>
                @A["Questions"]: @Model.Questions.Count ·
                @A["CurrentMarks"]: @Model.CurrentMarks.ToString("0.##") / @assessment.MaxScore.ToString("0.##")
                @if (Model.ClassMasteryPercentage.HasValue)
                {
                    <span> · @A["ClassMastery"]: @Model.ClassMasteryPercentage.Value.ToString("0.#")%</span>
                }
            </p>
        </div>
        <div class="assessment-actions">
            <a class="school-button" asp-controller="Assessments" asp-action="Details" asp-route-id="@assessment.Id">@A["Assessment"]</a>
        </div>
    </header>

    @if (TempData["Success"] is string success)
    {
        <div class="school-alert school-alert-success" role="status">@success</div>
    }
    @if (TempData["Error"] is string error)
    {
        <div class="school-alert school-alert-error" role="alert">@error</div>
    }

    <div class="assessment-info">@A[Model.ReadinessMessage]</div>

    @if (assessment.Status == AssessmentStatus.Draft)
    {
        <section class="assessment-card">
            <h2>@A["DeliverySettings"]</h2>
            <p>@A["DeliverySettingsHelp"]</p>
            <form asp-action="UpdateDeliverySettings" asp-route-assessmentId="@assessment.Id" method="post" class="assessment-form">
                @Html.AntiForgeryToken()
                <input type="hidden" name="rowVersion" value="@rowVersion" />

                <label>@A["AssessmentTarget"]</label>
                <select name="targetType">
                    <option value="@AssessmentTargetType.Class" selected="@(assessment.TargetType == AssessmentTargetType.Class)">@A["TargetClass"]</option>
                    <option value="@AssessmentTargetType.Student" selected="@(assessment.TargetType == AssessmentTargetType.Student)">@A["TargetIndividualStudent"]</option>
                </select>

                <label>@A["TargetStudent"]</label>
                <select name="targetStudentProfileId">
                    <option value="">@A["SelectStudentWhenIndividual"]</option>
                    @foreach (var student in targetStudents)
                    {
                        <option value="@student.Id" selected="@(assessment.TargetStudentProfileId == student.Id)">@student.DisplayName (@student.StudentNumber)</option>
                    }
                </select>

                <label>@A["DeliveryMode"]</label>
                <select name="deliveryMode">
                    <option value="@AssessmentDeliveryMode.Online" selected="@(assessment.DeliveryMode == AssessmentDeliveryMode.Online)">@A["DeliveryOnline"]</option>
                    <option value="@AssessmentDeliveryMode.Offline" selected="@(assessment.DeliveryMode == AssessmentDeliveryMode.Offline)">@A["DeliveryOffline"]</option>
                </select>

                <label>@A["AssessmentDifficulty"]</label>
                <select name="difficultyBand">
                    <option value="@AssessmentDifficultyBand.AtClassLevel" selected="@(assessment.DifficultyBand == AssessmentDifficultyBand.AtClassLevel)">@A["AtClassLevel"]</option>
                    <option value="@AssessmentDifficultyBand.Stretch" selected="@(assessment.DifficultyBand == AssessmentDifficultyBand.Stretch)">@A["Stretch"]</option>
                    <option value="@AssessmentDifficultyBand.Challenge" selected="@(assessment.DifficultyBand == AssessmentDifficultyBand.Challenge)">@A["Challenge"]</option>
                </select>

                <button class="school-button school-button-primary" type="submit">@A["SaveDeliverySettings"]</button>
            </form>
        </section>

        <section class="assessment-card">
            <h2>@A["AddQuestionManually"]</h2>
            <form asp-action="CreateManual" asp-route-assessmentId="@assessment.Id" method="post" class="assessment-form">
                @Html.AntiForgeryToken()
                <input type="hidden" name="rowVersion" value="@rowVersion" />
                <label>@A["QuestionPrompt"]</label>
                <textarea name="prompt" maxlength="1000" rows="3" required></textarea>
                <label>@A["CorrectAnswer"]</label>
                <input name="correctAnswer" maxlength="1000" required />
                <label>@A["Solution"]</label>
                <textarea name="solution" maxlength="4000" rows="3" required></textarea>
                <label>@A["QuestionMaxScore"]</label>
                <input name="maxScore" type="number" min="0.01" max="10000" step="0.01" required />
                <label>@A["Order"]</label>
                <input name="order" type="number" min="1" value="@(Model.Questions.Count + 1)" required />
                <label>@A["Difficulty"]</label>
                <select name="difficulty">
                    <option value="@AssessmentItemDifficulty.Easy">@A["DifficultyEasy"]</option>
                    <option value="@AssessmentItemDifficulty.Medium" selected>@A["DifficultyMedium"]</option>
                    <option value="@AssessmentItemDifficulty.Challenging">@A["DifficultyChallenging"]</option>
                </select>
                <fieldset>
                    <legend>@A["LearningOutcomes"]</legend>
                    @foreach (var outcome in Model.Details.EligibleOutcomes)
                    {
                        <label><input type="checkbox" name="outcomeIds" value="@outcome.Id" /> <strong>@outcome.Code</strong> — @outcome.Description</label>
                    }
                </fieldset>
                <button class="school-button school-button-primary" type="submit">@A["AddQuestionManually"]</button>
            </form>
        </section>

        <section class="assessment-card">
            <h2>@A["GenerateQuestionsWithAI"]</h2>
            <p>@A["NativeGenerationNotice"]</p>
            @if (Model.CanGenerateNatively)
            {
                <form asp-action="Generate" asp-route-assessmentId="@assessment.Id" method="post" class="assessment-form">
                    @Html.AntiForgeryToken()
                    <input type="hidden" name="rowVersion" value="@rowVersion" />
                    <label>@A["PlannedQuestionCount"]</label>
                    <input name="questionCount" type="number" min="1" max="100" value="5" required />
                    <label>@A["QuestionMaxScore"]</label>
                    <input name="maxScorePerQuestion" type="number" min="0.01" step="0.01" value="1" required />
                    <label>@A["AssessmentDifficulty"]</label>
                    <select name="difficulty">
                        <option value="@AssessmentBuilderDifficulty.AtClassLevel">@A["AtClassLevel"]</option>
                        <option value="@AssessmentBuilderDifficulty.Stretch">@A["Stretch"]</option>
                        <option value="@AssessmentBuilderDifficulty.Challenge">@A["Challenge"]</option>
                    </select>
                    <input type="hidden" name="seed" value="0" />
                    <fieldset>
                        <legend>@A["LearningOutcomes"]</legend>
                        @foreach (var outcome in Model.Details.EligibleOutcomes)
                        {
                            <label><input type="checkbox" name="outcomeIds" value="@outcome.Id" /> <strong>@outcome.Code</strong> — @outcome.Description</label>
                        }
                    </fieldset>
                    <button class="school-button school-button-primary" type="submit">@A["GenerateQuestionsWithAI"]</button>
                </form>
            }
            else
            {
                <div class="assessment-empty">@A["NativeGenerationUnavailable"]</div>
            }
        </section>
    }

    <section class="assessment-question-list">
        <h2>@A["Questions"]</h2>
        @if (Model.Questions.Count == 0)
        {
            <div class="assessment-empty">@A["NoQuestions"]</div>
        }
        @foreach (var question in Model.Questions)
        {
            <article class="assessment-question-card">
                <header>
                    <div>
                        <h3>@question.Order. @question.Prompt</h3>
                        <p>@A["QuestionMaxScore"]: @question.MaxScore.ToString("0.##") · @A["Source"]: @(question.Source?.ToString() ?? A["Legacy"].Value) · @A["Difficulty"]: @(question.Difficulty?.ToString() ?? A["Legacy"].Value) · @A["Status"]: @A[$"BuilderStatus{question.Status}"]</p>
                    </div>
                </header>
                @if (!string.IsNullOrWhiteSpace(question.CorrectAnswer))
                {
                    <p><strong>@A["CorrectAnswer"]:</strong> @question.CorrectAnswer</p>
                    <p><strong>@A["Solution"]:</strong> @question.Solution</p>
                }
                <div class="assessment-mapping-tags">
                    @foreach (var outcomeId in question.OutcomeIds)
                    {
                        var outcome = Model.Details.EligibleOutcomes.FirstOrDefault(x => x.Id == outcomeId);
                        if (outcome is not null)
                        {
                            <span class="assessment-mapping-tag"><strong>@outcome.Code</strong> — @outcome.Description</span>
                        }
                    }
                </div>
                @if (assessment.Status == AssessmentStatus.Draft && question.Status != AssessmentBuilderQuestionStatus.Legacy)
                {
                    <details>
                        <summary>@A["EditQuestion"]</summary>
                        <form asp-action="Edit" asp-route-assessmentId="@assessment.Id" asp-route-questionId="@question.Id" method="post" class="assessment-form">
                            @Html.AntiForgeryToken()
                            <input type="hidden" name="rowVersion" value="@rowVersion" />
                            <label>@A["QuestionPrompt"]</label>
                            <textarea name="prompt" maxlength="1000" rows="3" required>@question.Prompt</textarea>
                            <label>@A["CorrectAnswer"]</label>
                            <input name="correctAnswer" maxlength="1000" value="@question.CorrectAnswer" required />
                            <label>@A["Solution"]</label>
                            <textarea name="solution" maxlength="4000" rows="3" required>@question.Solution</textarea>
                            <label>@A["QuestionMaxScore"]</label>
                            <input name="maxScore" type="number" min="0.01" step="0.01" value="@question.MaxScore" required />
                            <label>@A["Difficulty"]</label>
                            <select name="difficulty">
                                <option value="@AssessmentItemDifficulty.Easy">@A["DifficultyEasy"]</option>
                                <option value="@AssessmentItemDifficulty.Medium">@A["DifficultyMedium"]</option>
                                <option value="@AssessmentItemDifficulty.Challenging">@A["DifficultyChallenging"]</option>
                            </select>
                            <button class="school-button" type="submit">@A["EditQuestion"]</button>
                        </form>
                    </details>
                    <div class="assessment-actions">
                        @if (question.Status != AssessmentBuilderQuestionStatus.Approved)
                        {
                            <form asp-action="Approve" asp-route-assessmentId="@assessment.Id" asp-route-questionId="@question.Id" method="post">
                                @Html.AntiForgeryToken()<input type="hidden" name="rowVersion" value="@rowVersion" />
                                <button class="school-button school-button-primary" type="submit">@A["ApproveQuestion"]</button>
                            </form>
                        }
                        @if (question.Source == AssessmentItemSource.SystemGenerated)
                        {
                            <form asp-action="Regenerate" asp-route-assessmentId="@assessment.Id" asp-route-questionId="@question.Id" method="post">
                                @Html.AntiForgeryToken()<input type="hidden" name="rowVersion" value="@rowVersion" /><input type="hidden" name="seed" value="@(question.Order + 1)" />
                                <button class="school-button" type="submit">@A["RegenerateQuestion"]</button>
                            </form>
                        }
                        <form asp-action="Delete" asp-route-assessmentId="@assessment.Id" asp-route-questionId="@question.Id" method="post">
                            @Html.AntiForgeryToken()<input type="hidden" name="rowVersion" value="@rowVersion" />
                            <button class="school-button" type="submit">@A["DeleteQuestion"]</button>
                        </form>
                    </div>
                }
            </article>
        }
    </section>

    @if (assessment.Status == AssessmentStatus.Draft)
    {
        <section class="assessment-lifecycle">
            @if (Model.ReadyToPublish)
            {
                <form asp-action="Publish" asp-route-assessmentId="@assessment.Id" method="post">
                    @Html.AntiForgeryToken()<input type="hidden" name="rowVersion" value="@rowVersion" />
                    <button class="school-button school-button-primary" type="submit">@A["PublishAssessment"]</button>
                </form>
            }
            else
            {
                <button class="school-button" type="button" disabled>@A["PublishAssessment"]</button>
            }
        </section>
    }
</section>
''',
    encoding="utf-8",
)

append_resx(
    "src/Edulytics.Web/Resources/AssessmentBuilderResource.resx",
    [
        ("DeliverySettings", "Assessment delivery"),
        ("DeliverySettingsHelp", "Choose who receives this assessment, how it is delivered, and its intended difficulty before publishing."),
        ("AssessmentTarget", "Assessment target"),
        ("TargetClass", "Class"),
        ("TargetIndividualStudent", "Individual student"),
        ("TargetStudent", "Student"),
        ("SelectStudentWhenIndividual", "Select a student when the target is Individual student"),
        ("DeliveryMode", "Delivery mode"),
        ("DeliveryOnline", "Online assessment — student answers and submits in Edulytics"),
        ("DeliveryOffline", "Offline / manual assessment — teacher records or imports results"),
        ("SaveDeliverySettings", "Save delivery settings"),
        ("DeliverySettingsSaved", "Assessment delivery settings saved."),
    ],
)
append_resx(
    "src/Edulytics.Web/Resources/AssessmentBuilderResource.pl.resx",
    [
        ("DeliverySettings", "Sposób przeprowadzenia sprawdzianu"),
        ("DeliverySettingsHelp", "Przed publikacją wybierz odbiorców sprawdzianu, sposób jego przeprowadzenia oraz zamierzony poziom trudności."),
        ("AssessmentTarget", "Odbiorca sprawdzianu"),
        ("TargetClass", "Klasa"),
        ("TargetIndividualStudent", "Pojedynczy uczeń"),
        ("TargetStudent", "Uczeń"),
        ("SelectStudentWhenIndividual", "Wybierz ucznia, gdy odbiorcą jest pojedynczy uczeń"),
        ("DeliveryMode", "Sposób przeprowadzenia"),
        ("DeliveryOnline", "Online — uczeń odpowiada i wysyła sprawdzian w Edulityks"),
        ("DeliveryOffline", "Offline / ręcznie — nauczyciel wprowadza lub importuje wyniki"),
        ("SaveDeliverySettings", "Zapisz ustawienia"),
        ("DeliverySettingsSaved", "Zapisano ustawienia sposobu przeprowadzenia sprawdzianu."),
    ],
)

# Teacher results and manual entry honor Student targeting.
replace(
    "src/Edulytics.Services/Assessments/AssessmentService.cs",
    '''        var studentIds = snapshot.StudentEnrollments
            .Where(x => x.AcademicYearId == assessment.AcademicYearId &&
                        x.ClassGroupId == assessment.ClassGroupId)
            .Select(x => x.StudentProfileId)
            .ToHashSet();

        var students = snapshot.StudentProfiles''',
    '''        var studentIds = snapshot.StudentEnrollments
            .Where(x => x.AcademicYearId == assessment.AcademicYearId &&
                        x.ClassGroupId == assessment.ClassGroupId)
            .Select(x => x.StudentProfileId)
            .ToHashSet();

        if (assessment.TargetType == AssessmentTargetType.Student)
        {
            if (assessment.TargetStudentProfileId.HasValue)
                studentIds.RemoveWhere(x => x != assessment.TargetStudentProfileId.Value);
            else
                studentIds.Clear();
        }

        var students = snapshot.StudentProfiles''',
)

replace(
    "src/Edulytics.Services/Assessments/AssessmentService.Commands.cs",
    '''        if (!await _repo.IsStudentEnrolledAsync(
                schoolId,
                assessment.AcademicYearId,
                assessment.ClassGroupId,
                student.Id,
                cancellationToken))
            return Fail(AssessmentErrorCode.StudentNotEnrolled);

        var snapshot = await _repo.GetSnapshotAsync(schoolId, cancellationToken);''',
    '''        if (!await _repo.IsStudentEnrolledAsync(
                schoolId,
                assessment.AcademicYearId,
                assessment.ClassGroupId,
                student.Id,
                cancellationToken))
            return Fail(AssessmentErrorCode.StudentNotEnrolled);

        if (assessment.TargetType == AssessmentTargetType.Student &&
            assessment.TargetStudentProfileId != student.Id)
            return Fail(AssessmentErrorCode.StudentNotEnrolled);

        var snapshot = await _repo.GetSnapshotAsync(schoolId, cancellationToken);''',
)

replace(
    "src/Edulytics.Services/Assessments/AssessmentService.Commands.cs",
    '''        if (!mappedOutcomeIds.SetEquals(eligibleMappedOutcomeIds))
            return Fail(AssessmentErrorCode.OutcomeDoesNotMatchAssessment);

        var previousStatus =''',
    '''        if (!mappedOutcomeIds.SetEquals(eligibleMappedOutcomeIds))
            return Fail(AssessmentErrorCode.OutcomeDoesNotMatchAssessment);

        if (assessment.TargetType == AssessmentTargetType.Student)
        {
            if (!assessment.TargetStudentProfileId.HasValue)
                return Fail(AssessmentErrorCode.StudentNotFound);

            var targetStudent = await _repo.GetStudentProfileAsync(
                scope.School.Id, assessment.TargetStudentProfileId.Value, cancellationToken);
            if (targetStudent is null || targetStudent.IsArchived || targetStudent.Status != AcademicStructureStatus.Active)
                return Fail(AssessmentErrorCode.StudentNotFound);

            if (!await _repo.IsStudentEnrolledAsync(
                    scope.School.Id,
                    assessment.AcademicYearId,
                    assessment.ClassGroupId,
                    targetStudent.Id,
                    cancellationToken))
                return Fail(AssessmentErrorCode.StudentNotEnrolled);
        }

        var previousStatus =''',
)

# Student portal contracts expose delivery mode, difficulty, target and submission state.
replace(
    "src/Edulytics.Services/StudentPortal/StudentPortalContracts.cs",
    "namespace Edulytics.Services.StudentPortal;\n",
    "using Edulytics.Core.Enums;\n\nnamespace Edulytics.Services.StudentPortal;\n",
)
replace(
    "src/Edulytics.Services/StudentPortal/StudentPortalContracts.cs",
    '''public sealed record StudentAssessmentItem(
    Guid AssessmentId,
    string Title,
    string SubjectName,
    string ClassName,
    DateOnly AssessmentDate,
    decimal MaxScore);''',
    '''public sealed record StudentAssessmentItem(
    Guid AssessmentId,
    string Title,
    string SubjectName,
    string ClassName,
    DateOnly AssessmentDate,
    decimal MaxScore,
    AssessmentDeliveryMode DeliveryMode = AssessmentDeliveryMode.Offline,
    AssessmentDifficultyBand DifficultyBand = AssessmentDifficultyBand.AtClassLevel,
    AssessmentTargetType TargetType = AssessmentTargetType.Class,
    bool IsSubmitted = false);''',
)

replace(
    "src/Edulytics.Services/StudentPortal/StudentPortalService.cs",
    '''            .Where(x =>
                x.Status == AssessmentStatus.Open &&
                enrollmentKeys.Contains((x.ClassGroupId, x.AcademicYearId)))''',
    '''            .Where(x =>
                x.Status == AssessmentStatus.Open &&
                enrollmentKeys.Contains((x.ClassGroupId, x.AcademicYearId)) &&
                (x.TargetType == AssessmentTargetType.Class ||
                 x.TargetStudentProfileId == snapshot.Profile.Id))''',
)
replace(
    "src/Edulytics.Services/StudentPortal/StudentPortalService.cs",
    '''                return new StudentAssessmentItem(
                    x.Id,
                    x.Title,
                    subject?.Name ?? string.Empty,
                    classGroup?.Name ?? string.Empty,
                    x.AssessmentDate,
                    x.MaxScore);''',
    '''                return new StudentAssessmentItem(
                    x.Id,
                    x.Title,
                    subject?.Name ?? string.Empty,
                    classGroup?.Name ?? string.Empty,
                    x.AssessmentDate,
                    x.MaxScore,
                    x.DeliveryMode,
                    x.DifficultyBand,
                    x.TargetType,
                    snapshot.Results.Any(result =>
                        result.AssessmentId == x.Id &&
                        result.StudentProfileId == snapshot.Profile.Id));''',
)

# Official online assessment delivery uses the same AssessmentResult / StudentAnswer / outbox pipeline.
Path("src/Edulytics.Services/Assessments/StudentAssessmentDeliveryContracts.cs").write_text(
    r'''using Edulytics.Core.Enums;

namespace Edulytics.Services.Assessments;

public enum StudentAssessmentDeliveryErrorCode
{
    AccessDenied = 1,
    SchoolNotActive = 2,
    ProfileNotLinked = 3,
    AssessmentNotFound = 4,
    AssessmentNotOpen = 5,
    AssessmentOffline = 6,
    NotTargeted = 7,
    AlreadySubmitted = 8,
    InvalidSubmission = 9,
    PersistenceError = 10
}

public sealed record StudentAssessmentQuestion(Guid Id, int Order, string Prompt, decimal MaxScore);

public sealed record StudentAssessmentAttempt(
    Guid AssessmentId,
    string Title,
    DateOnly AssessmentDate,
    decimal MaxScore,
    AssessmentDifficultyBand DifficultyBand,
    IReadOnlyList<StudentAssessmentQuestion> Questions);

public sealed record StudentAssessmentResponse(Guid QuestionId, string ResponseText);

public sealed record StudentAssessmentSubmission(
    Guid AssessmentId,
    string Title,
    decimal Score,
    decimal MaxScore,
    decimal Percentage,
    DateTime SubmittedAtUtc);

public sealed record StudentAssessmentDeliveryResult<T>(T? Value, StudentAssessmentDeliveryErrorCode? Error)
    where T : class
{
    public static StudentAssessmentDeliveryResult<T> Success(T value) => new(value, null);
    public static StudentAssessmentDeliveryResult<T> Failure(StudentAssessmentDeliveryErrorCode error) => new(null, error);
}

public interface IStudentAssessmentDeliveryService
{
    Task<StudentAssessmentDeliveryResult<StudentAssessmentAttempt>> GetAttemptAsync(
        Guid actorUserId,
        Guid assessmentId,
        CancellationToken cancellationToken = default);

    Task<StudentAssessmentDeliveryResult<StudentAssessmentSubmission>> SubmitAsync(
        Guid actorUserId,
        Guid assessmentId,
        IReadOnlyList<StudentAssessmentResponse> responses,
        CancellationToken cancellationToken = default);
}
''',
    encoding="utf-8",
)

Path("src/Edulytics.Services/Assessments/StudentAssessmentDeliveryService.cs").write_text(
    r'''using System.Globalization;
using System.Text.Json;
using Edulytics.Core.Assessments;
using Edulytics.Core.Constants;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;
using Edulytics.Core.Realtime;
using Edulytics.Services.Auditing;

namespace Edulytics.Services.Assessments;

public sealed class StudentAssessmentDeliveryService(
    IAssessmentRepository assessments,
    IAssessmentBuilderRepository builder,
    ISchoolUserRepository users,
    ISchoolRepository schools,
    IAuditService? audit = null) : IStudentAssessmentDeliveryService
{
    public async Task<StudentAssessmentDeliveryResult<StudentAssessmentAttempt>> GetAttemptAsync(
        Guid actorUserId,
        Guid assessmentId,
        CancellationToken cancellationToken = default)
    {
        var resolved = await ResolveAsync(actorUserId, assessmentId, cancellationToken);
        if (resolved.Error.HasValue)
            return StudentAssessmentDeliveryResult<StudentAssessmentAttempt>.Failure(resolved.Error.Value);

        if (resolved.Snapshot!.Results.Any(x =>
                x.AssessmentId == assessmentId &&
                x.StudentProfileId == resolved.Profile!.Id))
            return StudentAssessmentDeliveryResult<StudentAssessmentAttempt>.Failure(
                StudentAssessmentDeliveryErrorCode.AlreadySubmitted);

        var context = await builder.GetContextAsync(resolved.SchoolId, assessmentId, cancellationToken);
        if (context is null)
            return StudentAssessmentDeliveryResult<StudentAssessmentAttempt>.Failure(
                StudentAssessmentDeliveryErrorCode.AssessmentNotFound);

        var itemIds = context.Items.Select(x => x.Id).ToHashSet();
        if (context.Questions.Count == 0 || context.Questions.Any(x => !itemIds.Contains(x.Id)))
            return StudentAssessmentDeliveryResult<StudentAssessmentAttempt>.Failure(
                StudentAssessmentDeliveryErrorCode.AssessmentNotFound);

        return StudentAssessmentDeliveryResult<StudentAssessmentAttempt>.Success(
            new StudentAssessmentAttempt(
                context.Assessment.Id,
                context.Assessment.Title,
                context.Assessment.AssessmentDate,
                context.Assessment.MaxScore,
                context.Assessment.DifficultyBand,
                context.Questions
                    .OrderBy(x => x.Order)
                    .Select(x => new StudentAssessmentQuestion(x.Id, x.Order, x.Prompt, x.MaxScore))
                    .ToArray()));
    }

    public async Task<StudentAssessmentDeliveryResult<StudentAssessmentSubmission>> SubmitAsync(
        Guid actorUserId,
        Guid assessmentId,
        IReadOnlyList<StudentAssessmentResponse> responses,
        CancellationToken cancellationToken = default)
    {
        var resolved = await ResolveAsync(actorUserId, assessmentId, cancellationToken);
        if (resolved.Error.HasValue)
            return StudentAssessmentDeliveryResult<StudentAssessmentSubmission>.Failure(resolved.Error.Value);

        if (resolved.Snapshot!.Results.Any(x =>
                x.AssessmentId == assessmentId &&
                x.StudentProfileId == resolved.Profile!.Id))
            return StudentAssessmentDeliveryResult<StudentAssessmentSubmission>.Failure(
                StudentAssessmentDeliveryErrorCode.AlreadySubmitted);

        var context = await builder.GetContextAsync(resolved.SchoolId, assessmentId, cancellationToken);
        if (context is null)
            return StudentAssessmentDeliveryResult<StudentAssessmentSubmission>.Failure(
                StudentAssessmentDeliveryErrorCode.AssessmentNotFound);

        var questions = context.Questions.OrderBy(x => x.Order).ToArray();
        if (questions.Length == 0 ||
            responses.Count != questions.Length ||
            responses.Select(x => x.QuestionId).Distinct().Count() != questions.Length)
            return StudentAssessmentDeliveryResult<StudentAssessmentSubmission>.Failure(
                StudentAssessmentDeliveryErrorCode.InvalidSubmission);

        var responseMap = responses.ToDictionary(x => x.QuestionId);
        var itemMap = context.Items.ToDictionary(x => x.Id);
        if (questions.Any(x => !responseMap.ContainsKey(x.Id) || !itemMap.ContainsKey(x.Id)))
            return StudentAssessmentDeliveryResult<StudentAssessmentSubmission>.Failure(
                StudentAssessmentDeliveryErrorCode.InvalidSubmission);

        decimal score = 0m;
        var scored = new List<(AssessmentQuestion Question, string Response, decimal Score)>();
        foreach (var question in questions)
        {
            var response = (responseMap[question.Id].ResponseText ?? string.Empty).Trim();
            if (response.Length > 4000)
                return StudentAssessmentDeliveryResult<StudentAssessmentSubmission>.Failure(
                    StudentAssessmentDeliveryErrorCode.InvalidSubmission);

            var earned = AnswersEquivalent(response, itemMap[question.Id].CorrectAnswer)
                ? question.MaxScore
                : 0m;
            earned = Round(earned);
            score += earned;
            scored.Add((question, response, earned));
        }

        score = Round(score);
        var percentage = decimal.Round(
            score / context.Assessment.MaxScore * 100m,
            2,
            MidpointRounding.AwayFromZero);
        var now = DateTime.UtcNow;

        var result = new AssessmentResult
        {
            Id = Guid.NewGuid(),
            SchoolId = resolved.SchoolId,
            AssessmentId = assessmentId,
            StudentProfileId = resolved.Profile!.Id,
            Score = score,
            Percentage = percentage,
            EnteredByUserId = actorUserId,
            EnteredAtUtc = now,
            UpdatedAtUtc = now
        };
        await assessments.AddAsync(result, cancellationToken);

        foreach (var row in scored)
        {
            await assessments.AddAsync(
                new StudentAnswer
                {
                    Id = Guid.NewGuid(),
                    SchoolId = resolved.SchoolId,
                    AssessmentResultId = result.Id,
                    AssessmentQuestionId = row.Question.Id,
                    ResponseText = row.Response,
                    Score = row.Score,
                    UpdatedAtUtc = now
                },
                cancellationToken);
        }

        var eventId = Guid.NewGuid();
        var changed = new AssessmentResultChangedEvent(
            eventId,
            resolved.SchoolId,
            assessmentId,
            result.Id,
            context.Assessment.ClassGroupId,
            context.Assessment.SubjectId,
            resolved.Profile.Id,
            now);

        await assessments.AddOutboxAsync(
            new OutboxMessage
            {
                Id = eventId,
                SchoolId = resolved.SchoolId,
                EventType = RealtimeEventTypes.AssessmentResultEntered,
                PayloadJson = JsonSerializer.Serialize(changed),
                OccurredAtUtc = now,
                AvailableAtUtc = now,
                ProcessingAttempts = 0,
                CorrelationId = $"assessment-result:{eventId:N}"
            },
            cancellationToken);

        if (audit is not null)
        {
            await audit.QueueAsync(
                new AuditEvent(
                    SchoolId: resolved.SchoolId,
                    Action: "StudentAssessment.Submitted",
                    EntityType: "AssessmentResult",
                    EntityId: result.Id.ToString("D"),
                    Feature: "Assessments",
                    NewValues: new Dictionary<string, object?>
                    {
                        ["assessmentId"] = assessmentId,
                        ["studentProfileId"] = resolved.Profile.Id,
                        ["answerCount"] = scored.Count
                    },
                    ResultSummary: "Online assessment submitted by student.",
                    ActorUserIdOverride: actorUserId,
                    ActorRoleOverride: RoleNames.Student),
                cancellationToken);
        }

        var saved = await assessments.SaveAsync(cancellationToken);
        if (!saved.Succeeded)
            return StudentAssessmentDeliveryResult<StudentAssessmentSubmission>.Failure(
                StudentAssessmentDeliveryErrorCode.PersistenceError);

        return StudentAssessmentDeliveryResult<StudentAssessmentSubmission>.Success(
            new StudentAssessmentSubmission(
                assessmentId,
                context.Assessment.Title,
                score,
                context.Assessment.MaxScore,
                percentage,
                now));
    }

    private async Task<ResolvedDelivery> ResolveAsync(
        Guid actorUserId,
        Guid assessmentId,
        CancellationToken cancellationToken)
    {
        var actor = await users.GetActorAsync(actorUserId, cancellationToken);
        if (actor is null || !actor.IsActive || actor.IsLocked || !actor.SchoolId.HasValue ||
            actor.Roles.Count != 1 || actor.Roles[0] != RoleNames.Student)
            return ResolvedDelivery.Fail(StudentAssessmentDeliveryErrorCode.AccessDenied);

        var school = await schools.GetByIdAsync(actor.SchoolId.Value, cancellationToken);
        if (school is null || school.Status != SchoolStatus.Active)
            return ResolvedDelivery.Fail(StudentAssessmentDeliveryErrorCode.SchoolNotActive);

        var snapshot = await assessments.GetSnapshotAsync(school.Id, cancellationToken);
        var profile = snapshot.StudentProfiles.SingleOrDefault(x =>
            x.UserId == actorUserId &&
            !x.IsArchived &&
            x.Status == AcademicStructureStatus.Active);
        if (profile is null)
            return ResolvedDelivery.Fail(StudentAssessmentDeliveryErrorCode.ProfileNotLinked);

        var assessment = snapshot.Assessments.SingleOrDefault(x => x.Id == assessmentId);
        if (assessment is null)
            return ResolvedDelivery.Fail(StudentAssessmentDeliveryErrorCode.AssessmentNotFound);
        if (assessment.Status != AssessmentStatus.Open)
            return ResolvedDelivery.Fail(StudentAssessmentDeliveryErrorCode.AssessmentNotOpen);
        if (assessment.DeliveryMode != AssessmentDeliveryMode.Online)
            return ResolvedDelivery.Fail(StudentAssessmentDeliveryErrorCode.AssessmentOffline);

        var enrolled = snapshot.StudentEnrollments.Any(x =>
            x.StudentProfileId == profile.Id &&
            x.AcademicYearId == assessment.AcademicYearId &&
            x.ClassGroupId == assessment.ClassGroupId);
        if (!enrolled)
            return ResolvedDelivery.Fail(StudentAssessmentDeliveryErrorCode.NotTargeted);

        if (assessment.TargetType == AssessmentTargetType.Student &&
            assessment.TargetStudentProfileId != profile.Id)
            return ResolvedDelivery.Fail(StudentAssessmentDeliveryErrorCode.NotTargeted);

        return ResolvedDelivery.Ok(school.Id, profile, snapshot);
    }

    private static bool AnswersEquivalent(string actual, string expected)
    {
        actual = actual.Trim();
        expected = (expected ?? string.Empty).Trim();

        if (decimal.TryParse(actual, NumberStyles.Number, CultureInfo.InvariantCulture, out var actualNumber) &&
            decimal.TryParse(expected, NumberStyles.Number, CultureInfo.InvariantCulture, out var expectedNumber))
            return actualNumber == expectedNumber;

        return string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase);
    }

    private static decimal Round(decimal value) =>
        decimal.Round(value, 2, MidpointRounding.AwayFromZero);

    private sealed record ResolvedDelivery(
        Guid SchoolId,
        StudentProfile? Profile,
        AssessmentSnapshot? Snapshot,
        StudentAssessmentDeliveryErrorCode? Error)
    {
        public static ResolvedDelivery Ok(Guid schoolId, StudentProfile profile, AssessmentSnapshot snapshot) =>
            new(schoolId, profile, snapshot, null);

        public static ResolvedDelivery Fail(StudentAssessmentDeliveryErrorCode error) =>
            new(Guid.Empty, null, null, error);
    }
}
''',
    encoding="utf-8",
)

# Student portal controller gets Start -> Answer -> Submit routes.
Path("src/Edulytics.Web/Controllers/StudentPortalController.cs").write_text(
    r'''using System.Security.Claims;
using System.Globalization;
using Edulytics.Services.Assessments;
using Edulytics.Services.Notifications;
using Edulytics.Services.LessonContent;
using Edulytics.Services.StudentPortal;
using Edulytics.Web.ViewModels.StudentPortal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

[Authorize(Policy = "StudentPortal")]
[Route("student")]
public sealed class StudentPortalController : Controller
{
    private readonly IStudentPortalService _portal;
    private readonly INotificationService _notifications;
    private readonly ILessonContentService _lessonContent;
    private readonly IStudentAssessmentDeliveryService _assessmentDelivery;

    public StudentPortalController(
        IStudentPortalService portal,
        INotificationService notifications,
        ILessonContentService lessonContent,
        IStudentAssessmentDeliveryService assessmentDelivery)
    {
        _portal = portal;
        _notifications = notifications;
        _lessonContent = lessonContent;
        _assessmentDelivery = assessmentDelivery;
    }

    [HttpGet("")]
    [HttpGet("dashboard")]
    public async Task<IActionResult> Dashboard(CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        var workspace = await _portal.GetWorkspaceAsync(actorId, cancellationToken);
        if (workspace.Value is null) return HandlePortalError(workspace.Error);
        var notifications = await _notifications.ListInboxAsync(actorId, cancellationToken);
        return View(new StudentDashboardViewModel(workspace.Value, notifications.Value ?? []));
    }

    [HttpGet("learning")]
    public async Task<IActionResult> Learning(CancellationToken cancellationToken)
    {
        var workspace = await WorkspaceAsync(cancellationToken);
        if (workspace.Result is not null) return workspace.Result;
        if (!TryActor(out var actorId)) return Forbid();
        var lessons = await _lessonContent.ListPublishedForStudentAsync(
            actorId, CultureInfo.CurrentUICulture.Name, cancellationToken);
        if (lessons.Value is null)
            return lessons.Error == LessonContentErrorCode.AccessDenied ? Forbid() : NotFound();
        return View(nameof(Learning), new StudentLearningViewModel(workspace.Workspace!, lessons.Value));
    }

    [HttpGet("learning/lesson/{id:guid}")]
    public async Task<IActionResult> Lesson(Guid id, CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        var workspace = await _portal.GetWorkspaceAsync(actorId, cancellationToken);
        if (workspace.Value is null) return HandlePortalError(workspace.Error);
        var lesson = await _lessonContent.GetPublishedForStudentAsync(
            actorId, id, CultureInfo.CurrentUICulture.Name, cancellationToken);
        if (lesson.Value is null)
            return lesson.Error == LessonContentErrorCode.AccessDenied ? Forbid() : NotFound();
        return View(nameof(Lesson), lesson.Value);
    }

    [HttpGet("assessments")]
    public async Task<IActionResult> Assessments(CancellationToken cancellationToken)
    {
        var workspace = await WorkspaceAsync(cancellationToken);
        return workspace.Result ?? View(nameof(Assessments), new StudentAssessmentsViewModel(workspace.Workspace!));
    }

    [HttpGet("assessments/{id:guid}")]
    public async Task<IActionResult> TakeAssessment(Guid id, CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        var attempt = await _assessmentDelivery.GetAttemptAsync(actorId, id, cancellationToken);
        if (attempt.Value is not null) return View(nameof(TakeAssessment), attempt.Value);
        return attempt.Error switch
        {
            StudentAssessmentDeliveryErrorCode.AlreadySubmitted => RedirectToAction(nameof(Results)),
            StudentAssessmentDeliveryErrorCode.AccessDenied or
            StudentAssessmentDeliveryErrorCode.SchoolNotActive or
            StudentAssessmentDeliveryErrorCode.ProfileNotLinked or
            StudentAssessmentDeliveryErrorCode.NotTargeted => Forbid(),
            _ => NotFound()
        };
    }

    [HttpPost("assessments/{id:guid}/submit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubmitAssessment(
        Guid id,
        Guid[]? questionIds,
        string[]? responses,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        questionIds ??= [];
        responses ??= [];
        if (questionIds.Length != responses.Length) return BadRequest();
        var payload = questionIds
            .Select((questionId, index) => new StudentAssessmentResponse(questionId, responses[index]))
            .ToArray();
        var submitted = await _assessmentDelivery.SubmitAsync(actorId, id, payload, cancellationToken);
        if (submitted.Value is not null) return View("AssessmentSubmitted", submitted.Value);
        return submitted.Error switch
        {
            StudentAssessmentDeliveryErrorCode.AlreadySubmitted => RedirectToAction(nameof(Results)),
            StudentAssessmentDeliveryErrorCode.InvalidSubmission => BadRequest(),
            StudentAssessmentDeliveryErrorCode.AccessDenied or
            StudentAssessmentDeliveryErrorCode.SchoolNotActive or
            StudentAssessmentDeliveryErrorCode.ProfileNotLinked or
            StudentAssessmentDeliveryErrorCode.NotTargeted => Forbid(),
            _ => NotFound()
        };
    }

    [HttpGet("results")]
    public async Task<IActionResult> Results(CancellationToken cancellationToken)
    {
        var workspace = await WorkspaceAsync(cancellationToken);
        return workspace.Result ?? View(nameof(Results), new StudentResultsViewModel(workspace.Workspace!));
    }

    [HttpGet("notifications")]
    public async Task<IActionResult> Notifications(CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        var workspace = await _portal.GetWorkspaceAsync(actorId, cancellationToken);
        if (workspace.Value is null) return HandlePortalError(workspace.Error);
        var notifications = await _notifications.ListInboxAsync(actorId, cancellationToken);
        if (notifications.Value is null) return Forbid();
        return View(new StudentNotificationsViewModel(workspace.Value, notifications.Value));
    }

    [HttpPost("notifications/{id:guid}/read")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetNotificationReadState(
        Guid id, bool isRead, CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        var workspace = await _portal.GetWorkspaceAsync(actorId, cancellationToken);
        if (workspace.Value is null) return HandlePortalError(workspace.Error);
        var result = await _notifications.SetReadStateAsync(actorId, id, isRead, cancellationToken);
        if (result.Value is null) return Forbid();
        return RedirectToAction(nameof(Notifications));
    }

    private async Task<(StudentPortalWorkspace? Workspace, IActionResult? Result)> WorkspaceAsync(
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return (null, Forbid());
        var workspace = await _portal.GetWorkspaceAsync(actorId, cancellationToken);
        return workspace.Value is null
            ? (null, HandlePortalError(workspace.Error))
            : (workspace.Value, null);
    }

    private IActionResult HandlePortalError(StudentPortalErrorCode? error) =>
        error switch
        {
            StudentPortalErrorCode.AccessDenied => Forbid(),
            StudentPortalErrorCode.ProfileNotLinked => Forbid(),
            StudentPortalErrorCode.SchoolNotActive => Forbid(),
            _ => NotFound()
        };

    private bool TryActor(out Guid actorUserId) =>
        Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out actorUserId);
}
''',
    encoding="utf-8",
)

Path("src/Edulytics.Web/Views/StudentPortal/Assessments.cshtml").write_text(
    r'''@using Edulytics.Core.Enums
@model Edulytics.Web.ViewModels.StudentPortal.StudentAssessmentsViewModel
@inject Microsoft.Extensions.Localization.IStringLocalizer<Edulytics.Web.StudentResource> S
@{
    Layout = "_StudentLayout";
    ViewData["Title"] = S["AssessmentsTitle"].Value;
}
<header class="student-page-header">
    <span class="student-eyebrow">@S["Assessments"]</span>
    <h1>@S["AssessmentsTitle"]</h1>
    <p>@S["AssessmentsSubtitle"]</p>
</header>

@if (Model.Workspace.Assessments.Count == 0)
{
    <div class="student-empty-state">@S["NoActiveAssessments"]</div>
}
else
{
    <div class="student-card-list">
        @foreach (var item in Model.Workspace.Assessments)
        {
            <article class="student-assessment-card">
                <div>
                    <span class="student-subject-code">@item.SubjectName</span>
                    <h2>@item.Title</h2>
                    <p>@S["Class"]: @item.ClassName</p>
                    <p>@S["AssessmentMode"]: @(item.DeliveryMode == AssessmentDeliveryMode.Online ? S["OnlineAssessment"] : S["OfflineAssessment"])</p>
                    <p>@S["AssessmentDifficulty"]: @S[$"AssessmentDifficulty{item.DifficultyBand}"]</p>
                </div>
                <dl>
                    <div><dt>@S["AssessmentDate"]</dt><dd>@item.AssessmentDate.ToString("yyyy-MM-dd")</dd></div>
                    <div><dt>@S["MaxScore"]</dt><dd>@item.MaxScore.ToString("0.##")</dd></div>
                </dl>
                <div class="student-actions">
                    @if (item.IsSubmitted)
                    {
                        <a class="student-button" asp-action="Results">@S["ViewResults"]</a>
                    }
                    else if (item.DeliveryMode == AssessmentDeliveryMode.Online)
                    {
                        <a class="student-button student-button-primary" asp-action="TakeAssessment" asp-route-id="@item.AssessmentId">@S["StartAssessment"]</a>
                    }
                    else
                    {
                        <span class="student-muted">@S["TeacherManagedAssessment"]</span>
                    }
                </div>
            </article>
        }
    </div>
}
''',
    encoding="utf-8",
)

Path("src/Edulytics.Web/Views/StudentPortal/TakeAssessment.cshtml").write_text(
    r'''@model Edulytics.Services.Assessments.StudentAssessmentAttempt
@inject Microsoft.Extensions.Localization.IStringLocalizer<Edulytics.Web.StudentResource> S
@{
    Layout = "_StudentLayout";
    ViewData["Title"] = Model.Title;
}
<header class="student-page-header">
    <span class="student-eyebrow">@S["OnlineAssessment"]</span>
    <h1>@Model.Title</h1>
    <p>@S["AssessmentDifficulty"]: @S[$"AssessmentDifficulty{Model.DifficultyBand}"] · @S["MaxScore"]: @Model.MaxScore.ToString("0.##")</p>
</header>
<form asp-action="SubmitAssessment" asp-route-id="@Model.AssessmentId" method="post" class="student-card-list">
    @Html.AntiForgeryToken()
    @foreach (var question in Model.Questions)
    {
        <article class="student-assessment-card">
            <div>
                <span class="student-subject-code">@S["Question"] @question.Order</span>
                <h2>@question.Prompt</h2>
                <p>@S["MaxScore"]: @question.MaxScore.ToString("0.##")</p>
            </div>
            <input type="hidden" name="questionIds" value="@question.Id" />
            <label for="answer-@question.Id">@S["YourAnswer"]</label>
            <textarea id="answer-@question.Id" name="responses" maxlength="4000" rows="4"></textarea>
        </article>
    }
    <button class="student-button student-button-primary" type="submit">@S["SubmitAssessment"]</button>
</form>
''',
    encoding="utf-8",
)

Path("src/Edulytics.Web/Views/StudentPortal/AssessmentSubmitted.cshtml").write_text(
    r'''@model Edulytics.Services.Assessments.StudentAssessmentSubmission
@inject Microsoft.Extensions.Localization.IStringLocalizer<Edulytics.Web.StudentResource> S
@{
    Layout = "_StudentLayout";
    ViewData["Title"] = S["AssessmentSubmittedTitle"].Value;
}
<header class="student-page-header">
    <span class="student-eyebrow">@S["Assessments"]</span>
    <h1>@S["AssessmentSubmittedTitle"]</h1>
    <p>@S["AssessmentSubmittedSubtitle"]</p>
</header>
<section class="student-assessment-card">
    <h2>@Model.Title</h2>
    <dl>
        <div><dt>@S["AssessmentScore"]</dt><dd>@Model.Score.ToString("0.##") / @Model.MaxScore.ToString("0.##")</dd></div>
        <div><dt>@S["Percentage"]</dt><dd>@Model.Percentage.ToString("0.##")%</dd></div>
    </dl>
    <a class="student-button student-button-primary" asp-action="Results">@S["ViewResults"]</a>
</section>
''',
    encoding="utf-8",
)

append_resx(
    "src/Edulytics.Web/Resources/StudentResource.resx",
    [
        ("AssessmentMode", "Mode"),
        ("OnlineAssessment", "Online assessment"),
        ("OfflineAssessment", "Offline / manual assessment"),
        ("AssessmentDifficulty", "Difficulty"),
        ("AssessmentDifficultyAtClassLevel", "At my class level"),
        ("AssessmentDifficultyStretch", "Stretch"),
        ("AssessmentDifficultyChallenge", "Challenge"),
        ("StartAssessment", "Start assessment"),
        ("TeacherManagedAssessment", "Your teacher will record or import this assessment result."),
        ("ViewResults", "View results"),
        ("Question", "Question"),
        ("YourAnswer", "Your answer"),
        ("SubmitAssessment", "Submit assessment"),
        ("AssessmentSubmittedTitle", "Assessment submitted"),
        ("AssessmentSubmittedSubtitle", "Your official assessment has been submitted and scored."),
        ("AssessmentScore", "Score"),
        ("Percentage", "Percentage"),
    ],
)
append_resx(
    "src/Edulytics.Web/Resources/StudentResource.pl.resx",
    [
        ("AssessmentMode", "Tryb"),
        ("OnlineAssessment", "Sprawdzian online"),
        ("OfflineAssessment", "Sprawdzian offline / ręczny"),
        ("AssessmentDifficulty", "Trudność"),
        ("AssessmentDifficultyAtClassLevel", "Na poziomie mojej klasy"),
        ("AssessmentDifficultyStretch", "Rozszerzony"),
        ("AssessmentDifficultyChallenge", "Wymagający"),
        ("StartAssessment", "Rozpocznij sprawdzian"),
        ("TeacherManagedAssessment", "Nauczyciel wprowadzi lub zaimportuje wynik tego sprawdzianu."),
        ("ViewResults", "Zobacz wyniki"),
        ("Question", "Pytanie"),
        ("YourAnswer", "Twoja odpowiedź"),
        ("SubmitAssessment", "Wyślij sprawdzian"),
        ("AssessmentSubmittedTitle", "Sprawdzian wysłany"),
        ("AssessmentSubmittedSubtitle", "Twój oficjalny sprawdzian został wysłany i oceniony."),
        ("AssessmentScore", "Wynik"),
        ("Percentage", "Procent"),
    ],
)

# Focused contract coverage for enum compatibility and response persistence.
Path("tests/Edulytics.Tests/Phase41AssessmentDeliveryContractTests.cs").write_text(
    r'''using Edulytics.Core.Entities;
using Edulytics.Core.Enums;

namespace Edulytics.Tests;

public sealed class Phase41AssessmentDeliveryContractTests
{
    [Fact]
    public void LegacyDefaultsRemainOfflineClassAtClassLevel()
    {
        var assessment = new Assessment();
        Assert.Equal(AssessmentTargetType.Class, assessment.TargetType);
        Assert.Equal(AssessmentDeliveryMode.Offline, assessment.DeliveryMode);
        Assert.Equal(AssessmentDifficultyBand.AtClassLevel, assessment.DifficultyBand);
    }

    [Fact]
    public void StudentAnswerPersistsResponseTextContract()
    {
        var answer = new StudentAnswer { ResponseText = "42" };
        Assert.Equal("42", answer.ResponseText);
    }
}
''',
    encoding="utf-8",
)

print("PHASE41_FUNCTIONAL_PATCH_APPLIED")
