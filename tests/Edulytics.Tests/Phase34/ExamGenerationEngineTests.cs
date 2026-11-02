using Edulytics.Core.AssessmentIntelligence;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.ExamGeneration;
using Edulytics.Core.MathematicsGeneration;
using Edulytics.Services.ExamGeneration;

namespace Edulytics.Tests.Phase34;

public sealed class ExamGenerationEngineTests
{
    private readonly ExamGenerationEngine _engine = new();

    [Fact]
    public void FormalExam_IsCreatedAsDraftAndNeverAutoPublished()
    {
        var request = FormalRequest();

        var draft = _engine.CreateDraft(request);

        Assert.Equal(GeneratedExamKind.FormalTeacherAssessment, draft.Kind);
        Assert.Equal(GeneratedExamStatus.Draft, draft.Status);
        Assert.Equal(request.Blueprint.QuestionCount, draft.Questions.Count);
        Assert.Throws<InvalidOperationException>(() => _engine.Publish(draft));
    }

    [Fact]
    public void FormalExam_RequiresReviewAndApprovalBeforePublish()
    {
        var draft = _engine.CreateDraft(FormalRequest());

        var reviewed = _engine.MarkReviewed(draft);
        var approved = _engine.Approve(reviewed);
        var published = _engine.Publish(approved);

        Assert.Equal(GeneratedExamStatus.Reviewed, reviewed.Status);
        Assert.Equal(GeneratedExamStatus.Approved, approved.Status);
        Assert.Equal(GeneratedExamStatus.Published, published.Status);
        Assert.Contains(published.AuditTrail, x => x.Contains("TeacherPublished", StringComparison.Ordinal));
    }

    [Fact]
    public void StudentPersonalTest_IsDistinctAndReadyToStartWithoutFormalPublication()
    {
        var request = PersonalRequest();

        var test = _engine.CreateDraft(request);

        Assert.Equal(GeneratedExamKind.StudentPersonalTest, test.Kind);
        Assert.Equal(GeneratedExamStatus.ReadyToStart, test.Status);
        Assert.Null(test.FormalContext);
        Assert.Throws<InvalidOperationException>(() => _engine.MarkReviewed(test));
        Assert.Throws<InvalidOperationException>(() => _engine.Publish(test));
    }

    [Fact]
    public void ReplacementMustBeNewAndEquivalentAndInvalidatesReview()
    {
        var reviewed = _engine.MarkReviewed(_engine.CreateDraft(FormalRequest()));
        var current = reviewed.Questions[0];
        var replacement = EquivalentReplacement(current.GeneratedItem, "replacement-fingerprint");

        var changed = _engine.ReplaceQuestion(reviewed, current.Order, replacement);

        Assert.Equal(GeneratedExamStatus.Draft, changed.Status);
        Assert.Equal(
            replacement.Item.ExposureFingerprint,
            changed.Questions[0].GeneratedItem.Item.ExposureFingerprint);
        Assert.Throws<InvalidOperationException>(() =>
            _engine.ReplaceQuestion(
                reviewed,
                current.Order,
                EquivalentReplacement(
                    current.GeneratedItem,
                    current.GeneratedItem.Item.ExposureFingerprint)));
    }

    [Fact]
    public void ReplacementCannotChangeOutcomeDifficultyTypeOrFamily()
    {
        var draft = _engine.CreateDraft(FormalRequest());
        var current = draft.Questions[0];
        var replacement = EquivalentReplacement(current.GeneratedItem, "different") with
        {
            OutcomeLink = new AssessmentItemOutcome
            {
                Id = Guid.NewGuid(),
                SchoolId = current.GeneratedItem.OutcomeLink.SchoolId,
                AssessmentItemId = current.GeneratedItem.Item.Id,
                LearningOutcomeId = Guid.NewGuid()
            }
        };

        Assert.Throws<InvalidOperationException>(() =>
            _engine.ReplaceQuestion(draft, current.Order, replacement));
    }

    [Fact]
    public void FormalMaterializationBridgesExactAssessmentItemBySharedIdentity()
    {
        var approved = _engine.Approve(
            _engine.MarkReviewed(
                _engine.CreateDraft(FormalRequest())));

        var materialized = _engine.MaterializeFormalAssessment(approved);

        Assert.Equal(AssessmentStatus.Draft, materialized.Assessment.Status);
        Assert.Equal(approved.Id, materialized.Assessment.Id);
        Assert.Equal(approved.Questions.Count, materialized.Questions.Count);
        Assert.Equal(approved.Questions.Count, materialized.AssessmentItems.Count);

        foreach (var question in materialized.Questions)
        {
            var item = Assert.Single(materialized.AssessmentItems, x => x.Id == question.Id);
            var questionOutcome = Assert.Single(
                materialized.OutcomeMappings,
                x => x.AssessmentQuestionId == question.Id);
            var itemOutcome = Assert.Single(
                materialized.ItemOutcomeMappings,
                x => x.AssessmentItemId == item.Id);
            Assert.Equal(itemOutcome.LearningOutcomeId, questionOutcome.LearningOutcomeId);
        }
    }

    [Fact]
    public void PublishedMaterializationOpensOnlyAfterExplicitPublish()
    {
        var draft = _engine.CreateDraft(FormalRequest());
        var published = _engine.Publish(_engine.Approve(_engine.MarkReviewed(draft)));

        var materialized = _engine.MaterializeFormalAssessment(published);

        Assert.Equal(AssessmentStatus.Open, materialized.Assessment.Status);
    }

    [Fact]
    public void MismatchedScopeOrExcludedExposureIsRejected()
    {
        var request = FormalRequest();
        var badBatch = request.GeneratedItems with
        {
            CurriculumAdoptionId = Guid.NewGuid()
        };
        Assert.Throws<InvalidOperationException>(() =>
            _engine.CreateDraft(request with { GeneratedItems = badBatch }));

        var excluded = request.GeneratedItems.Items[0].Item.ExposureFingerprint;
        var blueprint = request.Blueprint with
        {
            ExcludedExposureFingerprints = [excluded]
        };
        Assert.Throws<InvalidOperationException>(() =>
            _engine.CreateDraft(request with { Blueprint = blueprint }));
    }

    [Fact]
    public void FormalAndPersonalContextsCannotBeMixed()
    {
        var personal = PersonalRequest();
        Assert.Throws<InvalidOperationException>(() =>
            _engine.CreateDraft(personal with { FormalContext = FormalContext() }));

        var formal = FormalRequest();
        Assert.Throws<InvalidOperationException>(() =>
            _engine.CreateDraft(formal with { FormalContext = null }));
    }

    private static ExamGenerationRequest FormalRequest()
    {
        var fixture = Fixture(AssessmentPurpose.TeacherAssessment);
        return new ExamGenerationRequest(
            "Generated Mathematics Assessment",
            fixture.Blueprint,
            fixture.Batch,
            FormalContext());
    }

    private static ExamGenerationRequest PersonalRequest()
    {
        var fixture = Fixture(AssessmentPurpose.StudentPersonalTest);
        return new ExamGenerationRequest(
            "My Mathematics Test",
            fixture.Blueprint,
            fixture.Batch,
            null);
    }

    private static FormalExamContext FormalContext() =>
        new(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new DateOnly(2026, 9, 1));

    private static (AssessmentBlueprint Blueprint, MathematicsGenerationBatch Batch) Fixture(
        AssessmentPurpose purpose)
    {
        var schoolId = Guid.NewGuid();
        var adoptionId = Guid.NewGuid();
        var outcome1 = Guid.NewGuid();
        var outcome2 = Guid.NewGuid();
        var topicId = Guid.NewGuid();

        var blueprint = new AssessmentBlueprint(
            schoolId,
            adoptionId,
            "LEVEL-6",
            topicId,
            null,
            purpose,
            4,
            [
                new OutcomeBlueprintAllocation(outcome1, 2, 100m, "test"),
                new OutcomeBlueprintAllocation(outcome2, 2, 90m, "test")
            ],
            [
                new DifficultyBlueprintAllocation(AssessmentItemDifficulty.Easy, 1),
                new DifficultyBlueprintAllocation(AssessmentItemDifficulty.Medium, 2),
                new DifficultyBlueprintAllocation(AssessmentItemDifficulty.Challenging, 1)
            ],
            [
                new QuestionFamilyBlueprintAllocation(AssessmentQuestionFamily.DirectComputation, 1),
                new QuestionFamilyBlueprintAllocation(AssessmentQuestionFamily.StructuredMethod, 1),
                new QuestionFamilyBlueprintAllocation(AssessmentQuestionFamily.AppliedProblem, 1),
                new QuestionFamilyBlueprintAllocation(AssessmentQuestionFamily.MathematicalReasoning, 1)
            ],
            [
                new ItemTypeBlueprintAllocation(AssessmentItemType.Numeric, 2),
                new ItemTypeBlueprintAllocation(AssessmentItemType.ShortAnswer, 1),
                new ItemTypeBlueprintAllocation(AssessmentItemType.MultipleChoice, 1)
            ],
            [
                new OutcomeEvidenceRequirement(outcome1, 2, true, true),
                new OutcomeEvidenceRequirement(outcome2, 2, true, true)
            ],
            [],
            "phase32-v1");

        var items = new[]
        {
            Generated(schoolId, adoptionId, topicId, outcome1, AssessmentItemDifficulty.Easy, AssessmentItemType.Numeric, AssessmentQuestionFamily.DirectComputation, "a"),
            Generated(schoolId, adoptionId, topicId, outcome1, AssessmentItemDifficulty.Medium, AssessmentItemType.Numeric, AssessmentQuestionFamily.StructuredMethod, "b"),
            Generated(schoolId, adoptionId, topicId, outcome2, AssessmentItemDifficulty.Medium, AssessmentItemType.ShortAnswer, AssessmentQuestionFamily.AppliedProblem, "c"),
            Generated(schoolId, adoptionId, topicId, outcome2, AssessmentItemDifficulty.Challenging, AssessmentItemType.MultipleChoice, AssessmentQuestionFamily.MathematicalReasoning, "d")
        };

        var batch = new MathematicsGenerationBatch(
            schoolId,
            adoptionId,
            "LEVEL-6",
            items,
            "phase33-v1");

        return (blueprint, batch);
    }

    private static GeneratedMathematicsItem Generated(
        Guid schoolId,
        Guid adoptionId,
        Guid topicId,
        Guid outcomeId,
        AssessmentItemDifficulty difficulty,
        AssessmentItemType type,
        AssessmentQuestionFamily family,
        string fingerprint)
    {
        var item = new AssessmentItem
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            CurriculumAdoptionId = adoptionId,
            CurriculumTopicId = topicId,
            Source = AssessmentItemSource.SystemGenerated,
            ItemType = type,
            Difficulty = difficulty,
            Prompt = $"Prompt {fingerprint}",
            CorrectAnswer = "4",
            Solution = "2 + 2 = 4",
            GenerationMethod = "deterministic-reviewed-family",
            GenerationFamily = "TestFamily",
            GenerationParametersJson = "{\"a\":2,\"b\":2}",
            ExposureFingerprint = fingerprint,
            ValidationMetadataJson = "{\"validated\":true}",
            CreatedAtUtc = DateTime.UtcNow
        };
        var outcome = new AssessmentItemOutcome
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            AssessmentItemId = item.Id,
            LearningOutcomeId = outcomeId
        };
        return new GeneratedMathematicsItem(item, outcome, family, "phase33-v1");
    }

    private static GeneratedMathematicsItem EquivalentReplacement(
        GeneratedMathematicsItem original,
        string fingerprint)
    {
        var replacement = new AssessmentItem
        {
            Id = Guid.NewGuid(),
            SchoolId = original.Item.SchoolId,
            CurriculumAdoptionId = original.Item.CurriculumAdoptionId,
            CurriculumTopicId = original.Item.CurriculumTopicId,
            CurriculumPedagogicalLessonId = original.Item.CurriculumPedagogicalLessonId,
            Source = AssessmentItemSource.SystemGenerated,
            ItemType = original.Item.ItemType,
            Difficulty = original.Item.Difficulty,
            Prompt = $"Replacement {fingerprint}",
            CorrectAnswer = "5",
            Solution = "2 + 3 = 5",
            GenerationMethod = "deterministic-reviewed-family",
            GenerationFamily = original.Item.GenerationFamily,
            GenerationParametersJson = "{\"a\":2,\"b\":3}",
            ExposureFingerprint = fingerprint,
            ValidationMetadataJson = "{\"validated\":true}",
            CreatedAtUtc = DateTime.UtcNow
        };
        var outcome = new AssessmentItemOutcome
        {
            Id = Guid.NewGuid(),
            SchoolId = replacement.SchoolId,
            AssessmentItemId = replacement.Id,
            LearningOutcomeId = original.OutcomeLink.LearningOutcomeId
        };
        return new GeneratedMathematicsItem(
            replacement,
            outcome,
            original.BlueprintFamily,
            "phase33-v1");
    }
}
