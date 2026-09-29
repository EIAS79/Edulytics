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
                                  "title": "Cooking pasta perfectly",
                                  "channelId": "UC-kitchen",
                                  "channelTitle": "Kitchen Lessons",
                                  "description": "Recipe and kitchen timing guide",
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
