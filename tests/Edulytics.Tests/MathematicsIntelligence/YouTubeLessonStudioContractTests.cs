namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class YouTubeLessonStudioContractTests
{
    private static readonly string Root = FindRoot();

    [Fact]
    public void SharedStudio_RemovesExplanatoryHeader_AndAutoLoadsDiscovery()
    {
        var partial = Read(
            "src/Edulytics.Web/Views/Shared/_LessonYouTubeStudio.cshtml");
        var script = Read(
            "src/Edulytics.Web/wwwroot/js/lesson-youtube-studio.js");

        Assert.DoesNotContain(
            "YouTube Learning Studio",
            partial,
            StringComparison.Ordinal);

        Assert.DoesNotContain(
            "Videos are ranked against the lesson topic",
            partial,
            StringComparison.Ordinal);

        Assert.DoesNotContain(
            "yt-studio-section-header",
            partial,
            StringComparison.Ordinal);

        Assert.Contains(
            "load(\"\");",
            script,
            StringComparison.Ordinal);
    }

    [Fact]
    public void StudentRichLesson_UsesSingleColumnCards_ForKeyConceptsExamplesAndMistakes()
    {
        var css = Read("src/Edulytics.Web/wwwroot/css/site.css");
        var richLesson = Read(
            "src/Edulytics.Web/Views/Shared/_RichLessonContentV2.cshtml");

        Assert.Contains(
            ".lesson-reader--student .rich-concept-grid,",
            css, StringComparison.Ordinal);
        Assert.Contains(
            ".lesson-reader--student .rich-example-grid,",
            css, StringComparison.Ordinal);
        Assert.Contains(
            ".lesson-reader--student .rich-mistake-grid {",
            css, StringComparison.Ordinal);
        Assert.Contains(
            "grid-template-columns: minmax(0, 1fr);",
            css, StringComparison.Ordinal);
        Assert.Contains(
            "class=\"rich-solution-list\"",
            richLesson, StringComparison.Ordinal);
    }

    [Fact]
    public void YouTubeSearch_RequiresConfiguredDataApiKey_AndDoesNotClaimResults()
    {
        var discovery = Read(
            "src/Edulytics.Services/LessonContent/YouTubeLessonDiscovery.cs");
        var settings = Read("src/Edulytics.Web/appsettings.json");

        Assert.Contains(
            "string.IsNullOrWhiteSpace(_options.ApiKey)",
            discovery, StringComparison.Ordinal);
        Assert.Contains(
            "YouTube Data API key is not configured",
            discovery, StringComparison.Ordinal);
        Assert.Contains(
            "\"ApiKey\": \"\"",
            settings, StringComparison.Ordinal);
    }

    [Fact]
    public void StaffAndStudent_UseTheSameYouTubeStudioPartial()
    {
        var staff = Read(
            "src/Edulytics.Web/Views/LessonContent/Detail.cshtml");
        var student = Read(
            "src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml");

        Assert.Contains(
            "_LessonYouTubeStudio",
            staff,
            StringComparison.Ordinal);

        Assert.Contains(
            "_LessonYouTubeStudio",
            student,
            StringComparison.Ordinal);
    }

    [Fact]
    public void YouTubeDiscovery_DetectsSessionRedirects_AndLogsSafeProviderStatus()
    {
        var script = Read("src/Edulytics.Web/wwwroot/js/lesson-youtube-studio.js");
        var discovery = Read("src/Edulytics.Services/LessonContent/YouTubeLessonDiscovery.cs");

        Assert.Contains("response.redirected", script, StringComparison.Ordinal);
        Assert.Contains("session-redirected", script, StringComparison.Ordinal);
        Assert.Contains("unexpected-non-json", script, StringComparison.Ordinal);
        Assert.Contains("YouTube Data API discovery failed; statusCode=", discovery,
            StringComparison.Ordinal);
        Assert.DoesNotContain("exception.Message", discovery,
            StringComparison.Ordinal);
    }

    private static string Read(string relative) =>
        File.ReadAllText(Path.Combine(Root, relative));

    private static string FindRoot()
    {
        for (
            var directory =
                new DirectoryInfo(AppContext.BaseDirectory);
            directory is not null;
            directory = directory.Parent)
        {
            if (File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
                return directory.FullName;
        }

        throw new DirectoryNotFoundException("Repository root not found.");
    }
}
