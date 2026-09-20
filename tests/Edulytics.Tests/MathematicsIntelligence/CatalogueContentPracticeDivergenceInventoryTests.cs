using System.Text.Json;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Data.Seeding;
using Edulytics.Web.GameRouting;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class CatalogueContentPracticeDivergenceInventoryTests
{
    [Fact]
    public void WriteWholeCatalogueContentAndPracticeDivergenceInventory()
    {
        var rows = new List<object>();
        var genericFallbacks = new List<string>();
        var missingCapabilities = new List<string>();
        var missingRenderers = new List<string>();
        var supportingTargets = new List<string>();
        var officialTargets = new List<string>();
        var polishTargets = new List<string>();
        var stage6Targets = new List<string>();

        foreach (var document in MathematicsCanonicalLessonContentSeeder
                     .LoadEmbeddedDocuments())
        {
            foreach (var lesson in document.Lessons)
            {
                var translation = ChooseTranslation(lesson);
                var title = translation?.Title ?? string.Empty;
                var context = BuildContext(translation);
                var supporting = lesson.OutcomeCodes.Count == 0;

                var hasCapability =
                    LessonPracticeCapabilityResolver.TryResolve(
                        lesson.LessonCode,
                        out var contract);

                var route = GameLessonRouteResolver.Resolve(
                    lesson.LessonCode,
                    lesson.UnitTitle,
                    title,
                    context,
                    supporting,
                    enableMathematicsV2Pilot: false);

                var genericFallback =
                    hasCapability &&
                    (!route.IsPlayable || route.RendererKey is null);

                if (genericFallback)
                    genericFallbacks.Add(lesson.LessonCode);

                if (!hasCapability)
                    missingCapabilities.Add(lesson.LessonCode);

                if (hasCapability &&
                    route.IsPlayable &&
                    route.RendererKey is null)
                {
                    missingRenderers.Add(lesson.LessonCode);
                }

                if (SupportingLessonPracticeContentCorrections.IsTarget(
                        document,
                        lesson))
                {
                    supportingTargets.Add(lesson.LessonCode);
                }

                if (OfficialLessonPracticeContentCorrections.IsTarget(
                        document,
                        lesson))
                {
                    officialTargets.Add(lesson.LessonCode);
                }

                if (PolishLessonPracticeContentCorrections.IsTarget(
                        document,
                        lesson))
                {
                    polishTargets.Add(lesson.LessonCode);
                }

                if (CambridgePrimaryStage6LessonContentCorrections.IsTarget(
                        document,
                        lesson))
                {
                    stage6Targets.Add(lesson.LessonCode);
                }

                rows.Add(new
                {
                    lessonCode = lesson.LessonCode,
                    packCode = document.PackCode,
                    documentVersion = document.ContentVersion,
                    supporting,
                    outcomeCount = lesson.OutcomeCodes.Count,
                    effectiveTitle = title,
                    hasPracticeCapability = hasCapability,
                    skillId = contract?.SkillId,
                    families = contract?.AllowedQuestionFamilies ?? [],
                    routePlayable = route.IsPlayable,
                    routeRenderer = route.RendererKey,
                    routeMechanic = route.Mechanic,
                    routeSource = route.ClassificationSource,
                    genericLessonPracticeFallback = genericFallback,
                    supportingContentCorrectionTarget =
                        SupportingLessonPracticeContentCorrections.IsTarget(
                            document,
                            lesson),
                    officialContentCorrectionTarget =
                        OfficialLessonPracticeContentCorrections.IsTarget(
                            document,
                            lesson),
                    polishContentCorrectionTarget =
                        PolishLessonPracticeContentCorrections.IsTarget(
                            document,
                            lesson),
                    stage6ContentCorrectionTarget =
                        CambridgePrimaryStage6LessonContentCorrections.IsTarget(
                            document,
                            lesson)
                });
            }
        }

        var report = new
        {
            summary = new
            {
                lessonCount = rows.Count,
                missingPracticeCapabilityCount =
                    missingCapabilities.Count,
                genericLessonPracticeFallbackCount =
                    genericFallbacks.Count,
                playableWithoutRendererCount =
                    missingRenderers.Count,
                supportingContentCorrectionTargetCount =
                    supportingTargets.Distinct(StringComparer.Ordinal).Count(),
                officialContentCorrectionTargetCount =
                    officialTargets.Distinct(StringComparer.Ordinal).Count(),
                polishContentCorrectionTargetCount =
                    polishTargets.Distinct(StringComparer.Ordinal).Count(),
                stage6ContentCorrectionTargetCount =
                    stage6Targets.Distinct(StringComparer.Ordinal).Count()
            },
            knownRegressionFixtures = new[]
            {
                "PED:CAMBRIDGE-INTL-MATH:S6:6AS-MD-1:BUILD",
                "PED:CAMBRIDGE-INTL-MATH:S6:6F-3:BUILD",
                "PED:CAMBRIDGE-INTL-MATH:S6:6NPV-4:BUILD",
                "PED:CAMBRIDGE-INTL-MATH:S6:6F-2:BUILD"
            },
            missingPracticeCapabilities =
                missingCapabilities.OrderBy(x => x, StringComparer.Ordinal).ToArray(),
            genericLessonPracticeFallbacks =
                genericFallbacks.OrderBy(x => x, StringComparer.Ordinal).ToArray(),
            contentCorrectionTargets = new
            {
                supporting = supportingTargets
                    .Distinct(StringComparer.Ordinal)
                    .OrderBy(x => x, StringComparer.Ordinal)
                    .ToArray(),
                official = officialTargets
                    .Distinct(StringComparer.Ordinal)
                    .OrderBy(x => x, StringComparer.Ordinal)
                    .ToArray(),
                polish = polishTargets
                    .Distinct(StringComparer.Ordinal)
                    .OrderBy(x => x, StringComparer.Ordinal)
                    .ToArray(),
                stage6 = stage6Targets
                    .Distinct(StringComparer.Ordinal)
                    .OrderBy(x => x, StringComparer.Ordinal)
                    .ToArray()
            },
            lessons = rows
        };

        var root = FindRoot();
        var artifactDirectory = Path.Combine(
            root,
            "artifacts",
            "math-intelligence");
        Directory.CreateDirectory(artifactDirectory);

        var output = Path.Combine(
            artifactDirectory,
            "catalogue-content-practice-divergence.json");

        File.WriteAllText(
            output,
            JsonSerializer.Serialize(
                report,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                }));

        Console.WriteLine(
            JsonSerializer.Serialize(
                report.summary,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                }));

        Assert.Equal(4453, rows.Count);
        Assert.Empty(missingCapabilities);

        var fixtureRows = rows
            .Where(row =>
            {
                var code = row.GetType()
                    .GetProperty("lessonCode")?
                    .GetValue(row)?
                    .ToString();
                return report.knownRegressionFixtures.Contains(
                    code,
                    StringComparer.Ordinal);
            })
            .ToArray();

        Assert.Equal(4, fixtureRows.Length);
    }

    private static CanonicalLessonContentPackTranslation? ChooseTranslation(
        CanonicalLessonContentPackLesson lesson) =>
        lesson.Translations.FirstOrDefault(x =>
            x.CultureCode.StartsWith(
                "en",
                StringComparison.OrdinalIgnoreCase))
        ?? lesson.Translations.FirstOrDefault();

    private static string BuildContext(
        CanonicalLessonContentPackTranslation? translation)
    {
        if (translation is null)
            return string.Empty;

        return string.Join(
            ". ",
            new[]
            {
                translation.Title,
                translation.Explanation,
                translation.KeyConceptsAndRules,
                translation.WorkedExamples,
                translation.StepByStepSolutions,
                translation.CommonMistakes,
                translation.QuickSummary
            }.Where(x => !string.IsNullOrWhiteSpace(x)));
    }

    private static string FindRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            if (File.Exists(
                    Path.Combine(
                        directory.FullName,
                        "Edulytics.sln")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException(
            "Edulytics solution root not found.");
    }
}
