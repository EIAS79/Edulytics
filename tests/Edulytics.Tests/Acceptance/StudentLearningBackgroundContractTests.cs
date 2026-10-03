namespace Edulytics.Tests.Acceptance;

public sealed class StudentLearningBackgroundContractTests
{
    [Fact]
    public void My_learning_page_uses_classic_student_portal_layout()
    {
        var layout = ReadRepositoryFile("src", "Edulytics.Web", "Views", "Shared", "_StudentLayout.cshtml");
        var view = ReadRepositoryFile("src", "Edulytics.Web", "Views", "StudentPortal", "Learning.cshtml");

        Assert.DoesNotContain("student-learning-page", layout, StringComparison.Ordinal);
        Assert.Contains("student-app-shell", layout, StringComparison.Ordinal);
        Assert.Contains("student-page-header", view, StringComparison.Ordinal);
        Assert.Contains("student-learning-stack", view, StringComparison.Ordinal);
        Assert.Contains("student-published-lessons", view, StringComparison.Ordinal);
        Assert.Contains("student-lesson-grid", view, StringComparison.Ordinal);
    }

    [Fact]
    public void My_learning_page_restores_curriculum_and_lesson_context_blocks()
    {
        var view = ReadRepositoryFile("src", "Edulytics.Web", "Views", "StudentPortal", "Learning.cshtml");

        Assert.Contains("student-learning-card", view, StringComparison.Ordinal);
        Assert.Contains("student-learning-meta", view, StringComparison.Ordinal);
        Assert.Contains("student-curriculum-tree", view, StringComparison.Ordinal);
        Assert.Contains("@L[\"AvailableLessons\"]", view, StringComparison.Ordinal);
        Assert.Contains("student-lesson-card", view, StringComparison.Ordinal);
        Assert.Contains("@lesson.TopicName", view, StringComparison.Ordinal);
        Assert.Contains("@lesson.FrameworkName", view, StringComparison.Ordinal);
    }

    [Fact]
    public void Classic_lesson_card_keeps_explicit_open_lesson_action()
    {
        var view = ReadRepositoryFile("src", "Edulytics.Web", "Views", "StudentPortal", "Learning.cshtml");

        Assert.Contains("class=\"student-primary-link\"", view, StringComparison.Ordinal);
        Assert.Contains("asp-controller=\"StudentPortal\"", view, StringComparison.Ordinal);
        Assert.Contains("asp-action=\"Lesson\"", view, StringComparison.Ordinal);
        Assert.Contains("asp-route-id=\"@lesson.Id\"", view, StringComparison.Ordinal);
        Assert.Contains("@L[\"OpenLesson\"]", view, StringComparison.Ordinal);
        Assert.DoesNotContain("floating-lesson-card", view, StringComparison.Ordinal);
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
