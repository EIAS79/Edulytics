using System.Text.Json;
using Edulytics.Core.AssessmentIntelligence;
using Edulytics.Core.Assessments;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Assessment;
using Edulytics.Core.MathematicsGeneration;
using Edulytics.Services.Mathematics;

namespace Edulytics.Services.Assessments;

/// <summary>
/// Exact server-side Assessment Builder generator for official outcomes whose
/// Mathematics Intelligence SkillContracts are READY_VERIFIED.
/// </summary>
public sealed class Stage19AssessmentSkillContractEngine
{
    public MathematicsGenerationBatch Generate(
        Guid schoolId,
        Guid curriculumAdoptionId,
        string curriculumLevelKey,
        LearningOutcome outcome,
        Stage19AssessmentSkillContract contract,
        AssessmentBuilderDifficulty requestedDifficulty,
        int questionCount,
        int seed,
        IReadOnlyCollection<string> excludedExposureFingerprints,
        Guid createdByUserId,
        int? preferredVariant = null)
    {
        ArgumentNullException.ThrowIfNull(outcome);
        ArgumentNullException.ThrowIfNull(contract);
        ArgumentNullException.ThrowIfNull(excludedExposureFingerprints);

        if (schoolId == Guid.Empty ||
            curriculumAdoptionId == Guid.Empty ||
            createdByUserId == Guid.Empty ||
            string.IsNullOrWhiteSpace(curriculumLevelKey) ||
            outcome.Id == Guid.Empty ||
            questionCount is < 1 or > 50 ||
            !string.Equals(outcome.Code, contract.OutcomeCode, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Stage 19 Assessment generation requires an exact official outcome contract.");
        }

        var exact = new ExactSkillContractQuestionEngine().Generate(
            "stage19",
            contract.OutcomeCode,
            contract.AllowedQuestionFamilies,
            ResolveDifficulty(requestedDifficulty),
            questionCount,
            seed,
            excludedExposureFingerprints,
            preferredVariant);

        var items = exact.Select(question =>
        {
            if (!ExactSkillContractQuestionEngine.Verify(
                    question.Family,
                    question.Parameters,
                    question.CorrectAnswer))
            {
                throw new InvalidOperationException(
                    $"Stage 19 verifier rejected exact Assessment Builder output for {question.Family}.");
            }

            var item = new AssessmentItem
            {
                Id = Guid.NewGuid(),
                SchoolId = schoolId,
                CurriculumAdoptionId = curriculumAdoptionId,
                CurriculumTopicId = outcome.TopicId,
                Source = AssessmentItemSource.SystemGenerated,
                ItemType = question.ItemType,
                Difficulty = ResolveItemDifficulty(requestedDifficulty),
                Prompt = question.Prompt,
                CorrectAnswer = question.CorrectAnswer,
                Solution = question.Solution,
                CreatedByUserId = createdByUserId,
                GenerationMethod = Stage19AssessmentSkillContracts.GenerationMethod,
                GenerationFamily = question.Family,
                GenerationParametersJson = JsonSerializer.Serialize(new
                {
                    skillId = contract.SkillId,
                    outcomeCode = contract.OutcomeCode,
                    questionFamily = question.Family,
                    questionVariant = question.VariantId,
                    parameters = question.Parameters
                }),
                ExposureFingerprint = question.ExposureFingerprint,
                ValidationMetadataJson = JsonSerializer.Serialize(new
                {
                    stage = 19,
                    alignment = "official-outcome-skill-contract-verified",
                    readiness = "READY_VERIFIED",
                    skillContract = contract.SkillId,
                    officialOutcomeCode = contract.OutcomeCode,
                    allowedFamily = question.Family,
                    questionVariant = question.VariantId,
                    solver = Stage19AssessmentSkillContracts.SolverIdentifier,
                    verifier = Stage19AssessmentSkillContracts.VerifierIdentifier,
                    solverVerified = true,
                    broadFallbackUsed = false,
                    teacherReviewRequired = true,
                    workflow = "Select-Generate-Review-Approve-Publish"
                }),
                CreatedAtUtc = DateTime.UtcNow,
                RowVersion = []
            };

            return new GeneratedMathematicsItem(
                item,
                new AssessmentItemOutcome
                {
                    Id = Guid.NewGuid(),
                    SchoolId = schoolId,
                    AssessmentItemId = item.Id,
                    LearningOutcomeId = outcome.Id
                },
                ResolveBlueprintFamily(question.Family),
                Stage19AssessmentSkillContracts.GenerationMethod);
        }).ToArray();

        return new MathematicsGenerationBatch(
            schoolId,
            curriculumAdoptionId,
            curriculumLevelKey,
            items,
            Stage19AssessmentSkillContracts.GenerationMethod);
    }

    public static bool VerifyPersistedItem(
        Stage19AssessmentSkillContract contract,
        AssessmentItem item)
    {
        ArgumentNullException.ThrowIfNull(contract);
        ArgumentNullException.ThrowIfNull(item);

        if (!string.Equals(
                item.GenerationMethod,
                Stage19AssessmentSkillContracts.GenerationMethod,
                StringComparison.Ordinal) ||
            string.IsNullOrWhiteSpace(item.GenerationFamily) ||
            !contract.AllowedQuestionFamilies.Contains(item.GenerationFamily, StringComparer.Ordinal) ||
            string.IsNullOrWhiteSpace(item.GenerationParametersJson))
        {
            return false;
        }

        try
        {
            using var document = JsonDocument.Parse(item.GenerationParametersJson);
            var root = document.RootElement;

            if (!string.Equals(root.GetProperty("skillId").GetString(), contract.SkillId, StringComparison.Ordinal) ||
                !string.Equals(root.GetProperty("outcomeCode").GetString(), contract.OutcomeCode, StringComparison.OrdinalIgnoreCase) ||
                !string.Equals(root.GetProperty("questionFamily").GetString(), item.GenerationFamily, StringComparison.Ordinal))
            {
                return false;
            }

            var parameters = root.GetProperty("parameters");
            var values = new Dictionary<string, int>(StringComparer.Ordinal);
            foreach (var property in parameters.EnumerateObject())
                values[property.Name] = property.Value.GetInt32();

            return ExactSkillContractQuestionEngine.Verify(
                item.GenerationFamily,
                values,
                item.CorrectAnswer);
        }
        catch (Exception exception) when (
            exception is JsonException or
            KeyNotFoundException or
            InvalidOperationException or
            FormatException)
        {
            return false;
        }
    }

    private static ExactSkillQuestionDifficulty ResolveDifficulty(
        AssessmentBuilderDifficulty difficulty) =>
        difficulty switch
        {
            AssessmentBuilderDifficulty.Stretch => ExactSkillQuestionDifficulty.Stretch,
            AssessmentBuilderDifficulty.Challenge => ExactSkillQuestionDifficulty.Challenge,
            _ => ExactSkillQuestionDifficulty.Standard
        };

    private static AssessmentItemDifficulty ResolveItemDifficulty(
        AssessmentBuilderDifficulty difficulty) =>
        difficulty switch
        {
            AssessmentBuilderDifficulty.AtClassLevel => AssessmentItemDifficulty.Easy,
            AssessmentBuilderDifficulty.Stretch => AssessmentItemDifficulty.Medium,
            AssessmentBuilderDifficulty.Challenge => AssessmentItemDifficulty.Challenging,
            _ => AssessmentItemDifficulty.Medium
        };

    private static AssessmentQuestionFamily ResolveBlueprintFamily(string family) =>
        family switch
        {
            "ratio.unit_rate.direct" or
            "ratio.unit_rate.equivalent_ratio" => AssessmentQuestionFamily.AppliedProblem,

            "fractions.equivalent.number_line" or
            "fractions.equivalent.recognize" => AssessmentQuestionFamily.MathematicalReasoning,

            _ => AssessmentQuestionFamily.StructuredMethod
        };
}
