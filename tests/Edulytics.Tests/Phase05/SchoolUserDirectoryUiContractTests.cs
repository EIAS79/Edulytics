namespace Edulytics.Tests.Phase05;

public sealed class SchoolUserDirectoryUiContractTests
{
    [Fact]
    public void Directory_UsesUnifiedSearchAndCompactDependentFilters()
    {
        var root = FindRepositoryRoot();
        var view = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/SchoolUsers/Index.cshtml"));
        var css = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/css/site.css"));

        Assert.Contains("class=\"user-directory-filter-card\"", view);
        Assert.Contains("name=\"search\"", view);
        Assert.Contains("data-user-filter-year", view);
        Assert.Contains("data-user-filter-program", view);
        Assert.Contains("data-user-filter-class", view);
        Assert.Contains("syncAcademicFilters", view);

        Assert.DoesNotContain("name=\"name\"", view);
        Assert.DoesNotContain("name=\"userId\"", view);
        Assert.DoesNotContain("name=\"email\"", view);
        Assert.DoesNotContain("user-filter-group-identity", view);
        Assert.DoesNotContain("user-filter-group-academic", view);
        Assert.DoesNotContain("user-filter-group-account", view);

        Assert.Contains(
            "Manage Users V3 — unified search + compact dependent filters.",
            css);
        Assert.Contains(".user-directory-filter-grid", css);
        Assert.Contains(".user-directory-search-strip", css);
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
                return directory.FullName;

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException();
    }
}
