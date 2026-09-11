using System.Globalization;
using Edulytics.Core.Interfaces;
using Edulytics.Web.GameRouting;
using Edulytics.Web.ViewModels.Platform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

[Authorize(Policy = "PlatformAdministration")]
public sealed class TemporaryPracticePreviewController : Controller
{
    private const string FrameworkCode = "CAMBRIDGE-INTL-MATH";
    private const string JoinGroupsLessonCode = "PED:CAMBRIDGE-INTL-MATH:S1:L10";

    private readonly ICurriculumRepository _curriculum;
    private readonly ILessonContentRepository _lessonContent;

    public TemporaryPracticePreviewController(
        ICurriculumRepository curriculum,
        ILessonContentRepository lessonContent)
    {
        _curriculum = curriculum;
        _lessonContent = lessonContent;
    }

    [HttpGet("/_preview/join-groups-to-add")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public Task<IActionResult> JoinGroupsToAdd(CancellationToken cancellationToken) =>
        RenderLessonAsync(JoinGroupsLessonCode, "JoinGroupsToAdd", "Stage 1", cancellationToken);

    [HttpGet("/_preview/count-touch-and-check")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public Task<IActionResult> CountTouchAndCheck(CancellationToken cancellationToken) =>
        RenderLessonAsync(GameLessonRouter.CountTouchLessonCode, "CountTouchAndCheck", "Stage 1", cancellationToken);

    [HttpGet("/_preview/angle-explorer")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public Task<IActionResult> AngleExplorer(CancellationToken cancellationToken) =>
        RenderLessonAsync(GameLessonRouter.AngleExplorerLessonCode, "AngleExplorer", "Stage 5", cancellationToken);

    [HttpGet("/_preview/fraction-forge")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public Task<IActionResult> FractionForge(CancellationToken cancellationToken) =>
        RenderLessonAsync(GameLessonRouter.FractionForgeLessonCode, "FractionForge", "Stage 5", cancellationToken);

    private async Task<IActionResult> RenderLessonAsync(
        string lessonCode,
        string viewName,
        string stageLabel,
        CancellationToken cancellationToken)
    {
        Response.Headers["X-Robots-Tag"] = "noindex, nofollow, noarchive";

        var frameworkVersionId =
            await _curriculum.GetActivePlatformFrameworkVersionIdAsync(
                FrameworkCode,
                cancellationToken);
        if (!frameworkVersionId.HasValue)
            return NotFound();

        var lessons =
            await _lessonContent.ListPedagogicalLessonsAsync(
                [frameworkVersionId.Value],
                cancellationToken);
        var lesson = lessons.SingleOrDefault(x =>
            string.Equals(
                x.Code,
                lessonCode,
                StringComparison.Ordinal));
        if (lesson is null)
            return NotFound();

        var content =
            (await _lessonContent.ListCanonicalContentsAsync(
                [lesson.Id],
                cancellationToken))
            .SingleOrDefault();
        if (content is null)
            return NotFound();

        var translation = content.Translations.FirstOrDefault(x =>
            string.Equals(
                NormalizeCulture(x.CultureCode),
                "en",
                StringComparison.Ordinal));
        if (translation is null)
            return NotFound();

        var outcomes =
            await _lessonContent.ListOfficialOutcomesAsync(
                frameworkVersionId.Value,
                lesson.Id,
                cancellationToken);

        var model = new TemporaryPracticePreviewViewModel(
            lesson.Code,
            translation.Title,
            lesson.UnitTitle,
            "Cambridge Primary Mathematics (0096)",
            stageLabel,
            "Mathematics",
            "MATH",
            translation.Explanation,
            translation.KeyConceptsAndRules,
            translation.WorkedExamples,
            translation.StepByStepSolutions,
            translation.CommonMistakes,
            translation.QuickSummary,
            content.PublishedAtUtc ?? content.UpdatedAtUtc,
            lesson.IsSupporting ?? lesson.OfficialOutcomeCount == 0,
            outcomes);

        return View(viewName, model);
    }

    private static string NormalizeCulture(string cultureCode)
    {
        if (string.IsNullOrWhiteSpace(cultureCode))
            return "en";

        var value = cultureCode.Trim();
        var separator = value.IndexOf('-');
        return (separator > 0 ? value[..separator] : value)
            .ToLowerInvariant();
    }
}
