using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Data.Contexts;
using Edulytics.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Tests.Phase29;

public sealed class Phase29CurriculumExperienceRuntimeTests
{
    private static EdulyticsDbContext Db(string name) =>
        new(
            new DbContextOptionsBuilder<EdulyticsDbContext>()
                .UseInMemoryDatabase(name)
                .Options);

    [Fact]
    public async Task AcademicSnapshot_IncludesExplicitCurriculumAdoptions()
    {
        await using var db = Db($"phase29-runtime-snapshot-{Guid.NewGuid():N}");

        var schoolId = Guid.NewGuid();
        var adoptionId = Guid.NewGuid();

        db.SchoolCurriculumAdoptions.Add(new SchoolCurriculumAdoption
        {
            Id = adoptionId,
            SchoolId = schoolId,
            AcademicYearId = Guid.NewGuid(),
            AcademicProgramId = Guid.NewGuid(),
            GradeLevelId = Guid.NewGuid(),
            SubjectId = Guid.NewGuid(),
            FrameworkVersionId = Guid.NewGuid(),
            CurriculumLevelKey = "TEST:L01:SHARED",
            CurriculumLogicalLevel = 1,
            CurriculumLevelLabel = "Test Level",
            CurriculumStage = "Test Stage",
            IsPrimary = true,
            IsActive = true,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        });
        await db.SaveChangesAsync();

        var snapshot = await new AcademicStructureRepository(db)
            .GetSnapshotAsync(schoolId);

        var adoption = Assert.Single(snapshot.CurriculumAdoptions);
        Assert.Equal(adoptionId, adoption.Id);
        Assert.Equal(schoolId, adoption.SchoolId);
    }

    [Fact]
    public void UaeIdentityRegistry_MatchesVerifiedCurrentSourceTopology()
    {
        MathematicsCurriculumPackRegistry.Validate();

        var levels = CurriculumLevelIdentityRegistry.ForPack(
            MathematicsCurriculumPackRegistry.UaeCode);

        Assert.Equal(20, levels.Count);

        foreach (var logicalLevel in Enumerable.Range(1, 4))
        {
            var level = Assert.Single(
      levels,
      x => x.LogicalLevel == logicalLevel);
            Assert.Equal("Common", level.Pathway);
        }

        foreach (var logicalLevel in Enumerable.Range(5, 8))
        {
            var levelPathways = levels
                .Where(x => x.LogicalLevel == logicalLevel)
                .Select(x => x.Pathway)
                .OrderBy(x => x)
                .ToArray();

            Assert.Equal(new string?[] { "Advanced", "General" }, levelPathways);
        }
    }
}
