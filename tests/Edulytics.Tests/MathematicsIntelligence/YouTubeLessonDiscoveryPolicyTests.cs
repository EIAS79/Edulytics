using System.Net;
using System.Text;
using Edulytics.Services.LessonContent;

using System.Reflection;
using Edulytics.Web.Controllers;
using Edulytics.Web.Resilience;
using Microsoft.AspNetCore.RateLimiting;
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

    [Fact]
    public async Task LearnerQuery_TruncatesWithoutSplittingAstralUnicode()
    {
        var service =
            new YouTubeLessonDiscoveryService(
                new HttpClient(),
                new YouTubeLessonDiscoveryOptions
                {
                    Enabled = true,
                    ApiKey = string.Empty
                });

        var refinement =
            new string('a', 79) +
            "😀" +
            "x";

        var result =
            await service.DiscoverAsync(
                "PED:TEST:G8:UNICODE",
                "Solve linear equations",
                "Grade 8",
                "en",
                refinement);

        Assert.Contains(
            "😀",
            result.SearchQuery,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "x math",
            result.SearchQuery,
            StringComparison.Ordinal);
        Assert.StartsWith(
            "https://www.youtube.com/results?search_query=",
            result.SearchUrl,
            StringComparison.Ordinal);
    }

    [Fact]
    public async Task Cache_IsolatedByImmutableLessonTopic_WhenFlattenedSearchQueryMatches()
    {
        using var client =
            new HttpClient(
                new CacheIsolationYouTubeHandler());

        var service =
            new YouTubeLessonDiscoveryService(
                client,
                new YouTubeLessonDiscoveryOptions
                {
                    Enabled = true,
                    ApiKey = "cache-isolation-test-key",
                    MinimumRelevancePercent = 34,
                    SearchResultCount = 8,
                    RelatedResultCount = 6
                });

        var broadLesson =
            await service.DiscoverAsync(
                "PED:TEST:G8:CACHE-BROAD",
                "Solve",
                "Grade 8",
                "en",
                "linear equations");

        Assert.NotNull(
            broadLesson.Featured);

        var preciseLesson =
            await service.DiscoverAsync(
                "PED:TEST:G8:CACHE-PRECISE",
                "Solve linear equations",
                "Grade 8",
                "en",
                null);

        Assert.Null(
            preciseLesson.Featured);
        Assert.Empty(
            preciseLesson.Related);
    }

    [Fact]
    public async Task LearnerRefinement_CannotMakeOffTopicVideoPassLessonGate()
    {
        using var client =
            new HttpClient(
                new LowRelevanceYouTubeHandler());

        var service =
            new YouTubeLessonDiscoveryService(
                client,
                new YouTubeLessonDiscoveryOptions
                {
                    Enabled = true,
                    ApiKey = "test-key",
                    MinimumRelevancePercent = 34,
                    SearchResultCount = 8,
                    RelatedResultCount = 6
                });

        var result =
            await service.DiscoverAsync(
                "PED:TEST:G8:OFFTOPIC-REFINEMENT",
                "Solve linear equations",
                "Grade 8",
                "en",
                "cooking pasta recipe kitchen timing guide");

        Assert.True(result.Available);
        Assert.Null(result.Featured);
        Assert.Empty(result.Related);
    }

    [Fact]
    public async Task HardRelevanceThreshold_DoesNotFallBackToUnrelatedVideo()
    {
        using var client =
            new HttpClient(
                new LowRelevanceYouTubeHandler());

        var service =
            new YouTubeLessonDiscoveryService(
                client,
                new YouTubeLessonDiscoveryOptions
                {
                    Enabled = true,
                    ApiKey = "test-key",
                    MinimumRelevancePercent = 95,
                    SearchResultCount = 8,
                    RelatedResultCount = 6
                });

        var result =
            await service.DiscoverAsync(
                "PED:TEST:G8:UNRELATED",
                "Solve linear equations",
                "Grade 8",
                "en",
                null);

        Assert.True(result.Available);
        Assert.Null(result.Featured);
        Assert.Empty(result.Related);
        Assert.Contains(
            "No embeddable YouTube result passed",
            result.Message,
            StringComparison.Ordinal);
    }

    [Fact]
    public async Task UpstreamTimeout_FailsOpenWithoutCancellingLessonRequest()
    {
        using var client =
            new HttpClient(
                new TimeoutYouTubeHandler())
            {
                Timeout =
                    TimeSpan.FromMilliseconds(20)
            };

        var service =
            new YouTubeLessonDiscoveryService(
                client,
                new YouTubeLessonDiscoveryOptions
                {
                    Enabled = true,
                    ApiKey = "test-key"
                });

        var result =
            await service.DiscoverAsync(
                "PED:TEST:G8:TIMEOUT",
                "Solve linear equations",
                "Grade 8",
                "en",
                null,
                CancellationToken.None);

        Assert.False(result.Available);
        Assert.Null(result.Featured);
        Assert.Contains(
            "timed out",
            result.Message,
            StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void StudentYouTubeEndpoint_UsesActorPartitionedNamedRatePolicy()
    {
        var action =
            typeof(StudentPortalController)
                .GetMethod(
                    nameof(StudentPortalController.LessonYouTube));

        Assert.NotNull(action);

        var attribute =
            action!.GetCustomAttribute<
                EnableRateLimitingAttribute>();

        Assert.NotNull(attribute);
        Assert.Equal(
            BackendResiliencePolicyNames.YouTubeLessonSearch,
            attribute!.PolicyName);
    }

    [Fact]
    public void StaffYouTubeEndpoint_UsesSameActorPartitionedRatePolicy()
    {
        var action =
            typeof(LessonContentController)
                .GetMethod(
                    nameof(LessonContentController.LessonYouTube));

        Assert.NotNull(action);

        var attribute =
            action!.GetCustomAttribute<
                EnableRateLimitingAttribute>();

        Assert.NotNull(attribute);
        Assert.Equal(
            BackendResiliencePolicyNames.YouTubeLessonSearch,
            attribute!.PolicyName);
    }

    [Fact]
    public async Task SemanticDiscovery_ExposesFinalLessonMatchSignals()
    {
        using var client =
            new HttpClient(
                new SemanticYouTubeHandler());

        var service =
            new YouTubeLessonDiscoveryService(
                client,
                new YouTubeLessonDiscoveryOptions
                {
                    Enabled = true,
                    ApiKey = "semantic-test-key",
                    MinimumRelevancePercent = 20,
                    SearchResultCount = 8,
                    RelatedResultCount = 6
                });

        var result = await service.DiscoverAsync(
            new YouTubeLessonDiscoveryRequest(
                "PED:TEST:G10:LINEAR-MODELLING",
                "Linear modelling — advanced reasoning",
                "Grade 10",
                "Cambridge Mathematics",
                "en",
                null,
                [
                    "Construct and interpret linear models from contextual information."
                ]));

        Assert.NotNull(result.Featured);
        Assert.DoesNotContain(
            "advanced reasoning",
            result.SearchQuery,
            StringComparison.OrdinalIgnoreCase);
        Assert.Contains(
            "Linear modelling",
            result.SearchQuery,
            StringComparison.OrdinalIgnoreCase);
        Assert.True(result.Featured!.MatchPercent > 0);
        Assert.True(result.Featured.ObjectiveMatchPercent > 0);
        Assert.True(result.Featured.DifficultyMatchPercent > 0);
        Assert.True(result.Featured.TeachingQualityPercent > 0);
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
    private sealed class CacheIsolationYouTubeHandler
        : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var path =
                request.RequestUri?.AbsolutePath
                ?? string.Empty;

            var json =
                path.EndsWith(
                    "/channels",
                    StringComparison.Ordinal)
                    ? """{"items":[]}"""
                    : path.EndsWith(
                        "/search",
                        StringComparison.Ordinal)
                        ? """
                          {
                            "items": [
                              {
                                "id": { "videoId": "cache-isolation-1" },
                                "snippet": {
                                  "title": "Solve a cooking recipe",
                                  "channelId": "UC-cache",
                                  "channelTitle": "Cache Test",
                                  "description": "A cooking demonstration",
                                  "thumbnails": {
                                    "high": {
                                      "url": "https://i.ytimg.com/vi/cache-isolation-1/hqdefault.jpg"
                                    }
                                  }
                                }
                              }
                            ]
                          }
                          """
                        : """
                          {
                            "items": [
                              {
                                "id": "cache-isolation-1",
                                "status": {
                                  "embeddable": true,
                                  "privacyStatus": "public"
                                },
                                "snippet": {
                                  "title": "Solve a cooking recipe",
                                  "channelId": "UC-cache",
                                  "channelTitle": "Cache Test",
                                  "description": "A cooking demonstration",
                                  "thumbnails": {
                                    "high": {
                                      "url": "https://i.ytimg.com/vi/cache-isolation-1/hqdefault.jpg"
                                    }
                                  }
                                },
                                "contentDetails": {
                                  "duration": "PT4M"
                                },
                                "statistics": {
                                  "viewCount": "2000",
                                  "likeCount": "100"
                                }
                              }
                            ]
                          }
                          """;

            return Task.FromResult(
                new HttpResponseMessage(
                    HttpStatusCode.OK)
                {
                    Content =
                        new StringContent(
                            json,
                            Encoding.UTF8,
                            "application/json")
                });
        }
    }

    private sealed class TimeoutYouTubeHandler
        : HttpMessageHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(
                TimeSpan.FromSeconds(5),
                cancellationToken);

            return new HttpResponseMessage(
                HttpStatusCode.OK);
        }
    }

    private sealed class SemanticYouTubeHandler
        : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var path = request.RequestUri?.AbsolutePath ?? string.Empty;
            var json =
                path.EndsWith("/channels", StringComparison.Ordinal)
                    ? """{"items":[]}"""
                    : path.EndsWith("/search", StringComparison.Ordinal)
                        ? """
                          {
                            "items": [
                              {
                                "id": { "videoId": "linear-model-1" },
                                "snippet": {
                                  "title": "Linear Modeling Explained with Worked Examples",
                                  "channelId": "UC-semantic",
                                  "channelTitle": "Math Teaching",
                                  "description": "Construct and interpret linear models from contextual information with step by step examples.",
                                  "thumbnails": {
                                    "high": {
                                      "url": "https://i.ytimg.com/vi/linear-model-1/hqdefault.jpg"
                                    }
                                  }
                                }
                              }
                            ]
                          }
                          """
                        : """
                          {
                            "items": [
                              {
                                "id": "linear-model-1",
                                "status": {
                                  "embeddable": true,
                                  "privacyStatus": "public"
                                },
                                "snippet": {
                                  "title": "Linear Modeling Explained with Worked Examples",
                                  "channelId": "UC-semantic",
                                  "channelTitle": "Math Teaching",
                                  "description": "Construct and interpret linear models from contextual information with step by step examples.",
                                  "thumbnails": {
                                    "high": {
                                      "url": "https://i.ytimg.com/vi/linear-model-1/hqdefault.jpg"
                                    }
                                  }
                                },
                                "contentDetails": {
                                  "duration": "PT12M"
                                },
                                "statistics": {
                                  "viewCount": "5000",
                                  "likeCount": "350"
                                }
                              }
                            ]
                          }
                          """;

            return Task.FromResult(
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        json,
                        Encoding.UTF8,
                        "application/json")
                });
        }
    }

    private sealed class LowRelevanceYouTubeHandler
        : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var path =
                request.RequestUri?.AbsolutePath
                ?? string.Empty;

            var json =
                path.EndsWith(
                    "/channels",
                    StringComparison.Ordinal)
                    ? """{"items":[]}"""
                    : path.EndsWith(
                        "/search",
                        StringComparison.Ordinal)
                        ? """
                          {
                            "items": [
                              {
                                "id": { "videoId": "unrelated-1" },
                                "snippet": {
                                  "title": "Cooking pasta recipe kitchen timing guide",
                                  "channelId": "UC-kitchen",
                                  "channelTitle": "Kitchen Lessons",
                                  "description": "Cooking pasta recipe kitchen timing guide",
                                  "thumbnails": {
                                    "high": {
                                      "url": "https://i.ytimg.com/vi/unrelated-1/hqdefault.jpg"
                                    }
                                  }
                                }
                              }
                            ]
                          }
                          """
                        : """
                          {
                            "items": [
                              {
                                "id": "unrelated-1",
                                "status": {
                                  "embeddable": true,
                                  "privacyStatus": "public"
                                },
                                "snippet": {
                                  "title": "Cooking pasta perfectly",
                                  "channelId": "UC-kitchen",
                                  "channelTitle": "Kitchen Lessons",
                                  "description": "Recipe and kitchen timing guide",
                                  "thumbnails": {
                                    "high": {
                                      "url": "https://i.ytimg.com/vi/unrelated-1/hqdefault.jpg"
                                    }
                                  }
                                },
                                "contentDetails": {
                                  "duration": "PT5M"
                                },
                                "statistics": {
                                  "viewCount": "1000000",
                                  "likeCount": "10000"
                                }
                              }
                            ]
                          }
                          """;

            return Task.FromResult(
                new HttpResponseMessage(
                    HttpStatusCode.OK)
                {
                    Content =
                        new StringContent(
                            json,
                            Encoding.UTF8,
                            "application/json")
                });
        }
    }

}
