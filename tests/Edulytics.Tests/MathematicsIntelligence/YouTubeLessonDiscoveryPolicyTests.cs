using Edulytics.Services.LessonContent;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class YouTubeLessonDiscoveryPolicyTests
{
    [Fact]
    public async Task PrimaryBand_UsesFivePrimaryTeachingChannels()
    {
        var result = await DiscoverAsync(
            "PED:CAMBRIDGE-INTL-MATH:S6:6F-3:BUILD",
            "Compare fractions with different denominators",
            "Cambridge Primary Stage 6");

        Assert.Equal(
            "Primary · Grades / Stages 1–6",
            result.GradeBand);

        Assert.Equal(
            [
                "Math Antics",
                "Khan Academy",
                "Math with Mr. J",
                "MashUp Math",
                "Numberock"
            ],
            result.PreferredChannels.Select(x => x.Name).ToArray());
    }

    [Fact]
    public async Task LowerSecondaryBand_UsesFiveLevelSevenToNineChannels()
    {
        var result = await DiscoverAsync(
            "PED:US-CCSS-MATH:G8:U03:L12",
            "Solve linear equations",
            "Grade 8");

        Assert.Equal(
            "Lower secondary · Grades / Stages 7–9",
            result.GradeBand);

        Assert.Equal(
            [
                "Khan Academy",
                "The Organic Chemistry Tutor",
                "Brian McLogan",
                "MashUp Math",
                "Math with Mr. J"
            ],
            result.PreferredChannels.Select(x => x.Name).ToArray());
    }

    [Fact]
    public async Task HigherBand_UsesFiveAdvancedMathematicsChannels()
    {
        var result = await DiscoverAsync(
            "PED:UAE-MOE-MATH:L10:ADVANCED:01:06:RATES-AND-UNIT-RATES",
            "Rates and unit rates",
            "Grade 10");

        Assert.Equal(
            "Higher mathematics · Grade 10+ / IGCSE / AS / A Level",
            result.GradeBand);

        Assert.Equal(
            [
                "Khan Academy",
                "The Organic Chemistry Tutor",
                "Professor Leonard",
                "PatrickJMT",
                "3Blue1Brown"
            ],
            result.PreferredChannels.Select(x => x.Name).ToArray());
    }

    [Fact]
    public async Task MissingApiKey_FailsOpenToYouTubeOnlyLessonSearchLinks()
    {
        var result = await DiscoverAsync(
            "PED:US-CCSS-MATH:G7:U06:L15",
            "Solve inequalities",
            "Grade 7");

        Assert.False(result.Available);
        Assert.Null(result.Featured);
        Assert.Empty(result.Related);
        Assert.Equal(5, result.PreferredChannels.Count);

        Assert.StartsWith(
            "https://www.youtube.com/results?search_query=",
            result.SearchUrl,
            StringComparison.Ordinal);

        Assert.All(result.PreferredChannels, channel =>
        {
            Assert.StartsWith(
                "https://www.youtube.com/@",
                channel.ChannelSearchUrl,
                StringComparison.Ordinal);
            Assert.DoesNotContain(
                "google",
                channel.ChannelSearchUrl,
                StringComparison.OrdinalIgnoreCase);
        });
    }

    [Fact]
    public async Task LearnerSearch_RemainsScopedToExactLesson()
    {
        var result = await DiscoverAsync(
            "PED:CAMBRIDGE-INTL-MATH:S6:6F-3:BUILD",
            "Compare fractions with different denominators",
            "Stage 6",
            "number line");

        Assert.Contains(
            "Compare fractions with different denominators",
            result.SearchQuery,
            StringComparison.OrdinalIgnoreCase);
        Assert.Contains(
            "fractions compare unlike denominators",
            result.SearchQuery,
            StringComparison.OrdinalIgnoreCase);
        Assert.Contains(
            "number line",
            result.SearchQuery,
            StringComparison.OrdinalIgnoreCase);
    }

    private static Task<YouTubeLessonDiscoveryResult> DiscoverAsync(
        string lessonCode,
        string title,
        string grade,
        string? learnerQuery = null)
    {
        var service = new YouTubeLessonDiscoveryService(
            new HttpClient(),
            new YouTubeLessonDiscoveryOptions
            {
                Enabled = true,
                ApiKey = string.Empty
            });

        return service.DiscoverAsync(
            lessonCode,
            title,
            grade,
            "en",
            learnerQuery);
    }
}
