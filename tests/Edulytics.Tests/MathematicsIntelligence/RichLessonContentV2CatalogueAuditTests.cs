using System.Text.Json;
using System.Text.RegularExpressions;
using Edulytics.Core.Curriculum;
using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class RichLessonContentV2CatalogueAuditTests
{
    [Fact]
    public void WriteCatalogueWideRichLessonContentV2Audit()
    {
        var documents = MathematicsCanonicalLessonContentSeeder
            .LoadEmbeddedDocuments()
            .OrderBy(x => x.PackCode, StringComparer.Ordinal)
            .ThenBy(x => x.VersionCode, StringComparer.Ordinal)
            .ToArray();

        var blueprintMetadata = PedagogicalLessonBlueprintRegistry
            .LoadEmbeddedDocuments()
            .SelectMany(document => document.Lessons.Select(lesson => new
            {
                lesson.LessonCode,
                lesson.UnitTitle,
                NativeLevel =
                    document.SchemaVersion == 1
                        ? document.NativeLevel
                        : string.Empty,
                LogicalLevel =
                    document.SchemaVersion == 1
                        ? document.LogicalLevel
                        : 0,
                document.CourseCode,
                document.Pathway
            }))
            .GroupBy(x => x.LessonCode, StringComparer.Ordinal)
            .ToDictionary(
                x => x.Key,
                x => x.First(),
                StringComparer.Ordinal);

        var rows = documents
            .SelectMany(document =>
                document.Lessons.Select(lesson =>
                {
                    var translation = ChooseTranslation(document, lesson);
                    var audit = RichLessonContentQualityAudit.Evaluate(
                        document,
                        lesson,
                        translation);

                    blueprintMetadata.TryGetValue(
                        lesson.LessonCode,
                        out var blueprint);

                    var gradeLevel =
                        !string.IsNullOrWhiteSpace(blueprint?.NativeLevel)
                            ? blueprint.NativeLevel
                            : InferLevel(lesson.LessonCode);

                    var unit =
                        !string.IsNullOrWhiteSpace(blueprint?.UnitTitle)
                            ? blueprint.UnitTitle
                            : !string.IsNullOrWhiteSpace(lesson.SourceTitle)
                                ? lesson.SourceTitle
                                : "<not-modelled-in-content-pack>";

                    return new
                    {
                        lessonId = lesson.LessonCode,
                        lessonCode = lesson.LessonCode,
                        curriculum = document.PackCode,
                        frameworkAuthority = document.SourceAuthority,
                        gradeLevel,
                        unit,
                        topic = translation.Title,
                        title = translation.Title,
                        officialOutcomeCount = lesson.OutcomeCodes.Count,
                        isSupporting =
                            lesson.IsSupporting ||
                            lesson.OutcomeCodes.Count == 0,
                        contentVersion = document.ContentVersion,
                        sourcePolicyVersion = document.SourcePolicyVersion,
                        pedagogicalSourceType =
                            document.PedagogicalSourceType.ToString(),
                        pedagogicalSourceTitle =
                            document.PedagogicalSourceTitle,
                        pedagogicalSourcePublisher =
                            document.PedagogicalSourcePublisher,
                        pedagogicalSourceUrl =
                            document.PedagogicalSourceUrl,
                        pedagogicalSourceRights =
                            document.PedagogicalSourceRightsNote,
                        sourceResearchRequired =
                            audit.SourceResearchRequired,
                        explanation = ToSection(audit.Explanation),
                        keyConceptsAndRules =
                            ToSection(audit.KeyConceptsAndRules),
                        workedExamples =
                            ToSection(audit.WorkedExamples),
                        stepByStepSolutions =
                            ToSection(audit.StepByStepSolutions),
                        commonMistakes =
                            ToSection(audit.CommonMistakes),
                        quickSummary =
                            ToSection(audit.QuickSummary),
                        visualQuality =
                            audit.VisualQuality.ToString(),
                        videoStatus =
                            audit.VideoStatus.ToString(),
                        overallQuality =
                            audit.OverallQuality.ToString(),
                        findings = audit.Findings
                    };
                }))
            .OrderBy(x => x.curriculum, StringComparer.Ordinal)
            .ThenBy(x => x.gradeLevel, StringComparer.Ordinal)
            .ThenBy(x => x.unit, StringComparer.Ordinal)
            .ThenBy(x => x.title, StringComparer.Ordinal)
            .ThenBy(x => x.lessonCode, StringComparer.Ordinal)
            .ToArray();

        Assert.NotEmpty(rows);
        Assert.Equal(
            rows.Length,
            rows.Select(x => x.lessonCode)
                .Distinct(StringComparer.Ordinal)
                .Count());
        Assert.All(rows, row =>
        {
            Assert.False(string.IsNullOrWhiteSpace(row.curriculum));
            Assert.False(string.IsNullOrWhiteSpace(row.title));
            Assert.False(string.IsNullOrWhiteSpace(row.overallQuality));
        });

        var summary = new
        {
            generatedAtUtc = DateTimeOffset.UtcNow,
            lessonCount = rows.Length,
            curriculumCount =
                rows.Select(x => x.curriculum)
                    .Distinct(StringComparer.Ordinal)
                    .Count(),
            officialLessonCount =
                rows.Count(x => !x.isSupporting),
            supportingLessonCount =
                rows.Count(x => x.isSupporting),
            sourceResearchRequiredCount =
                rows.Count(x => x.sourceResearchRequired),
            genericFallbackVisualCount =
                rows.Count(x =>
                    x.visualQuality ==
                    RichLessonVisualQuality
                        .GenericFallbackOnly
                        .ToString()),
            noVideoCount =
                rows.Count(x =>
                    x.videoStatus ==
                    RichLessonVideoStatus.None.ToString()),
            quality = rows
                .GroupBy(x => x.overallQuality, StringComparer.Ordinal)
                .OrderBy(x => x.Key, StringComparer.Ordinal)
                .ToDictionary(
                    x => x.Key,
                    x => x.Count(),
                    StringComparer.Ordinal),
            byCurriculum = rows
                .GroupBy(x => x.curriculum, StringComparer.Ordinal)
                .OrderBy(x => x.Key, StringComparer.Ordinal)
                .ToDictionary(
                    x => x.Key,
                    x => new
                    {
                        lessons = x.Count(),
                        official = x.Count(row => !row.isSupporting),
                        supporting = x.Count(row => row.isSupporting),
                        good = x.Count(row =>
                            row.overallQuality ==
                            RichLessonOverallQuality.Good.ToString()),
                        needsExpansion = x.Count(row =>
                            row.overallQuality ==
                            RichLessonOverallQuality.NeedsExpansion.ToString()),
                        generic = x.Count(row =>
                            row.overallQuality ==
                            RichLessonOverallQuality.Generic.ToString()),
                        sourceResearchRequired = x.Count(row =>
                            row.overallQuality ==
                            RichLessonOverallQuality.SourceResearchRequired.ToString())
                    },
                    StringComparer.Ordinal)
        };

        var root = FindRoot();
        var outputDirectory = Path.Combine(
            root,
            "artifacts",
            "lesson-content-v2");
        Directory.CreateDirectory(outputDirectory);

        WriteJson(
            Path.Combine(
                outputDirectory,
                "catalogue-inventory.json"),
            new
            {
                schemaVersion = 1,
                programme = "RichLessonContentV2",
                summary,
                lessons = rows.Select(row => new
                {
                    row.lessonId,
                    row.lessonCode,
                    row.curriculum,
                    row.frameworkAuthority,
                    row.gradeLevel,
                    row.unit,
                    row.topic,
                    row.title,
                    row.officialOutcomeCount,
                    row.isSupporting,
                    row.contentVersion,
                    row.sourcePolicyVersion,
                    row.pedagogicalSourceType
                })
            });

        WriteJson(
            Path.Combine(
                outputDirectory,
                "content-quality-audit.json"),
            new
            {
                schemaVersion = 1,
                programme = "RichLessonContentV2",
                authority =
                    nameof(RichLessonContentQualityAudit),
                summary,
                lessons = rows
            });

        File.WriteAllText(
            Path.Combine(
                outputDirectory,
                "catalogue-summary.md"),
            BuildMarkdownSummary(summary));

        Console.WriteLine(
            JsonSerializer.Serialize(
                summary,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                }));
    }

    [Fact]
    public void QualityContract_FlagsKnownThinSupportingLesson()
    {
        var document = MathematicsCanonicalLessonContentSeeder
            .LoadEmbeddedDocuments()
            .Single(x =>
                x.PackCode == "CAMBRIDGE-INTL-MATH" &&
                x.Lessons.Any(lesson =>
                    lesson.LessonCode ==
                    "PED:CAMBRIDGE-INTL-MATH:S6:6AS-MD-2:BUILD"));

        var lesson = Assert.Single(
            document.Lessons,
            x =>
                x.LessonCode ==
                "PED:CAMBRIDGE-INTL-MATH:S6:6AS-MD-2:BUILD");

        var translation = ChooseTranslation(document, lesson);
        var audit = RichLessonContentQualityAudit.Evaluate(
            document,
            lesson,
            translation);

        Assert.NotEqual(
            RichLessonOverallQuality.Good,
            audit.OverallQuality);
        Assert.True(
            audit.WorkedExamples.Quality is
                RichLessonSectionQuality.Generic or
                RichLessonSectionQuality.NeedsExpansion);
        Assert.True(
            audit.StepByStepSolutions.Quality is
                RichLessonSectionQuality.Generic or
                RichLessonSectionQuality.NeedsExpansion);
    }

    private static object ToSection(
        RichLessonSectionAudit section) =>
        new
        {
            quality = section.Quality.ToString(),
            section.CharacterCount,
            section.SentenceCount,
            section.ConcreteMathSignalCount,
            section.StructureMarkerCount,
            section.Findings
        };

    private static CanonicalLessonContentPackTranslation ChooseTranslation(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson)
    {
        return lesson.Translations.FirstOrDefault(x =>
                string.Equals(
                    x.CultureCode,
                    document.AcademicLanguage,
                    StringComparison.OrdinalIgnoreCase))
            ?? lesson.Translations.FirstOrDefault(x =>
                x.CultureCode.StartsWith(
                    "en",
                    StringComparison.OrdinalIgnoreCase))
            ?? lesson.Translations.First();
    }

    private static string InferLevel(string lessonCode)
    {
        foreach (var pattern in new[]
        {
            @":(?:L|G|S)(?<level>\d+)(?=:)",
            @":GRADE-(?<level>\d+)(?=:)"
        })
        {
            var match = Regex.Match(
                lessonCode,
                pattern,
                RegexOptions.IgnoreCase |
                RegexOptions.CultureInvariant);

            if (match.Success)
                return match.Groups["level"].Value;
        }

        if (lessonCode.Contains(
                ":AS:",
                StringComparison.OrdinalIgnoreCase))
            return "AS";

        if (lessonCode.Contains(
                ":A2:",
                StringComparison.OrdinalIgnoreCase) ||
            lessonCode.Contains(
                "A-LEVEL",
                StringComparison.OrdinalIgnoreCase))
            return "A-Level";

        return "<not-encoded>";
    }

    private static void WriteJson(string path, object value)
    {
        File.WriteAllText(
            path,
            JsonSerializer.Serialize(
                value,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                }));
    }

    private static string BuildMarkdownSummary(
        object summary)
    {
        var data = JsonSerializer.SerializeToElement(summary);
        var quality = data.GetProperty("quality");

        var lines = new List<string>
        {
            "# Rich Lesson Content V2 — Catalogue Audit Summary",
            string.Empty,
            $"- Lessons: **{data.GetProperty("lessonCount").GetInt32()}**",
            $"- Curricula: **{data.GetProperty("curriculumCount").GetInt32()}**",
            $"- Official lessons: **{data.GetProperty("officialLessonCount").GetInt32()}**",
            $"- Supporting lessons: **{data.GetProperty("supportingLessonCount").GetInt32()}**",
            $"- Source research required: **{data.GetProperty("sourceResearchRequiredCount").GetInt32()}**",
            $"- Generic-fallback-only visual evidence: **{data.GetProperty("genericFallbackVisualCount").GetInt32()}**",
            $"- Lessons with no curated video: **{data.GetProperty("noVideoCount").GetInt32()}**",
            string.Empty,
            "## Overall quality",
            string.Empty
        };

        foreach (var item in quality.EnumerateObject())
            lines.Add($"- {item.Name}: **{item.Value.GetInt32()}**");

        lines.Add(string.Empty);
        lines.Add(
            "This is a deterministic first-pass structural audit. " +
            "It is not a substitute for mathematical or academic review.");

        return string.Join(
            Environment.NewLine,
            lines);
    }

    private static string FindRoot()
    {
        var directory =
            new DirectoryInfo(AppContext.BaseDirectory);

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
