namespace Edulytics.Tests.Phase15;

public sealed class TestingHostIsolationTests
{
    [Fact]
    public void TestingHost_DoesNotRegisterPostgreSqlOnlyWorkers()
    {
        var root =
            FindRoot();

        var program =
            File.ReadAllText(
                Path.Combine(
                    root,
                    "src",
                    "Edulytics.Web",
                    "Program.cs"));

        var realtime =
            File.ReadAllText(
                Path.Combine(
                    root,
                    "src",
                    "Edulytics.Web",
                    "Extensions",
                    "RealtimeRegistrationExtensions.cs"));

        Assert.Contains(
            "AddRealtimeDashboardsPhase10",
            program);

        Assert.Contains(
            "builder.Environment",
            program);

        Assert.Contains(
            "IHostEnvironment environment",
            realtime);

        Assert.Contains(
            "IsEnvironment(",
            realtime);

        Assert.Contains(
            "\"Testing\"",
            realtime);

        var normalizedRealtime =
            realtime.Replace("\r\n", "\n");

        var testingGate =
            normalizedRealtime.IndexOf(
                "if (!environment.IsEnvironment(",
                StringComparison.Ordinal);

        var outboxRegistration =
            normalizedRealtime.IndexOf(
                "AddHostedService<"
                + "\n"
                + "                "
                + "OutboxProcessorBackgroundService",
                StringComparison.Ordinal);

        var analyticsRegistration =
            normalizedRealtime.IndexOf(
                "AddHostedService<"
                + "\n"
                + "                "
                + "AnalyticsRefreshBackgroundService",
                StringComparison.Ordinal);

        Assert.True(
            testingGate >= 0);

        Assert.True(
            outboxRegistration >
            testingGate);

        Assert.True(
            analyticsRegistration >
            testingGate);
    }

    [Fact]
    public void TestingReadiness_DoesNotRequireDisabledOutboxWorker()
    {
        var root =
            FindRoot();

        var text =
            File.ReadAllText(
                Path.Combine(
                    root,
                    "src",
                    "Edulytics.Web",
                    "Extensions",
                    "ProductionHardeningRegistrationExtensions.cs"));

        var testingGate =
            text.IndexOf(
                "if (!environment.IsEnvironment(",
                StringComparison.Ordinal);

        var workerHealth =
            text.IndexOf(
                "OutboxWorkerReadinessHealthCheck",
                StringComparison.Ordinal);

        Assert.True(
            testingGate >= 0);

        Assert.True(
            workerHealth >
            testingGate);
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

            directory =
                directory.Parent;
        }

        throw new DirectoryNotFoundException(
            "Edulytics repository root not found.");
    }
}
