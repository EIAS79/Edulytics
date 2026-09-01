using System.Text;
using Edulytics.Core.Analytics;
using Edulytics.Core.AssessmentIntelligence;
using Edulytics.Core.Enums;
using Edulytics.Core.MathematicsGeneration;
using Edulytics.Core.Recovery;
using Edulytics.Services.AssessmentIntelligence;

namespace Edulytics.Services.Recovery;

public sealed class WeaknessRecoveryEngine
{
    public const string FormulaVersion = "phase36-v1";
    private readonly AssessmentBlueprintEngine _blueprintEngine;

    public WeaknessRecoveryEngine()
        : this(new AssessmentBlueprintEngine())
    {
    }

    public WeaknessRecoveryEngine(AssessmentBlueprintEngine blueprintEngine)
    {
        _blueprintEngine = blueprintEngine ?? throw new ArgumentNullException(nameof(blueprintEngine));
    }

    public WeaknessRecoveryPlan BuildPlan(WeaknessRecoveryRequest request)
    {
        ValidateRequest(request);

        var outcome = request.StudentProfile.Outcomes
            .SingleOrDefault(x => x.LearningOutcomeId == request.LearningOutcomeId)
            ?? throw new InvalidOperationException("The recovery Outcome is not present in the Student Learning Profile.");

        if (outcome.Band is MasteryBand.Secure or MasteryBand.Strong)
        {
            throw new InvalidOperationException("Weakness recovery requires an Outcome that is not already secure or strong.");
        }

        var excluded = request.PreviousExposureFingerprints
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim())
            .Distinct(StringComparer.Ordinal)
            .OrderBy(x => x, StringComparer.Ordinal)
            .ToArray();
        var promptShapes = request.PreviousPrompts
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(NormalizePromptShape)
            .Where(x => x.Length > 0)
            .Distinct(StringComparer.Ordinal)
            .OrderBy(x => x, StringComparer.Ordinal)
            .ToArray();

        var practiceRequest = new AssessmentBlueprintRequest(
            request.SchoolId,
            request.CurriculumAdoptionId,
            request.CurriculumLevelKey,
            request.CurriculumTopicId,
            request.CurriculumPedagogicalLessonId,
            [request.LearningOutcomeId],
            request.StudentProfile,
            AssessmentPurpose.Practice,
            request.PracticeQuestionCount,
            AssessmentDifficultyPolicy.Supportive,
            excluded);

        var reassessmentRequest = new AssessmentBlueprintRequest(
            request.SchoolId,
            request.CurriculumAdoptionId,
            request.CurriculumLevelKey,
            request.CurriculumTopicId,
            request.CurriculumPedagogicalLessonId,
            [request.LearningOutcomeId],
            request.StudentProfile,
            AssessmentPurpose.EquivalentReassessment,
            request.ReassessmentQuestionCount,
            request.ComparableDifficultyPolicy,
            excluded);

        return new WeaknessRecoveryPlan(
            request.SchoolId,
            request.StudentProfile.StudentProfileId,
            request.CurriculumAdoptionId,
            request.CurriculumLevelKey.Trim(),
            request.LearningOutcomeId,
            request.CurriculumPedagogicalLessonId,
            outcome.MasteryPercentage,
            outcome.Band,
            _blueprintEngine.Build(practiceRequest),
            _blueprintEngine.Build(reassessmentRequest),
            excluded,
            promptShapes,
            ExcludePreviouslySeenQuestions: true,
            FormulaVersion);
    }

    public void ValidateEquivalentReassessment(
        WeaknessRecoveryPlan plan,
        MathematicsGenerationBatch batch)
    {
        ArgumentNullException.ThrowIfNull(plan);
        ArgumentNullException.ThrowIfNull(batch);

        var blueprint = plan.EquivalentReassessmentBlueprint;
        if (!plan.ExcludePreviouslySeenQuestions ||
            blueprint.Purpose != AssessmentPurpose.EquivalentReassessment)
        {
            throw new InvalidOperationException("Equivalent reassessment must explicitly exclude previously seen questions.");
        }

        if (batch.SchoolId != plan.SchoolId ||
            batch.CurriculumAdoptionId != plan.CurriculumAdoptionId ||
            !string.Equals(batch.CurriculumLevelKey, plan.CurriculumLevelKey, StringComparison.Ordinal))
        {
            throw new InvalidOperationException("Equivalent reassessment batch is outside the recovery scope.");
        }

        if (batch.Items.Count != blueprint.QuestionCount)
            throw new InvalidOperationException("Equivalent reassessment item count does not match the blueprint.");

        var excluded = plan.ExcludedExposureFingerprints.ToHashSet(StringComparer.Ordinal);
        var fingerprints = new HashSet<string>(StringComparer.Ordinal);
        var previousShapes = plan.PreviousPromptShapes.ToHashSet(StringComparer.Ordinal);

        foreach (var generated in batch.Items)
        {
            var item = generated.Item;
            var link = generated.OutcomeLink;

            if (item.SchoolId != plan.SchoolId ||
                item.CurriculumAdoptionId != plan.CurriculumAdoptionId ||
                item.CurriculumPedagogicalLessonId != plan.CurriculumPedagogicalLessonId ||
                link.SchoolId != plan.SchoolId ||
                link.AssessmentItemId != item.Id ||
                link.LearningOutcomeId != plan.LearningOutcomeId)
            {
                throw new InvalidOperationException("Equivalent reassessment item violates recovery Outcome or lesson scope.");
            }

            if (string.IsNullOrWhiteSpace(item.ExposureFingerprint) ||
                excluded.Contains(item.ExposureFingerprint) ||
                !fingerprints.Add(item.ExposureFingerprint))
            {
                throw new InvalidOperationException("Equivalent reassessment reused a previous or duplicate exposure.");
            }

            if (previousShapes.Contains(NormalizePromptShape(item.Prompt)))
            {
                throw new InvalidOperationException("Equivalent reassessment contains a trivial near-duplicate of a previously seen question.");
            }
        }

        var expectedDifficulty = blueprint.DifficultyAllocations
            .ToDictionary(x => x.Difficulty, x => x.ItemCount);
        var actualDifficulty = batch.Items
            .GroupBy(x => x.Item.Difficulty)
            .ToDictionary(x => x.Key, x => x.Count());

        foreach (var difficulty in Enum.GetValues<AssessmentItemDifficulty>())
        {
            var expected = expectedDifficulty.GetValueOrDefault(difficulty);
            var actual = actualDifficulty.GetValueOrDefault(difficulty);
            if (expected != actual)
                throw new InvalidOperationException("Equivalent reassessment difficulty distribution is not comparable to the requested blueprint.");
        }
    }

    public RecoveryEvaluation Evaluate(
        WeaknessRecoveryPlan plan,
        StudentLearningProfile updatedProfile)
    {
        ArgumentNullException.ThrowIfNull(plan);
        ArgumentNullException.ThrowIfNull(updatedProfile);

        if (updatedProfile.SchoolId != plan.SchoolId ||
            updatedProfile.StudentProfileId != plan.StudentProfileId ||
            updatedProfile.CurriculumAdoptionId != plan.CurriculumAdoptionId)
        {
            throw new InvalidOperationException("Updated mastery profile is outside the recovery scope.");
        }

        var updated = updatedProfile.Outcomes
            .SingleOrDefault(x => x.LearningOutcomeId == plan.LearningOutcomeId)
            ?? throw new InvalidOperationException("Updated mastery profile does not contain the recovery Outcome.");

        var delta = Math.Round(updated.MasteryPercentage - plan.BaselineMastery, 2, MidpointRounding.AwayFromZero);
        var result = updated.Band is MasteryBand.Secure or MasteryBand.Strong || updated.MasteryPercentage >= 80m
            ? RecoveryOutcome.Mastered
            : delta >= 5m
                ? RecoveryOutcome.Improved
                : RecoveryOutcome.StillWeak;

        return new RecoveryEvaluation(
            result,
            plan.BaselineMastery,
            updated.MasteryPercentage,
            delta,
            FormulaVersion);
    }

    internal static string NormalizePromptShape(string prompt)
    {
        if (string.IsNullOrWhiteSpace(prompt))
            return string.Empty;

        var builder = new StringBuilder(prompt.Length);
        var inNumber = false;
        var pendingSpace = false;

        foreach (var raw in prompt.Trim().ToLowerInvariant())
        {
            if (char.IsDigit(raw) || raw == '.' || raw == ',')
            {
                if (!inNumber)
                    builder.Append('#');
                inNumber = true;
                pendingSpace = false;
                continue;
            }

            inNumber = false;
            if (char.IsLetter(raw))
            {
                if (pendingSpace && builder.Length > 0 && builder[^1] != ' ')
                    builder.Append(' ');
                builder.Append(raw);
                pendingSpace = false;
            }
            else
            {
                pendingSpace = true;
            }
        }

        return builder.ToString().Trim();
    }

    private static void ValidateRequest(WeaknessRecoveryRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.StudentProfile);
        ArgumentNullException.ThrowIfNull(request.PreviousExposureFingerprints);
        ArgumentNullException.ThrowIfNull(request.PreviousPrompts);
        ArgumentNullException.ThrowIfNull(request.ComparableDifficultyPolicy);

        if (request.SchoolId == Guid.Empty ||
            request.CurriculumAdoptionId == Guid.Empty ||
            request.CurriculumPedagogicalLessonId == Guid.Empty ||
            request.LearningOutcomeId == Guid.Empty ||
            string.IsNullOrWhiteSpace(request.CurriculumLevelKey))
        {
            throw new InvalidOperationException("Weakness recovery requires explicit school, curriculum, level, lesson and Outcome scope.");
        }

        if (request.StudentProfile.SchoolId != request.SchoolId ||
            request.StudentProfile.CurriculumAdoptionId != request.CurriculumAdoptionId)
        {
            throw new InvalidOperationException("Student Learning Profile is outside the recovery scope.");
        }

        if (request.PracticeQuestionCount is < 1 or > 50 ||
            request.ReassessmentQuestionCount is < 1 or > 50)
        {
            throw new InvalidOperationException("Recovery question counts must be between 1 and 50.");
        }
    }
}
