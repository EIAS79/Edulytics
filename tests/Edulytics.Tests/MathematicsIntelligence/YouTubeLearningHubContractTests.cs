namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class YouTubeLearningHubContractTests
{
    [Fact]
    public void YoutubeHub_UsesCuratedChannelsAndCanonicalGradeCodes()
    {
        var root = FindRoot();
        var service = File.ReadAllText(Path.Combine(
            root, "src/Edulytics.Web/YouTubeLearning/YouTubeLearningService.cs"));

        Assert.Contains("primary-1-6", service, StringComparison.Ordinal);
        Assert.Contains("middle-7-9", service, StringComparison.Ordinal);
        Assert.Contains("higher-10-plus", service, StringComparison.Ordinal);
        Assert.Contains(@"(?:^|:)(?:s|g|l)(?<level>\\d{1,2})(?::|$)", service, StringComparison.Ordinal);
        Assert.DoesNotContain("SearchAnyChannelAsync", service, StringComparison.Ordinal);
        Assert.Contains("videoEmbeddable=true", service, StringComparison.Ordinal);
        Assert.Contains("safeSearch=strict", service, StringComparison.Ordinal);
    }

    [Fact]
    public void YoutubeHub_PreservesReviewedResourcesAndPermitsThumbnailOrigin()
    {
        var root = FindRoot();
        var view = File.ReadAllText(Path.Combine(
            root, "src/Edulytics.Web/Views/Shared/_RichLessonContentV2.cshtml"));
        var security = File.ReadAllText(Path.Combine(
            root, "src/Edulytics.Web/Middleware/SecurityHeadersMiddleware.cs"));

        Assert.Contains("externalHelp.ApprovedResources", view, StringComparison.Ordinal);
        Assert.Contains("data-yt-learning-hub", view, StringComparison.Ordinal);
        Assert.Contains("https://i.ytimg.com", security, StringComparison.Ordinal);
        Assert.Contains("https://www.youtube-nocookie.com", security, StringComparison.Ordinal);
    }

    private static string FindRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
                return directory.FullName;
            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Edulytics solution root not found.");
    }
}
