namespace Edulytics.Tests.Acceptance;

public sealed class CurriculumSpecializedPracticeContractTests
{
    [Fact]
    public void StudentPractice_PreservesDedicatedSolidGeometryAndSpecializedTwoDimensionalGeometry()
    {
        var root = FindRoot();
        var view = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/StudentPractice/Game.cshtml"));
        var runtime = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/js/curriculum-specialized-practice-v3.js"));

        Assert.Contains("VOLUME_BUILD", view, StringComparison.Ordinal);
        Assert.Contains("SURFACE_AREA_BUILD", view, StringComparison.Ordinal);
        Assert.Contains("solid-geometry-runtime.js", view, StringComparison.Ordinal);

        Assert.Contains("PERIMETER_TRACE", view, StringComparison.Ordinal);
        Assert.Contains("AREA_TILE", view, StringComparison.Ordinal);
        Assert.Contains("curriculum-specialized-practice-v3.js", view, StringComparison.Ordinal);

        Assert.Contains("function renderPerimeter()", runtime, StringComparison.Ordinal);
        Assert.Contains("2 * (width + height)", runtime, StringComparison.Ordinal);
        Assert.Contains("function renderArea()", runtime, StringComparison.Ordinal);
        Assert.Contains("columns * rows", runtime, StringComparison.Ordinal);
    }

    [Fact]
    public void MoneyPractice_CanRecoverAfterOvershootingTheTarget()
    {
        var root = FindRoot();
        var view = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/StudentPractice/Game.cshtml"));
        var runtime = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/js/curriculum-specialized-practice-v3.js"));

        Assert.Contains("MONEY_VALUE", view, StringComparison.Ordinal);
        Assert.Contains("data-money-reset", runtime, StringComparison.Ordinal);
        Assert.Contains("total = 0;", runtime, StringComparison.Ordinal);
        Assert.Contains("output.textContent = '0';", runtime, StringComparison.Ordinal);
        Assert.Contains(
            "Practice is feedback, not an official grade.",
            runtime,
            StringComparison.Ordinal);
    }

    private static string FindRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
                return directory.FullName;

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Edulytics solution root not found.");
    }
}
