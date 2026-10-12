namespace Edulytics.Tests.Phase25;

public sealed class Phase25ScaleFoundationTests
{
    [Fact]
    public void DataProtection_UsesSharedDatabasePersistence()
    {
        var source =
            ReadSource(
                "src/Edulytics.Web/Extensions/"
                + "ServiceCollectionExtensions.cs");

        Assert.Contains(
            "PersistKeysToDbContext",
            source,
            StringComparison.Ordinal);

        Assert.Contains(
            "SetApplicationName",
            source,
            StringComparison.Ordinal);

        Assert.Contains(
            "ProtectKeysWithCertificate",
            source,
            StringComparison.Ordinal);
    }

    [Fact]
    public void Phase25Discovery_DocumentsScaleLimitationsWithoutAnObsoletePlan()
    {
        var evidence = ReadSource("docs/PHASE_25_SCALE_DISCOVERY.md");
        Assert.Contains("process-independent", evidence, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("distributed sensitive quotas", evidence, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("2-web / 2-worker staging topology", evidence, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("not automatically global security", evidence, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Discovery_DoesNotClaimPhase26PerformanceAcceptance()
    {
        var evidence = ReadSource("docs/PHASE_25_SCALE_DISCOVERY.md");
        Assert.Contains("still needs explicit qualification", evidence, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Known Phase 25 boundaries", evidence, StringComparison.OrdinalIgnoreCase);
        // Historical discovery is evidence, never a live performance sign-off.
        Assert.DoesNotContain("Phase 26 accepted", evidence, StringComparison.OrdinalIgnoreCase);
    }

    private static string ReadSource(
        string relativePath) =>
        File.ReadAllText(
            Path.Combine(
                FindRepositoryRoot(),
                relativePath));

    private static string FindRepositoryRoot()
    {
        var current =
            new DirectoryInfo(
                AppContext.BaseDirectory);

        while (current is not null)
        {
            if (File.Exists(
                    Path.Combine(
                        current.FullName,
                        "Edulytics.sln")))
            {
                return current.FullName;
            }

            current = current.Parent;
        }

        throw new DirectoryNotFoundException(
            "Repository root not found.");
    }
}
