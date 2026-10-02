namespace Edulytics.Tests.Acceptance;

public sealed class StudentLearningBackgroundContractTests
{
    [Fact]
    public void My_learning_page_uses_full_depth_background_without_affecting_other_student_pages()
    {
        var layout = ReadRepositoryFile("src", "Edulytics.Web", "Views", "Shared", "_StudentLayout.cshtml");
        var css = ReadRepositoryFile("src", "Edulytics.Web", "wwwroot", "css", "student-floating-learning.css");

        Assert.Contains("controller == \"StudentPortal\" && action == \"Learning\" ? \"student-learning-page\"", layout, StringComparison.Ordinal);
        Assert.Contains(".student-learning-page .student-main", css, StringComparison.Ordinal);
        Assert.Contains(".student-learning-page .student-page-frame", css, StringComparison.Ordinal);
        Assert.Contains(".student-learning-page .ed-v3-footer", css, StringComparison.Ordinal);
        Assert.Contains("background:#04162f", css.Replace(" ", string.Empty), StringComparison.Ordinal);
    }

    [Fact]
    public void My_learning_scene_contains_math_specific_atmosphere_and_2_5d_cards()
    {
        var view = ReadRepositoryFile("src", "Edulytics.Web", "Views", "StudentPortal", "Learning.cshtml");
        var css = ReadRepositoryFile("src", "Edulytics.Web", "wwwroot", "css", "student-floating-learning.css");

        Assert.Contains("learning-math-atmosphere", view, StringComparison.Ordinal);
        Assert.Contains("a² + b² = c²", view, StringComparison.Ordinal);
        Assert.Contains("⅓ + ⅔ = 1", view, StringComparison.Ordinal);
        Assert.Contains("math-wireframe--cube", view, StringComparison.Ordinal);
        Assert.Contains("math-wireframe--graph", view, StringComparison.Ordinal);
        Assert.Contains("perspective:1400px", css.Replace(" ", string.Empty), StringComparison.Ordinal);
        Assert.Contains("transform-style:preserve-3d", css.Replace(" ", string.Empty), StringComparison.Ordinal);
        Assert.Contains("translateZ(38px)", css.Replace(" ", string.Empty), StringComparison.Ordinal);
        Assert.Contains("@keyframes math-formula-float", css, StringComparison.Ordinal);
        Assert.Contains("@keyframes math-globe-drift", css, StringComparison.Ordinal);
    }

    [Fact]
    public void Whole_lesson_card_is_clickable_and_hover_keeps_idle_motion_stable()
    {
        var view = ReadRepositoryFile("src", "Edulytics.Web", "Views", "StudentPortal", "Learning.cshtml");
        var css = ReadRepositoryFile("src", "Edulytics.Web", "wwwroot", "css", "student-floating-learning.css");

        Assert.Contains("class=\"floating-lesson-card-link\"", view, StringComparison.Ordinal);
        Assert.Contains("asp-action=\"Lesson\"", view, StringComparison.Ordinal);
        Assert.Contains("asp-route-id=\"@lesson.Id\"", view, StringComparison.Ordinal);
        Assert.Contains("<span class=\"floating-open-lesson\">@L[\"OpenLesson\"]</span>", view, StringComparison.Ordinal);
        Assert.Contains("animation-play-state:running!important", css.Replace(" ", string.Empty), StringComparison.Ordinal);
        Assert.Contains(".floating-lesson-card-link", css, StringComparison.Ordinal);
        Assert.Contains("background:transparent!important", css.Replace(" ", string.Empty), StringComparison.Ordinal);
        Assert.Contains("learning-math-atmosphere::after", css, StringComparison.Ordinal);
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