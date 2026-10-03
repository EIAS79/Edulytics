namespace Edulytics.Tests.Acceptance;

public sealed class FloatingLearningVisualContractTests
{
    [Fact]
    public void Student_learning_uses_classic_cards_without_floating_visual_runtime()
    {
        var view = ReadRepositoryFile("src", "Edulytics.Web", "Views", "StudentPortal", "Learning.cshtml");

        Assert.Contains("student-lesson-card", view, StringComparison.Ordinal);
        Assert.Contains(@"asp-action=""Lesson""", view, StringComparison.Ordinal);
        Assert.Contains(@"asp-route-id=""@lesson.Id""", view, StringComparison.Ordinal);
        Assert.Contains("LessonVisualKind", view, StringComparison.Ordinal);
        Assert.Contains("classic-lesson-icon", view, StringComparison.Ordinal);
        Assert.DoesNotContain("data-floating-lesson", view, StringComparison.Ordinal);
        Assert.DoesNotContain("learning-math-atmosphere", view, StringComparison.Ordinal);
    }

    [Fact]
    public void Classic_learning_view_does_not_reference_floating_assets()
    {
        var view = ReadRepositoryFile("src", "Edulytics.Web", "Views", "StudentPortal", "Learning.cshtml");

        Assert.DoesNotContain("student-floating-learning.css", view, StringComparison.Ordinal);
        Assert.DoesNotContain("student-floating-learning.js", view, StringComparison.Ordinal);
        Assert.Contains("round5-ux-fixes.css", view, StringComparison.Ordinal);
    }

    private static string ReadRepositoryFile(params string[] relativeSegments)
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
            directory = directory.Parent;

        var root = directory?.FullName
            ?? throw new InvalidOperationException("Repository root not found.");

        return File.ReadAllText(Path.Combine([root, .. relativeSegments]));
    }
}
