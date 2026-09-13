using System.Text;

namespace Edulytics.Tests.Acceptance;

public sealed class BulkImportAndPracticeHistoryContractTests
{
    [Fact]
    public void Student_practice_history_exposes_search_status_dates_and_pagination()
    {
        var source = Read("src/Edulytics.Web/Views/StudentPractice/Index.cshtml");

        Assert.Contains("practice-history-search", source, StringComparison.Ordinal);
        Assert.Contains("practice-history-status", source, StringComparison.Ordinal);
        Assert.Contains("practice-history-from", source, StringComparison.Ordinal);
        Assert.Contains("practice-history-to", source, StringComparison.Ordinal);
        Assert.Contains("practice-history-page-size", source, StringComparison.Ordinal);
        Assert.Contains("practice-history-prev", source, StringComparison.Ordinal);
        Assert.Contains("practice-history-next", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Product_import_templates_use_visible_class_names_and_bulk_account_types()
    {
        var adapter = Read("src/Edulytics.Web/Imports/MathOnlyImportAdapter.cs");
        var service = Read("src/Edulytics.Services/Imports/ProductDataImportService.cs");

        Assert.Contains("\"ClassName\"", adapter, StringComparison.Ordinal);
        Assert.Contains("ImportType.SubjectSupervisors", adapter, StringComparison.Ordinal);
        Assert.Contains("RoleNames.Teacher", service, StringComparison.Ordinal);
        Assert.Contains("RoleNames.SubjectSupervisor", service, StringComparison.Ordinal);
        Assert.Contains("PasswordSetupToken", service, StringComparison.Ordinal);
        Assert.Contains("Validation never creates accounts", service, StringComparison.Ordinal);
    }

    [Fact]
    public void Import_batch_supports_state_messages_filters_row_removal_and_resend()
    {
        var view = Read("src/Edulytics.Web/Views/Imports/Details.cshtml");
        var actions = Read("src/Edulytics.Web/Controllers/ImportBatchActionsController.cs");
        var editing = Read("src/Edulytics.Services/Imports/ImportBatchEditingService.cs");

        Assert.Contains("Validation successful. Review the data below, then confirm the import.", view, StringComparison.Ordinal);
        Assert.Contains("Delete selected", view, StringComparison.Ordinal);
        Assert.Contains("data-import-row-filter", view, StringComparison.Ordinal);
        Assert.Contains("Invitation sent", view, StringComparison.Ordinal);
        Assert.Contains("Invitation failed", view, StringComparison.Ordinal);
        Assert.Contains("resend-invitation", view, StringComparison.Ordinal);
        Assert.Contains("RemoveRows", actions, StringComparison.Ordinal);
        Assert.Contains("GeneratePasswordSetupAsync", actions, StringComparison.Ordinal);
        Assert.Contains("RecordInvitationOutcomesAsync", editing, StringComparison.Ordinal);
    }

    [Fact]
    public void Public_home_uses_requested_character_assets_and_v44_bundle()
    {
        var hero = Read("src/Edulytics.Web/wwwroot/js/public-home-cartoon-cleanup.js");
        var ai = Read("src/Edulytics.Web/wwwroot/js/public-home-ai-spotlight-v22.js");
        var layout = Read("src/Edulytics.Web/Views/Shared/_PublicLayout.cshtml");
        var mascotCss = Read("src/Edulytics.Web/wwwroot/css/public-home-mascot-transparency-v35.css");

        Assert.Contains("/images/public/edulaytiks-character.png?v=43", hero, StringComparison.Ordinal);
        Assert.Contains("/images/public/edulaytiks-character-background.png?v=43", ai, StringComparison.Ordinal);
        Assert.Contains("public-site-v44.css", layout, StringComparison.Ordinal);
        Assert.Contains("public-site-v44.js", layout, StringComparison.Ordinal);
        Assert.Contains("overflow:clip!important", mascotCss, StringComparison.Ordinal);
        Assert.Contains("max-width:100%", mascotCss, StringComparison.Ordinal);
    }

    private static string Read(string relativePath)
    {
        var root = FindRepositoryRoot();
        return File.ReadAllText(Path.Combine(root, relativePath), Encoding.UTF8);
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

        throw new DirectoryNotFoundException("Could not locate repository root.");
    }
}
