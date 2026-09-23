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
                questionVariant = question.VariantId,
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
                questionVariant = question.VariantId,
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

    public IReadOnlyList<AssessmentItem> GenerateComposed(
        Guid schoolId,
        Guid curriculumAdoptionId,
        Guid lessonId,
        Stage18PracticeSkillContract contract,
        StudentPrivatePracticeDifficulty requestedDifficulty,
        int requestedQuestionCount,
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
            requestedQuestionCount is < 1 or > 30 ||
            contract.AllowedQuestionFamilies.Count == 0)
        {
            throw new InvalidOperationException(
                "Composed Practice generation requires a valid exact lesson contract and request scope.");
        }

        var blueprint = new PracticeAssessmentComposer().Compose(
            contract.AllowedQuestionFamilies,
            requestedQuestionCount,
            ResolveCognitiveDifficulty(requestedDifficulty));

        var exactEngine = new ExactSkillContractQuestionEngine();
        var historical = excludedExposureFingerprints
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .ToHashSet(StringComparer.Ordinal);
        var attemptFingerprints = new HashSet<string>(StringComparer.Ordinal);
        var semanticKeys = new HashSet<string>(StringComparer.Ordinal);
        var items = new List<AssessmentItem>(requestedQuestionCount);
        var minimumAcceptableCount = Math.Min(3, requestedQuestionCount);

        foreach (var candidate in blueprint.Candidates)
        {
            if (items.Count >= requestedQuestionCount)
                break;

            var exactDifficulty = ResolveExactDifficulty(
                candidate.PlannedDifficulty);
            var roundSeed = unchecked(
                (seed == 0 ? 1 : seed) ^
                (candidate.CandidateOrder * 7919));

            ExactSkillGeneratedQuestion question;
            try
            {
                question = exactEngine.Generate(
                    "stage18",
                    contract.LessonCode,
                    [candidate.Family],
                    exactDifficulty,
                    1,
                    roundSeed,
                    historical
                        .Concat(attemptFingerprints)
                        .Distinct(StringComparer.Ordinal)
                        .ToArray(),
                    candidate.PreferredVariantSlot)[0];
            }
            catch (ExactSkillQuestionPoolExhaustedException)
                when (historical.Count > 0)
            {
                try
                {
                    // Historical exposure is a preference. Current-session
                    // fingerprint and semantic uniqueness remain mandatory.
                    question = exactEngine.Generate(
                        "stage18",
                        contract.LessonCode,
                        [candidate.Family],
                        exactDifficulty,
                        1,
                        roundSeed,
                        attemptFingerprints.ToArray(),
                        candidate.PreferredVariantSlot)[0];
                }
                catch (ExactSkillQuestionPoolExhaustedException)
                {
                    continue;
                }
            }
            catch (ExactSkillQuestionPoolExhaustedException)
            {
                continue;
            }

            var semanticIdentity =
                PracticeSemanticQuestionIdentityPolicy.Create(
                    question.Family,
                    question.Parameters);
            if (!semanticKeys.Add(semanticIdentity.Key))
                continue;

            if (!attemptFingerprints.Add(question.ExposureFingerprint))
                continue;

            var alignment = LessonPracticeAlignmentValidator.Validate(
                contract,
                question);
            if (!alignment.IsAligned)
            {
                throw new InvalidOperationException(
                    $"Composed Practice alignment validator rejected {question.Family}: {alignment.ReasonCode}.");
            }

            var actualVariantSlot =
                question.Parameters.TryGetValue(
                    "variant",
                    out var persistedVariant)
                    ? persistedVariant
                    : candidate.PreferredVariantSlot;
            var actualCapability =
                PracticeQuestionFormCapabilityRegistry.ResolveForVariant(
                    question.Family,
                    actualVariantSlot);
            var actualForm =
                actualCapability?.Form ?? candidate.Form;
            var actualOperation =
                actualCapability?.CognitiveOperation ??
                candidate.CognitiveOperation;

            items.Add(new AssessmentItem
            {
                Id = Guid.NewGuid(),
                SchoolId = schoolId,
                CurriculumAdoptionId = curriculumAdoptionId,
                CurriculumPedagogicalLessonId = lessonId,
                Source = AssessmentItemSource.SystemGenerated,
                ItemType = question.ItemType,
                Difficulty = exactDifficulty == ExactSkillQuestionDifficulty.Standard
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
                    questionVariant = question.VariantId,
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
                    composer = "practice-assessment-v2",
                    candidateOrder = candidate.CandidateOrder,
                    questionForm = actualForm.ToString(),
                    cognitiveOperation = actualOperation.ToString(),
                    cognitiveDifficulty = candidate.PlannedDifficulty.ToString(),
                    semanticKey = semanticIdentity.Key,
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

        if (items.Count < minimumAcceptableCount)
        {
            throw new ExactSkillQuestionPoolExhaustedException();
        }

        return items;
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
            PracticeCognitiveDifficulty.Standard,
            PracticeCognitiveDifficulty.Standard,
            PracticeCognitiveDifficulty.Standard,
            PracticeCognitiveDifficulty.Stretch,
            PracticeCognitiveDifficulty.Stretch,
            PracticeCognitiveDifficulty.Stretch,
            PracticeCognitiveDifficulty.Challenge,
            PracticeCognitiveDifficulty.Challenge
        };

        if (!SupportsHonestProgression(
                contract.AllowedQuestionFamilies,
                progression))
        {
            // A difficulty label must not outrun the implemented question form.
            // Narrow lessons therefore use an honest composed Standard session
            // rather than re-labelling the same mechanic as Stretch/Challenge.
            return GenerateComposed(
                schoolId,
                curriculumAdoptionId,
                lessonId,
                contract,
                StudentPrivatePracticeDifficulty.MyLevel,
                progression.Length,
                seed,
                excludedExposureFingerprints,
                createdByUserId);
        }

        var composer = new PracticeAssessmentComposer();
        var exactEngine = new ExactSkillContractQuestionEngine();
        var historical = excludedExposureFingerprints
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .ToHashSet(StringComparer.Ordinal);
        var attemptFingerprints = new HashSet<string>(StringComparer.Ordinal);
        var semanticKeys = new HashSet<string>(StringComparer.Ordinal);
        var items = new List<AssessmentItem>(progression.Length);

        for (var index = 0; index < progression.Length; index++)
        {
            var targetDifficulty = progression[index];
            var blueprint = composer.Compose(
                contract.AllowedQuestionFamilies,
                1,
                targetDifficulty);

            ExactSkillGeneratedQuestion? acceptedQuestion = null;
            PracticeAssessmentBlueprintItem? acceptedCandidate = null;
            PracticeQuestionFormCapability? acceptedCapability = null;
            PracticeSemanticQuestionIdentity? acceptedSemanticIdentity = null;

            var candidateCount = blueprint.Candidates.Count;
            for (var offset = 0;
                 offset < candidateCount && acceptedQuestion is null;
                 offset++)
            {
                var candidate =
                    blueprint.Candidates[
                        (index + offset) % candidateCount];
                var exactDifficulty = ResolveExactDifficulty(
                    targetDifficulty);
                var roundSeed = unchecked(
                    (seed == 0 ? 1 : seed) ^
                    ((index + 1) * 7919) ^
                    ((offset + 1) * 104729));

                ExactSkillGeneratedQuestion question;
                try
                {
                    question = exactEngine.Generate(
                        "stage18",
                        contract.LessonCode,
                        [candidate.Family],
                        exactDifficulty,
                        1,
                        roundSeed,
                        historical
                            .Concat(attemptFingerprints)
                            .Distinct(StringComparer.Ordinal)
                            .ToArray(),
                        candidate.PreferredVariantSlot)[0];
                }
                catch (ExactSkillQuestionPoolExhaustedException)
                    when (historical.Count > 0)
                {
                    try
                    {
                        question = exactEngine.Generate(
                            "stage18",
                            contract.LessonCode,
                            [candidate.Family],
                            exactDifficulty,
                            1,
                            roundSeed,
                            attemptFingerprints.ToArray(),
                            candidate.PreferredVariantSlot)[0];
                    }
                    catch (ExactSkillQuestionPoolExhaustedException)
                    {
                        continue;
                    }
                }
                catch (ExactSkillQuestionPoolExhaustedException)
                {
                    continue;
                }

                var semanticIdentity =
                    PracticeSemanticQuestionIdentityPolicy.Create(
                        question.Family,
                        question.Parameters);
                if (semanticKeys.Contains(semanticIdentity.Key) ||
                    attemptFingerprints.Contains(question.ExposureFingerprint))
                {
                    continue;
                }

                var alignment = LessonPracticeAlignmentValidator.Validate(
                    contract,
                    question);
                if (!alignment.IsAligned)
                {
                    throw new InvalidOperationException(
                        $"Progressive Practice alignment validator rejected {question.Family}: {alignment.ReasonCode}.");
                }

                var actualVariantSlot =
                    question.Parameters.TryGetValue(
                        "variant",
                        out var persistedVariant)
                        ? persistedVariant
                        : candidate.PreferredVariantSlot;
                var capability =
                    PracticeQuestionFormCapabilityRegistry.ResolveForVariant(
                        question.Family,
                        actualVariantSlot);

                if (capability is null ||
                    targetDifficulty < capability.MinimumDifficulty ||
                    targetDifficulty > capability.MaximumDifficulty)
                {
                    // Never attach a difficulty label to a form that does not
                    // explicitly support that cognitive level.
                    continue;
                }

                acceptedQuestion = question;
                acceptedCandidate = candidate;
                acceptedCapability = capability;
                acceptedSemanticIdentity = semanticIdentity;
            }

            if (acceptedQuestion is null ||
                acceptedCandidate is null ||
                acceptedCapability is null ||
                acceptedSemanticIdentity is null)
            {
                throw new ExactSkillQuestionPoolExhaustedException();
            }

            semanticKeys.Add(acceptedSemanticIdentity.Key);
            attemptFingerprints.Add(
                acceptedQuestion.ExposureFingerprint);

            var exactLevel = ResolveExactDifficulty(targetDifficulty);
            items.Add(new AssessmentItem
            {
                Id = Guid.NewGuid(),
                SchoolId = schoolId,
                CurriculumAdoptionId = curriculumAdoptionId,
                CurriculumPedagogicalLessonId = lessonId,
                Source = AssessmentItemSource.SystemGenerated,
                ItemType = acceptedQuestion.ItemType,
                Difficulty = exactLevel == ExactSkillQuestionDifficulty.Standard
                    ? AssessmentItemDifficulty.Medium
                    : AssessmentItemDifficulty.Challenging,
                Prompt = acceptedQuestion.Prompt,
                CorrectAnswer = acceptedQuestion.CorrectAnswer,
                Solution = acceptedQuestion.Solution,
                CreatedByUserId = createdByUserId,
                GenerationMethod = Stage18PracticeSkillContracts.GenerationMethod,
                GenerationFamily = acceptedQuestion.Family,
                GenerationParametersJson = JsonSerializer.Serialize(new
                {
                    skillId = contract.SkillId,
                    questionFamily = acceptedQuestion.Family,
                    questionVariant = acceptedQuestion.VariantId,
                    parameters = acceptedQuestion.Parameters
                }),
                ExposureFingerprint = acceptedQuestion.ExposureFingerprint,
                ValidationMetadataJson = JsonSerializer.Serialize(new
                {
                    stage = 18,
                    alignment = "skill-contract-verified",
                    readiness = "READY_VERIFIED",
                    skillContract = contract.SkillId,
                    allowedFamily = acceptedQuestion.Family,
                    composer = "practice-assessment-v2",
                    progression = "honest-cognitive-v2",
                    difficulty = targetDifficulty.ToString(),
                    cognitiveDifficulty = targetDifficulty.ToString(),
                    progressionIndex = index + 1,
                    questionForm = acceptedCapability.Form.ToString(),
                    cognitiveOperation =
                        acceptedCapability.CognitiveOperation.ToString(),
                    semanticKey = acceptedSemanticIdentity.Key,
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

    private static bool SupportsHonestProgression(
        IReadOnlyList<string> families,
        IReadOnlyList<PracticeCognitiveDifficulty> progression)
    {
        foreach (var difficulty in progression.Distinct())
        {
            var supported = families.Any(family =>
                PracticeQuestionFormCapabilityRegistry
                    .Resolve(family)
                    .Any(capability =>
                        difficulty >= capability.MinimumDifficulty &&
                        difficulty <= capability.MaximumDifficulty));

            if (!supported)
                return false;
        }

        return true;
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

    private static PracticeCognitiveDifficulty ResolveCognitiveDifficulty(
        StudentPrivatePracticeDifficulty difficulty) =>
        difficulty switch
        {
            StudentPrivatePracticeDifficulty.Stretch =>
                PracticeCognitiveDifficulty.Stretch,
            StudentPrivatePracticeDifficulty.Challenge =>
                PracticeCognitiveDifficulty.Challenge,
            _ => PracticeCognitiveDifficulty.Standard
        };

    private static ExactSkillQuestionDifficulty ResolveExactDifficulty(
        PracticeCognitiveDifficulty difficulty) =>
        difficulty switch
        {
            PracticeCognitiveDifficulty.Stretch =>
                ExactSkillQuestionDifficulty.Stretch,
            PracticeCognitiveDifficulty.Challenge =>
                ExactSkillQuestionDifficulty.Challenge,
            _ => ExactSkillQuestionDifficulty.Standard
        };

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
