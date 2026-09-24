namespace Edulytics.Tests.Phase28;

public sealed class Phase28StudentDashboardUiContractTests
{
    [Fact]
    public void Dashboard_UsesProgressFirstLayoutAndSingleAwaitingSection()
    {
        var root = FindRoot();

        var dashboard = File.ReadAllText(
            Path.Combine(
                root,
                "src",
                "Edulytics.Web",
                "Views",
                "StudentPortal",
                "Dashboard.cshtml"));

        var css = File.ReadAllText(
            Path.Combine(
                root,
                "src",
                "Edulytics.Web",
                "wwwroot",
                "css",
                "round4-ux-display.css"));

        Assert.Contains(
            "student-dashboard-overview-grid",
            dashboard);

        Assert.Contains(
            "student-average-donut",
            dashboard);

        Assert.Contains(
            "student-dashboard-stat-grid",
            dashboard);

        Assert.Contains(
            "student-awaiting-panel",
            dashboard);

        Assert.Contains(
            "student-dashboard-content-grid",
            dashboard);

        Assert.Contains(
            "student-learning-panel-wide",
            dashboard);

        Assert.DoesNotContain(
            "student-summary-grid",
            dashboard);

        Assert.DoesNotContain(
            "student-dashboard-status-grid",
            dashboard);

        Assert.Equal(
            1,
            CountOccurrences(
                dashboard,
                "class=\"student-panel student-awaiting-panel\""));

        Assert.Contains(
            "conic-gradient(",
            css);

        Assert.Contains(
            ".student-dashboard-overview-grid",
            css);

        Assert.Contains(
            ".student-dashboard-content-grid",
            css);

        Assert.Contains(
            ".student-subject-tiles-dashboard",
            css);
    }

    private static int CountOccurrences(
        string value,
        string token)
    {
        var count = 0;
        var offset = 0;

        while (true)
        {
            var index = value.IndexOf(
                token,
                offset,
                StringComparison.Ordinal);

            if (index < 0)
                return count;

            count++;
            offset = index + token.Length;
        }
    }

    private static string FindRoot()
    {
        var directory =
            new DirectoryInfo(
                AppContext.BaseDirectory);

        while (directory is not null)
        {
            if (File.Exists(
                    Path.Combine(
                        directory.FullName,
                        "Edulytics.sln")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException();
    }
}
