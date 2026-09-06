using Edulytics.Web.Presentation;

namespace Edulytics.Tests.Acceptance;

public sealed class CurriculumAiUxAcceptanceTests
{
    [Theory]
    [InlineData("CAM:OUT:0862:7Ni.01", "7Ni.01")]
    [InlineData("7Ni.01", "7Ni.01")]
    [InlineData("  CAM:OUT:0862:7Np.02  ", "7Np.02")]
    public void LearningOutcomePresentation_UsesCompactLocatorCode(
        string code,
        string expected)
    {
        Assert.Equal(
            expected,
            LearningOutcomePresentation.DisplayCode(code));
    }

    [Fact]
    public void OfficialCurriculumUi_PreservesSelectedContextWithoutManualTopicMutation()
    {
        var controller = ReadRepositoryFile(
            "src",
            "Edulytics.Web",
            "Controllers",
            "CurriculumController.cs");

        var view = ReadRepositoryFile(
            "src",
            "Edulytics.Web",
            "Views",
            "Curriculum",
            "Index.cshtml");

        Assert.Contains(
            "CurriculumContextRouteValues",
            controller,
            StringComparison.Ordinal);

        Assert.Contains(
            "name=\"academicYearId\"",
            view,
            StringComparison.Ordinal);

        Assert.Contains(
            "name=\"academicProgramId\"",
            view,
            StringComparison.Ordinal);

        Assert.Contains(
            "name=\"curriculumAdoptionId\"",
            view,
            StringComparison.Ordinal);

        Assert.DoesNotContain(
            "asp-action=\"CreateCurriculumTopic\"",
            view,
            StringComparison.Ordinal);

        Assert.DoesNotContain(
            "asp-action=\"CreateCurriculumOfficialOutcome\"",
            view,
            StringComparison.Ordinal);

        Assert.DoesNotContain(
            "asp-action=\"EditTopic\"",
            view,
            StringComparison.Ordinal);
    }

    [Fact]
    public void LearningOutcomeUi_ShowsCapabilityAndUsesReadableSelectors()
    {
        var curriculumView = ReadRepositoryFile(
            "src",
            "Edulytics.Web",
            "Views",
            "Curriculum",
            "Index.cshtml");

        var builderView = ReadRepositoryFile(
            "src",
            "Edulytics.Web",
            "Views",
            "AssessmentBuilder",
            "Index.cshtml");

        var builderCss = ReadRepositoryFile(
            "src",
            "Edulytics.Web",
            "wwwroot",
            "css",
            "assessment-builder.css");

        Assert.Contains(
            "NativeMathematicsOutcomeProfileResolver.Supports",
            curriculumView,
            StringComparison.Ordinal);

        Assert.Contains(
            "LearningOutcomePresentation.DisplayCode",
            curriculumView,
            StringComparison.Ordinal);

        Assert.Contains(
            "LearningOutcomePresentation.DisplayTitle",
            curriculumView,
            StringComparison.Ordinal);

        Assert.Contains(
            "AiSupported",
            curriculumView,
            StringComparison.Ordinal);

        Assert.Contains(
            "ManualOnly",
            curriculumView,
            StringComparison.Ordinal);

        Assert.Contains(
            "ed-builder-outcome-select",
            builderView,
            StringComparison.Ordinal);

        Assert.Contains(
            "LearningOutcomePresentation.DisplayCode",
            builderView,
            StringComparison.Ordinal);

        Assert.Contains(
            "LearningOutcomePresentation.DisplayTitle",
            builderView,
            StringComparison.Ordinal);

        Assert.Contains(
            "disabled=\"@(!aiSupported)\"",
            builderView,
            StringComparison.Ordinal);

        Assert.Contains(
            ".ed-builder-outcome-select",
            builderCss,
            StringComparison.Ordinal);
    }

    private static string ReadRepositoryFile(
        params string[] relativeSegments)
    {
        var root = FindRoot();

        return File.ReadAllText(
            Path.Combine(
                [root, .. relativeSegments]));
    }

    private static string FindRoot()
    {
        var directory =
            new DirectoryInfo(
                AppContext.BaseDirectory);

        while (
            directory is not null &&
            !File.Exists(
                Path.Combine(
                    directory.FullName,
                    "Edulytics.sln")))
        {
            directory = directory.Parent;
        }

        return directory?.FullName
            ?? throw new InvalidOperationException(
                "Repository root not found.");
    }
}
