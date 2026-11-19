using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Data.Contexts;
using Edulytics.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Edulytics.Tests.Phase29;

public sealed class CleanNeonEmbeddedJsonReadTests
{
    [Fact]
    public void CanonicalIndex_ContainsAuditedUniqueJsonLessonIdentities()
    {
        var index = new EmbeddedCanonicalLessonContentIndex();
        // Grade4 owner 93->132, plus owner-approved Grade6 General 59->128.
        // Original G7 General English 62 retired; 78 first-party Arabic lessons replace them.
        // Owner-retired UAE Grade 9: 96 Advanced + 82 General + 41
        // official Advanced + 1 pilot = 220 unique JSON lesson identities.
        Assert.Equal(5183 - 93 + 132 - 59 + 128 - 62 + 78 - 77 + 76 - 77 - 220, index.Count);
        Assert.True(index.TryGet("PED:UAE-MOE-MATH:L7:GENERAL:05:02:ARITHMETIC-SEQUENCES", out var grade7));
        Assert.Equal(CanonicalLessonContentStatus.Published, grade7.Status);
        Assert.Contains(grade7.Translations, t => t.CultureCode == "ar");
        Assert.False(index.TryGet("PED:UAE-MOE-MATH:L7:GENERAL:01:01:UNIT-RATES-INVOLVING-RATIOS-OF-FRACTIONS", out _));
        Assert.True(index.TryGet("PED:UAE-MOE-MATH:L6:GENERAL:01:01:FACTORS-AND-MULTIPLES", out var grade6));
        Assert.Equal(CanonicalLessonContentStatus.Published, grade6.Status);
        Assert.Contains(grade6.Translations, t => t.CultureCode == "ar");
        Assert.False(index.TryGet("PED:UAE-MOE-MATH:L6:GENERAL:05:05:FACTORS-AND-MULTIPLES", out _));
        Assert.False(index.TryGet("PED:UAE:G9:ADV:T1:L1-2", out _));
        Assert.False(index.TryGet("PED:UAE-MOE-MATH:L9:GENERAL:01:01:VARIABLES-AND-EXPRESSIONS", out _));
        Assert.True(index.TryGet("PED:UAE-MOE-MATH:L8:GENERAL:01:01:RATIONAL-NUMBERS", out var body));
        Assert.Equal(CanonicalLessonContentStatus.Published, body.Status);
        Assert.Contains(body.Translations, t => t.CultureCode == "ar");
    }

    [Fact]
    public async Task JsonRead_ReturnsLocalizedBodyWithoutDbTranslationRows()
    {
        var index = new EmbeddedCanonicalLessonContentIndex();
        const string code = "PED:UAE-MOE-MATH:L8:GENERAL:01:01:RATIONAL-NUMBERS";
        Assert.True(index.TryGet(code, out var body));

        var lessonId = Guid.NewGuid();
        var frameworkId = Guid.NewGuid();
        await using var db = CreateDb();
        db.CurriculumPedagogicalLessons.Add(new CurriculumPedagogicalLesson
        {
            Id = lessonId,
            FrameworkVersionId = frameworkId,
            Code = code,
            Title = "Order of Operations",
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        });
        db.CurriculumLessonContents.Add(new CurriculumLessonContent
        {
            Id = Guid.NewGuid(),
            FrameworkVersionId = frameworkId,
            PedagogicalLessonId = lessonId,
            Status = body.Status,
            ContentVersion = body.ContentVersion,
            VerifiedAtUtc = DateTime.UtcNow,
            PublishedAtUtc = DateTime.UtcNow,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        });
        await db.SaveChangesAsync();

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Edulytics:LessonContent:ReadFromJson"] = "true"
            })
            .Build();
        var repository = new LessonContentRepository(db, config);
        var found = await repository.ListCanonicalContentsAsync([lessonId]);

        Assert.Single(found);
        Assert.Equal(code, (await db.CurriculumPedagogicalLessons
            .SingleAsync()).Code);
        Assert.NotEmpty(found[0].Translations);
        Assert.NotEmpty(found[0].Translations[0].StepByStepSolutions);
        Assert.Empty(await db.CurriculumLessonContentTranslations.ToArrayAsync());
    }

    [Fact]
    public async Task JsonRead_RejectsMetadataVersionDrift()
    {
        var index = new EmbeddedCanonicalLessonContentIndex();
        const string code = "PED:UAE-MOE-MATH:L8:GENERAL:01:01:RATIONAL-NUMBERS";
        Assert.True(index.TryGet(code, out var body));

        var lessonId = Guid.NewGuid();
        await using var db = CreateDb();
        db.CurriculumPedagogicalLessons.Add(new CurriculumPedagogicalLesson
        {
            Id = lessonId, Code = code, FrameworkVersionId = Guid.NewGuid()
        });
        db.CurriculumLessonContents.Add(new CurriculumLessonContent
        {
            Id = Guid.NewGuid(), PedagogicalLessonId = lessonId,
            FrameworkVersionId = Guid.NewGuid(),
            Status = body.Status, ContentVersion = "intentionally-stale"
        });
        await db.SaveChangesAsync();
        var cfg = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Edulytics:LessonContent:ReadFromJson"] = "true"
            }).Build();

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await new LessonContentRepository(db, cfg)
                .ListCanonicalContentsAsync([lessonId]));
    }

    private static EdulyticsDbContext CreateDb() =>
        new(new DbContextOptionsBuilder<EdulyticsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);
}
