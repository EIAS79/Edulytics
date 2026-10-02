using Edulytics.Core.Entities;
using Edulytics.Data.Contexts;
using Edulytics.Data.Seeding;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class RehearsalLessonLinkRepairTests
{
    [Fact]
    public async Task Repair_clears_only_unsupported_labels_and_preserves_evidence_and_other_schools()
    {
        await using var db = CreateDb();
        var school = new School { Id = Guid.NewGuid(), SchoolCode = "REHEARSAL-GB-PRIMARY" };
        var otherSchool = new School { Id = Guid.NewGuid(), SchoolCode = "CUSTOMER" };
        var adoption = new SchoolCurriculumAdoption { Id = Guid.NewGuid(), SchoolId = school.Id,
            FrameworkVersionId = Guid.NewGuid(), CurriculumLogicalLevel = 4 };
        var lesson = new CurriculumPedagogicalLesson { Id = Guid.NewGuid(),
            FrameworkVersionId = adoption.FrameworkVersionId, LogicalLevelFrom = 4, LogicalLevelTo = 4 };
        var aligned = new LearningOutcome { Id = Guid.NewGuid(), SchoolId = school.Id,
            CurriculumAdoptionId = adoption.Id, OfficialContentNodeId = Guid.NewGuid() };
        var unrelated = new LearningOutcome { Id = Guid.NewGuid(), SchoolId = school.Id,
            CurriculumAdoptionId = adoption.Id, OfficialContentNodeId = Guid.NewGuid() };
        var unverified = new LearningOutcome { Id = Guid.NewGuid(), SchoolId = school.Id,
            CurriculumAdoptionId = adoption.Id };
        db.AddRange(school, otherSchool, adoption, lesson, aligned, unrelated, unverified);
        db.CurriculumPedagogicalLessonOutcomes.Add(new CurriculumPedagogicalLessonOutcome {
            PedagogicalLessonId = lesson.Id, FrameworkVersionId = adoption.FrameworkVersionId,
            OutcomeNodeId = aligned.OfficialContentNodeId!.Value });

        AssessmentItem AddItem(Guid owner, params LearningOutcome[] outcomes)
        {
            var item = new AssessmentItem { Id = Guid.NewGuid(), SchoolId = owner,
                CurriculumAdoptionId = adoption.Id, CurriculumPedagogicalLessonId = lesson.Id,
                Prompt = "Original question", CorrectAnswer = "Original answer", Solution = "Original solution",
                ExposureFingerprint = "Original fingerprint", GenerationParametersJson = "{\"historical\":true}" };
            db.AssessmentItems.Add(item);
            foreach (var outcome in outcomes)
                db.AssessmentItemOutcomes.Add(new AssessmentItemOutcome { Id = Guid.NewGuid(),
                    SchoolId = owner, AssessmentItemId = item.Id, LearningOutcomeId = outcome.Id });
            return item;
        }

        var good = AddItem(school.Id, aligned);
        var wrong = AddItem(school.Id, unrelated);
        var mixed = AddItem(school.Id, aligned, unrelated);
        var missing = AddItem(school.Id);
        var unsupported = AddItem(school.Id, unverified);
        var customer = AddItem(otherSchool.Id, unrelated);

        PracticeAttempt AddAttempt(params AssessmentItem[] items)
        {
            var attempt = new PracticeAttempt { Id = Guid.NewGuid(), SchoolId = school.Id,
                CurriculumAdoptionId = adoption.Id, CurriculumPedagogicalLessonId = lesson.Id,
                IsPrivate = true, Score = 1m, MaxScore = 2m, Percentage = 50m };
            db.PracticeAttempts.Add(attempt);
            foreach (var item in items)
                db.PracticeAttemptItems.Add(new PracticeAttemptItem { Id = Guid.NewGuid(),
                    SchoolId = school.Id, PracticeAttemptId = attempt.Id, AssessmentItemId = item.Id });
            return attempt;
        }

        var goodAttempt = AddAttempt(good);
        var mixedAttempt = AddAttempt(good, wrong);
        var emptyAttempt = AddAttempt();
        var evidence = new LearningEvidence { Id = Guid.NewGuid(), SchoolId = school.Id,
            AssessmentItemId = wrong.Id, LearningOutcomeId = unrelated.Id, Score = 0.75m, MaxScore = 1m };
        db.LearningEvidence.Add(evidence);
        await db.SaveChangesAsync();

        var result = await RehearsalLessonLinkRepair.RunAsync(db, [school.SchoolCode]);
        Assert.Equal((4, 2), result);
        Assert.Equal(lesson.Id, good.CurriculumPedagogicalLessonId);
        Assert.Equal(lesson.Id, customer.CurriculumPedagogicalLessonId);
        Assert.Equal(lesson.Id, goodAttempt.CurriculumPedagogicalLessonId);
        Assert.All(new[] { wrong, mixed, missing, unsupported }, x => Assert.Null(x.CurriculumPedagogicalLessonId));
        Assert.Null(mixedAttempt.CurriculumPedagogicalLessonId);
        Assert.Null(emptyAttempt.CurriculumPedagogicalLessonId);
        Assert.All(await db.AssessmentItems.ToArrayAsync(), x => {
            Assert.Equal("Original question", x.Prompt);
            Assert.Equal("Original answer", x.CorrectAnswer);
            Assert.Equal("Original solution", x.Solution);
            Assert.Equal("Original fingerprint", x.ExposureFingerprint);
            Assert.Equal("{\"historical\":true}", x.GenerationParametersJson);
        });
        Assert.Equal(0.75m, (await db.LearningEvidence.SingleAsync()).Score);
        Assert.All(await db.PracticeAttempts.ToArrayAsync(), x => {
            Assert.True(x.IsPrivate);
            Assert.Equal(1m, x.Score);
            Assert.Equal(50m, x.Percentage);
        });
        Assert.Equal((0, 0), await RehearsalLessonLinkRepair.RunAsync(db, [school.SchoolCode]));
        Assert.Equal(6, await db.AssessmentItems.CountAsync());
        Assert.Equal(6, await db.AssessmentItemOutcomes.CountAsync());
    }

    [Theory]
    [InlineData("framework")]
    [InlineData("level")]
    [InlineData("pathway")]
    public async Task Official_mapping_alone_does_not_override_curriculum_scope(string mismatch)
    {
        await using var db = CreateDb();
        var school = new School { Id = Guid.NewGuid(), SchoolCode = "REHEARSAL-GB-PRIMARY" };
        var adoption = new SchoolCurriculumAdoption { Id = Guid.NewGuid(), SchoolId = school.Id,
            FrameworkVersionId = Guid.NewGuid(), CurriculumLogicalLevel = 4, CurriculumPathway = "STANDARD" };
        var lesson = new CurriculumPedagogicalLesson { Id = Guid.NewGuid(),
            FrameworkVersionId = mismatch == "framework" ? Guid.NewGuid() : adoption.FrameworkVersionId,
            LogicalLevelFrom = mismatch == "level" ? 5 : 4, LogicalLevelTo = 5,
            Pathway = mismatch == "pathway" ? "ADVANCED" : "STANDARD" };
        var outcome = new LearningOutcome { Id = Guid.NewGuid(), SchoolId = school.Id,
            CurriculumAdoptionId = adoption.Id, OfficialContentNodeId = Guid.NewGuid() };
        var item = new AssessmentItem { Id = Guid.NewGuid(), SchoolId = school.Id,
            CurriculumAdoptionId = adoption.Id, CurriculumPedagogicalLessonId = lesson.Id };
        db.AddRange(school, adoption, lesson, outcome, item);
        db.Add(new AssessmentItemOutcome { Id = Guid.NewGuid(), SchoolId = school.Id,
            AssessmentItemId = item.Id, LearningOutcomeId = outcome.Id });
        db.Add(new CurriculumPedagogicalLessonOutcome { PedagogicalLessonId = lesson.Id,
            FrameworkVersionId = adoption.FrameworkVersionId, OutcomeNodeId = outcome.OfficialContentNodeId!.Value });
        await db.SaveChangesAsync();
        Assert.Equal((1, 0), await RehearsalLessonLinkRepair.RunAsync(db, [school.SchoolCode]));
        Assert.Null(item.CurriculumPedagogicalLessonId);
    }

    private static EdulyticsDbContext CreateDb() => new(new DbContextOptionsBuilder<EdulyticsDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
}
