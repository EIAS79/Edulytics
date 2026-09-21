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
        Stage18PracticeSkillContract contract,
        StudentPrivatePracticeDifficulty requestedDifficulty,
        int questionCount,
        int seed,
        IReadOnlyCollection<string> excludedExposureFingerprints,
        Guid createdByUserId)
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
            excludedExposureFingerprints);

        return exact.Select(question =>
        {
            var alignment = LessonPracticeAlignmentValidator.Validate(contract, question);
            if (!alignment.IsAligned)
            {
                throw new InvalidOperationException(
                    $"Exact Practice alignment validator rejected {question.Family}: {alignment.ReasonCode}.");
            }

            return new AssessmentItem
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
                skillId = contract.SkillId,
                questionFamily = question.Family,
                parameters = question.Parameters
            }),
            ExposureFingerprint = question.ExposureFingerprint,
            ValidationMetadataJson = JsonSerializer.Serialize(new
            {
                stage = 18,
                alignment = "skill-contract-verified",
                alignmentReason = alignment.ReasonCode,
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
            };
        }).ToArray();
    }

    public IReadOnlyList<AssessmentItem> GenerateProgressiveLesson(
        Guid schoolId,
        Guid curriculumAdoptionId,
        Guid lessonId,
        Stage18PracticeSkillContract contract,
        int seed,
        IReadOnlyCollection<string> excludedExposureFingerprints,
        Guid createdByUserId)
    {
        ArgumentNullException.ThrowIfNull(contract);
        ArgumentNullException.ThrowIfNull(excludedExposureFingerprints);

        if (schoolId == Guid.Empty ||
            curriculumAdoptionId == Guid.Empty ||
            lessonId == Guid.Empty ||
            createdByUserId == Guid.Empty ||
            contract.AllowedQuestionFamilies.Count == 0)
        {
            throw new InvalidOperationException(
                "Progressive lesson Practice requires a valid exact lesson contract and request scope.");
        }

        var progression = new[]
        {
            ExactSkillQuestionDifficulty.Standard,
            ExactSkillQuestionDifficulty.Standard,
            ExactSkillQuestionDifficulty.Standard,
            ExactSkillQuestionDifficulty.Stretch,
            ExactSkillQuestionDifficulty.Stretch,
            ExactSkillQuestionDifficulty.Stretch,
            ExactSkillQuestionDifficulty.Challenge,
            ExactSkillQuestionDifficulty.Challenge
        };

        var exactEngine = new ExactSkillContractQuestionEngine();
        var historical = excludedExposureFingerprints
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToHashSet(StringComparer.Ordinal);
        var attemptFingerprints = new HashSet<string>(StringComparer.Ordinal);
        var items = new List<AssessmentItem>(progression.Length);

        for (var index = 0; index < progression.Length; index++)
        {
            var difficulty = progression[index];
            var roundSeed = unchecked((seed == 0 ? 1 : seed) ^ ((index + 1) * 7919));
            var exclusions = historical
                .Concat(attemptFingerprints)
                .Distinct(StringComparer.Ordinal)
                .ToArray();

            var selectedFamily =
                contract.AllowedQuestionFamilies[
                    index % contract.AllowedQuestionFamilies.Count];

            ExactSkillGeneratedQuestion question;
            try
            {
                question = exactEngine.Generate(
                    "stage18",
                    contract.LessonCode,
                    [selectedFamily],
                    difficulty,
                    1,
                    roundSeed,
                    exclusions)[0];
            }
            catch (ExactSkillQuestionPoolExhaustedException) when (historical.Count > 0)
            {
                // Historical exposure should improve variety, never block a
                // lesson. Keep the current 8-question attempt unique and allow
                // old questions to re-enter only after unseen variants are
                // exhausted.
                question = exactEngine.Generate(
                    "stage18",
                    contract.LessonCode,
                    [selectedFamily],
                    difficulty,
                    1,
                    roundSeed,
                    attemptFingerprints.ToArray())[0];
            }

            if (!attemptFingerprints.Add(question.ExposureFingerprint))
            {
                throw new InvalidOperationException(
                    "Progressive lesson Practice generated a duplicate question in the same attempt.");
            }

            var alignment = LessonPracticeAlignmentValidator.Validate(contract, question);
            if (!alignment.IsAligned)
            {
                throw new InvalidOperationException(
                    $"Exact Practice alignment validator rejected {question.Family}: {alignment.ReasonCode}.");
            }

            items.Add(new AssessmentItem
            {
                Id = Guid.NewGuid(),
                SchoolId = schoolId,
                CurriculumAdoptionId = curriculumAdoptionId,
                CurriculumPedagogicalLessonId = lessonId,
                Source = AssessmentItemSource.SystemGenerated,
                ItemType = question.ItemType,
                Difficulty = difficulty == ExactSkillQuestionDifficulty.Standard
                    ? AssessmentItemDifficulty.Medium
                    : AssessmentItemDifficulty.Challenging,
                Prompt = question.Prompt,
                CorrectAnswer = question.CorrectAnswer,
                Solution = question.Solution,
                CreatedByUserId = createdByUserId,
                GenerationMethod = Stage18PracticeSkillContracts.GenerationMethod,
                GenerationFamily = question.Family,
                GenerationParametersJson = JsonSerializer.Serialize(new
                {
                    skillId = contract.SkillId,
                    questionFamily = question.Family,
                    parameters = question.Parameters
                }),
                ExposureFingerprint = question.ExposureFingerprint,
                ValidationMetadataJson = JsonSerializer.Serialize(new
                {
                    stage = 18,
                    alignment = "skill-contract-verified",
                    alignmentReason = alignment.ReasonCode,
                    readiness = "READY_VERIFIED",
                    skillContract = contract.SkillId,
                    allowedFamily = question.Family,
                    difficulty = difficulty.ToString(),
                    progressionIndex = index + 1,
                    solver = Stage18PracticeSkillContracts.SolverIdentifier,
                    verifier = Stage18PracticeSkillContracts.VerifierIdentifier,
                    solverVerified = true,
                    broadFallbackUsed = false,
                    officialMasteryEvidence = false
                }),
                CreatedAtUtc = DateTime.UtcNow,
                RowVersion = []
            });
        }

        return items;
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

            if (!ExactSkillContractQuestionEngine.Verify(
                    item.GenerationFamily,
                    values,
                    item.CorrectAnswer))
            {
                return false;
            }

            return LessonPracticeAlignmentValidator.ValidatePersisted(
                contract,
                item.GenerationFamily,
                values,
                item.CorrectAnswer).IsAligned;
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
