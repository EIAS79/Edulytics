namespace Edulytics.Tests.Acceptance;

public sealed class FloatingLearningVisualContractTests
{
    [Fact]
    public void Student_learning_uses_contextual_holographic_visuals_without_changing_lesson_route()
    {
        var view = ReadRepositoryFile("src", "Edulytics.Web", "Views", "StudentPortal", "Learning.cshtml");

        Assert.Contains("LessonVisualKind", view, StringComparison.Ordinal);
        Assert.Contains(@"data-visual=""@visualKind""", view, StringComparison.Ordinal);
        Assert.Contains(@"case ""add-subtract""", view, StringComparison.Ordinal);
        Assert.Contains(@"case ""fraction""", view, StringComparison.Ordinal);
        Assert.Contains(@"case ""angle""", view, StringComparison.Ordinal);
        Assert.Contains(@"case ""area""", view, StringComparison.Ordinal);
        Assert.Contains(@"case ""coordinates""", view, StringComparison.Ordinal);
        Assert.Contains(@"case ""shape-net""", view, StringComparison.Ordinal);
        Assert.Contains(@"case ""probability""", view, StringComparison.Ordinal);
        Assert.Contains(@"case ""data""", view, StringComparison.Ordinal);
        Assert.Contains(@"asp-action=""Lesson""", view, StringComparison.Ordinal);
        Assert.Contains(@"asp-route-id=""@lesson.Id""", view, StringComparison.Ordinal);
    }

    [Fact]
    public void Floating_motion_runs_independently_of_pointer_and_respects_reduced_motion()
    {
        var css = ReadRepositoryFile("src", "Edulytics.Web", "wwwroot", "css", "student-floating-learning.css");
        var js = ReadRepositoryFile("src", "Edulytics.Web", "wwwroot", "js", "student-floating-learning.js");

        Assert.Contains("animation:lesson-float", css, StringComparison.Ordinal);
        Assert.Contains("@keyframes lesson-float", css, StringComparison.Ordinal);
        Assert.Contains("infinite alternate", css, StringComparison.Ordinal);
        Assert.Contains("@media(prefers-reduced-motion:reduce)", css, StringComparison.Ordinal);
        Assert.Contains("prefers-reduced-motion: reduce", js, StringComparison.Ordinal);
        Assert.Contains("pointermove", js, StringComparison.Ordinal);
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