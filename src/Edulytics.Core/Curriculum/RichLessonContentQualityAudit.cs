using System.Text.RegularExpressions;

namespace Edulytics.Core.Curriculum;

public enum RichLessonSectionQuality
{
    Missing = 0,
    Generic = 1,
    NeedsExpansion = 2,
    Strong = 3
}

public enum RichLessonVisualQuality
{
    GenericFallbackOnly = 0,
    TopicVisualEvidence = 1,
    ExplicitInstructionalVisual = 2
}

public enum RichLessonVideoStatus
{
    None = 0,
    Candidate = 1,
    Reviewed = 2,
    Approved = 3,
    Unavailable = 4,
    Rejected = 5
}

public enum RichLessonOverallQuality
{
    Good = 1,
    NeedsExpansion = 2,
    Generic = 3,
    SourceResearchRequired = 4,
    RequiresAcademicReview = 5
}

public sealed record RichLessonSectionAudit(
    RichLessonSectionQuality Quality,
    int CharacterCount,
    int SentenceCount,
    int ConcreteMathSignalCount,
    int StructureMarkerCount,
    IReadOnlyList<string> Findings);

public sealed record RichLessonContentAuditResult(
    string LessonCode,
    string PackCode,
    bool IsSupporting,
    int OutcomeCount,
    string Title,
    RichLessonSectionAudit Explanation,
    RichLessonSectionAudit KeyConceptsAndRules,
    RichLessonSectionAudit WorkedExamples,
    RichLessonSectionAudit StepByStepSolutions,
    RichLessonSectionAudit CommonMistakes,
    RichLessonSectionAudit QuickSummary,
    RichLessonVisualQuality VisualQuality,
    RichLessonVideoStatus VideoStatus,
    bool SourceResearchRequired,
    RichLessonOverallQuality OverallQuality,
    IReadOnlyList<string> Findings);

/// <summary>
/// Deterministic first-pass quality contract for Rich Lesson Content V2.
///
/// This evaluator intentionally does not rewrite lesson content and does not
/// alter curriculum mappings. It measures whether the existing learner-facing
/// body has enough concrete structure to warrant academic review as a rich
/// lesson. Passing this heuristic is not a substitute for mathematical review.
/// </summary>
public static partial class RichLessonContentQualityAudit
{
    private static readonly Regex SentenceRegex =
        new(@"(?<=[.!?])\s+", RegexOptions.CultureInvariant | RegexOptions.Compiled);

    private static readonly Regex MathSignalRegex =
        new(
            @"(?:\d|[=<>±×÷+−*/^%]|\b(?:fraction|decimal|percent|ratio|equation|angle|area|perimeter|coordinate|graph|probability|mean|median|mode|range|integer|multiple|factor|prime|volume|scale|proportion|ułam|procent|równan|kąt|pole|obwód|współrzęd|wykres)\w*)",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

    private static readonly Regex ExampleMarkerRegex =
        new(
            @"\b(?:worked\s+example|example|przykład)\s*\d*\b",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

    private static readonly Regex StepMarkerRegex =
        new(
            @"\b(?:step|krok)\s*\d+\b",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

    private static readonly Regex MistakeMarkerRegex =
        new(
            @"\b(?:mistake|wrong|incorrect|avoid|do\s+not|common\s+error|błąd|źle|niepoprawn|unikaj)\w*",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

    private static readonly Regex VisualDescriptorRegex =
        new(
            @"\bDescription\s*:",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

    private static readonly Regex VisualTermRegex =
        new(
            @"\b(?:number\s+line|double\s+number\s+line|fraction\s+bar|ratio\s+bar|tape\s+diagram|coordinate\s+plane|array|grid|diagram|graph|chart|geometric\s+figure|triangle|rectangle|polygon|scale|place[- ]value\s+chart|oś\s+liczbowa|wykres|diagram|siatka)\b",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

    private static readonly Regex GenericTemplateRegex =
        new(
            @"(?:read the problem and identify the quantities or properties|" +
            @"represent the situation with numbers, a diagram, a number line, an equation or a labelled shape|" +
            @"apply the relevant rule while preserving place value, units and relationships|" +
            @"calculate carefully and write the result with its meaning|" +
            @"check using an inverse operation, estimation, an alternative representation or the stated geometric properties|" +
            @"identify the invariant relationship, apply the required scale or inverse operation, then verify|" +
            @"use known number relationships to derive new exact results|" +
            @"build the idea by representing it in more than one way and explaining why the representations agree|" +
            @"understand, represent, check)",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Singleline | RegexOptions.Compiled);

    public static RichLessonContentAuditResult Evaluate(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        CanonicalLessonContentPackTranslation translation)
    {
        ArgumentNullException.ThrowIfNull(document);
        ArgumentNullException.ThrowIfNull(lesson);
        ArgumentNullException.ThrowIfNull(translation);

        var explanation = AuditSection(
            translation.Explanation,
            minimumCharacters: 320,
            minimumSentences: 4,
            minimumMathSignals: 2,
            markerCount: 0);

        var concepts = AuditSection(
            translation.KeyConceptsAndRules,
            minimumCharacters: 240,
            minimumSentences: 3,
            minimumMathSignals: 2,
            markerCount: 0,
            duplicateAgainst: translation.Explanation);

        var worked = AuditSection(
            translation.WorkedExamples,
            minimumCharacters: 320,
            minimumSentences: 3,
            minimumMathSignals: 4,
            markerCount: ExampleMarkerRegex.Matches(translation.WorkedExamples ?? string.Empty).Count,
            minimumMarkers: 2);

        var steps = AuditSection(
            translation.StepByStepSolutions,
            minimumCharacters: 360,
            minimumSentences: 4,
            minimumMathSignals: 4,
            markerCount: StepMarkerRegex.Matches(translation.StepByStepSolutions ?? string.Empty).Count,
            minimumMarkers: 3);

        var mistakes = AuditSection(
            translation.CommonMistakes,
            minimumCharacters: 220,
            minimumSentences: 3,
            minimumMathSignals: 1,
            markerCount: MistakeMarkerRegex.Matches(translation.CommonMistakes ?? string.Empty).Count,
            minimumMarkers: 2);

        var summary = AuditSection(
            translation.QuickSummary,
            minimumCharacters: 140,
            minimumSentences: 2,
            minimumMathSignals: 1,
            markerCount: 0);

        var allBody = string.Join(
            "\n",
            translation.Explanation,
            translation.KeyConceptsAndRules,
            translation.WorkedExamples,
            translation.StepByStepSolutions,
            translation.CommonMistakes,
            translation.QuickSummary);

        var visualQuality =
            VisualDescriptorRegex.IsMatch(allBody)
                ? RichLessonVisualQuality.ExplicitInstructionalVisual
                : VisualTermRegex.IsMatch(allBody)
                    ? RichLessonVisualQuality.TopicVisualEvidence
                    : RichLessonVisualQuality.GenericFallbackOnly;

        var sourceResearchRequired =
            document.SourcePolicyVersion < 2 ||
            document.PedagogicalSourceType == PedagogicalSourceType.LegacyUnspecified ||
            string.IsNullOrWhiteSpace(document.PedagogicalSourceSelectionReason) ||
            string.IsNullOrWhiteSpace(document.PedagogicalSourceRightsNote);

        var sections = new[]
        {
            explanation,
            concepts,
            worked,
            steps,
            mistakes,
            summary
        };

        var genericCount = sections.Count(x => x.Quality == RichLessonSectionQuality.Generic);
        var weakCount = sections.Count(x =>
            x.Quality is RichLessonSectionQuality.Missing or
            RichLessonSectionQuality.NeedsExpansion);

        var findings = new List<string>();

        if (sourceResearchRequired)
            findings.Add("Pedagogical source dossier requires Source Policy v2 research/rights evidence.");

        if (genericCount > 0)
            findings.Add($"{genericCount} lesson section(s) contain generic/template teaching language.");

        if (weakCount > 0)
            findings.Add($"{weakCount} lesson section(s) do not yet meet the Rich Lesson Content V2 structural floor.");

        if (visualQuality == RichLessonVisualQuality.GenericFallbackOnly)
            findings.Add("No explicit topic-specific instructional visual evidence is present in the canonical lesson body.");

        var overall =
            sourceResearchRequired
                ? RichLessonOverallQuality.SourceResearchRequired
                : genericCount >= 2
                    ? RichLessonOverallQuality.Generic
                    : weakCount > 0 ||
                      genericCount > 0 ||
                      visualQuality == RichLessonVisualQuality.GenericFallbackOnly
                        ? RichLessonOverallQuality.NeedsExpansion
                        : RichLessonOverallQuality.Good;

        return new(
            lesson.LessonCode,
            document.PackCode,
            lesson.IsSupporting || lesson.OutcomeCodes.Count == 0,
            lesson.OutcomeCodes.Count,
            translation.Title,
            explanation,
            concepts,
            worked,
            steps,
            mistakes,
            summary,
            visualQuality,
            RichLessonVideoStatus.None,
            sourceResearchRequired,
            overall,
            findings);
    }

    private static RichLessonSectionAudit AuditSection(
        string? raw,
        int minimumCharacters,
        int minimumSentences,
        int minimumMathSignals,
        int markerCount,
        int minimumMarkers = 0,
        string? duplicateAgainst = null)
    {
        var value = Normalize(raw);
        var findings = new List<string>();

        if (value.Length == 0)
        {
            return new(
                RichLessonSectionQuality.Missing,
                0,
                0,
                0,
                markerCount,
                ["Section is empty."]);
        }

        var sentenceCount = SentenceRegex
            .Split(value)
            .Count(x => !string.IsNullOrWhiteSpace(x));

        var mathSignals = MathSignalRegex.Matches(value).Count;

        var generic = GenericTemplateRegex.IsMatch(value);
        if (generic)
            findings.Add("Contains known generic/template teaching language.");

        var duplicate = !string.IsNullOrWhiteSpace(duplicateAgainst) &&
            string.Equals(
                NormalizeForComparison(value),
                NormalizeForComparison(duplicateAgainst),
                StringComparison.Ordinal);

        if (duplicate)
            findings.Add("Repeats another lesson section instead of providing section-specific teaching content.");

        if (value.Length < minimumCharacters)
            findings.Add($"Too short for the V2 structural floor ({value.Length} < {minimumCharacters} characters).");

        if (sentenceCount < minimumSentences)
            findings.Add($"Too little explanatory structure ({sentenceCount} < {minimumSentences} sentence/block units).");

        if (mathSignals < minimumMathSignals)
            findings.Add($"Insufficient concrete mathematical signals ({mathSignals} < {minimumMathSignals}).");

        if (markerCount < minimumMarkers)
            findings.Add($"Insufficient explicit worked structure ({markerCount} < {minimumMarkers} markers).");

        var quality =
            generic || duplicate
                ? RichLessonSectionQuality.Generic
                : findings.Count == 0
                    ? RichLessonSectionQuality.Strong
                    : RichLessonSectionQuality.NeedsExpansion;

        return new(
            quality,
            value.Length,
            sentenceCount,
            mathSignals,
            markerCount,
            findings);
    }

    private static string Normalize(string? value) =>
        Regex.Replace(value ?? string.Empty, @"\s+", " ").Trim();

    private static string NormalizeForComparison(string? value) =>
        Normalize(value).ToLowerInvariant();
}
