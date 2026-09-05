namespace Edulytics.Tests.Acceptance;

public sealed class StudentPracticePresentationAcceptanceTests
{
    [Fact]
    public void PrivatePractice_DoesNotRenderInternalLessonOrUnitIdentifiers()
    {
        var view = ReadRepositoryFile(
            "src",
            "Edulytics.Web",
            "Views",
            "StudentPractice",
            "Index.cshtml");

        Assert.Contains(
            "@lesson.UnitTitle · @lesson.LessonTitle",
            view,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "@lesson.UnitTitle · @lesson.LessonCode · @lesson.LessonTitle",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "<option value=\"@unit.UnitKey\">@unit.UnitTitle</option>",
            view,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "<option value=\"@unit\">@unit</option>",
            view,
            StringComparison.Ordinal);
    }

    [Fact]
    public void Workspace_ExposesHumanReadableUnitOptionsWithoutChangingPostedKey()
    {
        var contracts = ReadRepositoryFile(
            "src",
            "Edulytics.Services",
            "Practice",
            "StudentPrivatePracticeContracts.cs");
        var service = ReadRepositoryFile(
            "src",
            "Edulytics.Services",
            "Practice",
            "StudentPrivatePracticeService.cs");

        Assert.Contains(
            "StudentPrivatePracticeUnitOption",
            contracts,
            StringComparison.Ordinal);
        Assert.Contains(
            "first.UnitTitle",
            service,
            StringComparison.Ordinal);
        Assert.Contains(
            "unitOptions.Select(x => x.UnitKey)",
            service,
            StringComparison.Ordinal);
    }

    private static string ReadRepositoryFile(params string[] relativeSegments)
    {
        var root = FindRoot();
        return File.ReadAllText(Path.Combine([root, .. relativeSegments]));
    }

    private static string FindRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null &&
               !File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
        {
            directory = directory.Parent;
        }

        return directory?.FullName
            ?? throw new InvalidOperationException("Repository root not found.");
    }
}
