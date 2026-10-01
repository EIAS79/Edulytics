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
