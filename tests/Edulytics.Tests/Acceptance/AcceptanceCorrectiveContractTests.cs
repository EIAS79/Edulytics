namespace Edulytics.Tests.Acceptance;

public sealed class AcceptanceCorrectiveContractTests
{
    [Fact]
    public void IdempotencyReservation_UsesAtomicPostgresConflictHandling()
    {
        var source = ReadRepositoryFile(
            "src",
            "Edulytics.Data",
            "Repositories",
            "IdempotencyRepository.cs");

        Assert.Contains(
            "ON CONFLICT",
            source,
            StringComparison.Ordinal);

        Assert.Contains(
            "DO NOTHING",
            source,
            StringComparison.Ordinal);

        Assert.Contains(
            "ActorUserId",
            source,
            StringComparison.Ordinal);

        Assert.DoesNotContain(
            "catch (DbUpdateException)",
            source,
            StringComparison.Ordinal);
    }

    [Fact]
    public void ErrorPage_ReturnsToSameOriginHistoryWithWorkspaceFallback()
    {
        var view = ReadRepositoryFile(
            "src",
            "Edulytics.Web",
            "Views",
            "Shared",
            "Error.cshtml");

        var script = ReadRepositoryFile(
            "src",
            "Edulytics.Web",
            "wwwroot",
            "js",
            "acceptance-corrective.js");

        Assert.Contains(
            "data-ed-error-back",
            view,
            StringComparison.Ordinal);

        Assert.Contains(
            "/school/dashboard",
            view,
            StringComparison.Ordinal);

        Assert.Contains(
            "referrer.origin !== window.location.origin",
            script,
            StringComparison.Ordinal);

        Assert.Contains(
            "window.history.back()",
            script,
            StringComparison.Ordinal);
    }

    [Fact]
    public void SchoolUserCreate_BlocksDuplicateBrowserSubmissions()
    {
        var view = ReadRepositoryFile(
            "src",
            "Edulytics.Web",
            "Views",
            "SchoolUsers",
            "Create.cshtml");

        var script = ReadRepositoryFile(
            "src",
            "Edulytics.Web",
            "wwwroot",
            "js",
            "acceptance-corrective.js");

        Assert.Contains(
            "data-prevent-double-submit=\"true\"",
            view,
            StringComparison.Ordinal);

        Assert.Contains(
            "form.dataset.submitting",
            script,
            StringComparison.Ordinal);
    }

    [Fact]
    public void TeacherAssignments_PreserveMultiSelectAndShowFullClassLabels()
    {
        var script = ReadRepositoryFile(
            "src",
            "Edulytics.Web",
            "wwwroot",
            "js",
            "acceptance-corrective.js");

        var css = ReadRepositoryFile(
            "src",
            "Edulytics.Web",
            "wwwroot",
            "css",
            "acceptance-corrective.css");

        Assert.Contains(
            "select.name !== \"classGroupIds\"",
            script,
            StringComparison.Ordinal);

        Assert.Contains(
            "checkbox.name = \"classGroupIds\"",
            script,
            StringComparison.Ordinal);

        Assert.Contains(
            "option.textContent",
            script,
            StringComparison.Ordinal);

        Assert.Contains(
            "white-space: normal",
            css,
            StringComparison.Ordinal);

        Assert.Contains(
            "overflow-wrap: anywhere",
            css,
            StringComparison.Ordinal);
    }

    [Fact]
    public void AppLayout_LoadsCorrectiveAssetsAfterSiteAssets()
    {
        var layout = ReadRepositoryFile(
            "src",
            "Edulytics.Web",
            "Views",
            "Shared",
            "_AppLayout.cshtml");

        Assert.Contains(
            "~/css/acceptance-corrective.css",
            layout,
            StringComparison.Ordinal);

        Assert.Contains(
            "~/js/acceptance-corrective.js",
            layout,
            StringComparison.Ordinal);

        Assert.True(
            layout.IndexOf(
                "~/js/acceptance-corrective.js",
                StringComparison.Ordinal) >
            layout.IndexOf(
                "~/js/site.js",
                StringComparison.Ordinal));
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
