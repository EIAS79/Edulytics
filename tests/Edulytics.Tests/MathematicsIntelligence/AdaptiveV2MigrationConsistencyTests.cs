using Edulytics.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class AdaptiveV2MigrationConsistencyTests
{
    [Fact]
    public void AdaptiveV2ModelMatchesMigrationSnapshot()
    {
        var options =
            new DbContextOptionsBuilder<EdulyticsDbContext>()
                .UseNpgsql(
                    "Host=127.0.0.1;Database=edulytics_snapshot_gate;Username=postgres;Password=postgres")
                .Options;

        using var context =
            new EdulyticsDbContext(options);

        var snapshotType =
            typeof(EdulyticsDbContext).Assembly.GetType(
                "Edulytics.Data.Migrations.EdulyticsDbContextModelSnapshot",
                throwOnError: true)!;

        var snapshot =
            (ModelSnapshot)Activator.CreateInstance(
                snapshotType,
                nonPublic: true)!;

        var designTimeModel =
            context.GetService<IDesignTimeModel>().Model;
        var differ =
            context.GetService<IMigrationsModelDiffer>();

        var hasDifferences = differ.HasDifferences(
            snapshot.Model.GetRelationalModel(),
            designTimeModel.GetRelationalModel());

        Assert.False(
            hasDifferences,
            "EF design-time model differs from EdulyticsDbContextModelSnapshot. " +
            "Generate or align the migration snapshot before deployment.");
    }
}
