using Edulytics.Core.Curriculum;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class RichLessonExternalHelpTests
{
    [Fact]
    public void Phase8A_ApprovedResourceRegistry_IsValid()
    {
        RichLessonExternalHelpRegistry.Validate();
    }

    [Fact]
    public void Phase8A_EquivalentFractionsLesson_ResolvesOnlyApprovedResources()
    {
        var help = RichLessonExternalHelpRegistry.Resolve(
            "PED:US-CCSS-MATH:G4:U02:L07",
            "Equivalent fractions",
            "en");

        Assert.NotEmpty(help.ApprovedResources);
        Assert.All(help.ApprovedResources, resource =>
        {
            Assert.Equal(
                RichLessonExternalResourceReviewStatus.Approved,
                resource.ReviewStatus);
            Assert.StartsWith(
                "https://",
                resource.Url,
                StringComparison.OrdinalIgnoreCase);
            Assert.False(string.IsNullOrWhiteSpace(resource.WhyRecommended));
        });
    }

    [Fact]
    public void Phase8A_SearchHelp_IsDeterministicAndContainsNoLearnerIdentity()
    {
        var first = RichLessonExternalHelpRegistry.Resolve(
            "PED:US-CCSS-MATH:G4:U02:L07",
            "Equivalent fractions",
            "en");

        var second = RichLessonExternalHelpRegistry.Resolve(
            "PED:US-CCSS-MATH:G4:U02:L07",
            "Equivalent fractions",
            "en");

        Assert.Equal(first.SearchSuggestions, second.SearchSuggestions);
        Assert.Equal(2, first.SearchSuggestions.Count);

        Assert.Contains(
            first.SearchSuggestions,
            x => x.Provider == "Google" &&
                 x.Url.StartsWith(
                     "https://www.google.com/search?q=",
                     StringComparison.Ordinal));
        Assert.Contains(
            first.SearchSuggestions,
            x => x.Provider == "YouTube" &&
                 x.Url.StartsWith(
                     "https://www.youtube.com/results?search_query=",
                     StringComparison.Ordinal));

        var serialized = string.Join(
            " ",
            first.SearchSuggestions.SelectMany(x =>
                new[] { x.Label, x.Query, x.Url }));

        Assert.DoesNotContain(
            "student",
            serialized,
            StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain(
            "schoolId",
            serialized,
            StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain(
            "userId",
            serialized,
            StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Phase8A_UnknownLesson_FailsClosedForCuratedResources()
    {
        var help = RichLessonExternalHelpRegistry.Resolve(
            "PED:DOES-NOT-EXIST",
            "Unknown mathematics lesson",
            "en");

        Assert.Empty(help.ApprovedResources);
        Assert.Empty(help.ApprovedVideos);
        Assert.Equal(2, help.SearchSuggestions.Count);
    }

    [Fact]
    public void Phase8A_SearchSuggestions_LocalizeTheHelpIntent()
    {
        var polish = RichLessonExternalHelpRegistry.Resolve(
            "PED:US-CCSS-MATH:G4:U02:L07",
            "Ułamki równoważne",
            "pl-PL");
        var arabic = RichLessonExternalHelpRegistry.Resolve(
            "PED:US-CCSS-MATH:G4:U02:L07",
            "الكسور المتكافئة",
            "ar");

        Assert.Contains(
            polish.SearchSuggestions,
            x => x.Query.Contains(
                "wyjaśnienie krok po kroku",
                StringComparison.Ordinal));
        Assert.Contains(
            arabic.SearchSuggestions,
            x => x.Query.Contains(
                "شرح خطوة بخطوة",
                StringComparison.Ordinal));
    }

    [Fact]
    public void Phase8A_RoundingLesson_ResolvesReviewedEmbeddedVideo()
    {
        var help = RichLessonExternalHelpRegistry.Resolve(
            "PED:UAE-MOE-MATH:L4:COMMON:01:02:ROUNDING-TO-POWERS-OF-TEN",
            "Rounding to powers of ten",
            "en");

        var video = Assert.Single(help.ApprovedVideos);
        Assert.Equal("YouTube", video.Provider);
        Assert.Equal("fd-E18EqSVk", video.VideoId);
        Assert.Equal(
            RichLessonExternalResourceReviewStatus.Approved,
            video.ReviewStatus);
    }

    [Fact]
    public void Phase8A_SharedRenderer_EmbedsReviewedHelpAndDoesNotExposeSearchExits()
    {
        var root = FindRoot();
        var partial = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Shared/_RichLessonContentV2.cshtml"));

        Assert.Contains(
            "RichLessonExternalHelpRegistry.Resolve",
            partial,
            StringComparison.Ordinal);
        Assert.Contains(
            "approvedExternalVideos",
            partial,
            StringComparison.Ordinal);
        Assert.Contains(
            "youtube-nocookie.com/embed/",
            partial,
            StringComparison.Ordinal);
        Assert.Contains(
            "Help content never affects mastery, assessment, or Adaptive decisions",
            partial,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "externalHelp.SearchSuggestions.Count",
            partial,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "suggestion.Url",
            partial,
            StringComparison.Ordinal);
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
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException(
            "Edulytics solution root not found.");
    }
}
