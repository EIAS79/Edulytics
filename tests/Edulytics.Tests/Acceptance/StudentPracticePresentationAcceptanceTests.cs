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
            ">@unit.UnitTitle</option>",
            view,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            ">@unit.UnitKey</option>",
            view,
            StringComparison.Ordinal);
    }

    [Fact]
    public void Workspace_ExposesHumanReadableOptionsAndExplicitOfficialMappings()
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
            "OfficialOutcomeNodeIds",
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
        Assert.Contains(
            "context.LessonOutcomes",
            service,
            StringComparison.Ordinal);
        Assert.Contains(
            "group.SelectMany(x => x.OfficialOutcomeNodeIds)",
            service,
            StringComparison.Ordinal);
    }


    [Fact]
    public void Practice_PresentsDeterministicMathVisualsFromExactGenerationMetadata()
    {
        var contracts = ReadRepositoryFile(
            "src",
            "Edulytics.Services",
            "Practice",
            "PracticeContracts.cs");
        var service = ReadRepositoryFile(
            "src",
            "Edulytics.Services",
            "Practice",
            "PracticeService.cs");
        var view = ReadRepositoryFile(
            "src",
            "Edulytics.Web",
            "Views",
            "StudentPractice",
            "Attempt.cshtml");
        var renderer = ReadRepositoryFile(
            "src",
            "Edulytics.Web",
            "Presentation",
            "PracticeMathVisualRenderer.cs");

        Assert.Contains("GenerationFamily", contracts, StringComparison.Ordinal);
        Assert.Contains("GenerationParametersJson", contracts, StringComparison.Ordinal);
        Assert.Contains("item.GenerationFamily", service, StringComparison.Ordinal);
        Assert.Contains("item.GenerationParametersJson", service, StringComparison.Ordinal);
        Assert.Contains("PracticeMathVisualRenderer.RenderSvg", view, StringComparison.Ordinal);
        Assert.Contains("practice-math-visual-wrap", view, StringComparison.Ordinal);
        Assert.Contains("geometry.right_triangle.pythagorean.exact", renderer, StringComparison.Ordinal);
        Assert.Contains("trigonometry.right_triangle.find_side_exact", renderer, StringComparison.Ordinal);
        Assert.Contains("geometry.coordinate.gradient_between_points", renderer, StringComparison.Ordinal);
        Assert.Contains("vectors.magnitude.exact", renderer, StringComparison.Ordinal);
        Assert.DoesNotContain("http://", renderer, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("https://", renderer, StringComparison.OrdinalIgnoreCase);
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
