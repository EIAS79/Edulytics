using System.Text.Json;
using Edulytics.Web.Resilience;

namespace Edulytics.Tests.Phase14;

public sealed class SchoolUserProvisioningTimeoutContractTests
{
    [Fact]
    public void Interactive_write_timeout_covers_multi_step_user_provisioning()
    {
        var defaults = new BackendResilienceOptions();

        Assert.True(
            defaults.InteractiveWriteTimeoutSeconds >= 45,
            "Multi-step school-user provisioning must have enough request budget to commit the account and queue its initial invitation.");

        var root = FindRepositoryRoot();
        var appSettings = File.ReadAllText(
            Path.Combine(
                root,
                "src/Edulytics.Web/appsettings.json"));

        using var json = JsonDocument.Parse(appSettings);
        var configuredTimeout = json.RootElement
            .GetProperty("Edulytics")
            .GetProperty("Resilience")
            .GetProperty("InteractiveWriteTimeoutSeconds")
            .GetInt32();

        Assert.True(configuredTimeout >= 45);

        var controller = File.ReadAllText(
            Path.Combine(
                root,
                "src/Edulytics.Web/Controllers/SchoolUsersController.cs"));

        Assert.Contains(
            "[RequestTimeout(BackendResiliencePolicyNames.InteractiveWrite)]",
            controller,
            StringComparison.Ordinal);
    }

    private static string FindRepositoryRoot()
    {
        var directory =
            new DirectoryInfo(AppContext.BaseDirectory);

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

        throw new DirectoryNotFoundException(
            "Edulytics repository root not found.");
    }
}
