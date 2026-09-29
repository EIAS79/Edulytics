using System.Collections.Concurrent;
using System.Globalization;
using System.Net;
using System.Text.Json;

namespace Edulytics.Web.YouTubeLearning;

public sealed record YouTubeLearningVideo(
    string VideoId,
    string Title,
    string ChannelTitle,
    string ChannelHandle,
    string ThumbnailUrl,
    long ViewCount,
    long LikeCount,
    double RelevanceScore,
    bool PreferredChannel);

public sealed record YouTubeLearningResult(
    string Query,
    string Band,
    IReadOnlyList<string> PreferredChannels,
    YouTubeLearningVideo? Featured,
    IReadOnlyList<YouTubeLearningVideo> Related,
    bool UsedFallback,
    bool IsConfigured,
    string? Message);

public interface IYouTubeLearningService
{
    Task<YouTubeLearningResult> SearchAsync(
        string lessonCode,
        string lessonTitle,
        string cultureCode,
        string? userQuery,
        CancellationToken cancellationToken);
}

public sealed class YouTubeLearningService(
    IHttpClientFactory httpClientFactory,
    IConfiguration configuration,
    IMemoryCache cache,
    ILogger<YouTubeLearningService> logger)
    : IYouTubeLearningService
{
    private sealed record ChannelProfile(string Name, string Handle);

    private static readonly IReadOnlyDictionary<string, ChannelProfile[]> Bands =
        new Dictionary<string, ChannelProfile[]>(StringComparer.Ordinal)
        {
            ["primary-1-6"] =
            [
                new("Math Antics", "@mathantics"),
                new("Khan Academy", "@khanacademy"),
                new("Numberblocks", "@Numberblocks"),
                new("Math with Mr. J", "@MathwithMrJ"),
                new("Scratch Garden", "@ScratchGarden")
            ],
            ["middle-7-9"] =
            [
                new("Khan Academy", "@khanacademy"),
                new("Math Antics", "@mathantics"),
                new("Math with Mr. J", "@MathwithMrJ"),
                new("Eddie Woo", "@misterwootube"),
                new("The Organic Chemistry Tutor", "@TheOrganicChemistryTutor")
            ],
            ["higher-10-plus"] =
            [
                new("Khan Academy", "@khanacademy"),
                new("3Blue1Brown", "@3blue1brown"),
                new("Professor Leonard", "@ProfessorLeonard"),
                new("The Organic Chemistry Tutor", "@TheOrganicChemistryTutor"),
                new("blackpenredpen", "@blackpenredpen")
            ]
        };

    public async Task<YouTubeLearningResult> SearchAsync(
        string lessonCode,
        string lessonTitle,
        string cultureCode,
        string? userQuery,
        CancellationToken cancellationToken)
    {
        var band = ResolveBand(lessonCode);
        var preferred = Bands[band];
        var query = BuildQuery(lessonTitle, cultureCode, userQuery);
        var apiKey = configuration["Edulytics:YouTube:ApiKey"] ??
                     configuration["YouTube:ApiKey"];

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return new(
                query,
                band,
                preferred.Select(x => x.Name).ToArray(),
                null,
                [],
                false,
                false,
                "YouTube search is not configured on this environment.");
        }

        var cacheKey = $"yt-learning:v1:{band}:{cultureCode}:{query}".ToLowerInvariant();
        if (cache.TryGetValue(cacheKey, out YouTubeLearningResult? cached) &&
            cached is not null)
        {
            return cached;
        }

        var client = httpClientFactory.CreateClient("YouTubeLearning");
        var candidates = new List<YouTubeLearningVideo>();

        // Search preferred channels in priority order. Stop once the pool is
        // large enough; this avoids spending search quota on unnecessary calls.
        foreach (var channel in preferred)
        {
            var channelId = await ResolveChannelIdAsync(
                client,
                channel,
                apiKey,
                cancellationToken);

            if (string.IsNullOrWhiteSpace(channelId))
                continue;

            var found = await SearchChannelAsync(
                client,
                channel,
                channelId,
                query,
                apiKey,
                cancellationToken);

            candidates.AddRange(found);

            if (candidates.Count >= 10)
                break;
        }

        var usedFallback = false;
        if (candidates.Count == 0)
        {
            usedFallback = true;
            candidates.AddRange(await SearchAnyChannelAsync(
                client,
                query,
                apiKey,
                cancellationToken));
        }

        var ranked = candidates
            .GroupBy(x => x.VideoId, StringComparer.Ordinal)
            .Select(x => x.First())
            .OrderByDescending(x => x.RelevanceScore)
            .ThenByDescending(x => x.ViewCount)
            .ThenByDescending(x => x.LikeCount)
            .Take(8)
            .ToArray();

        var result = new YouTubeLearningResult(
            query,
            band,
            preferred.Select(x => x.Name).ToArray(),
            ranked.FirstOrDefault(),
            ranked.Skip(1).Take(7).ToArray(),
            usedFallback,
            true,
            ranked.Length == 0
                ? "No sufficiently relevant YouTube video was found."
                : null);

        cache.Set(cacheKey, result, TimeSpan.FromHours(6));
        return result;
    }

    private async Task<string?> ResolveChannelIdAsync(
        HttpClient client,
        ChannelProfile channel,
        string apiKey,
        CancellationToken cancellationToken)
    {
        var key = $"yt-channel:v1:{channel.Handle}".ToLowerInvariant();
        if (cache.TryGetValue(key, out string? cached) &&
            !string.IsNullOrWhiteSpace(cached))
        {
            return cached;
        }

        var handle = channel.Handle.TrimStart('@');
        var url =
            $"channels?part=id&forHandle={Uri.EscapeDataString(handle)}&key={Uri.EscapeDataString(apiKey)}";

        try
        {
            using var response = await client.GetAsync(url, cancellationToken);
            if (!response.IsSuccessStatusCode)
                return null;

            using var doc = JsonDocument.Parse(
                await response.Content.ReadAsStreamAsync(cancellationToken));

            var id = doc.RootElement
                .GetProperty("items")
                .EnumerateArray()
                .Select(x => x.GetProperty("id").GetString())
                .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));

            if (!string.IsNullOrWhiteSpace(id))
                cache.Set(key, id, TimeSpan.FromDays(7));

            return id;
        }
        catch (Exception ex) when (
            ex is HttpRequestException or JsonException or TaskCanceledException)
        {
            logger.LogWarning(ex, "Unable to resolve YouTube channel {Handle}.", channel.Handle);
            return null;
        }
    }

    private async Task<IReadOnlyList<YouTubeLearningVideo>> SearchChannelAsync(
        HttpClient client,
        ChannelProfile channel,
        string channelId,
        string query,
        string apiKey,
        CancellationToken cancellationToken)
    {
        var url =
            $"search?part=snippet&type=video&videoEmbeddable=true&safeSearch=strict&maxResults=8" +
            $"&order=relevance&channelId={Uri.EscapeDataString(channelId)}" +
            $"&q={Uri.EscapeDataString(query)}&key={Uri.EscapeDataString(apiKey)}";

        return await SearchAndEnrichAsync(
            client,
            url,
            query,
            channel.Handle,
            true,
            apiKey,
            cancellationToken);
    }

    private async Task<IReadOnlyList<YouTubeLearningVideo>> SearchAnyChannelAsync(
        HttpClient client,
        string query,
        string apiKey,
        CancellationToken cancellationToken)
    {
        var url =
            $"search?part=snippet&type=video&videoEmbeddable=true&safeSearch=strict&maxResults=20" +
            $"&order=relevance&q={Uri.EscapeDataString(query + " mathematics lesson")}" +
            $"&key={Uri.EscapeDataString(apiKey)}";

        return await SearchAndEnrichAsync(
            client,
            url,
            query,
            string.Empty,
            false,
            apiKey,
            cancellationToken);
    }

    private async Task<IReadOnlyList<YouTubeLearningVideo>> SearchAndEnrichAsync(
        HttpClient client,
        string searchUrl,
        string query,
        string channelHandle,
        bool preferredChannel,
        string apiKey,
        CancellationToken cancellationToken)
    {
        try
        {
            using var response = await client.GetAsync(searchUrl, cancellationToken);
            if (!response.IsSuccessStatusCode)
                return [];

            using var doc = JsonDocument.Parse(
                await response.Content.ReadAsStreamAsync(cancellationToken));

            var raw = doc.RootElement.GetProperty("items").EnumerateArray()
                .Select(item =>
                {
                    var snippet = item.GetProperty("snippet");
                    return new
                    {
                        Id = item.GetProperty("id").GetProperty("videoId").GetString() ?? string.Empty,
                        Title = WebUtility.HtmlDecode(snippet.GetProperty("title").GetString() ?? string.Empty),
                        Channel = WebUtility.HtmlDecode(snippet.GetProperty("channelTitle").GetString() ?? string.Empty),
                        Description = WebUtility.HtmlDecode(snippet.GetProperty("description").GetString() ?? string.Empty),
                        Thumbnail = ResolveThumbnail(snippet.GetProperty("thumbnails"))
                    };
                })
                .Where(x => x.Id.Length == 11)
                .ToArray();

            if (raw.Length == 0)
                return [];

            var stats = await LoadStatisticsAsync(
                client,
                raw.Select(x => x.Id).ToArray(),
                apiKey,
                cancellationToken);

            return raw
                .Select(x =>
                {
                    stats.TryGetValue(x.Id, out var s);
                    var relevance = ScoreRelevance(
                        query,
                        x.Title,
                        x.Description,
                        preferredChannel,
                        s.Views,
                        s.Likes);

                    return new YouTubeLearningVideo(
                        x.Id,
                        x.Title,
                        x.Channel,
                        channelHandle,
                        x.Thumbnail,
                        s.Views,
                        s.Likes,
                        relevance,
                        preferredChannel);
                })
                .Where(x => x.RelevanceScore >= (preferredChannel ? 0.34 : 0.50))
                .ToArray();
        }
        catch (Exception ex) when (
            ex is HttpRequestException or JsonException or TaskCanceledException)
        {
            logger.LogWarning(ex, "YouTube lesson search failed.");
            return [];
        }
    }

    private static async Task<Dictionary<string, (long Views, long Likes)>> LoadStatisticsAsync(
        HttpClient client,
        IReadOnlyList<string> ids,
        string apiKey,
        CancellationToken cancellationToken)
    {
        var result = new Dictionary<string, (long Views, long Likes)>(StringComparer.Ordinal);
        if (ids.Count == 0)
            return result;

        var url =
            $"videos?part=statistics&id={Uri.EscapeDataString(string.Join(',', ids))}" +
            $"&key={Uri.EscapeDataString(apiKey)}";

        using var response = await client.GetAsync(url, cancellationToken);
        if (!response.IsSuccessStatusCode)
            return result;

        using var doc = JsonDocument.Parse(
            await response.Content.ReadAsStreamAsync(cancellationToken));

        foreach (var item in doc.RootElement.GetProperty("items").EnumerateArray())
        {
            var id = item.GetProperty("id").GetString();
            if (string.IsNullOrWhiteSpace(id))
                continue;

            var statistics = item.GetProperty("statistics");
            result[id] = (
                ReadLong(statistics, "viewCount"),
                ReadLong(statistics, "likeCount"));
        }

        return result;
    }

    private static long ReadLong(JsonElement element, string property) =>
        element.TryGetProperty(property, out var value) &&
        long.TryParse(value.GetString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed)
            ? parsed
            : 0;

    private static string ResolveThumbnail(JsonElement thumbnails)
    {
        foreach (var key in new[] { "maxres", "standard", "high", "medium", "default" })
        {
            if (thumbnails.TryGetProperty(key, out var value) &&
                value.TryGetProperty("url", out var url))
            {
                return url.GetString() ?? string.Empty;
            }
        }

        return string.Empty;
    }

    private static double ScoreRelevance(
        string query,
        string title,
        string description,
        bool preferred,
        long views,
        long likes)
    {
        var tokens = Tokenize(query);
        if (tokens.Length == 0)
            return 0;

        var titleText = title.ToLowerInvariant();
        var descriptionText = description.ToLowerInvariant();
        var titleMatches = tokens.Count(titleText.Contains);
        var bodyMatches = tokens.Count(descriptionText.Contains);
        var lexical = Math.Min(1d, (titleMatches * 1.7 + bodyMatches * .35) / tokens.Length);
        var popularity = Math.Min(1d, Math.Log10(Math.Max(10, views)) / 8d);
        var approval = views <= 0 ? 0d : Math.Min(1d, (double)likes / views * 35d);

        return lexical * .68 +
               popularity * .16 +
               approval * .06 +
               (preferred ? .10 : 0d);
    }

    private static string[] Tokenize(string value) =>
        value.ToLowerInvariant()
            .Split([' ', '-', '_', ':', ',', '.', '(', ')', '/', '\\'],
                StringSplitOptions.RemoveEmptyEntries)
            .Where(x => x.Length > 2)
            .Distinct(StringComparer.Ordinal)
            .Take(14)
            .ToArray();

    private static string BuildQuery(
        string lessonTitle,
        string cultureCode,
        string? userQuery)
    {
        var requested = string.IsNullOrWhiteSpace(userQuery)
            ? lessonTitle
            : userQuery.Trim();

        var language = cultureCode.Split('-', StringSplitOptions.RemoveEmptyEntries)
            .FirstOrDefault()?.ToLowerInvariant();

        var suffix = language switch
        {
            "pl" => "matematyka wyjaśnienie",
            "ar" => "رياضيات شرح",
            _ => "math explained"
        };

        return $"{requested} {suffix}".Trim();
    }

    internal static string ResolveBand(string lessonCode)
    {
        var code = lessonCode.ToLowerInvariant();
        var grade = ExtractLevel(code);

        if (grade is >= 1 and <= 6)
            return "primary-1-6";

        if (grade is >= 7 and <= 9)
            return "middle-7-9";

        return "higher-10-plus";
    }

    private static int? ExtractLevel(string code)
    {
        foreach (var marker in new[] { "stage", "grade", "-g", "-l" })
        {
            var index = code.IndexOf(marker, StringComparison.Ordinal);
            if (index < 0)
                continue;

            index += marker.Length;
            var digits = new string(code.Skip(index).TakeWhile(char.IsDigit).ToArray());
            if (int.TryParse(digits, out var value))
                return value;
        }

        return null;
    }
}
