using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class PracticeSessionQualityValidatorTests
{
    [Fact]
    public void StandardShapeSessionIsReadyNarrowRatherThanPadded()
    {
        var (contract, items) = GenerateStandardShapeSession();

        var result = new PracticeSessionQualityValidator().Validate(
            contract,
            items,
            10);

        Assert.True(result.IsReady);
        Assert.Equal(
            PracticeSessionReadinessStatus.ReadyNarrow,
            result.Status);
        Assert.Equal("READY_NARROW", result.StatusCode);
        Assert.Equal(8, result.ActualQuestionCount);
        Assert.Equal(8, result.DistinctSemanticQuestions);
        Assert.Contains(
            "VALID_SESSION_SHORTER_THAN_REQUESTED",
            result.ReasonCodes);
    }

    [Fact]
    public void SemanticDuplicateBlocksTheSessionEvenWithUniqueFingerprint()
    {
        var (contract, generated) = GenerateStandardShapeSession();
        var items = generated.ToArray();

        items[1] = Clone(
            items[0],
            exposureFingerprint:
                items[0].ExposureFingerprint + "|duplicate-copy");

        var result = new PracticeSessionQualityValidator().Validate(
            contract,
            items,
            10);

        Assert.False(result.IsReady);
        Assert.Equal(
            PracticeSessionReadinessStatus.BlockedInsufficientVariety,
            result.Status);
        Assert.Contains(
            "DUPLICATE_SEMANTIC_QUESTION",
            result.ReasonCodes);
    }

    [Fact]
    public void ExplicitPromptAnswerLeakBlocksTheSession()
    {
        var (contract, generated) = GenerateStandardShapeSession();
        var items = generated.ToArray();

        items[0] = Clone(
            items[0],
            prompt:
                $"The correct answer is {items[0].CorrectAnswer}.");

        var result = new PracticeSessionQualityValidator().Validate(
            contract,
            items,
            10);

        Assert.False(result.IsReady);
        Assert.Equal(
            PracticeSessionReadinessStatus.BlockedPedagogicalQa,
            result.Status);
        Assert.Contains(
            "PROMPT_EXPLICITLY_REVEALS_ANSWER",
            result.ReasonCodes);
    }

    [Fact]
    public void FalseDisplayDifficultyBlocksTheSession()
    {
        var (contract, generated) = GenerateStandardShapeSession();
        var items = generated.ToArray();

        items[0] = Clone(
            items[0],
            difficulty:
                AssessmentItemDifficulty.Challenging);

        var result = new PracticeSessionQualityValidator().Validate(
            contract,
            items,
            10);

        Assert.False(result.IsReady);
        Assert.Equal(
            PracticeSessionReadinessStatus.BlockedPedagogicalQa,
            result.Status);
        Assert.Contains(
            "DISPLAY_DIFFICULTY_MISMATCH",
            result.ReasonCodes);
    }

    [Fact]
    public void ReadinessStampIsPersistedIntoEveryItemMetadata()
    {
        var (contract, items) = GenerateStandardShapeSession();
        var result = new PracticeSessionQualityValidator().Validate(
            contract,
            items,
            10);

        Assert.True(result.IsReady);

        PracticeSessionQualityValidator.StampReadiness(
            items,
            result);

        Assert.All(
            items,
            item =>
            {
                Assert.Contains(
                    "\"sessionReadiness\":\"READY_NARROW\"",
                    item.ValidationMetadataJson,
                    StringComparison.Ordinal);
                Assert.Contains(
                    "\"sessionQualityValidated\":true",
                    item.ValidationMetadataJson,
                    StringComparison.Ordinal);
            });
    }

    private static (
        Stage18PracticeSkillContract Contract,
        IReadOnlyList<AssessmentItem> Items)
        GenerateStandardShapeSession()
    {
        const string family =
            "supporting.geometry.shape_dimension";
        var lessonContract =
            LessonPracticeContractRegistry.All.First(
                contract =>
                    contract.AllowedQuestionFamilies.Count == 1 &&
                    string.Equals(
                        contract.AllowedQuestionFamilies[0],
                        family,
                        StringComparison.Ordinal));
        var contract =
            lessonContract.ToLegacyStage18Contract();

        var items =
            new Stage18SkillContractPracticeEngine()
                .GenerateComposed(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    contract,
                    StudentPrivatePracticeDifficulty.MyLevel,
                    10,
                    20260923,
                    [],
                    Guid.NewGuid());

        return (contract, items);
    }

    private static AssessmentItem Clone(
        AssessmentItem source,
        string? exposureFingerprint = null,
        string? prompt = null,
        AssessmentItemDifficulty? difficulty = null) =>
        new()
        {
            Id = Guid.NewGuid(),
            SchoolId = source.SchoolId,
            CurriculumAdoptionId =
                source.CurriculumAdoptionId,
            CurriculumPedagogicalLessonId =
                source.CurriculumPedagogicalLessonId,
            CurriculumTopicId =
                source.CurriculumTopicId,
            Source = source.Source,
            ItemType = source.ItemType,
            Difficulty =
                difficulty ?? source.Difficulty,
            Prompt = prompt ?? source.Prompt,
            CorrectAnswer = source.CorrectAnswer,
            Solution = source.Solution,
            CreatedByUserId = source.CreatedByUserId,
            GenerationMethod = source.GenerationMethod,
            GenerationFamily = source.GenerationFamily,
            GenerationParametersJson =
                source.GenerationParametersJson,
            ExposureFingerprint =
                exposureFingerprint ??
                source.ExposureFingerprint,
            ValidationMetadataJson =
                source.ValidationMetadataJson,
            CreatedAtUtc = source.CreatedAtUtc,
            RowVersion = []
        };
}
