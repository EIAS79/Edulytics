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
    public void My_learning_page_keeps_curriculum_context_but_removes_redundant_lesson_copy()
    {
        var view = ReadRepositoryFile("src", "Edulytics.Web", "Views", "StudentPortal", "Learning.cshtml");

        Assert.Contains("student-learning-card", view, StringComparison.Ordinal);
        Assert.Contains("student-learning-meta", view, StringComparison.Ordinal);
        Assert.Contains("student-curriculum-tree", view, StringComparison.Ordinal);
        Assert.DoesNotContain("@L[\"AvailableLessons\"]", view, StringComparison.Ordinal);
        Assert.Contains("student-lesson-card", view, StringComparison.Ordinal);
        Assert.Contains("@lesson.TopicName", view, StringComparison.Ordinal);
        Assert.DoesNotContain("@lesson.FrameworkName", view, StringComparison.Ordinal);
        Assert.DoesNotContain("student-lesson-primary-context", view, StringComparison.Ordinal);
        Assert.DoesNotContain("student-lesson-secondary-context", view, StringComparison.Ordinal);
        Assert.DoesNotContain("<p class=\"student-empty\">@S[\"NoLearningYet\"]</p>", view, StringComparison.Ordinal);
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


    [Fact]
    public void Classic_lesson_cards_include_contextual_math_icons()
    {
        var view = ReadRepositoryFile("src", "Edulytics.Web", "Views", "StudentPortal", "Learning.cshtml");
        var css = ReadRepositoryFile("src", "Edulytics.Web", "wwwroot", "css", "classic-learning-icons.css");

        Assert.Contains("LessonVisualKind", view, StringComparison.Ordinal);
        Assert.Contains("classic-lesson-icon", view, StringComparison.Ordinal);
        Assert.Contains("data-lesson-visual", view, StringComparison.Ordinal);
        Assert.Contains("case \"fraction\"", view, StringComparison.Ordinal);
        Assert.Contains("case \"coordinates\"", view, StringComparison.Ordinal);
        Assert.Contains("case \"geometry\"", view, StringComparison.Ordinal);
        Assert.Contains("classic-lesson-icon", css, StringComparison.Ordinal);
        Assert.Contains("html[dir=\"rtl\"]", css, StringComparison.Ordinal);
    }


    // CI guard: classic lesson spacing remains intentionally compact.
    [Fact]
    public void Classic_lesson_cards_use_tighter_vertical_spacing()
    {
        var css = ReadRepositoryFile("src", "Edulytics.Web", "wwwroot", "css", "classic-learning-icons.css");

        Assert.Contains("student-learning-stack + .student-published-lessons", css, StringComparison.Ordinal);
        Assert.Contains("margin-top:16px", css, StringComparison.Ordinal);
        Assert.Contains("min-height:0", css, StringComparison.Ordinal);
        Assert.Contains("height:auto", css, StringComparison.Ordinal);
        Assert.Contains("margin:0 0 14px", css, StringComparison.Ordinal);
        Assert.Contains("margin:0 0 16px", css, StringComparison.Ordinal);
        Assert.Contains("margin-top:0", css, StringComparison.Ordinal);
    }


    [Fact]
    public void Student_lesson_cards_strip_non_title_pathway_labels()
    {
        var view = ReadRepositoryFile("src", "Edulytics.Web", "Views", "StudentPortal", "Learning.cshtml");

        Assert.Contains("DisplayLessonTitle", view, StringComparison.Ordinal);
        Assert.Contains("Build the Idea|Reason and Apply", view, StringComparison.Ordinal);
        Assert.Contains("same", view, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("@DisplayLessonTitle(lesson.Title)", view, StringComparison.Ordinal);
        Assert.DoesNotContain("<h3>@lesson.Title</h3>", view, StringComparison.Ordinal);
    }


    [Fact]
    public void Student_lesson_title_cleanup_removes_leftover_same_fragment()
    {
        var view = ReadRepositoryFile("src", "Edulytics.Web", "Views", "StudentPortal", "Learning.cshtml");

        Assert.Contains(@"\s*\(\s*same\s*$", view, StringComparison.Ordinal);
        Assert.Contains(@"\s*\(\s*same\s*(?::\s*)?(Build the Idea|Reason and Apply)?\s*\)?\s*$", view, StringComparison.Ordinal);
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
