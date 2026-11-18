using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
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
    public async Task NormalGlobalMaintenancePreservesHistoricAdvancedLessonAndCanonicalContent()
    {
        await using var db = new EdulyticsDbContext(
            new DbContextOptionsBuilder<EdulyticsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options);
        await new MathematicsCurriculumPackSeeder(db).SeedAsync();
        var seeder = new MathematicsPedagogicalLessonSeeder(db);
        await seeder.SeedAsync();

        var general = await db.CurriculumPedagogicalLessons.SingleAsync(
            x => x.Code == "PED:UAE-MOE-MATH:L8:GENERAL:05:01:LINES");
        var historic = new CurriculumPedagogicalLesson
        {
            Id = Guid.NewGuid(),
            FrameworkVersionId = general.FrameworkVersionId,
            Code = "PED:UAE-MOE-MATH:L8:ADVANCED:01:06:RATES-AND-UNIT-RATES",
            Title = "Historic Grade 8 Advanced content",
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };
        var historicContent = new CurriculumLessonContent
        {
            Id = Guid.NewGuid(),
            FrameworkVersionId = historic.FrameworkVersionId,
            PedagogicalLessonId = historic.Id,
            ContentVersion = "historic-advanced-v1",
            Status = CanonicalLessonContentStatus.Published,
            PublishedAtUtc = DateTime.UtcNow,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };
        db.CurriculumPedagogicalLessons.Add(historic);
        db.CurriculumLessonContents.Add(historicContent);
        await db.SaveChangesAsync();

        await seeder.SeedAsync();
        await seeder.SeedAsync();

        var after = await db.CurriculumPedagogicalLessons.SingleAsync(x => x.Id == historic.Id);
        Assert.Equal(historic.Code, after.Code);
        var content = await db.CurriculumLessonContents.SingleAsync(x =>
            x.PedagogicalLessonId == historic.Id);
        Assert.Equal(historicContent.Id, content.Id);
        Assert.Equal("historic-advanced-v1", content.ContentVersion);
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
