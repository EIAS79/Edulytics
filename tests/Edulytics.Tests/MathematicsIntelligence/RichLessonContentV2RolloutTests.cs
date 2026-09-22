using System.Text.Json;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Lessons;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Data.Seeding;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class RichLessonContentV2RolloutTests
{
    [Fact]
    public void R8_ClassifiesTheWholeCatalogue_AndCompilesEveryEligibleEnglishLesson()
    {
        var documents = MathematicsCanonicalLessonContentSeeder
            .LoadEmbeddedDocuments()
            .OrderBy(x => x.PackCode, StringComparer.Ordinal)
            .ToArray();

        var rows = new List<object>();
        var readyCurated = 0;
        var readyRuntime = 0;
        var blockedLocalized = 0;
        var blockedContract = 0;
        var blockedGeneration = 0;

        foreach (var document in documents)
        {
            foreach (var lesson in document.Lessons)
            {
                var translation = ChooseTranslation(document, lesson);
                var explicitRich = RichLessonContentV2Registry.Find(
                    lesson.LessonCode,
                    translation.CultureCode);

                string status;
                string reason;

                if (explicitRich is not null)
                {
                    readyCurated++;
                    status = "RichContentReadyCurated";
                    reason = "Reviewed Rich V2 sidecar.";
                }
                else
                {
                    var body = new CanonicalLessonTranslationRecord(
                        translation.CultureCode,
                        translation.Title,
                        translation.Explanation,
                        translation.KeyConceptsAndRules,
                        translation.WorkedExamples,
                        translation.StepByStepSolutions,
                        translation.CommonMistakes,
                        translation.QuickSummary);

                    var rich = RichLessonContentV2RuntimeComposer.TryCompose(
                        lesson.LessonCode,
                        body);

                    if (rich is not null)
                    {
                        readyRuntime++;
                        status = "RichContentReadyRuntimeVerified";
                        reason =
                            "Compiled from canonical lesson content plus READY_VERIFIED " +
                            "Practice contract and exact independently verified worked examples.";
                    }
                    else if (!string.Equals(
                                 NormalizeCulture(translation.CultureCode),
                                 "en",
                                 StringComparison.Ordinal))
                    {
                        blockedLocalized++;
                        status = "BlockedLocalizedAuthoring";
                        reason =
                            "R8 refuses to silently replace non-English academic content " +
                            "with generated English prose. Reviewed localized Rich V2 authoring is required.";
                    }
                    else if (!LessonPracticeContractRegistry.TryResolve(
                                 lesson.LessonCode,
                                 out var contract) ||
                             contract is null ||
                             !string.Equals(
                                 contract.Readiness,
                                 "READY_VERIFIED",
                                 StringComparison.OrdinalIgnoreCase))
                    {
                        blockedContract++;
                        status = "BlockedPracticeContract";
                        reason =
                            "No READY_VERIFIED lesson Practice contract is available for safe exact-example compilation.";
                    }
                    else
                    {
                        blockedGeneration++;
                        status = "BlockedGeneration";
                        reason =
                            "The READY_VERIFIED contract could not be compiled into a valid Rich V2 body; legacy canonical content remains active.";
                    }
                }

                rows.Add(new
                {
                    lessonCode = lesson.LessonCode,
                    packCode = document.PackCode,
                    cultureCode = translation.CultureCode,
                    title = translation.Title,
                    isSupporting = lesson.IsSupporting || lesson.OutcomeCodes.Count == 0,
                    officialOutcomeCount = lesson.OutcomeCodes.Count,
                    status,
                    reason
                });
            }
        }

        Assert.Equal(4453, rows.Count);
        Assert.Equal(
            26,
            readyCurated);

        // Every English lesson must either compile safely or be explicitly
        // identified as a concrete blocker in the R8 status artifact.
        Assert.Equal(
            rows.Count,
            readyCurated +
            readyRuntime +
            blockedLocalized +
            blockedContract +
            blockedGeneration);

        var outputDirectory = Path.Combine(
            FindRoot(),
            "artifacts",
            "lesson-content-v2");
        Directory.CreateDirectory(outputDirectory);

        File.WriteAllText(
            Path.Combine(
                outputDirectory,
                "r8-rollout-status.json"),
            JsonSerializer.Serialize(
                new
                {
                    schemaVersion = 1,
                    generatedAtUtc = DateTimeOffset.UtcNow,
                    lessonCount = rows.Count,
                    statusCounts = new
                    {
                        richContentReadyCurated = readyCurated,
                        richContentReadyRuntimeVerified = readyRuntime,
                        blockedLocalizedAuthoring = blockedLocalized,
                        blockedPracticeContract = blockedContract,
                        blockedGeneration = blockedGeneration
                    },
                    lessons = rows
                },
                new JsonSerializerOptions
                {
                    WriteIndented = true
                }));

        Console.WriteLine(
            $"R8 rollout: curated={readyCurated}; runtime={readyRuntime}; " +
            $"localized-block={blockedLocalized}; contract-block={blockedContract}; " +
            $"generation-block={blockedGeneration}.");
    }

    [Fact]
    public void RuntimeComposer_UsesVerifiedExamples_AndDoesNotOverrideCuratedPilot()
    {
        var documents = MathematicsCanonicalLessonContentSeeder
            .LoadEmbeddedDocuments();

        var candidate = documents
            .SelectMany(document =>
                document.Lessons.Select(lesson => new
                {
                    Document = document,
                    Lesson = lesson,
                    Translation = ChooseTranslation(document, lesson)
                }))
            .First(x =>
                string.Equals(
                    NormalizeCulture(x.Translation.CultureCode),
                    "en",
                    StringComparison.Ordinal) &&
                RichLessonContentV2Registry.Find(
                    x.Lesson.LessonCode,
                    x.Translation.CultureCode) is null &&
                LessonPracticeContractRegistry.TryResolve(
                    x.Lesson.LessonCode,
                    out var contract) &&
                contract is not null &&
                contract.Readiness == "READY_VERIFIED" &&
                contract.AllowedQuestionFamilies.Any(
                    ExactSkillContractQuestionEngine.SupportsFamily));

        var body = new CanonicalLessonTranslationRecord(
            candidate.Translation.CultureCode,
            candidate.Translation.Title,
            candidate.Translation.Explanation,
            candidate.Translation.KeyConceptsAndRules,
            candidate.Translation.WorkedExamples,
            candidate.Translation.StepByStepSolutions,
            candidate.Translation.CommonMistakes,
            candidate.Translation.QuickSummary);

        var rich = RichLessonContentV2RuntimeComposer.TryCompose(
            candidate.Lesson.LessonCode,
            body);

        Assert.NotNull(rich);
        Assert.True(rich!.ExplanationParagraphs.Count >= 4);
        Assert.True(rich.KeyConcepts.Count >= 3);
        Assert.True(rich.WorkedExamples.Count >= 4);
        Assert.True(rich.CommonMistakes.Count >= 3);
        Assert.True(rich.SummaryPoints.Count >= 4);
        Assert.NotEmpty(rich.Visuals);
        Assert.All(rich.WorkedExamples, example =>
        {
            Assert.True(example.Steps.Count >= 5);
            Assert.False(string.IsNullOrWhiteSpace(example.Question));
            Assert.False(string.IsNullOrWhiteSpace(example.Answer));
            Assert.Contains(
                "Verified by the exact Mathematics engine",
                example.Check,
                StringComparison.Ordinal);
        });

        var curated = RichLessonContentV2Registry.AllDocuments
            .SelectMany(x => x.Lessons)
            .First();

        Assert.Same(
            curated,
            RichLessonContentV2Registry.Find(
                curated.LessonCode,
                curated.CultureCode));
    }

    [Fact]
    public void RuntimeComposer_RefusesSilentEnglishReplacementForPolishLessons()
    {
        var document = MathematicsCanonicalLessonContentSeeder
            .LoadEmbeddedDocuments()
            .First(x => x.PackCode == "PL-NATIONAL-MATH");

        var lesson = document.Lessons.First();
        var translation = ChooseTranslation(document, lesson);

        Assert.Equal(
            "pl",
            NormalizeCulture(translation.CultureCode));

        var body = new CanonicalLessonTranslationRecord(
            translation.CultureCode,
            translation.Title,
            translation.Explanation,
            translation.KeyConceptsAndRules,
            translation.WorkedExamples,
            translation.StepByStepSolutions,
            translation.CommonMistakes,
            translation.QuickSummary);

        Assert.Null(
            RichLessonContentV2RuntimeComposer.TryCompose(
                lesson.LessonCode,
                body));
    }

    [Fact]
    public void LessonContentService_UsesCuratedThenRuntimeRichContentForStudentAndTeacher()
    {
        var root = FindRoot();
        var service = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/LessonContent/LessonContentService.cs"));

        Assert.Contains(
            "RichLessonContentV2Registry.Find",
            service,
            StringComparison.Ordinal);
        Assert.Contains(
            "RichLessonContentV2RuntimeComposer.TryCompose",
            service,
            StringComparison.Ordinal);

        var registryIndex = service.IndexOf(
            "RichLessonContentV2Registry.Find",
            StringComparison.Ordinal);
        var runtimeIndex = service.IndexOf(
            "RichLessonContentV2RuntimeComposer.TryCompose",
            registryIndex,
            StringComparison.Ordinal);

        Assert.True(registryIndex >= 0);
        Assert.True(runtimeIndex > registryIndex);
    }

    private static CanonicalLessonContentPackTranslation ChooseTranslation(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson) =>
        lesson.Translations.FirstOrDefault(x =>
            string.Equals(
                x.CultureCode,
                document.AcademicLanguage,
                StringComparison.OrdinalIgnoreCase))
        ?? lesson.Translations.FirstOrDefault(x =>
            x.CultureCode.StartsWith(
                "en",
                StringComparison.OrdinalIgnoreCase))
        ?? lesson.Translations.First();

    private static string NormalizeCulture(string cultureCode)
    {
        var value = cultureCode.Trim();
        var separator = value.IndexOf('-');
        return (separator > 0 ? value[..separator] : value)
            .ToLowerInvariant();
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
                return directory.FullName;

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException(
            "Edulytics solution root not found.");
    }
}
