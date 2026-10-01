using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Data.Contexts;
using Edulytics.Data.Repositories;
using Edulytics.Data.Seeding;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Tests.Phase44;

public sealed class VerifiedCurriculumReferenceTargetTests
{
    [Fact]
    public void RegistryValidationPasses() =>
        VerifiedCurriculumReferenceTargetRegistry.Validate();

    [Fact]
    public void Cambridge9709_ExposesAllThirtyEightReferencesAndEverySupportedRoute()
    {
        var aggregate =
            VerifiedCurriculumReferenceTargetRegistry.ForScope(
                MathematicsCurriculumPackRegistry.CambridgeCode,
                12,
                CurriculumPathwayCompatibility.CambridgeAdvancedAggregatePathway);

        Assert.Equal(38, aggregate.Count);
        Assert.All(
            aggregate,
            x =>
            {
                Assert.StartsWith("CAM:REF:9709:", x.Code, StringComparison.Ordinal);
                Assert.Equal(x.Code, x.OfficialReferenceCode);
                Assert.Equal("2026-2027", x.SourcePeriod);
            });

        var routes =
            VerifiedCurriculumReferenceTargetRegistry.Cambridge9709Routes;

        Assert.Equal(5, routes.Count);
        Assert.Equal(
            14,
            VerifiedCurriculumReferenceTargetRegistry.ForScope(
                MathematicsCurriculumPackRegistry.CambridgeCode,
                12,
                "AS-PURE").Count);
        Assert.Equal(
            13,
            VerifiedCurriculumReferenceTargetRegistry.ForScope(
                MathematicsCurriculumPackRegistry.CambridgeCode,
                12,
                "AS-MECHANICS").Count);
        Assert.Equal(
            13,
            VerifiedCurriculumReferenceTargetRegistry.ForScope(
                MathematicsCurriculumPackRegistry.CambridgeCode,
                12,
                "AS-STATISTICS").Count);
        Assert.Equal(
            27,
            VerifiedCurriculumReferenceTargetRegistry.ForScope(
                MathematicsCurriculumPackRegistry.CambridgeCode,
                13,
                "A-MECHANICS").Count);
        Assert.Equal(
            27,
            VerifiedCurriculumReferenceTargetRegistry.ForScope(
                MathematicsCurriculumPackRegistry.CambridgeCode,
                13,
                "A-STATISTICS").Count);
    }

    [Fact]
    public void Uae2025_2026_TargetsStayExplicitlyNonOfficial()
    {
        var scopes = new[]
        {
            (3, "Common"),
            (4, "Common"),
            (7, "Advanced"),
            (8, "Advanced"),
            (11, "Advanced"),
            (12, "Advanced")
        };

        foreach (var scope in scopes)
        {
            var targets =
                VerifiedCurriculumReferenceTargetRegistry.ForScope(
                    MathematicsCurriculumPackRegistry.UaeCode,
                    scope.Item1,
                    scope.Item2);

            Assert.True(targets.Count >= 5);
            Assert.All(
                targets,
                x =>
                {
                    Assert.StartsWith("UAE:REF:2025-26:", x.Code, StringComparison.Ordinal);
                    Assert.Equal("2025/2026", x.SourcePeriod);
                    Assert.Null(x.OfficialReferenceCode);
                });
        }
    }

    [Fact]
    public async Task UaeGrade3_MaterializesReferenceTargetsWithoutClaimingOfficialOutcomeNodes()
    {
        await using var db =
            CreateDb("uae-reference-" + Guid.NewGuid().ToString("N"));

        await new MathematicsCurriculumPackSeeder(db).SeedAsync();

        var adoption =
            await AddAdoptionAsync(
                db,
                MathematicsCurriculumPackRegistry.UaeCode,
                3,
                "Common");

        await OfficialCurriculumOutcomeMaterializer.EnsureAsync(db, adoption);
        await OfficialCurriculumOutcomeMaterializer.EnsureAsync(db, adoption);

        var outcomes = await db.LearningOutcomes
            .AsNoTracking()
            .Where(x => x.CurriculumAdoptionId == adoption.Id)
            .OrderBy(x => x.Code)
            .ToArrayAsync();

        Assert.Equal(5, outcomes.Length);
        Assert.All(
            outcomes,
            x =>
            {
                Assert.StartsWith("UAE:REF:2025-26:", x.Code, StringComparison.Ordinal);
                Assert.Null(x.OfficialContentNodeId);
                Assert.False(string.IsNullOrWhiteSpace(x.Description));
            });
    }

    [Fact]
    public async Task CambridgeAsLevel_MaterializesExistingOfficialReferencesWithoutReclassifyingThem()
    {
        await using var db =
            CreateDb("cambridge-reference-" + Guid.NewGuid().ToString("N"));

        await new MathematicsCurriculumPackSeeder(db).SeedAsync();

        var adoption =
            await AddAdoptionAsync(
                db,
                MathematicsCurriculumPackRegistry.CambridgeCode,
                12,
                CurriculumPathwayCompatibility.CambridgeAdvancedAggregatePathway);

        await OfficialCurriculumOutcomeMaterializer.EnsureAsync(db, adoption);

        var outcomes = await db.LearningOutcomes
            .AsNoTracking()
            .Where(x => x.CurriculumAdoptionId == adoption.Id)
            .ToArrayAsync();

        Assert.Equal(38, outcomes.Length);
        Assert.All(outcomes, x => Assert.NotNull(x.OfficialContentNodeId));

        var nodeIds = outcomes
            .Select(x => x.OfficialContentNodeId!.Value)
            .ToArray();

        var nodes = await db.CurriculumPackContentNodes
            .AsNoTracking()
            .Where(x => nodeIds.Contains(x.Id))
            .ToArrayAsync();

        Assert.Equal(38, nodes.Length);
        Assert.All(nodes, x => Assert.Equal("Reference", x.NodeKind));
        Assert.All(nodes, x => Assert.True(x.IsOfficial));
    }

    private static async Task<SchoolCurriculumAdoption> AddAdoptionAsync(
        EdulyticsDbContext db,
        string packCode,
        int logicalLevel,
        string pathway)
    {
        var state = await db.CurriculumPackImportStates
            .AsNoTracking()
            .SingleAsync(x => x.FrameworkCode == packCode);

        var adoption = new SchoolCurriculumAdoption
        {
            Id = Guid.NewGuid(),
            SchoolId = Guid.NewGuid(),
            AcademicProgramId = Guid.NewGuid(),
            GradeLevelId = Guid.NewGuid(),
            SubjectId = Guid.NewGuid(),
            FrameworkVersionId = state.FrameworkVersionId,
            CurriculumLevelKey =
                CurriculumLevelIdentityRegistry.BuildKey(
                    packCode,
                    logicalLevel,
                    pathway),
            CurriculumLogicalLevel = logicalLevel,
            CurriculumLevelLabel = $"Level {logicalLevel}",
            CurriculumStage = "test",
            CurriculumPathway = pathway,
            IsPrimary = true,
            IsActive = true,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
            RowVersion = []
        };

        db.SchoolCurriculumAdoptions.Add(adoption);
        await db.SaveChangesAsync();
        return adoption;
    }

    private static EdulyticsDbContext CreateDb(string name) =>
        new(
            new DbContextOptionsBuilder<EdulyticsDbContext>()
                .UseInMemoryDatabase(name)
                .Options);
}
