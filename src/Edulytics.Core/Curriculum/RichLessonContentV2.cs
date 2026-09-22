using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Edulytics.Core.Curriculum;

public enum RichLessonVideoReviewStatus
{
    Candidate = 0,
    Reviewed = 1,
    Approved = 2,
    Unavailable = 3,
    Rejected = 4
}

public enum RichLessonVisualKind
{
    CalculationChain = 1,
    NumberLine = 2,
    DoubleNumberLine = 3,
    FractionBars = 4,
    RatioTable = 5,
    PlaceValueChart = 6,
    Scale = 7,
    EquationSet = 8,
    ShapeDecomposition = 9
}

public sealed class RichLessonContentV2Document
{
    public int SchemaVersion { get; set; } = 1;
    public string ContentVersion { get; set; } = string.Empty;
    public string PackCode { get; set; } = string.Empty;
    public List<RichLessonContentV2Lesson> Lessons { get; set; } = [];
}

public sealed class RichLessonContentV2Lesson
{
    public string LessonCode { get; set; } = string.Empty;
    public string CultureCode { get; set; } = "en";
    public string Title { get; set; } = string.Empty;
    public List<string> ExplanationParagraphs { get; set; } = [];
    public List<RichLessonKeyConcept> KeyConcepts { get; set; } = [];
    public List<RichLessonWorkedExample> WorkedExamples { get; set; } = [];
    public List<RichLessonCommonMistake> CommonMistakes { get; set; } = [];
    public List<string> SummaryPoints { get; set; } = [];
    public List<RichLessonVisual> Visuals { get; set; } = [];
    public List<RichLessonVideoResource> Videos { get; set; } = [];
}

public sealed class RichLessonKeyConcept
{
    public string Title { get; set; } = string.Empty;
    public string Definition { get; set; } = string.Empty;
    public string Rule { get; set; } = string.Empty;
    public string Example { get; set; } = string.Empty;
}

public sealed class RichLessonWorkedExample
{
    public string Title { get; set; } = string.Empty;
    public string Question { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public List<string> Steps { get; set; } = [];
    public string Answer { get; set; } = string.Empty;
    public string Check { get; set; } = string.Empty;
}

public sealed class RichLessonCommonMistake
{
    public string Mistake { get; set; } = string.Empty;
    public string WhyWrong { get; set; } = string.Empty;
    public string Correction { get; set; } = string.Empty;
}

public sealed class RichLessonVisual
{
    public RichLessonVisualKind Kind { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string PrimaryLabel { get; set; } = string.Empty;
    public string SecondaryLabel { get; set; } = string.Empty;
    public List<string> Items { get; set; } = [];
    public List<string> SecondaryItems { get; set; } = [];
    public List<RichLessonVisualRow> Rows { get; set; } = [];
}

public sealed class RichLessonVisualRow
{
    public string Label { get; set; } = string.Empty;
    public int Numerator { get; set; }
    public int Denominator { get; set; }
    public List<string> Cells { get; set; } = [];
}

public sealed class RichLessonVideoResource
{
    public string Provider { get; set; } = "YouTube";
    public string VideoId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Creator { get; set; } = string.Empty;
    public string Language { get; set; } = "en";
    public string DurationLabel { get; set; } = string.Empty;
    public string WhyRecommended { get; set; } = string.Empty;
    public RichLessonVideoReviewStatus ReviewStatus { get; set; }
    public string CheckedAtUtc { get; set; } = string.Empty;
}

public static class RichLessonContentV2Registry
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    private static readonly Lazy<IReadOnlyList<RichLessonContentV2Document>>
        Documents = new(LoadDocuments);

    private static readonly Lazy<IReadOnlyDictionary<string, RichLessonContentV2Lesson>>
        Lessons = new(BuildLessonIndex);

    private static readonly Regex YouTubeVideoIdRegex =
        new(@"^[A-Za-z0-9_-]{11}$", RegexOptions.CultureInvariant | RegexOptions.Compiled);

    public static IReadOnlyList<RichLessonContentV2Document> AllDocuments =>
        Documents.Value;

    public static RichLessonContentV2Lesson? Find(
        string lessonCode,
        string cultureCode)
    {
        if (string.IsNullOrWhiteSpace(lessonCode))
            return null;

        var normalizedCulture = NormalizeCulture(cultureCode);

        if (Lessons.Value.TryGetValue(
                Key(lessonCode, normalizedCulture),
                out var exact))
        {
            return exact;
        }

        if (normalizedCulture != "en" &&
            Lessons.Value.TryGetValue(Key(lessonCode, "en"), out var english))
        {
            return english;
        }

        return null;
    }

    private static IReadOnlyList<RichLessonContentV2Document> LoadDocuments()
    {
        var assembly = typeof(RichLessonContentV2Registry).Assembly;
        var names = assembly.GetManifestResourceNames()
            .Where(x => x.EndsWith(
                ".rich-lesson-v2.json",
                StringComparison.OrdinalIgnoreCase))
            .OrderBy(x => x, StringComparer.Ordinal)
            .ToArray();

        var result = new List<RichLessonContentV2Document>();

        foreach (var name in names)
        {
            using var stream = assembly.GetManifestResourceStream(name)
                ?? throw new InvalidOperationException(
                    $"Embedded rich lesson resource was not found: {name}.");

            var document = JsonSerializer.Deserialize<RichLessonContentV2Document>(
                stream,
                JsonOptions)
                ?? throw new InvalidOperationException(
                    $"Embedded rich lesson resource is invalid: {name}.");

            Validate(document, name);
            result.Add(document);
        }

        return result;
    }

    private static IReadOnlyDictionary<string, RichLessonContentV2Lesson>
        BuildLessonIndex()
    {
        var index = new Dictionary<string, RichLessonContentV2Lesson>(
            StringComparer.Ordinal);

        foreach (var document in Documents.Value)
        {
            foreach (var lesson in document.Lessons)
            {
                var key = Key(
                    lesson.LessonCode,
                    NormalizeCulture(lesson.CultureCode));

                if (!index.TryAdd(key, lesson))
                {
                    throw new InvalidOperationException(
                        $"Duplicate Rich Lesson Content V2 entry: " +
                        $"{lesson.LessonCode}:{lesson.CultureCode}.");
                }
            }
        }

        return index;
    }

    public static void Validate(
        RichLessonContentV2Document document,
        string sourceName = "<memory>")
    {
        if (document.SchemaVersion != 1)
        {
            throw new InvalidOperationException(
                $"Unsupported Rich Lesson Content schema in {sourceName}: " +
                $"{document.SchemaVersion}.");
        }

        Require(document.ContentVersion, "ContentVersion", sourceName);
        Require(document.PackCode, "PackCode", sourceName);

        if (document.Lessons.Count == 0)
            throw new InvalidOperationException(
                $"Rich Lesson Content document contains no lessons: {sourceName}.");

        foreach (var lesson in document.Lessons)
        {
            Require(lesson.LessonCode, "LessonCode", sourceName);
            Require(lesson.CultureCode, "CultureCode", sourceName);
            Require(lesson.Title, "Title", sourceName);

            if (lesson.ExplanationParagraphs.Count < 2)
                throw new InvalidOperationException(
                    $"Rich lesson {lesson.LessonCode} requires at least two explanation paragraphs.");

            if (lesson.KeyConcepts.Count < 3)
                throw new InvalidOperationException(
                    $"Rich lesson {lesson.LessonCode} requires at least three key concepts.");

            if (lesson.WorkedExamples.Count < 3)
                throw new InvalidOperationException(
                    $"Rich lesson {lesson.LessonCode} requires at least three worked examples.");

            foreach (var example in lesson.WorkedExamples)
            {
                Require(example.Title, "WorkedExample.Title", lesson.LessonCode);
                Require(example.Question, "WorkedExample.Question", lesson.LessonCode);
                Require(example.Method, "WorkedExample.Method", lesson.LessonCode);
                Require(example.Answer, "WorkedExample.Answer", lesson.LessonCode);

                if (example.Steps.Count < 3)
                {
                    throw new InvalidOperationException(
                        $"Worked example {example.Title} in {lesson.LessonCode} " +
                        "requires at least three explicit steps.");
                }
            }

            if (lesson.CommonMistakes.Count < 2)
                throw new InvalidOperationException(
                    $"Rich lesson {lesson.LessonCode} requires at least two common mistakes.");

            if (lesson.SummaryPoints.Count < 3)
                throw new InvalidOperationException(
                    $"Rich lesson {lesson.LessonCode} requires at least three summary points.");

            foreach (var video in lesson.Videos.Where(x =>
                         x.ReviewStatus == RichLessonVideoReviewStatus.Approved))
            {
                if (!string.Equals(
                        video.Provider,
                        "YouTube",
                        StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException(
                        $"Approved pilot video provider is unsupported: {video.Provider}.");
                }

                if (!YouTubeVideoIdRegex.IsMatch(video.VideoId))
                {
                    throw new InvalidOperationException(
                        $"Approved YouTube video id is invalid in {lesson.LessonCode}: " +
                        $"{video.VideoId}.");
                }

                Require(video.Title, "Video.Title", lesson.LessonCode);
                Require(video.Creator, "Video.Creator", lesson.LessonCode);
                Require(video.WhyRecommended, "Video.WhyRecommended", lesson.LessonCode);
                Require(video.CheckedAtUtc, "Video.CheckedAtUtc", lesson.LessonCode);

                if (!DateTimeOffset.TryParse(video.CheckedAtUtc, out var checkedAt) ||
                    checkedAt.Offset != TimeSpan.Zero)
                {
                    throw new InvalidOperationException(
                        $"Video CheckedAtUtc must be UTC in {lesson.LessonCode}.");
                }
            }
        }
    }

    private static string Key(string lessonCode, string cultureCode) =>
        $"{lessonCode.Trim()}\n{NormalizeCulture(cultureCode)}";

    private static string NormalizeCulture(string cultureCode)
    {
        if (string.IsNullOrWhiteSpace(cultureCode))
            return "en";

        var value = cultureCode.Trim();
        var separator = value.IndexOf('-');
        return (separator > 0 ? value[..separator] : value)
            .ToLowerInvariant();
    }

    private static void Require(
        string? value,
        string field,
        string source)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException(
                $"Rich Lesson Content field is required: {source}:{field}.");
    }
}

public enum RichLessonSourceDossierStatus
{
    ApprovedForAdaptation = 1,
    IndependentAuthoringReferenceOnly = 2,
    ResearchRequired = 3
}

public sealed record RichLessonSourceDossier(
    string LessonCode,
    string PackCode,
    string CurriculumAuthority,
    string CurriculumSourceUrl,
    string PedagogicalSourceTitle,
    string PedagogicalSourcePublisher,
    string PedagogicalSourceEdition,
    string PedagogicalSourceUrl,
    string SourceLocator,
    string RightsNote,
    RichLessonSourceDossierStatus Status,
    bool SourceAdaptationPermitted,
    string DecisionReason,
    string ResearchQuery);

public static class RichLessonSourceDossierFactory
{
    private static readonly string[] AdaptationRightsTokens =
    [
        "Public Domain",
        "CC0",
        "CC BY 4.0",
        "Open Government Licence v3.0",
        "OGL v3.0"
    ];

    public static RichLessonSourceDossier Build(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string lessonTitle)
    {
        ArgumentNullException.ThrowIfNull(document);
        ArgumentNullException.ThrowIfNull(lesson);

        var researchReference =
            string.Equals(
                document.PackCode,
                "PL-NATIONAL-MATH",
                StringComparison.Ordinal)
                ? RichLessonResearchReferenceRegistry.ResolvePolish(lesson)
                : null;

        var combinedRights = string.Join(
            " ",
            document.PedagogicalSourceRightsNote,
            lesson.SourceRights,
            researchReference is null
                ? string.Empty
                : $"{researchReference.Licence}. {researchReference.ScopeNote}");

        var adaptationPermitted = AdaptationRightsTokens.Any(token =>
            combinedRights.Contains(
                token,
                StringComparison.OrdinalIgnoreCase));

        RichLessonSourceDossierStatus status;
        string reason;

        if (document.PedagogicalSourceType ==
                PedagogicalSourceType.OfficialFrameworkOnly &&
            researchReference is not null)
        {
            // R8 Polish lessons use the official framework as curriculum authority
            // and a reviewed open reference only for mathematical/pedagogical
            // cross-checking. Runtime prose is independently authored, so do not
            // relabel this route as source adaptation even when the reference is CC BY.
            adaptationPermitted = false;
            status =
                RichLessonSourceDossierStatus.IndependentAuthoringReferenceOnly;
            reason =
                "The official Polish framework remains the lesson target authority. " +
                $"The reviewed research reference “{researchReference.Title}” " +
                $"({researchReference.Licence}) is resolved for cross-checking. " +
                "Learner-facing Rich V2 prose is independently authored by Edulytics " +
                "and exact worked examples remain solver/verifier-backed.";
        }
        else if (document.PedagogicalSourceType ==
            PedagogicalSourceType.OfficialFrameworkOnly)
        {
            status = RichLessonSourceDossierStatus.ResearchRequired;
            reason =
                "The current pack provides curriculum/framework authority only. " +
                "A separate pedagogical source with clear rights must be resolved " +
                "before source-driven enrichment.";
        }
        else if (adaptationPermitted)
        {
            status = RichLessonSourceDossierStatus.ApprovedForAdaptation;
            reason =
                "The recorded pedagogical source includes explicit adaptation/reuse " +
                "rights accepted by the Rich Lesson Content V2 programme.";
        }
        else
        {
            status =
                RichLessonSourceDossierStatus.IndependentAuthoringReferenceOnly;
            reason =
                "The source may be used as curriculum/pedagogical reference, but " +
                "the recorded rights do not authorize copying or close adaptation. " +
                "Learner-facing content must be independently authored.";
        }

        var sourceTitle = researchReference?.Title ??
            (!string.IsNullOrWhiteSpace(lesson.SourceTitle)
                ? lesson.SourceTitle
                : document.PedagogicalSourceTitle);

        var sourcePublisher = researchReference?.Publisher ??
            (!string.IsNullOrWhiteSpace(lesson.SourcePublisher)
                ? lesson.SourcePublisher
                : document.PedagogicalSourcePublisher);

        var sourceEdition = researchReference?.Edition ??
            (!string.IsNullOrWhiteSpace(lesson.SourceEdition)
                ? lesson.SourceEdition
                : document.PedagogicalSourceEdition);

        var sourceUrl = researchReference?.Url ??
            (!string.IsNullOrWhiteSpace(lesson.SourceUrl)
                ? lesson.SourceUrl
                : document.PedagogicalSourceUrl);

        return new(
            lesson.LessonCode,
            document.PackCode,
            document.SourceAuthority,
            document.SourceUrl,
            sourceTitle,
            sourcePublisher,
            sourceEdition,
            sourceUrl,
            lesson.SourceLocator,
            combinedRights.Trim(),
            status,
            adaptationPermitted,
            reason,
            researchReference is null
                ? $"{lessonTitle} {lesson.SourceLocator} mathematics teaching guidance"
                : $"{lessonTitle} {lesson.SourceLocator} {researchReference.Title} {researchReference.ScopeNote}");
    }
}
