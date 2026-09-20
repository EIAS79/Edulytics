using System.Globalization;
using System.Security.Claims;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Practice;
using Edulytics.Web.GameRouting;
using Edulytics.Web.Resilience;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Localization;

namespace Edulytics.Web.Controllers;

[Authorize(Policy = "StudentPortal")]
[Route("student/practice")]
public sealed class StudentPracticeController(
    IStudentPrivatePracticeService privatePractice,
    IPracticeService practice,
    ILessonContentService lessonContent,
    Stage22ExactGameRuntime gameRuntime,
    IStringLocalizer<StudentResource> text) : Controller
{
    private const string LessonPracticePilotCode = "PED:CAMBRIDGE-INTL-MATH:S1:L10";
    private const string LessonGameMode = "lesson-game";
    private const int LessonPracticeQuestionCount = 10;

    [HttpGet("")]
    public async Task<IActionResult> Index(Guid? curriculumAdoptionId, CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        var workspace = await privatePractice.GetWorkspaceAsync(actorId, curriculumAdoptionId, cancellationToken);
        return View(workspace);
    }

    [HttpPost("generate"), ValidateAntiForgeryToken]
    [RequestTimeout(BackendResiliencePolicyNames.InteractiveWrite)]
    [EnableRateLimiting(BackendResiliencePolicyNames.HeavyWriteConcurrency)]
    public async Task<IActionResult> Generate(
        Guid curriculumAdoptionId,
        StudentPrivatePracticeScope scope,
        Guid? lessonId,
        string? unitKey,
        StudentPrivatePracticeDifficulty difficulty,
        int questionCount,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        var result = await privatePractice.GenerateAsync(actorId,
            new GenerateStudentPrivatePracticeRequest(
                curriculumAdoptionId, scope, lessonId, unitKey, difficulty, questionCount),
            cancellationToken);
        if (!result.Succeeded)
        {
            TempData["Error"] = PrivatePracticeErrorMessage(result.Error);
            return RedirectToAction(nameof(Index), new { curriculumAdoptionId });
        }
        return RedirectToAction(nameof(Attempt), new { id = result.AttemptId });
    }

    [HttpPost("lesson/start"), ValidateAntiForgeryToken]
    [RequestTimeout(BackendResiliencePolicyNames.InteractiveWrite)]
    [EnableRateLimiting(BackendResiliencePolicyNames.HeavyWriteConcurrency)]
    public async Task<IActionResult> StartLessonPractice(
        Guid curriculumAdoptionId,
        Guid lessonId,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();

        var workspace = await privatePractice.GetWorkspaceAsync(
            actorId,
            curriculumAdoptionId,
            cancellationToken);
        if (workspace.SelectedCurriculumAdoptionId != curriculumAdoptionId)
            return NotFound();

        var lesson = workspace.Lessons.SingleOrDefault(x => x.LessonId == lessonId);
        if (lesson is null)
            return NotFound();

        var detailResult = await lessonContent.GetPublishedForStudentAsync(
            actorId,
            lessonId,
            CultureInfo.CurrentUICulture.Name,
            cancellationToken);
        if (detailResult.Value is null)
            return detailResult.Error == LessonContentErrorCode.AccessDenied
                ? Forbid()
                : NotFound();

        if (!TryResolveLessonPracticePresentation(
                lesson,
                detailResult.Value,
                out var presentation) ||
            presentation is null)
        {
            return NotFound();
        }

        if (presentation.Kind ==
            LessonPracticePresentationKind.SpecializedGame)
        {
            return RedirectToAction(
                nameof(Game),
                new
                {
                    curriculumAdoptionId,
                    lessonId
                });
        }

        var result = await privatePractice.GenerateAsync(
            actorId,
            new GenerateStudentPrivatePracticeRequest(
                curriculumAdoptionId,
                StudentPrivatePracticeScope.Lesson,
                lessonId,
                null,
                StudentPrivatePracticeDifficulty.MyLevel,
                LessonPracticeQuestionCount),
            cancellationToken);

        if (!result.Succeeded)
        {
            TempData["Error"] = PrivatePracticeErrorMessage(result.Error);
            return RedirectToAction(
                "Lesson",
                "StudentPortal",
                new { id = lessonId });
        }

        return RedirectToAction(
            nameof(LessonAttempt),
            new
            {
                id = result.AttemptId,
                curriculumAdoptionId,
                lessonId
            });
    }

    [HttpPost("lesson-game/start"), ValidateAntiForgeryToken]
    public async Task<IActionResult> StartLessonGame(
        Guid curriculumAdoptionId,
        Guid lessonId,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        var workspace = await privatePractice.GetWorkspaceAsync(actorId, curriculumAdoptionId, cancellationToken);
        if (workspace.SelectedCurriculumAdoptionId != curriculumAdoptionId)
            return NotFound();

        var lesson = workspace.Lessons.SingleOrDefault(x => x.LessonId == lessonId);
        if (lesson is null ||
            !LessonPracticeCapabilityResolver.TryResolve(
                lesson.LessonCode,
                out _))
        {
            return NotFound();
        }

        var detailResult = await lessonContent.GetPublishedForStudentAsync(
            actorId,
            lessonId,
            CultureInfo.CurrentUICulture.Name,
            cancellationToken);
        if (detailResult.Value is null)
            return detailResult.Error == LessonContentErrorCode.AccessDenied ? Forbid() : NotFound();

        var route = ResolveLessonRoute(lesson, detailResult.Value);
        if (!route.IsPlayable || route.RendererKey is null)
            return NotFound();

        return RedirectToAction(nameof(Game), new { curriculumAdoptionId, lessonId });
    }

    [HttpGet("game")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Game(
        Guid curriculumAdoptionId,
        Guid lessonId,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        var workspace = await privatePractice.GetWorkspaceAsync(actorId, curriculumAdoptionId, cancellationToken);
        if (workspace.SelectedCurriculumAdoptionId != curriculumAdoptionId)
            return NotFound();

        var lesson = workspace.Lessons.SingleOrDefault(x => x.LessonId == lessonId);
        if (lesson is null ||
            !LessonPracticeCapabilityResolver.TryResolve(
                lesson.LessonCode,
                out _))
        {
            return NotFound();
        }

        var detailResult = await lessonContent.GetPublishedForStudentAsync(
            actorId,
            lessonId,
            CultureInfo.CurrentUICulture.Name,
            cancellationToken);
        if (detailResult.Value is null)
            return detailResult.Error == LessonContentErrorCode.AccessDenied ? Forbid() : NotFound();

        var detail = detailResult.Value;
        var route = ResolveLessonRoute(lesson, detail);
        if (!route.IsPlayable || route.RendererKey is null)
            return NotFound();

        Response.Headers["X-Robots-Tag"] = "noindex, nofollow, noarchive";
        return View(new StudentGameLaunchViewModel(
            curriculumAdoptionId,
            lesson.LessonId,
            lesson.LessonCode,
            detail.Title,
            lesson.UnitTitle,
            route));
    }

    [HttpPost("game/runtime/start"), ValidateAntiForgeryToken]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    [RequestTimeout(BackendResiliencePolicyNames.InteractiveWrite)]
    [EnableRateLimiting(BackendResiliencePolicyNames.HeavyWriteConcurrency)]
    public async Task<IActionResult> StartGameRuntime(
        [FromBody] Stage22GameRoundRequest request,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId))
            return Forbid();

        var workspace = await privatePractice.GetWorkspaceAsync(
            actorId,
            request.CurriculumAdoptionId,
            cancellationToken);
        if (workspace.SelectedCurriculumAdoptionId != request.CurriculumAdoptionId)
            return NotFound();

        var lesson = workspace.Lessons.SingleOrDefault(x => x.LessonId == request.LessonId);
        if (lesson is null)
            return NotFound();

        var detailResult = await lessonContent.GetPublishedForStudentAsync(
            actorId,
            request.LessonId,
            CultureInfo.CurrentUICulture.Name,
            cancellationToken);
        if (detailResult.Value is null)
            return detailResult.Error == LessonContentErrorCode.AccessDenied ? Forbid() : NotFound();

        if (!LessonPracticeCapabilityResolver.TryResolve(
                lesson.LessonCode,
                out _))
        {
            return NotFound();
        }

        var route = ResolveLessonRoute(lesson, detailResult.Value);
        if (!route.IsPlayable ||
            !string.Equals(
                route.RendererKey,
                MathematicsV2ProductMigrationPolicy.RendererKey,
                StringComparison.Ordinal) ||
            !Stage18PracticeSkillContracts.TryResolve(lesson.LessonCode, out var contract) ||
            contract is null ||
            !string.Equals(contract.Mechanic, route.Mechanic, StringComparison.Ordinal))
        {
            return NotFound();
        }

        try
        {
            var round = gameRuntime.CreateRound(
                actorId,
                request.CurriculumAdoptionId,
                request.LessonId,
                lesson.LessonCode,
                route.Mechanic,
                request.RoundIndex);
            return Json(round);
        }
        catch (InvalidOperationException)
        {
            return BadRequest(new { error = "game_runtime_round_rejected" });
        }
    }

    [HttpPost("game/runtime/answer"), ValidateAntiForgeryToken]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    [RequestTimeout(BackendResiliencePolicyNames.InteractiveWrite)]
    [EnableRateLimiting(BackendResiliencePolicyNames.HeavyWriteConcurrency)]
    public IActionResult AnswerGameRuntime(
        [FromBody] Stage22GameAnswerRequest request)
    {
        if (!TryActor(out var actorId))
            return Forbid();

        try
        {
            return Json(gameRuntime.EvaluateAnswer(actorId, request));
        }
        catch (InvalidOperationException)
        {
            return BadRequest(new { error = "game_runtime_answer_rejected" });
        }
    }

    [HttpPost("lesson-pilot/start"), ValidateAntiForgeryToken]
    [RequestTimeout(BackendResiliencePolicyNames.InteractiveWrite)]
    [EnableRateLimiting(BackendResiliencePolicyNames.HeavyWriteConcurrency)]
    public async Task<IActionResult> StartLessonPilot(
        Guid curriculumAdoptionId,
        Guid lessonId,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();

        var workspace = await privatePractice.GetWorkspaceAsync(
            actorId, curriculumAdoptionId, cancellationToken);
        var pilotLesson = workspace.Lessons.SingleOrDefault(x =>
            x.LessonId == lessonId &&
            string.Equals(x.LessonCode, LessonPracticePilotCode, StringComparison.Ordinal));
        if (pilotLesson is null || workspace.SelectedCurriculumAdoptionId != curriculumAdoptionId)
            return NotFound();

        var result = await privatePractice.GenerateAsync(
            actorId,
            new GenerateStudentPrivatePracticeRequest(
                curriculumAdoptionId,
                StudentPrivatePracticeScope.Lesson,
                lessonId,
                null,
                StudentPrivatePracticeDifficulty.MyLevel,
                LessonPracticeQuestionCount),
            cancellationToken);

        if (!result.Succeeded)
        {
            TempData["Error"] = PrivatePracticeErrorMessage(result.Error);
            return RedirectToAction("Lesson", "StudentPortal", new { id = lessonId });
        }

        return RedirectToAction(nameof(Attempt), new
        {
            id = result.AttemptId,
            mode = LessonGameMode
        });
    }

    [HttpGet("lesson-attempt/{id:guid}")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> LessonAttempt(
        Guid id,
        Guid curriculumAdoptionId,
        Guid lessonId,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();

        var workspace = await privatePractice.GetWorkspaceAsync(
            actorId,
            curriculumAdoptionId,
            cancellationToken);
        if (workspace.SelectedCurriculumAdoptionId != curriculumAdoptionId)
            return NotFound();

        var lesson = workspace.Lessons.SingleOrDefault(
            x => x.LessonId == lessonId);
        if (lesson is null)
            return NotFound();

        var detailResult = await lessonContent.GetPublishedForStudentAsync(
            actorId,
            lessonId,
            CultureInfo.CurrentUICulture.Name,
            cancellationToken);
        if (detailResult.Value is null)
            return detailResult.Error == LessonContentErrorCode.AccessDenied
                ? Forbid()
                : NotFound();

        var attempt = await practice.GetAttemptAsync(
            actorId,
            id,
            cancellationToken);
        if (attempt.Value is null)
            return attempt.Error == PracticeErrorCode.AccessDenied
                ? Forbid()
                : NotFound();

        ViewData["CurriculumAdoptionId"] = curriculumAdoptionId;
        ViewData["LessonId"] = lessonId;
        ViewData["LessonCode"] = lesson.LessonCode;
        ViewData["LessonTitle"] = detailResult.Value.Title;
        ViewData["LessonUnitTitle"] = lesson.UnitTitle;

        Response.Headers["X-Robots-Tag"] =
            "noindex, nofollow, noarchive";

        return View(attempt.Value);
    }

    [HttpPost("lesson-attempt/{id:guid}/answer"), ValidateAntiForgeryToken]
    [RequestTimeout(BackendResiliencePolicyNames.InteractiveWrite)]
    [EnableRateLimiting(BackendResiliencePolicyNames.HeavyWriteConcurrency)]
    public async Task<IActionResult> AnswerLessonAttempt(
        Guid id,
        Guid curriculumAdoptionId,
        Guid lessonId,
        Guid attemptItemId,
        string answer,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();

        var result = await practice.AnswerAsync(
            actorId,
            id,
            attemptItemId,
            answer,
            cancellationToken);

        if (result.Value is null)
        {
            TempData["Error"] =
                PracticeErrorMessage(result.Error);
        }
        else
        {
            TempData["PracticeFeedback"] =
                result.Value.IsCorrect
                    ? "correct"
                    : "incorrect";
            TempData["PracticeSolution"] =
                result.Value.Solution;
        }

        return RedirectToAction(
            nameof(LessonAttempt),
            new
            {
                id,
                curriculumAdoptionId,
                lessonId
            });
    }

    [HttpPost("lesson-attempt/{id:guid}/submit"), ValidateAntiForgeryToken]
    [RequestTimeout(BackendResiliencePolicyNames.InteractiveWrite)]
    [EnableRateLimiting(BackendResiliencePolicyNames.HeavyWriteConcurrency)]
    public async Task<IActionResult> SubmitLessonAttempt(
        Guid id,
        Guid curriculumAdoptionId,
        Guid lessonId,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();

        var result = await practice.SubmitAsync(
            actorId,
            id,
            cancellationToken);
        if (result.Value is null)
            TempData["Error"] =
                PracticeErrorMessage(result.Error);

        return RedirectToAction(
            nameof(LessonAttempt),
            new
            {
                id,
                curriculumAdoptionId,
                lessonId
            });
    }

    [HttpGet("attempt/{id:guid}")]
    public async Task<IActionResult> Attempt(Guid id, string? mode, CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        var result = await practice.GetAttemptAsync(actorId, id, cancellationToken);
        if (result.Value is null)
            return result.Error == PracticeErrorCode.AccessDenied ? Forbid() : NotFound();
        ViewData["LessonGameMode"] = string.Equals(
            mode, LessonGameMode, StringComparison.OrdinalIgnoreCase);
        return View(result.Value);
    }

    [HttpPost("attempt/{id:guid}/answer"), ValidateAntiForgeryToken]
    [RequestTimeout(BackendResiliencePolicyNames.InteractiveWrite)]
    [EnableRateLimiting(BackendResiliencePolicyNames.HeavyWriteConcurrency)]
    public async Task<IActionResult> Answer(
        Guid id,
        Guid attemptItemId,
        string answer,
        string? mode,
        CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        var result = await practice.AnswerAsync(actorId, id, attemptItemId, answer, cancellationToken);
        if (result.Value is null)
        {
            TempData["Error"] = PracticeErrorMessage(result.Error);
        }
        else
        {
            TempData["PracticeFeedback"] = result.Value.IsCorrect ? "correct" : "incorrect";
            TempData["PracticeSolution"] = result.Value.Solution;
        }
        return RedirectToAction(nameof(Attempt), new
        {
            id,
            mode = NormalizeMode(mode)
        });
    }

    [HttpPost("attempt/{id:guid}/submit"), ValidateAntiForgeryToken]
    [RequestTimeout(BackendResiliencePolicyNames.InteractiveWrite)]
    [EnableRateLimiting(BackendResiliencePolicyNames.HeavyWriteConcurrency)]
    public async Task<IActionResult> Submit(Guid id, string? mode, CancellationToken cancellationToken)
    {
        if (!TryActor(out var actorId)) return Forbid();
        var result = await practice.SubmitAsync(actorId, id, cancellationToken);
        if (result.Value is null)
            TempData["Error"] = PracticeErrorMessage(result.Error);
        return RedirectToAction(nameof(Attempt), new
        {
            id,
            mode = NormalizeMode(mode)
        });
    }

    private static bool TryResolveLessonPracticePresentation(
        StudentPrivatePracticeLessonOption lesson,
        StudentLessonDetail detail,
        out LessonPracticePresentationRoute? presentation)
    {
        var context = detail.IsSupporting
            ? BuildLessonPracticeContext(detail)
            : null;

        return LessonPracticePresentationResolver.TryResolve(
            lesson.LessonCode,
            lesson.UnitTitle,
            detail.Title,
            context,
            detail.IsSupporting,
            MathematicsV2ProductMigrationPolicy
                .IsEnabledFromEnvironment(),
            out presentation);
    }

    private static GameLessonRoute ResolveLessonRoute(
        StudentPrivatePracticeLessonOption lesson,
        StudentLessonDetail detail)
    {
        if (!detail.IsSupporting)
            return GameLessonRouteResolver.Resolve(
                lesson.LessonCode,
                lesson.UnitTitle,
                detail.Title);

        return GameLessonRouteResolver.Resolve(
            lesson.LessonCode,
            lesson.UnitTitle,
            detail.Title,
            BuildLessonPracticeContext(detail),
            requireLessonGrounding: true);
    }

    private static string BuildLessonPracticeContext(StudentLessonDetail lesson) =>
        string.Join(
            ". ",
            new[]
            {
                lesson.Title,
                lesson.TopicName,
                lesson.Explanation,
                lesson.KeyConceptsAndRules,
                lesson.WorkedExamples,
                lesson.StepByStepSolutions,
                lesson.CommonMistakes,
                lesson.QuickSummary
            }.Where(x => !string.IsNullOrWhiteSpace(x)));

    private static string? NormalizeMode(string? mode) =>
        string.Equals(mode, LessonGameMode, StringComparison.OrdinalIgnoreCase)
            ? LessonGameMode
            : null;

    private string PrivatePracticeErrorMessage(StudentPrivatePracticeError? error) => error switch
    {
        StudentPrivatePracticeError.NoSupportedOutcomes => text["PrivatePracticeNoSupportedOutcomes"].Value,
        StudentPrivatePracticeError.CurriculumNotAvailable => text["PrivatePracticeCurriculumUnavailable"].Value,
        StudentPrivatePracticeError.InvalidQuestionCount => text["PrivatePracticeInvalidQuestionCount"].Value,
        StudentPrivatePracticeError.InvalidScope => text["PrivatePracticeInvalidScope"].Value,
        StudentPrivatePracticeError.GenerationFailed => text["PrivatePracticeGenerationFailed"].Value,
        StudentPrivatePracticeError.AccessDenied => text["PrivatePracticeAccessDenied"].Value,
        _ => text["PrivatePracticeGenerationFailed"].Value
    };

    private string PracticeErrorMessage(PracticeErrorCode? error) => error switch
    {
        PracticeErrorCode.AttemptIncomplete => text["PracticeAnswerAll"].Value,
        PracticeErrorCode.InvalidAnswer => text["PracticeInvalidAnswer"].Value,
        PracticeErrorCode.AttemptNotInProgress => text["PracticeAttemptNotInProgress"].Value,
        _ => text["PracticeOperationFailed"].Value
    };

    private bool TryActor(out Guid actorUserId) =>
        Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out actorUserId);
}
