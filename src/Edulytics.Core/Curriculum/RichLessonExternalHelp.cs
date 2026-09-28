using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Core.Curriculum;

public enum RichLessonExternalResourceReviewStatus
{
    Candidate = 0,
    Reviewed = 1,
    Approved = 2,
    Unavailable = 3,
    Rejected = 4
}

public sealed record RichLessonExternalResource(
    string Provider,
    string Title,
    string Url,
    string Language,
    string WhyRecommended,
    RichLessonExternalResourceReviewStatus ReviewStatus,
    string CheckedAtUtc);

public sealed record RichLessonSearchSuggestion(
    string Provider,
    string Label,
    string Query,
    string Url);

public sealed record RichLessonExternalHelp(
    IReadOnlyList<RichLessonExternalResource> ApprovedResources,
    IReadOnlyList<RichLessonSearchSuggestion> SearchSuggestions);

/// <summary>
/// Phase 8A learner help registry.
///
/// External resources are optional enrichment only. They never affect
/// lesson availability, Practice eligibility, Assessment/Diagnostic routing,
/// mastery, progression, or Adaptive decisions.
///
/// Curated resources are emitted only when they are explicitly Approved.
/// Search suggestions are deterministic and are built solely from the lesson
/// title and exact lesson SkillContract; no learner data is included.
/// </summary>
public static class RichLessonExternalHelpRegistry
{
    private static readonly IReadOnlyDictionary<string, RichLessonExternalResource[]>
        ResourcesBySkill =
            new Dictionary<string, RichLessonExternalResource[]>(
                StringComparer.Ordinal)
            {
                ["fractions.equivalent"] =
                [
                    Approved(
                        "Khan Academy",
                        "Visualizing equivalent fractions review",
                        "https://www.khanacademy.org/math/arithmetic/fraction-arithmetic/arith-review-equivalent-fractions/a/equivalent-fractions-review",
                        "en",
                        "Uses fraction models and number lines to reinforce equivalent-fraction meaning.")
                ],
                ["fractions.compare.unlike_denominators"] =
                [
                    Approved(
                        "Khan Academy",
                        "Equivalent fractions and comparing fractions: FAQ",
                        "https://www.khanacademy.org/math/cc-fourth-grade-math/comparing-fractions-and-equivalent-fractions/imp-equivalent-fractions-2/a/equivalent-fractions-and-comparing-fractions-faq-4",
                        "en",
                        "Reviews common-denominator and benchmark strategies for comparing fractions with unlike denominators.")
                ],
                ["ratio.unit_rate"] =
                [
                    Approved(
                        "Khan Academy",
                        "Rate review",
                        "https://www.khanacademy.org/test-prep/get-ready-for-sat-prep-math/x9eb58585c728c6ea%3Aget-ready-problem-solving-and-data-analysis/x9eb58585c728c6ea%3Aget-ready-ratios-rates-and-proportions-rates/a/rate-review",
                        "en",
                        "Reviews rates, unit rates, and practice problems using the same mathematical relationship.")
                ]
            };

    public static RichLessonExternalHelp Resolve(
        string lessonCode,
        string lessonTitle,
        string cultureCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(lessonCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(lessonTitle);

        LessonPracticeContractRegistry.TryResolve(
            lessonCode,
            out var contract);

        var approvedResources =
            contract is not null &&
            ResourcesBySkill.TryGetValue(
                contract.SkillId,
                out var resources)
                ? resources
                    .Where(x =>
                        x.ReviewStatus ==
                        RichLessonExternalResourceReviewStatus.Approved)
                    .ToArray()
                : [];

        var skillLabel = contract is null
            ? string.Empty
            : Humanize(contract.SkillId);

        var baseQuery = string.IsNullOrWhiteSpace(skillLabel)
            ? $"{lessonTitle} mathematics"
            : $"{lessonTitle} {skillLabel} mathematics";

        var language = NormalizeCulture(cultureCode);
        var explanationSuffix = language switch
        {
            "pl" => "wyjaśnienie krok po kroku",
            "ar" => "شرح خطوة بخطوة",
            _ => "step by step explanation"
        };
        var examplesSuffix = language switch
        {
            "pl" => "przykłady z rozwiązaniami",
            "ar" => "أمثلة محلولة",
            _ => "worked examples"
        };

        var googleQuery = $"{baseQuery} {explanationSuffix}".Trim();
        var youtubeQuery = $"{baseQuery} {examplesSuffix}".Trim();

        return new RichLessonExternalHelp(
            approvedResources,
            [
                new RichLessonSearchSuggestion(
                    "Google",
                    "Search the web",
                    googleQuery,
                    "https://www.google.com/search?q=" +
                    Uri.EscapeDataString(googleQuery)),
                new RichLessonSearchSuggestion(
                    "YouTube",
                    "Search YouTube",
                    youtubeQuery,
                    "https://www.youtube.com/results?search_query=" +
                    Uri.EscapeDataString(youtubeQuery))
            ]);
    }

    public static void Validate()
    {
        foreach (var resource in ResourcesBySkill.Values.SelectMany(x => x))
        {
            if (resource.ReviewStatus !=
                RichLessonExternalResourceReviewStatus.Approved)
            {
                continue;
            }

            if (string.IsNullOrWhiteSpace(resource.Provider) ||
                string.IsNullOrWhiteSpace(resource.Title) ||
                string.IsNullOrWhiteSpace(resource.WhyRecommended) ||
                !Uri.TryCreate(
                    resource.Url,
                    UriKind.Absolute,
                    out var uri) ||
                uri.Scheme != Uri.UriSchemeHttps ||
                !DateTimeOffset.TryParse(
                    resource.CheckedAtUtc,
                    out var checkedAt) ||
                checkedAt.Offset != TimeSpan.Zero)
            {
                throw new InvalidOperationException(
                    $"Invalid approved Rich Lesson external resource: {resource.Title}.");
            }
        }
    }

    private static RichLessonExternalResource Approved(
        string provider,
        string title,
        string url,
        string language,
        string whyRecommended) =>
        new(
            provider,
            title,
            url,
            language,
            whyRecommended,
            RichLessonExternalResourceReviewStatus.Approved,
            "2026-09-28T00:00:00Z");

    private static string Humanize(string value)
    {
        var tokens = value
            .Split(
                ['.', '_', '-'],
                StringSplitOptions.RemoveEmptyEntries);

        if (tokens.Length == 0)
            return string.Empty;

        return string.Join(
            " ",
            tokens.Select(token =>
                token.Length == 0
                    ? token
                    : char.ToUpperInvariant(token[0]) +
                      token[1..].ToLowerInvariant()));
    }

    private static string NormalizeCulture(string? cultureCode)
    {
        if (string.IsNullOrWhiteSpace(cultureCode))
            return "en";

        var value = cultureCode.Trim();
        var separator = value.IndexOf('-');
        return (separator > 0 ? value[..separator] : value)
            .ToLowerInvariant();
    }
}
