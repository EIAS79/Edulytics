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
        Assert.Equal(5110, index.Count);
        Assert.True(index.TryGet("PED:UAE:G9:ADV:T1:L1-2", out var body));
        Assert.Equal(CanonicalLessonContentStatus.Published, body.Status);
        Assert.NotEmpty(body.Translations);
        Assert.All(body.Translations, t => Assert.False(
            string.IsNullOrWhiteSpace(t.Explanation)));
    }

    [Fact]
    public async Task JsonRead_ReturnsLocalizedBodyWithoutDbTranslationRows()
    {
        var index = new EmbeddedCanonicalLessonContentIndex();
        const string code = "PED:UAE:G9:ADV:T1:L1-2";
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
        const string code = "PED:UAE:G9:ADV:T1:L1-2";
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
