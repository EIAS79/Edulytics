using System.Collections.Concurrent;
using System.Globalization;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml;
using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Services.LessonContent;

public sealed class YouTubeLessonDiscoveryOptions
{
    public bool Enabled { get; set; } = true;
    public string ApiKey { get; set; } = string.Empty;
    public int CacheMinutes { get; set; } = 720;
    public int SearchResultCount { get; set; } = 50;
    public int RelatedResultCount { get; set; } = 6;
    public int MinimumRelevancePercent { get; set; } = 34;
}

public sealed record YouTubePreferredChannel(
    string Name,
    string Handle,
    string ChannelSearchUrl);

public sealed record YouTubeLessonVideo(
    string VideoId,
    string Title,
    string ChannelTitle,
    string WatchUrl,
    string EmbedUrl,
    string ThumbnailUrl,
    string DurationLabel,
    long ViewCount,
    long LikeCount,
    int RelevancePercent,
    bool IsPreferredChannel)
{
    public int MatchPercent { get; init; }
    public int ObjectiveMatchPercent { get; init; }
    public int SkillMatchPercent { get; init; }
    public int DifficultyMatchPercent { get; init; }
    public int PrerequisiteSuitabilityPercent { get; init; }
    public int TeachingQualityPercent { get; init; }
}

public sealed record YouTubeLessonDiscoveryResult(
    bool Available,
    string GradeBand,
    string SearchQuery,
    string SearchUrl,
    IReadOnlyList<YouTubePreferredChannel> PreferredChannels,
    YouTubeLessonVideo? Featured,
    IReadOnlyList<YouTubeLessonVideo> Related,
    bool UsedPreferredChannels,
    string? Message);

public sealed record YouTubeLessonDiscoveryRequest(
    string LessonCode,
    string LessonTitle,
    string GradeLabel,
    string FrameworkName,
    string CultureCode,
    string? LearnerQuery,
    IReadOnlyList<string> LearningObjectives);

public interface IYouTubeLessonDiscoveryService
{
    Task<YouTubeLessonDiscoveryResult> DiscoverAsync(
        YouTubeLessonDiscoveryRequest request,
        CancellationToken cancellationToken = default);

    Task<YouTubeLessonDiscoveryResult> DiscoverAsync(
        string lessonCode,
        string lessonTitle,
        string gradeLabel,
        string cultureCode,
        string? learnerQuery,
        CancellationToken cancellationToken = default);
}

public sealed record YouTubeChannelPolicy(
    string Name,
    string Handle,
    string? ChannelId = null,
    string? LegacyUsername = null);

public sealed record YouTubeLessonBandPolicy(
    string Code,
    string Label,
    IReadOnlyList<YouTubeChannelPolicy> Channels);

public static partial class YouTubeLessonChannelPolicy
{
    private static readonly YouTubeChannelPolicy KhanAcademy =
        new("Khan Academy", "khanacademy", "UC4a-Gbdw7vOaccHmFo40b9g");
    private static readonly YouTubeChannelPolicy MathAntics =
        new("Math Antics", "mathantics", null, "mathantics");
    private static readonly YouTubeChannelPolicy MathWithMrJ =
        new("Math with Mr. J", "mathwithmrj");
    private static readonly YouTubeChannelPolicy MashUpMath =
        new("MashUp Math", "MashupMath", "UCtBtcQJ8_jsrjPzb8i1tOsA");
    private static readonly YouTubeChannelPolicy Numberock =
        new("Numberock", "numberockllc");
    private static readonly YouTubeChannelPolicy OrganicChemistryTutor =
        new("The Organic Chemistry Tutor", "TheOrganicChemistryTutor", "UCEWpbFLzoYGPfuWUMFPSaoA");
    private static readonly YouTubeChannelPolicy BrianMcLogan =
        new("Brian McLogan", "brianmclogan", "UCQv3dpUXUWvDFQarHrS5P9A");
    private static readonly YouTubeChannelPolicy ProfessorLeonard =
        new("Professor Leonard", "professorleonard", "UCoHhuummRZaIVX7bD4t2czg");
    private static readonly YouTubeChannelPolicy PatrickJmt =
        new("PatrickJMT", "patrickjmt");
    private static readonly YouTubeChannelPolicy ThreeBlueOneBrown =
        new("3Blue1Brown", "3blue1brown", "UCYO_jab_esuFRV4b17AJtAw");

    public static YouTubeLessonBandPolicy Resolve(
        string lessonCode,
        string gradeLabel)
    {
        var level = ResolveLevel(lessonCode, gradeLabel);

        if (level is >= 1 and <= 6)
        {
            return new(
                "primary-1-6",
                "Primary · Grades / Stages 1–6",
                [MathAntics, KhanAcademy, MathWithMrJ, MashUpMath, Numberock]);
        }

        if (level is >= 7 and <= 9)
        {
            return new(
                "lower-secondary-7-9",
                "Lower secondary · Grades / Stages 7–9",
                [KhanAcademy, OrganicChemistryTutor, BrianMcLogan, MashUpMath, MathWithMrJ]);
        }

        return new(
            "higher-10-plus",
            "Higher mathematics · Grade 10+ / IGCSE / AS / A Level",
            [KhanAcademy, OrganicChemistryTutor, ProfessorLeonard, PatrickJmt, ThreeBlueOneBrown]);
    }

    private static int ResolveLevel(
        string lessonCode,
        string gradeLabel)
    {
        var gradeMatch = GradeLevelRegex().Match(gradeLabel ?? string.Empty);
        if (gradeMatch.Success &&
            int.TryParse(
                gradeMatch.Groups["level"].Value,
                NumberStyles.None,
                CultureInfo.InvariantCulture,
                out var gradeLevel))
        {
            return gradeLevel;
        }

        var codeMatch = LessonCodeLevelRegex().Match(lessonCode ?? string.Empty);
        if (codeMatch.Success &&
            int.TryParse(
                codeMatch.Groups["level"].Value,
                NumberStyles.None,
                CultureInfo.InvariantCulture,
                out var codeLevel))
        {
            return codeLevel;
        }

        var combined = $"{lessonCode} {gradeLabel}";
        if (combined.Contains("IGCSE", StringComparison.OrdinalIgnoreCase) ||
            combined.Contains("A-LEVEL", StringComparison.OrdinalIgnoreCase) ||
            combined.Contains("A LEVEL", StringComparison.OrdinalIgnoreCase) ||
            combined.Contains("AS LEVEL", StringComparison.OrdinalIgnoreCase) ||
            combined.Contains(":HS", StringComparison.OrdinalIgnoreCase))
        {
            return 10;
        }

        return 7;
    }

    [GeneratedRegex(
        @"(?ix)\b(?:grade|stage|level|year|klasa)\s*[-:]?\s*(?<level>\d{1,2})\b",
        RegexOptions.CultureInvariant)]
    private static partial Regex GradeLevelRegex();

    [GeneratedRegex(
        @"(?ix):(?:G|L|S)(?<level>\d{1,2})(?=:|$)",
        RegexOptions.CultureInvariant)]
    private static partial Regex LessonCodeLevelRegex();
}


internal sealed record YouTubeLessonSemanticContext(
    string TopicQuery,
    IReadOnlyList<string> LearningObjectives,
    IReadOnlyList<string> SkillTerms,
    IReadOnlyList<string> PrerequisiteTerms,
    string DifficultyLabel);

internal static class YouTubeLessonSemanticContextResolver
{
    private const string MappingResource =
        "Edulytics.Core.Mathematics.Curriculum.lesson-skill-mappings.v1.json";

    private static readonly Lazy<IReadOnlyDictionary<string, MappingSignals>>
        MappingIndex = new(LoadMappings);

    public static YouTubeLessonSemanticContext Resolve(
        string lessonCode,
        string lessonTitle,
        IReadOnlyList<string> learningObjectives)
    {
        var objectives = (learningObjectives ?? [])
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        var skills = new List<string>();
        var prerequisites = new List<string>();

        if (MappingIndex.Value.TryGetValue(lessonCode, out var mapping))
        {
            skills.AddRange(mapping.PrimarySkills);
            skills.AddRange(mapping.SecondarySkills);
            prerequisites.AddRange(mapping.Prerequisites);
        }

        if (LessonPracticeContractRegistry.TryResolve(lessonCode, out var practice) &&
            practice is not null)
        {
            skills.AddRange(practice.SkillIds);
        }

        var skillTerms = skills
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(ToSearchPhrase)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        var prerequisiteTerms = prerequisites
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(ToSearchPhrase)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        var difficulty = ResolveDifficulty(
            lessonTitle,
            objectives);

        var topicQuery = CleanLessonTopic(lessonTitle);

        return new(
            topicQuery,
            objectives,
            skillTerms,
            prerequisiteTerms,
            difficulty);
    }

    private static string ResolveDifficulty(
        string lessonTitle,
        IReadOnlyList<string> objectives)
    {
        var text = string.Join(" ", new[] { lessonTitle }.Concat(objectives))
            .ToLowerInvariant();

        if (ContainsAny(
                text,
                "advanced", "reasoning", "proof", "justify", "modelling",
                "modeling", "multi-step", "multistep", "challenge",
                "investigate", "interpret", "evaluate"))
        {
            return "challenging";
        }

        if (ContainsAny(
                text,
                "basic", "introduction", "introductory", "foundation",
                "recall", "simple", "recognise", "recognize"))
        {
            return "foundation";
        }

        return "standard";
    }

    private static bool ContainsAny(string text, params string[] values) =>
        values.Any(value => text.Contains(value, StringComparison.OrdinalIgnoreCase));

    internal static string CleanLessonTopic(string lessonTitle)
    {
        var cleaned = Regex.Replace(
            lessonTitle ?? string.Empty,
            @"\s*(?:—|–|-|:)\s*(?:advanced\s+reasoning|foundation(?:\s+explanation)?|worked\s+examples?)\s*$",
            string.Empty,
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        return Regex.Replace(cleaned, @"\s+", " ").Trim();
    }

    private static string ToSearchPhrase(string value) =>
        Regex.Replace(
            value.Replace('.', ' ').Replace('_', ' ').Replace('-', ' '),
            @"\s+",
            " ").Trim();

    private static IReadOnlyDictionary<string, MappingSignals> LoadMappings()
    {
        var assembly = typeof(LessonPracticeContractRegistry).Assembly;
        using var stream = assembly.GetManifestResourceStream(MappingResource);
        if (stream is null)
            return new Dictionary<string, MappingSignals>(StringComparer.Ordinal);

        using var document = JsonDocument.Parse(stream);
        var result = new Dictionary<string, MappingSignals>(StringComparer.Ordinal);

        if (!document.RootElement.TryGetProperty("mappings", out var mappings) ||
            mappings.ValueKind != JsonValueKind.Array)
        {
            return result;
        }

        foreach (var row in mappings.EnumerateArray())
        {
            var lessonCode = ReadString(row, "lessonCode");
            if (string.IsNullOrWhiteSpace(lessonCode))
                continue;

            result[lessonCode] = new(
                ReadStringArray(row, "primarySkills"),
                ReadStringArray(row, "secondarySkills"),
                ReadStringArray(row, "prerequisites"));
        }

        return result;
    }

    private static string ReadString(JsonElement element, string property) =>
        element.TryGetProperty(property, out var node) &&
        node.ValueKind == JsonValueKind.String
            ? node.GetString() ?? string.Empty
            : string.Empty;

    private static IReadOnlyList<string> ReadStringArray(
        JsonElement element,
        string property)
    {
        if (!element.TryGetProperty(property, out var node) ||
            node.ValueKind != JsonValueKind.Array)
        {
            return [];
        }

        return node.EnumerateArray()
            .Where(x => x.ValueKind == JsonValueKind.String)
            .Select(x => x.GetString())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Cast<string>()
            .ToArray();
    }

    private sealed record MappingSignals(
        IReadOnlyList<string> PrimarySkills,
        IReadOnlyList<string> SecondarySkills,
        IReadOnlyList<string> Prerequisites);
}

public sealed partial class YouTubeLessonDiscoveryService :
    IYouTubeLessonDiscoveryService
{
    private static readonly ConcurrentDictionary<string, CacheEntry>
        ResultCache = new(StringComparer.Ordinal);

    private static readonly ConcurrentDictionary<string, string>
        ChannelIdCache = new(StringComparer.OrdinalIgnoreCase);

    private static readonly SemaphoreSlim ApiGate = new(4, 4);
    private const int MaximumCacheEntries = 512;

    private static readonly HashSet<string> StopWords =
        new(
            [
                "a", "an", "and", "are", "as", "at", "be", "by", "for",
                "from", "how", "in", "into", "is", "it", "math", "mathematics",
                "of", "on", "or", "the", "to", "using", "with", "lesson",
                "grade", "stage", "level", "explanation", "examples", "example"
            ],
            StringComparer.OrdinalIgnoreCase);

    private readonly HttpClient _httpClient;
    private readonly YouTubeLessonDiscoveryOptions _options;

    public YouTubeLessonDiscoveryService(
        HttpClient httpClient,
        YouTubeLessonDiscoveryOptions options)
    {
        _httpClient = httpClient;
        _options = options;
    }

    public Task<YouTubeLessonDiscoveryResult> DiscoverAsync(
        string lessonCode,
        string lessonTitle,
        string gradeLabel,
        string cultureCode,
        string? learnerQuery,
        CancellationToken cancellationToken = default) =>
        DiscoverAsync(
            new YouTubeLessonDiscoveryRequest(
                lessonCode,
                lessonTitle,
                gradeLabel,
                string.Empty,
                cultureCode,
                learnerQuery,
                []),
            cancellationToken);

    public async Task<YouTubeLessonDiscoveryResult> DiscoverAsync(
        YouTubeLessonDiscoveryRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(request.LessonTitle);

        var lessonCode = request.LessonCode;
        var lessonTitle = request.LessonTitle;
        var gradeLabel = request.GradeLabel;
        var cultureCode = request.CultureCode;
        var learnerQuery = request.LearnerQuery;

        var policy = YouTubeLessonChannelPolicy.Resolve(
            lessonCode,
            gradeLabel);

        var semanticContext = YouTubeLessonSemanticContextResolver.Resolve(
            lessonCode,
            lessonTitle,
            request.LearningObjectives);

        var lessonTopicQuery = semanticContext.TopicQuery;

        var query = BuildQuery(
            lessonTitle,
            gradeLabel,
            request.FrameworkName,
            semanticContext,
            learnerQuery);

        var preferredLinks = policy.Channels
            .Select(channel =>
                new YouTubePreferredChannel(
                    channel.Name,
                    channel.Handle,
                    BuildChannelSearchUrl(channel.Handle, query)))
            .ToArray();

        var searchUrl =
            "https://www.youtube.com/results?search_query=" +
            Uri.EscapeDataString(query);

        if (!_options.Enabled ||
            string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            return new(
                false,
                policy.Label,
                query,
                searchUrl,
                preferredLinks,
                null,
                [],
                false,
                "YouTube lesson search is ready but the YouTube Data API key is not configured.");
        }

        var cacheKey = string.Join(
            "\n",
            "youtube-lesson-discovery-v2",
            policy.Code,
            NormalizeCulture(cultureCode),
            Math.Clamp(
                _options.MinimumRelevancePercent,
                0,
                100).ToString(
                    CultureInfo.InvariantCulture),
            Math.Clamp(
                _options.SearchResultCount,
                8,
                50).ToString(
                    CultureInfo.InvariantCulture),
            Math.Clamp(
                _options.RelatedResultCount,
                1,
                10).ToString(
                    CultureInfo.InvariantCulture),
            lessonCode.Trim(),
            request.FrameworkName.Trim(),
            string.Join("|", semanticContext.LearningObjectives),
            string.Join("|", semanticContext.SkillTerms),
            string.Join("|", semanticContext.PrerequisiteTerms),
            semanticContext.DifficultyLabel,
            lessonTopicQuery,
            query);

        if (ResultCache.TryGetValue(cacheKey, out var cached) &&
            cached.ExpiresAtUtc > DateTimeOffset.UtcNow)
        {
            return cached.Result;
        }

        await ApiGate.WaitAsync(cancellationToken);
        try
        {
            if (ResultCache.TryGetValue(cacheKey, out cached) &&
                cached.ExpiresAtUtc > DateTimeOffset.UtcNow)
            {
                return cached.Result;
            }

            var preferredIds = await ResolvePreferredChannelIdsAsync(
                policy,
                cancellationToken);

            var searchResults = await SearchAsync(
                query,
                cultureCode,
                cancellationToken);

            var candidates = await HydrateVideosAsync(
                searchResults,
                semanticContext,
                preferredIds,
                cancellationToken);

            var minimum = Math.Clamp(
                _options.MinimumRelevancePercent,
                0,
                100);

            var ranked = candidates
                .Where(x => x.RelevancePercent >= minimum)
                .OrderByDescending(x => x.RankScore)
                .ToArray();

            if (ranked.Length == 0)
            {
                var broadQuery = BuildBroadQuery(
                    semanticContext.TopicQuery,
                    learnerQuery);

                if (!string.Equals(
                        broadQuery,
                        query,
                        StringComparison.OrdinalIgnoreCase))
                {
                    var broadSearchResults = await SearchAsync(
                        broadQuery,
                        cultureCode,
                        cancellationToken);

                    var broadCandidates = await HydrateVideosAsync(
                        broadSearchResults,
                        semanticContext,
                        preferredIds,
                        cancellationToken);

                    ranked = broadCandidates
                        .Where(x => x.RelevancePercent >= minimum)
                        .OrderByDescending(x => x.RankScore)
                        .ToArray();
                }
            }

            var usedPreferred =
                ranked.FirstOrDefault()?.IsPreferredChannel == true;

            // MinimumRelevancePercent is a hard safety/quality boundary.
            // If no candidate clears it, return no discovered video rather
            // than silently bypassing the configured policy.
            var featured = ranked.FirstOrDefault()?.Video;
            var related = ranked
                .Skip(1)
                .Take(Math.Clamp(_options.RelatedResultCount, 1, 10))
                .Select(x => x.Video)
                .ToArray();

            var result = new YouTubeLessonDiscoveryResult(
                true,
                policy.Label,
                query,
                searchUrl,
                preferredLinks,
                featured,
                related,
                usedPreferred,
                featured is null
                    ? "No embeddable YouTube result passed the lesson relevance checks."
                    : usedPreferred
                        ? "Selected as the strongest lesson match; preferred-channel trust contributed a small ranking bonus."
                        : "Selected as the strongest lesson match across the relevant YouTube results.");

            StoreCachedResult(
                cacheKey,
                new(
                    DateTimeOffset.UtcNow.AddMinutes(
                        Math.Clamp(_options.CacheMinutes, 5, 1440)),
                    result));

            return result;
        }
        catch (OperationCanceledException)
            when (!cancellationToken.IsCancellationRequested)
        {
            return new(
                false,
                policy.Label,
                query,
                searchUrl,
                preferredLinks,
                null,
                [],
                false,
                "YouTube search timed out. You can still use the lesson-scoped YouTube search links.");
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (HttpRequestException)
        {
            return new(
                false,
                policy.Label,
                query,
                searchUrl,
                preferredLinks,
                null,
                [],
                false,
                "YouTube search is temporarily unavailable. You can still use the lesson-scoped YouTube search links.");
        }
        catch (JsonException)
        {
            return new(
                false,
                policy.Label,
                query,
                searchUrl,
                preferredLinks,
                null,
                [],
                false,
                "YouTube returned an unexpected response. You can still use the lesson-scoped YouTube search links.");
        }
        finally
        {
            ApiGate.Release();
        }
    }

    private async Task<HashSet<string>> ResolvePreferredChannelIdsAsync(
        YouTubeLessonBandPolicy policy,
        CancellationToken cancellationToken)
    {
        var ids = new HashSet<string>(StringComparer.Ordinal);

        foreach (var channel in policy.Channels)
        {
            if (!string.IsNullOrWhiteSpace(channel.ChannelId))
            {
                ids.Add(channel.ChannelId);
                continue;
            }

            var cacheKey =
                !string.IsNullOrWhiteSpace(channel.Handle)
                    ? $"handle:{channel.Handle}"
                    : $"username:{channel.LegacyUsername}";

            if (ChannelIdCache.TryGetValue(cacheKey, out var cachedId))
            {
                ids.Add(cachedId);
                continue;
            }

            var id = await ResolveChannelIdAsync(
                channel,
                cancellationToken);

            if (!string.IsNullOrWhiteSpace(id))
            {
                ChannelIdCache[cacheKey] = id;
                ids.Add(id);
            }
        }

        return ids;
    }

    private async Task<string?> ResolveChannelIdAsync(
        YouTubeChannelPolicy channel,
        CancellationToken cancellationToken)
    {
        var filter = !string.IsNullOrWhiteSpace(channel.Handle)
            ? $"forHandle={Uri.EscapeDataString(channel.Handle.TrimStart('@'))}"
            : $"forUsername={Uri.EscapeDataString(channel.LegacyUsername ?? string.Empty)}";

        var url =
            "https://www.googleapis.com/youtube/v3/channels" +
            $"?part=id&{filter}&key={Uri.EscapeDataString(_options.ApiKey)}";

        using var response = await _httpClient.GetAsync(
            url,
            cancellationToken);

        if (!response.IsSuccessStatusCode &&
            !string.IsNullOrWhiteSpace(channel.LegacyUsername) &&
            !string.IsNullOrWhiteSpace(channel.Handle))
        {
            var legacyUrl =
                "https://www.googleapis.com/youtube/v3/channels" +
                $"?part=id&forUsername={Uri.EscapeDataString(channel.LegacyUsername)}" +
                $"&key={Uri.EscapeDataString(_options.ApiKey)}";

            using var legacyResponse = await _httpClient.GetAsync(
                legacyUrl,
                cancellationToken);

            if (!legacyResponse.IsSuccessStatusCode)
                return null;

            return await ReadFirstChannelIdAsync(
                legacyResponse,
                cancellationToken);
        }

        if (!response.IsSuccessStatusCode)
            return null;

        var id = await ReadFirstChannelIdAsync(
            response,
            cancellationToken);

        if (!string.IsNullOrWhiteSpace(id))
            return id;

        if (string.IsNullOrWhiteSpace(channel.LegacyUsername))
            return null;

        var fallbackUrl =
            "https://www.googleapis.com/youtube/v3/channels" +
            $"?part=id&forUsername={Uri.EscapeDataString(channel.LegacyUsername)}" +
            $"&key={Uri.EscapeDataString(_options.ApiKey)}";

        using var fallbackResponse = await _httpClient.GetAsync(
            fallbackUrl,
            cancellationToken);

        return fallbackResponse.IsSuccessStatusCode
            ? await ReadFirstChannelIdAsync(
                fallbackResponse,
                cancellationToken)
            : null;
    }

    private static async Task<string?> ReadFirstChannelIdAsync(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        await using var stream = await response.Content.ReadAsStreamAsync(
            cancellationToken);
        using var document = await JsonDocument.ParseAsync(
            stream,
            cancellationToken: cancellationToken);

        if (!document.RootElement.TryGetProperty("items", out var items) ||
            items.ValueKind != JsonValueKind.Array)
        {
            return null;
        }

        foreach (var item in items.EnumerateArray())
        {
            if (item.TryGetProperty("id", out var idNode) &&
                !string.IsNullOrWhiteSpace(idNode.GetString()))
            {
                return idNode.GetString();
            }
        }

        return null;
    }

    private async Task<IReadOnlyList<SearchHit>> SearchAsync(
        string query,
        string cultureCode,
        CancellationToken cancellationToken)
    {
        var maxResults = Math.Clamp(
            _options.SearchResultCount,
            8,
            50);

        var language = NormalizeCulture(cultureCode);
        var languageParameter =
            language is "en" or "pl" or "ar"
                ? $"&relevanceLanguage={Uri.EscapeDataString(language)}"
                : string.Empty;

        var url =
            "https://www.googleapis.com/youtube/v3/search" +
            "?part=snippet" +
            "&type=video" +
            "&order=relevance" +
            "&safeSearch=strict" +
            "&videoEmbeddable=true" +
            "&videoSyndicated=true" +
            $"&maxResults={maxResults}" +
            languageParameter +
            $"&q={Uri.EscapeDataString(query)}" +
            $"&key={Uri.EscapeDataString(_options.ApiKey)}";

        using var response = await _httpClient.GetAsync(
            url,
            cancellationToken);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(
            cancellationToken);
        using var document = await JsonDocument.ParseAsync(
            stream,
            cancellationToken: cancellationToken);

        var result = new List<SearchHit>();

        if (!document.RootElement.TryGetProperty("items", out var items) ||
            items.ValueKind != JsonValueKind.Array)
        {
            return result;
        }

        var index = 0;
        foreach (var item in items.EnumerateArray())
        {
            index++;

            if (!item.TryGetProperty("id", out var id) ||
                !id.TryGetProperty("videoId", out var videoIdNode))
            {
                continue;
            }

            var videoId = videoIdNode.GetString();
            if (string.IsNullOrWhiteSpace(videoId))
                continue;

            var snippet = item.GetProperty("snippet");
            result.Add(
                new(
                    videoId,
                    HtmlDecode(GetString(snippet, "title")),
                    GetString(snippet, "channelId"),
                    HtmlDecode(GetString(snippet, "channelTitle")),
                    HtmlDecode(GetString(snippet, "description")),
                    ReadThumbnail(snippet),
                    index));
        }

        return result;
    }

    private async Task<IReadOnlyList<RankedVideo>> HydrateVideosAsync(
        IReadOnlyList<SearchHit> searchResults,
        YouTubeLessonSemanticContext semanticContext,
        HashSet<string> preferredChannelIds,
        CancellationToken cancellationToken)
    {
        if (searchResults.Count == 0)
            return [];

        var ids = string.Join(
            ",",
            searchResults
                .Select(x => x.VideoId)
                .Distinct(StringComparer.Ordinal));

        var url =
            "https://www.googleapis.com/youtube/v3/videos" +
            "?part=snippet,contentDetails,statistics,status" +
            $"&id={Uri.EscapeDataString(ids)}" +
            $"&key={Uri.EscapeDataString(_options.ApiKey)}";

        using var response = await _httpClient.GetAsync(
            url,
            cancellationToken);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(
            cancellationToken);
        using var document = await JsonDocument.ParseAsync(
            stream,
            cancellationToken: cancellationToken);

        var hitById = searchResults.ToDictionary(
            x => x.VideoId,
            StringComparer.Ordinal);

        var hydrated = new List<HydratedVideo>();

        if (document.RootElement.TryGetProperty("items", out var items) &&
            items.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in items.EnumerateArray())
            {
                var id = GetString(item, "id");
                if (string.IsNullOrWhiteSpace(id) ||
                    !hitById.TryGetValue(id, out var hit))
                {
                    continue;
                }

                var status = item.GetProperty("status");
                if (!GetBoolean(status, "embeddable") ||
                    !string.Equals(
                        GetString(status, "privacyStatus"),
                        "public",
                        StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var snippet = item.GetProperty("snippet");
                var statistics = item.GetProperty("statistics");
                var contentDetails = item.GetProperty("contentDetails");
                var channelId = GetString(snippet, "channelId");

                hydrated.Add(
                    new(
                        hit,
                        HtmlDecode(GetString(snippet, "title")),
                        HtmlDecode(GetString(snippet, "channelTitle")),
                        channelId,
                        HtmlDecode(GetString(snippet, "description")),
                        ReadThumbnail(snippet),
                        ReadLong(statistics, "viewCount"),
                        ReadLong(statistics, "likeCount"),
                        FormatDuration(GetString(contentDetails, "duration")),
                        ReadDurationSeconds(GetString(contentDetails, "duration")),
                        preferredChannelIds.Contains(channelId)));
            }
        }

        if (hydrated.Count == 0)
            return [];

        var semanticRanked = hydrated
            .Select(video =>
            {
                // Learner refinement can affect YouTube's search order, but
                // every hard lesson signal is calculated from immutable lesson
                // metadata: topic, official objectives, skill mapping,
                // difficulty and prerequisites.
                var topic = CalculateRelevance(
                    semanticContext.TopicQuery,
                    video.Title,
                    video.Description);
                var objective = CalculateAggregateMatch(
                    semanticContext.LearningObjectives,
                    video.Title,
                    video.Description,
                    neutralWhenEmpty: .5d);
                var skill = CalculateAggregateMatch(
                    semanticContext.SkillTerms,
                    video.Title,
                    video.Description,
                    neutralWhenEmpty: .5d);
                var prerequisite = CalculateAggregateMatch(
                    semanticContext.PrerequisiteTerms,
                    video.Title,
                    video.Description,
                    neutralWhenEmpty: .75d);
                var difficulty = CalculateDifficultyMatch(
                    semanticContext.DifficultyLabel,
                    video.Title,
                    video.Description);
                var teaching = CalculateTeachingQuality(
                    video.Title,
                    video.Description,
                    video.DurationSeconds);

                var semanticScore =
                    (topic * .20d) +
                    (objective * .25d) +
                    (skill * .20d) +
                    (difficulty * .12d) +
                    (prerequisite * .08d) +
                    (teaching * .08d);

                var topicPercent = Percent(topic);
                var candidate = new YouTubeLessonVideo(
                    video.Hit.VideoId,
                    video.Title,
                    video.ChannelTitle,
                    $"https://www.youtube.com/watch?v={video.Hit.VideoId}",
                    $"https://www.youtube-nocookie.com/embed/{video.Hit.VideoId}?rel=0&modestbranding=1",
                    video.ThumbnailUrl,
                    video.DurationLabel,
                    video.ViewCount,
                    video.LikeCount,
                    topicPercent,
                    video.IsPreferred)
                {
                    ObjectiveMatchPercent = Percent(objective),
                    SkillMatchPercent = Percent(skill),
                    DifficultyMatchPercent = Percent(difficulty),
                    PrerequisiteSuitabilityPercent = Percent(prerequisite),
                    TeachingQualityPercent = Percent(teaching)
                };

                return new RankedVideo(
                    candidate,
                    topicPercent,
                    video.IsPreferred,
                    semanticScore,
                    semanticScore);
            })
            .OrderByDescending(x => x.SemanticScore)
            .ToArray();

        // Diversity is applied before channel and popularity bonuses so a
        // duplicate cannot survive merely because it is popular or preferred.
        var diverse = RemoveDuplicateOrSimilarVideos(semanticRanked);
        if (diverse.Count == 0)
            return [];

        var maxViewLog = diverse.Max(x =>
            Math.Log10(Math.Max(1, x.Video.ViewCount) + 1));
        var maxLikeLog = diverse.Max(x =>
            Math.Log10(Math.Max(1, x.Video.LikeCount) + 1));

        return diverse
            .Select(video =>
            {
                var viewSignal = maxViewLog <= 0
                    ? 0
                    : Math.Log10(Math.Max(1, video.Video.ViewCount) + 1) /
                      maxViewLog;
                var likeSignal = maxLikeLog <= 0
                    ? 0
                    : Math.Log10(Math.Max(1, video.Video.LikeCount) + 1) /
                      maxLikeLog;
                var position = searchResults
                    .First(x => x.VideoId == video.Video.VideoId)
                    .Position;
                var positionSignal =
                    1d - Math.Min(1d, (position - 1d) / 50d);

                var youtubeSignal =
                    (viewSignal * .50d) +
                    (likeSignal * .30d) +
                    (positionSignal * .20d);

                var preferredBonus = video.IsPreferredChannel ? .04d : 0d;
                var finalScore =
                    video.SemanticScore +
                    preferredBonus +
                    (youtubeSignal * .03d);

                return video with
                {
                    Video = video.Video with
                    {
                        MatchPercent = Percent(finalScore)
                    },
                    RankScore = finalScore
                };
            })
            .OrderByDescending(x => x.RankScore)
            .ToArray();
    }

    private static double CalculateAggregateMatch(
        IReadOnlyList<string> signals,
        string title,
        string description,
        double neutralWhenEmpty)
    {
        if (signals.Count == 0)
            return neutralWhenEmpty;

        var scores = signals
            .Select(signal => CalculateRelevance(signal, title, description))
            .OrderByDescending(x => x)
            .Take(2)
            .ToArray();

        return scores.Length == 1
            ? scores[0]
            : (scores[0] * .65d) + (scores[1] * .35d);
    }

    private static double CalculateDifficultyMatch(
        string target,
        string title,
        string description)
    {
        var candidate = ClassifyDifficulty($"{title} {description}");
        if (string.Equals(candidate, target, StringComparison.Ordinal))
            return 1d;

        return (target, candidate) switch
        {
            ("foundation", "standard") or
            ("standard", "foundation") or
            ("standard", "challenging") or
            ("challenging", "standard") => .62d,
            _ => .28d
        };
    }

    private static string ClassifyDifficulty(string value)
    {
        var text = value.ToLowerInvariant();
        if (new[]
            {
                "advanced", "reasoning", "proof", "modelling", "modeling",
                "challenge", "exam", "hard", "multi-step", "multistep",
                "application", "interpret"
            }.Any(x => text.Contains(x, StringComparison.Ordinal)))
        {
            return "challenging";
        }

        if (new[]
            {
                "basic", "beginner", "introduction", "intro", "foundation",
                "simple", "easy", "for beginners"
            }.Any(x => text.Contains(x, StringComparison.Ordinal)))
        {
            return "foundation";
        }

        return "standard";
    }

    private static double CalculateTeachingQuality(
        string title,
        string description,
        long durationSeconds)
    {
        var text = $"{title} {description}".ToLowerInvariant();
        var instructionalTerms = new[]
        {
            "explain", "explained", "lesson", "tutorial", "worked example",
            "examples", "step by step", "concept", "reasoning", "practice",
            "how to", "understand"
        };

        var score = .42d;
        score += Math.Min(
            .32d,
            instructionalTerms.Count(term =>
                text.Contains(term, StringComparison.Ordinal)) * .08d);

        if (durationSeconds is >= 240 and <= 2700)
            score += .16d;
        else if (durationSeconds is >= 120 and <= 5400)
            score += .08d;
        else if (durationSeconds is > 0 and < 60)
            score -= .22d;

        if (text.Contains("#shorts", StringComparison.Ordinal) ||
            text.Contains("shorts", StringComparison.Ordinal))
        {
            score -= .12d;
        }

        if (new[] { "hack", "trick only", "answer only" }
            .Any(term => text.Contains(term, StringComparison.Ordinal)))
        {
            score -= .08d;
        }

        return Math.Clamp(score, 0d, 1d);
    }

    private static IReadOnlyList<RankedVideo> RemoveDuplicateOrSimilarVideos(
        IReadOnlyList<RankedVideo> ranked)
    {
        var selected = new List<RankedVideo>();

        foreach (var candidate in ranked)
        {
            if (selected.Any(existing =>
                    AreSimilar(existing.Video, candidate.Video)))
            {
                continue;
            }

            selected.Add(candidate);
        }

        return selected;
    }

    private static bool AreSimilar(
        YouTubeLessonVideo left,
        YouTubeLessonVideo right)
    {
        if (string.Equals(left.VideoId, right.VideoId, StringComparison.Ordinal))
            return true;

        var leftTokens = SimilarityTokens(left.Title);
        var rightTokens = SimilarityTokens(right.Title);
        if (leftTokens.Count == 0 || rightTokens.Count == 0)
            return false;

        var intersection = leftTokens.Intersect(rightTokens).Count();
        var union = leftTokens.Union(rightTokens).Count();
        var similarity = union == 0 ? 0d : (double)intersection / union;

        var sameChannel = string.Equals(
            left.ChannelTitle,
            right.ChannelTitle,
            StringComparison.OrdinalIgnoreCase);

        return similarity >= (sameChannel ? .70d : .84d);
    }

    private static HashSet<string> SimilarityTokens(string title) =>
        Tokenize(title)
            .Where(token => !StopWords.Contains(token))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

    private static int Percent(double value) =>
        (int)Math.Round(
            Math.Clamp(value, 0d, 1d) * 100d,
            MidpointRounding.AwayFromZero);

    private static long ReadDurationSeconds(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return 0;

        try
        {
            return (long)Math.Round(XmlConvert.ToTimeSpan(value).TotalSeconds);
        }
        catch (FormatException)
        {
            return 0;
        }
    }

    private static void StoreCachedResult(
        string cacheKey,
        CacheEntry entry)
    {
        if (ResultCache.Count >= MaximumCacheEntries)
        {
            var now = DateTimeOffset.UtcNow;
            foreach (var pair in ResultCache)
            {
                if (pair.Value.ExpiresAtUtc <= now)
                    ResultCache.TryRemove(pair.Key, out _);
            }

            if (ResultCache.Count >= MaximumCacheEntries)
                ResultCache.Clear();
        }

        ResultCache[cacheKey] = entry;
    }

    private static double CalculateRelevance(
        string query,
        string title,
        string description)
    {
        var tokens = Tokenize(query)
            .Where(token => !StopWords.Contains(token))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        if (tokens.Length == 0)
            return .5d;

        var titleTokens = Tokenize(title)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        var descriptionTokens = Tokenize(description)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var titleMatches = tokens.Count(titleTokens.Contains);
        var descriptionMatches = tokens.Count(descriptionTokens.Contains);

        var titleCoverage = (double)titleMatches / tokens.Length;
        var descriptionCoverage = (double)descriptionMatches / tokens.Length;

        var normalizedTitle = NormalizeText(title);
        var normalizedQuery = NormalizeText(query);
        var phraseBonus =
            normalizedQuery.Length > 5 &&
            normalizedTitle.Contains(
                normalizedQuery,
                StringComparison.OrdinalIgnoreCase)
                ? 1d
                : 0d;

        return Math.Clamp(
            (titleCoverage * .72d) +
            (descriptionCoverage * .18d) +
            (phraseBonus * .10d),
            0d,
            1d);
    }

    private static IEnumerable<string> Tokenize(string value) =>
        TokenRegex()
            .Matches(value ?? string.Empty)
            .Select(match => NormalizeToken(match.Value.ToLowerInvariant()));

    private static string NormalizeToken(string token) =>
        token switch
        {
            "modelling" or "modeling" or "models" => "model",
            "functions" => "function",
            "expressions" => "expression",
            "equations" => "equation",
            "inequalities" => "inequality",
            "fractions" => "fraction",
            "ratios" => "ratio",
            "percentages" => "percentage",
            "polynomials" => "polynomial",
            "quadratics" => "quadratic",
            "graphs" => "graph",
            _ => token
        };

    private static string NormalizeText(string value) =>
        Regex.Replace(
            (value ?? string.Empty).ToLowerInvariant(),
            @"\s+",
            " ").Trim();

    private static string BuildQuery(
        string lessonTitle,
        string gradeLabel,
        string frameworkName,
        YouTubeLessonSemanticContext context,
        string? learnerQuery)
    {
        var learner = CleanLearnerQuery(learnerQuery);

        var pieces = new[]
        {
            context.TopicQuery,
            context.SkillTerms.FirstOrDefault() ?? string.Empty,
            gradeLabel,
            learner,
            "math"
        }
        .Where(x => !string.IsNullOrWhiteSpace(x));

        var query = Regex.Replace(
            string.Join(" ", pieces),
            @"\s+",
            " ").Trim();

        return string.Concat(
            query.EnumerateRunes().Take(220).Select(x => x.ToString()));
    }

    private static string BuildBroadQuery(
        string topicQuery,
        string? learnerQuery)
    {
        var learner = CleanLearnerQuery(learnerQuery);
        var pieces = new[]
        {
            topicQuery,
            learner,
            "math"
        }
        .Where(x => !string.IsNullOrWhiteSpace(x));

        return Regex.Replace(
            string.Join(" ", pieces),
            @"\s+",
            " ").Trim();
    }

    private static string CleanLearnerQuery(string? learnerQuery)
    {
        var learner = Regex.Replace(
            learnerQuery ?? string.Empty,
            @"\s+",
            " ").Trim();

        return string.Concat(
            learner
                .EnumerateRunes()
                .Take(80)
                .Select(rune => rune.ToString()));
    }

    private static string BuildChannelSearchUrl(
        string handle,
        string query) =>
        $"https://www.youtube.com/@{handle.TrimStart('@')}/search?query=" +
        Uri.EscapeDataString(query);

    private static string NormalizeCulture(string? cultureCode)
    {
        if (string.IsNullOrWhiteSpace(cultureCode))
            return "en";

        var value = cultureCode.Trim();
        var separator = value.IndexOf('-');
        return (separator > 0 ? value[..separator] : value)
            .ToLowerInvariant();
    }

    private static string HtmlDecode(string value) =>
        WebUtility.HtmlDecode(value ?? string.Empty);

    private static string GetString(
        JsonElement element,
        string property) =>
        element.TryGetProperty(property, out var node) &&
        node.ValueKind == JsonValueKind.String
            ? node.GetString() ?? string.Empty
            : string.Empty;

    private static bool GetBoolean(
        JsonElement element,
        string property) =>
        element.TryGetProperty(property, out var node) &&
        node.ValueKind is JsonValueKind.True or JsonValueKind.False &&
        node.GetBoolean();

    private static long ReadLong(
        JsonElement element,
        string property) =>
        element.TryGetProperty(property, out var node) &&
        node.ValueKind == JsonValueKind.String &&
        long.TryParse(
            node.GetString(),
            NumberStyles.None,
            CultureInfo.InvariantCulture,
            out var value)
            ? value
            : 0;

    private static string ReadThumbnail(JsonElement snippet)
    {
        if (!snippet.TryGetProperty("thumbnails", out var thumbnails))
            return string.Empty;

        foreach (var key in new[] { "high", "medium", "default" })
        {
            if (thumbnails.TryGetProperty(key, out var candidate))
            {
                var url = GetString(candidate, "url");
                if (!string.IsNullOrWhiteSpace(url))
                    return url;
            }
        }

        return string.Empty;
    }

    private static string FormatDuration(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        try
        {
            var duration = XmlConvert.ToTimeSpan(value);
            return duration.TotalHours >= 1
                ? duration.ToString(@"h\:mm\:ss", CultureInfo.InvariantCulture)
                : duration.ToString(@"m\:ss", CultureInfo.InvariantCulture);
        }
        catch (FormatException)
        {
            return string.Empty;
        }
    }

    [GeneratedRegex(
        @"[\p{L}\p{N}]+",
        RegexOptions.CultureInvariant)]
    private static partial Regex TokenRegex();

    private sealed record CacheEntry(
        DateTimeOffset ExpiresAtUtc,
        YouTubeLessonDiscoveryResult Result);

    private sealed record SearchHit(
        string VideoId,
        string Title,
        string ChannelId,
        string ChannelTitle,
        string Description,
        string ThumbnailUrl,
        int Position);

    private sealed record HydratedVideo(
        SearchHit Hit,
        string Title,
        string ChannelTitle,
        string ChannelId,
        string Description,
        string ThumbnailUrl,
        long ViewCount,
        long LikeCount,
        string DurationLabel,
        long DurationSeconds,
        bool IsPreferred);

    private sealed record RankedVideo(
        YouTubeLessonVideo Video,
        int RelevancePercent,
        bool IsPreferredChannel,
        double SemanticScore,
        double RankScore);
}
