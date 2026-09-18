using System.Text.Json;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Services.Practice;

/// <summary>
/// Stage 18 Practice adapter over the shared exact Mathematics Intelligence
/// question kernel. Practice owns lesson alignment/persistence metadata; the
/// shared engine owns exact generation, solving and independent verification.
/// </summary>
public sealed class Stage18SkillContractPracticeEngine
{
    public IReadOnlyList<AssessmentItem> Generate(
        Guid schoolId,
        Guid curriculumAdoptionId,
        Guid lessonId,
        LessonPracticeContract contract,
        StudentPrivatePracticeDifficulty requestedDifficulty,
        int questionCount,
        int seed,
        IReadOnlyCollection<string> excludedExposureFingerprints,
        Guid createdByUserId,
        int? curriculumLogicalLevel = null)
    {
        ArgumentNullException.ThrowIfNull(contract);
        var legacy = contract.ToLegacyStage18Contract();
        var items = Generate(
            schoolId,
            curriculumAdoptionId,
            lessonId,
            legacy,
            requestedDifficulty,
            questionCount,
            seed,
            excludedExposureFingerprints,
            createdByUserId,
            contract.CurriculumLogicalLevel);

        foreach (var item in items)
        {
            var alignment = LessonPracticeAlignmentValidator.Validate(
                contract,
                contract.SkillId,
                item.GenerationFamily);
            if (!alignment.IsAligned ||
                !VerifyPersistedItem(legacy, item))
            {
                throw new InvalidOperationException(
                    "Generated lesson Practice item failed alignment or mathematical verification.");
            }

            item.ValidationMetadataJson = JsonSerializer.Serialize(new
            {
                schemaVersion = 1,
                stage = 18,
                contractVersion = contract.ContractVersion,
                alignment = "lesson-practice-contract-verified",
                readiness = contract.Readiness,
                primarySkills = contract.PrimarySkillIds,
                secondarySkills = contract.SecondarySkillIds,
                prerequisites = contract.PrerequisiteSkillIds,
                allowedFamily = item.GenerationFamily,
                solver = Stage18PracticeSkillContracts.SolverIdentifier,
                verifier = Stage18PracticeSkillContracts.VerifierIdentifier,
                alignmentVerified = true,
                solverVerified = true,
                broadFallbackUsed = false,
                officialMasteryEvidence = false
            });
        }

        return items;
    }

    public static bool VerifyPersistedItem(
        LessonPracticeContract contract,
        AssessmentItem item)
    {
        ArgumentNullException.ThrowIfNull(contract);
        ArgumentNullException.ThrowIfNull(item);

        if (!LessonPracticeAlignmentValidator.Validate(
                contract,
                contract.SkillId,
                item.GenerationFamily).IsAligned)
        {
            return false;
        }

        return VerifyPersistedItem(contract.ToLegacyStage18Contract(), item);
    }

    public IReadOnlyList<AssessmentItem> Generate(
        Guid schoolId,
        Guid curriculumAdoptionId,
        Guid lessonId,
        Stage18PracticeSkillContract contract,
        StudentPrivatePracticeDifficulty requestedDifficulty,
        int questionCount,
        int seed,
        IReadOnlyCollection<string> excludedExposureFingerprints,
        Guid createdByUserId,
        int? curriculumLogicalLevel = null)
    {
        ArgumentNullException.ThrowIfNull(contract);
        ArgumentNullException.ThrowIfNull(excludedExposureFingerprints);

        if (schoolId == Guid.Empty ||
            curriculumAdoptionId == Guid.Empty ||
            lessonId == Guid.Empty ||
            createdByUserId == Guid.Empty ||
            questionCount is < 1 or > 30 ||
            contract.AllowedQuestionFamilies.Count == 0)
        {
            throw new InvalidOperationException(
                "Stage 18 Practice generation requires a valid exact lesson contract and request scope.");
        }

        var exact = new ExactSkillContractQuestionEngine().Generate(
            "stage18",
            contract.LessonCode,
            contract.AllowedQuestionFamilies,
            ResolveDifficulty(requestedDifficulty),
            questionCount,
            seed,
            excludedExposureFingerprints,
            curriculumLogicalLevel);

        return exact.Select(question => new AssessmentItem
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            CurriculumAdoptionId = curriculumAdoptionId,
            CurriculumPedagogicalLessonId = lessonId,
            Source = AssessmentItemSource.SystemGenerated,
            ItemType = question.ItemType,
            Difficulty = ResolveItemDifficulty(requestedDifficulty),
            Prompt = question.Prompt,
            CorrectAnswer = question.CorrectAnswer,
            Solution = question.Solution,
            CreatedByUserId = createdByUserId,
            GenerationMethod = Stage18PracticeSkillContracts.GenerationMethod,
            GenerationFamily = question.Family,
            GenerationParametersJson = JsonSerializer.Serialize(new
            {
                schemaVersion = 1,
                skillId = contract.SkillId,
                questionFamily = question.Family,
                parameters = question.Parameters,
                representation = question.Representation
            }),
            ExposureFingerprint = question.ExposureFingerprint,
            ValidationMetadataJson = JsonSerializer.Serialize(new
            {
                stage = 18,
                alignment = "skill-contract-verified",
                readiness = "READY_VERIFIED",
                skillContract = contract.SkillId,
                allowedFamily = question.Family,
                solver = Stage18PracticeSkillContracts.SolverIdentifier,
                verifier = Stage18PracticeSkillContracts.VerifierIdentifier,
                solverVerified = true,
                broadFallbackUsed = false,
                officialMasteryEvidence = false
            }),
            CreatedAtUtc = DateTime.UtcNow,
            RowVersion = []
        }).ToArray();
    }

    public static bool VerifyPersistedItem(
        Stage18PracticeSkillContract contract,
        AssessmentItem item)
    {
        ArgumentNullException.ThrowIfNull(contract);
        ArgumentNullException.ThrowIfNull(item);

        if (!string.Equals(
                item.GenerationMethod,
                Stage18PracticeSkillContracts.GenerationMethod,
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
        StudentPrivatePracticeDifficulty difficulty) =>
        difficulty switch
        {
            StudentPrivatePracticeDifficulty.Stretch => ExactSkillQuestionDifficulty.Stretch,
            StudentPrivatePracticeDifficulty.Challenge => ExactSkillQuestionDifficulty.Challenge,
            _ => ExactSkillQuestionDifficulty.Standard
        };

    private static AssessmentItemDifficulty ResolveItemDifficulty(
        StudentPrivatePracticeDifficulty difficulty) =>
        difficulty switch
        {
            StudentPrivatePracticeDifficulty.Stretch or StudentPrivatePracticeDifficulty.Challenge =>
                AssessmentItemDifficulty.Challenging,
            StudentPrivatePracticeDifficulty.MyLevel =>
                AssessmentItemDifficulty.Medium,
            _ => AssessmentItemDifficulty.Medium
        };
}
