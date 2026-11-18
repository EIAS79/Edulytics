using Edulytics.Core.Curriculum;
using Edulytics.Data.Contexts;
using Edulytics.Data.Seeding;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UaeGrade8AdvancedRetirementTests
{
    [Fact]
    public async Task CleanSeedCreatesZeroAdvancedPedagogicalLessonsAndKeepsAll76General()
    {
        await using var db = new EdulyticsDbContext(
            new DbContextOptionsBuilder<EdulyticsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options);
        await new MathematicsCurriculumPackSeeder(db).SeedAsync();
        var seeder = new MathematicsPedagogicalLessonSeeder(db);
        await seeder.SeedAsync();
        await seeder.SeedAsync();

        Assert.Equal(0, await db.CurriculumPedagogicalLessons.CountAsync(x =>
            x.Code.StartsWith("PED:UAE-MOE-MATH:L8:ADVANCED:")));
        Assert.Equal(76, await db.CurriculumPedagogicalLessons.CountAsync(x =>
            x.Code.StartsWith("PED:UAE-MOE-MATH:L8:GENERAL:")));
    }

    [Fact]
    public void Grade8AdvancedIsNotAnOfferedOrSeedableCourseWhileGeneralArabicRemains()
    {
        const string advancedPrefix = "PED:UAE-MOE-MATH:L8:ADVANCED:";
        const string generalPrefix = "PED:UAE-MOE-MATH:L8:GENERAL:";

        var uae = Assert.Single(MathematicsCurriculumPackRegistry.All,
            p => p.Code == MathematicsCurriculumPackRegistry.UaeCode);
        Assert.DoesNotContain(uae.Levels,
            level => level.LogicalLevel == 8 &&
                     string.Equals(level.Pathway, "Advanced", StringComparison.Ordinal));
        Assert.Contains(uae.Levels,
            level => level.LogicalLevel == 8 &&
                     string.Equals(level.Pathway, "General", StringComparison.Ordinal));

        var blueprints = PedagogicalLessonBlueprintRegistry.LoadEmbeddedDocuments();
        Assert.DoesNotContain(blueprints,
            d => d.PackCode == MathematicsCurriculumPackRegistry.UaeCode &&
                 d.LogicalLevel == 8 &&
                 string.Equals(d.Pathway, "Advanced", StringComparison.Ordinal));

        var contents = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Where(d => d.PackCode == MathematicsCurriculumPackRegistry.UaeCode)
            .SelectMany(d => d.Lessons)
            .ToArray();
        Assert.DoesNotContain(contents,
            lesson => lesson.LessonCode.StartsWith(advancedPrefix, StringComparison.Ordinal));
        Assert.Equal(76, contents.Count(
            lesson => lesson.LessonCode.StartsWith(generalPrefix, StringComparison.Ordinal)));
    }
}
