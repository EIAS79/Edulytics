using System.Text.Json;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Data.Contexts;
using Edulytics.Data.Seeding;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class CambridgeReviewedExampleUpgradeTests
{
    [Fact]
    public async Task Historical_nineteen_bodies_upgrade_in_place_and_reseed_idempotently()
    {
        var fixture = await CreateFixtureAsync();
        await using var db = fixture.Db;
        var ids = await db.CurriculumLessonContents.Select(x => x.Id).ToArrayAsync();
        var officialMappings = await db.CurriculumPedagogicalLessonOutcomes.CountAsync();
        var seeder = new MathematicsCanonicalLessonContentSeeder(db);

        await seeder.SeedDocumentsAsync(fixture.Documents);
        var after = await SnapshotAsync(db);
        await seeder.SeedDocumentsAsync(fixture.Documents);

        Assert.Equal(19, ids.Length);
        Assert.Equal(ids.Order(), (await db.CurriculumLessonContents.Select(x => x.Id).ToArrayAsync()).Order());
        Assert.Equal(officialMappings, await db.CurriculumPedagogicalLessonOutcomes.CountAsync());
        Assert.All(await db.CurriculumLessonContents.ToArrayAsync(), content =>
            Assert.Equal(CambridgeReviewedExampleContentCorrections.CorrectionContentVersion, content.ContentVersion));
        Assert.Equal(after, await SnapshotAsync(db));
        foreach (var lesson in fixture.Documents.SelectMany(x => x.Lessons))
        {
            var storedLesson = await db.CurriculumPedagogicalLessons.SingleAsync(x => x.Code == lesson.LessonCode);
            var content = await db.CurriculumLessonContents.SingleAsync(x => x.PedagogicalLessonId == storedLesson.Id);
            var translation = await db.CurriculumLessonContentTranslations.SingleAsync(x => x.CurriculumLessonContentId == content.Id);
            Assert.Equal(lesson.Translations.Single().WorkedExamples, translation.WorkedExamples);
            Assert.Equal(lesson.Translations.Single().StepByStepSolutions, translation.StepByStepSolutions);
        }
    }

    [Fact]
    public async Task Already_reviewed_body_under_legacy_version_only_promotes_its_version()
    {
        var fixture = await CreateFixtureAsync();
        await using var db = fixture.Db;
        var lesson = fixture.Documents[0].Lessons[0];
        var storedLesson = await db.CurriculumPedagogicalLessons.SingleAsync(x => x.Code == lesson.LessonCode);
        var content = await db.CurriculumLessonContents.SingleAsync(x => x.PedagogicalLessonId == storedLesson.Id);
        var translation = await db.CurriculumLessonContentTranslations.SingleAsync(x => x.CurriculumLessonContentId == content.Id);
        CopyBody(lesson.Translations.Single(), translation);
        await db.SaveChangesAsync();
        var reviewedBody = JsonSerializer.Serialize(lesson.Translations.Single());

        await new MathematicsCanonicalLessonContentSeeder(db).SeedDocumentsAsync(fixture.Documents);

        Assert.Equal(CambridgeReviewedExampleContentCorrections.CorrectionContentVersion, content.ContentVersion);
        Assert.Equal(reviewedBody, JsonSerializer.Serialize(ToBody(translation)));
    }

    [Theory]
    [InlineData("stored-body")]
    [InlineData("stored-version")]
    [InlineData("extra-culture")]
    [InlineData("incoming-body")]
    public async Task Unknown_content_cannot_be_overwritten(string corruption)
    {
        var fixture = await CreateFixtureAsync();
        await using var db = fixture.Db;
        var lesson = fixture.Documents[0].Lessons[0];
        var storedLesson = await db.CurriculumPedagogicalLessons.SingleAsync(x => x.Code == lesson.LessonCode);
        var content = await db.CurriculumLessonContents.SingleAsync(x => x.PedagogicalLessonId == storedLesson.Id);
        var translation = await db.CurriculumLessonContentTranslations.SingleAsync(x => x.CurriculumLessonContentId == content.Id);
        switch (corruption)
        {
            case "stored-body":
                translation.WorkedExamples += " Unreviewed edit.";
                break;
            case "stored-version":
                content.ContentVersion = "unreviewed-customer-version";
                break;
            case "extra-culture":
                var extra = new CurriculumLessonContentTranslation
                {
                    Id = Guid.NewGuid(), CurriculumLessonContentId = content.Id,
                    CreatedAtUtc = DateTime.UtcNow, UpdatedAtUtc = DateTime.UtcNow, RowVersion = []
                };
                CopyBody(lesson.Translations.Single(), extra);
                extra.CultureCode = "pl";
                db.CurriculumLessonContentTranslations.Add(extra);
                break;
            case "incoming-body":
                lesson.Translations.Single().WorkedExamples += " Unreviewed edit.";
                break;
        }
        await db.SaveChangesAsync();
        var before = await SnapshotAsync(db);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new MathematicsCanonicalLessonContentSeeder(db).SeedDocumentsAsync(fixture.Documents));

        db.ChangeTracker.Clear();
        Assert.Equal(before, await SnapshotAsync(db));
    }

    private static async Task<(EdulyticsDbContext Db, CanonicalLessonContentPackDocument[] Documents)> CreateFixtureAsync()
    {
        var db = new EdulyticsDbContext(new DbContextOptionsBuilder<EdulyticsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        await new MathematicsCurriculumPackSeeder(db).SeedAsync();
        await new MathematicsPedagogicalLessonSeeder(db).SeedAsync();
        var documents = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Where(d => d.Lessons.Any(l => CambridgeReviewedExampleContentCorrections.IsTarget(d, l)))
            .ToArray();
        foreach (var document in documents)
            document.Lessons = document.Lessons.Where(l =>
                CambridgeReviewedExampleContentCorrections.IsTarget(document, l)).ToList();
        await new MathematicsCanonicalLessonContentSeeder(db).SeedDocumentsAsync(documents);

        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
            directory = directory.Parent;
        var root = directory?.FullName ?? throw new DirectoryNotFoundException();
        var historical = JsonSerializer.Deserialize<HistoricalBody[]>(File.ReadAllText(Path.Combine(
            root, "tests/Edulytics.Tests/Fixtures/cambridge-reviewed-examples-legacy.json")))!;
        Assert.Equal(19, historical.Length);
        foreach (var prior in historical)
        {
            var lesson = await db.CurriculumPedagogicalLessons.SingleAsync(x => x.Code == prior.LessonCode);
            var content = await db.CurriculumLessonContents.SingleAsync(x => x.PedagogicalLessonId == lesson.Id);
            content.ContentVersion = prior.ContentVersion;
            var translation = await db.CurriculumLessonContentTranslations.SingleAsync(x =>
                x.CurriculumLessonContentId == content.Id);
            translation.KeyConceptsAndRules = prior.KeyConceptsAndRules;
            translation.WorkedExamples = prior.WorkedExamples;
            translation.StepByStepSolutions = prior.StepByStepSolutions;
        }
        await db.SaveChangesAsync();
        return (db, documents);
    }

    private static async Task<string> SnapshotAsync(EdulyticsDbContext db) =>
        JsonSerializer.Serialize(new
        {
            Contents = await db.CurriculumLessonContents.AsNoTracking().OrderBy(x => x.Id)
                .Select(x => new { x.Id, x.PedagogicalLessonId, x.ContentVersion, x.Status }).ToArrayAsync(),
            Bodies = await db.CurriculumLessonContentTranslations.AsNoTracking().OrderBy(x => x.Id)
                .Select(x => new { x.Id, x.CurriculumLessonContentId, x.CultureCode, x.Title, x.Explanation,
                    x.KeyConceptsAndRules, x.WorkedExamples, x.StepByStepSolutions, x.CommonMistakes, x.QuickSummary })
                .ToArrayAsync()
        });

    private sealed record HistoricalBody(
        string LessonCode, string ContentVersion, string KeyConceptsAndRules,
        string WorkedExamples, string StepByStepSolutions);

    private static CanonicalLessonContentPackTranslation ToBody(CurriculumLessonContentTranslation translation) => new()
    {
        CultureCode = translation.CultureCode, Title = translation.Title, Explanation = translation.Explanation,
        KeyConceptsAndRules = translation.KeyConceptsAndRules, WorkedExamples = translation.WorkedExamples,
        StepByStepSolutions = translation.StepByStepSolutions, CommonMistakes = translation.CommonMistakes,
        QuickSummary = translation.QuickSummary
    };

    private static void CopyBody(CanonicalLessonContentPackTranslation from, CurriculumLessonContentTranslation to)
    {
        to.CultureCode = from.CultureCode;
        to.Title = from.Title;
        to.Explanation = from.Explanation;
        to.KeyConceptsAndRules = from.KeyConceptsAndRules;
        to.WorkedExamples = from.WorkedExamples;
        to.StepByStepSolutions = from.StepByStepSolutions;
        to.CommonMistakes = from.CommonMistakes;
        to.QuickSummary = from.QuickSummary;
    }
}
