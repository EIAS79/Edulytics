namespace Edulytics.Tests.Acceptance;

public sealed class StudentLessonPagePresentationContractTests
{
    [Fact]
    public void StudentLessonView_RemovesDuplicateHeadingDateAndOutcomeBoilerplate()
    {
        var view = Read("src", "Edulytics.Web", "Views", "StudentPortal", "Lesson.cshtml");
        var layout = Read("src", "Edulytics.Web", "Views", "Shared", "_StudentLayout.cshtml");

        Assert.DoesNotContain("PublishedAtUtc.ToString", view, StringComparison.Ordinal);
        Assert.DoesNotContain("outcome.Description", view, StringComparison.Ordinal);
        Assert.Contains("<strong>@outcome.Code</strong>", view, StringComparison.Ordinal);
        Assert.Contains("action == \"LessonBySlug\"", layout, StringComparison.Ordinal);
        Assert.Contains("action == \"Lesson\"", layout, StringComparison.Ordinal);
    }

    [Fact]
    public void StudentLessonRoutes_KeepGuidAccessAndYouTubeEndpoint()
    {
        var controller = Read("src", "Edulytics.Web", "Controllers", "StudentPortalController.cs");
        var learning = Read("src", "Edulytics.Web", "Views", "StudentPortal", "Learning.cshtml");

        Assert.Contains("[HttpGet(\"learning/lesson/{id:guid}\")]", controller, StringComparison.Ordinal);
        Assert.Contains("[HttpGet(\"learning/lesson/{slug}\")]", controller, StringComparison.Ordinal);
        Assert.Contains("[HttpGet(\"learning/lesson/{id:guid}/youtube\")]", controller, StringComparison.Ordinal);
        Assert.Contains("ListPublishedForStudentAsync", controller, StringComparison.Ordinal);
        Assert.Contains("return await Lesson(matches[0].Id, cancellationToken)", controller, StringComparison.Ordinal);
        Assert.Contains("asp-action=\"LessonBySlug\"", learning, StringComparison.Ordinal);
        Assert.Contains("asp-route-slug=\"@StudentLessonSlug.Create(lesson)\"", learning, StringComparison.Ordinal);
    }

    [Fact]
    public void ReviewedResources_AreRenamedInAllSupportedLanguages()
    {
        foreach (var path in new[]
        {
            Read("src", "Edulytics.Web", "Views", "Shared", "_LessonReviewedResources.cshtml"),
            Read("src", "Edulytics.Web", "Views", "Shared", "_RichLessonContentV2.cshtml")
        })
        {
            Assert.Contains("Additional Learning Resources", path, StringComparison.Ordinal);
            Assert.Contains("مصادر تعليمية إضافية", path, StringComparison.Ordinal);
            Assert.Contains("Dodatkowe materiały edukacyjne", path, StringComparison.Ordinal);
        }
    }

    private static string Read(params string[] parts)
    {
        var root = new DirectoryInfo(AppContext.BaseDirectory);
        while (root is not null && !File.Exists(Path.Combine(root.FullName, "Edulytics.sln")))
            root = root.Parent;

        if (root is null)
            throw new InvalidOperationException("Cannot locate the repository root.");

        return File.ReadAllText(Path.Combine([root.FullName, .. parts]));
    }
}
