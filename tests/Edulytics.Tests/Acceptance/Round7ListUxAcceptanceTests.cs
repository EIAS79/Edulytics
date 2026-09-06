namespace Edulytics.Tests.Acceptance;

public sealed class Round7ListUxAcceptanceTests
{
    [Fact]
    public void Student_assessments_are_newest_first_and_bounded()
    {
        var view = ReadRepositoryFile("src", "Edulytics.Web", "Views", "StudentPortal", "Assessments.cshtml");
        Assert.Contains("OrderByDescending(x => x.AssessmentDate)", view, StringComparison.Ordinal);
        Assert.Contains("student-card-list ed-scroll-list", view, StringComparison.Ordinal);
    }

    [Fact]
    public void Student_practice_history_is_newest_first_and_bounded()
    {
        var view = ReadRepositoryFile("src", "Edulytics.Web", "Views", "StudentPractice", "Index.cshtml");
        Assert.Contains("OrderByDescending(x => x.StartedAtUtc)", view, StringComparison.Ordinal);
        Assert.Contains("student-card-list ed-scroll-list", view, StringComparison.Ordinal);
    }

    [Fact]
    public void Student_results_are_newest_first_and_scrollable()
    {
        var view = ReadRepositoryFile("src", "Edulytics.Web", "Views", "StudentPortal", "Results.cshtml");
        Assert.Contains("OrderByDescending(x => x.AssessmentDate)", view, StringComparison.Ordinal);
        Assert.Contains("student-results-table-shell ed-scroll-list", view, StringComparison.Ordinal);
    }

    [Fact]
    public void Teacher_assessments_are_newest_first_and_bounded()
    {
        var view = ReadRepositoryFile("src", "Edulytics.Web", "Views", "Assessments", "Index.cshtml");
        Assert.Contains("OrderByDescending(x => x.AssessmentDate)", view, StringComparison.Ordinal);
        Assert.Contains("assessment-list-items ed-scroll-list", view, StringComparison.Ordinal);
    }

    [Fact]
    public void Assessment_builder_question_area_has_internal_scroll_without_reversing_question_order()
    {
        var css = ReadRepositoryFile("src", "Edulytics.Web", "wwwroot", "css", "assessment-builder.css");
        var view = ReadRepositoryFile("src", "Edulytics.Web", "Views", "AssessmentBuilder", "Index.cshtml");

        Assert.Contains("max-height: min(72vh, 820px)", css, StringComparison.Ordinal);
        Assert.Contains("overflow-y: auto", css, StringComparison.Ordinal);
        Assert.Contains("@foreach (var question in Model.Questions)", view, StringComparison.Ordinal);
        Assert.DoesNotContain("OrderByDescending(x => x.Order)", view, StringComparison.Ordinal);
    }

    private static string ReadRepositoryFile(params string[] relativeSegments)
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
            directory = directory.Parent;

        var root = directory?.FullName
            ?? throw new InvalidOperationException("Repository root not found.");
        var pathSegments = new[] { root }.Concat(relativeSegments).ToArray();
        return File.ReadAllText(Path.Combine(pathSegments));
    }
}
