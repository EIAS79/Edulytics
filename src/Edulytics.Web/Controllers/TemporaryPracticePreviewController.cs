using System.Globalization;
using Edulytics.Core.Interfaces;
using Edulytics.Web.ViewModels.Platform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Web.Controllers;

[Authorize(Policy = "PlatformAdministration")]
public sealed class TemporaryPracticePreviewController : Controller
{
    private const string FrameworkCode = "CAMBRIDGE-INTL-MATH";
    private const string LessonCode = "PED:CAMBRIDGE-INTL-MATH:S1:L10";

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
    public async Task<IActionResult> JoinGroupsToAdd(
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
                LessonCode,
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
            "Stage 1",
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

        return View("JoinGroupsToAdd", model);
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
