using Edulytics.Core.AdaptiveAssessment;
using Edulytics.Core.Analytics;
using Edulytics.Core.AssessmentIntelligence;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Assessment;
using Edulytics.Core.Mathematics.Difficulty;
using Edulytics.Services.Mathematics.Difficulty;

namespace Edulytics.Services.AdaptiveAssessment;

public sealed class AdaptiveDiagnosticAssessmentEngine
{
    public const string EngineVersion = "phase35-v1";
    public const string MathematicsEngineVersion = "stage20-math-aware-v1";

    private const decimal DiagnosticConfidenceThreshold = 45m;
    private const int DiagnosticEvidenceThreshold = 2;
    private const int MaximumComplexityScore = 200;
    private const int ComplexityStepLimit = 12;

    private readonly MathematicsDifficultyEngine mathematicsDifficultyEngine;

    public AdaptiveDiagnosticAssessmentEngine(
        MathematicsDifficultyEngine? mathematicsDifficultyEngine = null)
    {
        this.mathematicsDifficultyEngine =
            mathematicsDifficultyEngine ?? new MathematicsDifficultyEngine();
    }

    public AdaptiveAssessmentDecision DecideNext(AdaptiveAssessmentRequest request)
    {
        Validate(request);

        var selected = request.LearningOutcomeIds.Distinct().ToArray();
        var profileRows = request.StudentProfile?.Outcomes
            .Where(x => selected.Contains(x.LearningOutcomeId))
            .ToDictionary(x => x.LearningOutcomeId)
            ?? new Dictionary<Guid, StudentOutcomeLearningProfile>();

        var mathematicsStates = (request.MathematicsSkillStates ?? [])
            .ToDictionary(x => x.LearningOutcomeId);

        var exactOutcomeIds = selected
            .Where(id =>
                (profileRows.TryGetValue(id, out var row) &&
                 Stage19AssessmentSkillContracts.TryResolve(row.OutcomeCode, out _)) ||
                (mathematicsStates.TryGetValue(id, out var state) &&
                 Stage19AssessmentSkillContracts.TryResolve(state.OutcomeCode, out _)))
            .ToHashSet();

        if (exactOutcomeIds.Count > 0)
        {
            if (exactOutcomeIds.Count != selected.Length)
            {
                throw new InvalidOperationException(
                    "Stage 20 mathematics-aware adaptation fails closed when exact and non-exact Outcomes are mixed.");
            }

            if (mathematicsStates.Count != selected.Length ||
                selected.Any(id => !mathematicsStates.ContainsKey(id)))
            {
                throw new InvalidOperationException(
                    "Stage 20 exact Outcomes require complete mathematics-aware adaptive state.");
            }

            ValidateMathematicsStates(
                selected,
                profileRows,
                mathematicsStates);

            return DecideMathematicsAware(
                request,
                selected,
                profileRows,
                mathematicsStates);
        }

        if (mathematicsStates.Count > 0)
        {
            throw new InvalidOperationException(
                "Mathematics-aware state may only be used for exact Stage 19 Outcome contracts.");
        }

        return DecideLegacy(request, selected, profileRows);
    }

    private AdaptiveAssessmentDecision DecideMathematicsAware(
        AdaptiveAssessmentRequest request,
        IReadOnlyList<Guid> selected,
        IReadOnlyDictionary<Guid, StudentOutcomeLearningProfile> profileRows,
        IReadOnlyDictionary<Guid, AdaptiveMathematicsSkillState> states)
    {
        var diagnosticRequired =
            request.Purpose == AssessmentPurpose.Diagnostic ||
            request.StudentProfile is null ||
            selected.Any(id =>
                !profileRows.TryGetValue(id, out var row) ||
                row.EvidenceCount < DiagnosticEvidenceThreshold ||
                row.ConfidencePercentage < DiagnosticConfidenceThreshold);

        var latest = request.PreviousResponses
            .OrderByDescending(x => x.Sequence)
            .FirstOrDefault();

        Guid targetOutcome;
        if (!diagnosticRequired &&
            latest is not null &&
            !latest.IsCorrect &&
            states.ContainsKey(latest.LearningOutcomeId))
        {
            targetOutcome = latest.LearningOutcomeId;
        }
        else
        {
            targetOutcome = selected
                .OrderByDescending(id => AdaptivePriority(
                    states[id],
                    profileRows.GetValueOrDefault(id)))
                .ThenBy(id => profileRows.TryGetValue(id, out var row) ? row.EvidenceCount : -1)
                .ThenBy(id => profileRows.TryGetValue(id, out var row) ? row.ConfidencePercentage : -1m)
                .ThenBy(id => id)
                .First();
        }

        var state = states[targetOutcome];
        var representationFluency = AverageRepresentationFluency(state);
        var misconceptionCount = state.MisconceptionHistory.Sum(x => x.Count);

        var recommendation = mathematicsDifficultyEngine.Recommend(
            new MathematicsAdaptiveLearnerState(
                state.SkillMastery,
                state.PrerequisiteMastery,
                representationFluency,
                misconceptionCount,
                state.RecentSuccessfulItems));

        var targetResponses = request.PreviousResponses
            .Where(x => x.LearningOutcomeId == targetOutcome)
            .OrderBy(x => x.Sequence)
            .ToArray();
        var previous = targetResponses.LastOrDefault();

        var observedComplexity =
            previous?.MathematicsComplexityScore ??
            state.CurrentComplexityScore;

        var targetComplexity = AdjustComplexity(
            observedComplexity,
            recommendation.TargetComplexityScore,
            previous);

        var nextDifficulty = DifficultyForComplexity(targetComplexity);
        var reduced =
            observedComplexity > 0 &&
            targetComplexity < observedComplexity;

        var weakestRepresentation = SelectWeakestRepresentation(
            state,
            previous);

        var misconception = SelectMisconceptionFocus(
            state,
            previous);

        var questionFamily = SelectQuestionFamily(
            state,
            weakestRepresentation,
            misconception,
            previous);

        var mode = diagnosticRequired
            ? AdaptiveAssessmentMode.Diagnostic
            : AdaptiveAssessmentMode.Adaptive;

        var reason = BuildMathematicsReason(
            state,
            representationFluency,
            misconceptionCount,
            observedComplexity,
            targetComplexity,
            weakestRepresentation,
            misconception,
            mode);

        return new AdaptiveAssessmentDecision(
            mode,
            targetOutcome,
            nextDifficulty,
            CreditMultiplier(nextDifficulty),
            reduced,
            RequiresFreshExposure: true,
            reason,
            MathematicsEngineVersion,
            MathematicsAware: true,
            TargetSkillId: state.SkillId,
            TargetComplexityScore: targetComplexity,
            TargetQuestionFamily: questionFamily,
            TargetRepresentation: weakestRepresentation?.Representation,
            MisconceptionFocusId: misconception?.MisconceptionId);
    }

    private static AdaptiveAssessmentDecision DecideLegacy(
        AdaptiveAssessmentRequest request,
        IReadOnlyList<Guid> selected,
        IReadOnlyDictionary<Guid, StudentOutcomeLearningProfile> profileRows)
    {
        var diagnosticRequired =
            request.Purpose == AssessmentPurpose.Diagnostic ||
            request.StudentProfile is null ||
            selected.Any(id =>
                !profileRows.TryGetValue(id, out var row) ||
                row.EvidenceCount < DiagnosticEvidenceThreshold ||
                row.ConfidencePercentage < DiagnosticConfidenceThreshold);

        if (diagnosticRequired)
        {
            var target = selected
                .OrderBy(id => profileRows.ContainsKey(id) ? 1 : 0)
                .ThenBy(id => profileRows.TryGetValue(id, out var row) ? row.EvidenceCount : -1)
                .ThenBy(id => profileRows.TryGetValue(id, out var row) ? row.ConfidencePercentage : -1m)
                .ThenBy(id => profileRows.TryGetValue(id, out var row) ? row.MasteryPercentage : -1m)
                .ThenBy(id => id)
                .First();

            var difficulty = DiagnosticDifficulty(profileRows.GetValueOrDefault(target));

            return new AdaptiveAssessmentDecision(
                AdaptiveAssessmentMode.Diagnostic,
                target,
                difficulty,
                CreditMultiplier(difficulty),
                DifficultyReduced: false,
                RequiresFreshExposure: true,
                "Diagnostic calibration is required because evidence or confidence is insufficient.",
                EngineVersion);
        }

        var latest = request.PreviousResponses
            .OrderByDescending(x => x.Sequence)
            .FirstOrDefault();

        Guid targetOutcome;
        if (latest is not null && !latest.IsCorrect)
        {
            targetOutcome = latest.LearningOutcomeId;
        }
        else
        {
            targetOutcome = selected
                .OrderBy(id => profileRows[id].MasteryPercentage)
                .ThenBy(id => profileRows[id].ConfidencePercentage)
                .ThenBy(id => profileRows[id].EvidenceCount)
                .ThenBy(id => id)
                .First();
        }

        var targetProfile = profileRows[targetOutcome];
        var targetResponses = request.PreviousResponses
            .Where(x => x.LearningOutcomeId == targetOutcome)
            .OrderBy(x => x.Sequence)
            .ToArray();
        var previous = targetResponses.LastOrDefault();

        var baseline = previous?.Difficulty ?? DifficultyForMastery(targetProfile.MasteryPercentage);
        var next = previous is null
            ? baseline
            : previous.IsCorrect
                ? Increase(baseline)
                : Decrease(baseline);

        var reduced = previous is not null && Rank(next) < Rank(previous.Difficulty);
        var reason = previous switch
        {
            null => "Adaptive difficulty was selected from the current mastery profile.",
            { IsCorrect: false } => "The previous response was incorrect, so difficulty was reduced without increasing mastery credit.",
            _ => "The previous response was correct, so difficulty was increased within the configured bounds."
        };

        return new AdaptiveAssessmentDecision(
            AdaptiveAssessmentMode.Adaptive,
            targetOutcome,
            next,
            CreditMultiplier(next),
            reduced,
            RequiresFreshExposure: true,
            reason,
            EngineVersion);
    }

    private static void Validate(AdaptiveAssessmentRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.LearningOutcomeIds);
        ArgumentNullException.ThrowIfNull(request.PreviousResponses);

        if (request.SchoolId == Guid.Empty || request.CurriculumAdoptionId == Guid.Empty)
            throw new InvalidOperationException("Adaptive assessment requires School and Curriculum Adoption scope.");

        if (string.IsNullOrWhiteSpace(request.CurriculumLevelKey))
            throw new InvalidOperationException("Adaptive assessment requires Curriculum Level identity.");

        if (request.LearningOutcomeIds.Count == 0 ||
            request.LearningOutcomeIds.Any(x => x == Guid.Empty) ||
            request.LearningOutcomeIds.Distinct().Count() != request.LearningOutcomeIds.Count)
        {
            throw new InvalidOperationException("Adaptive assessment requires a non-empty distinct Outcome scope.");
        }

        if (request.Purpose is AssessmentPurpose.TeacherAssessment)
        {
            throw new InvalidOperationException(
                "Published formal Teacher Assessments use a fixed reviewed blueprint and are not adapted item-by-item.");
        }

        if (request.StudentProfile is not null &&
            (request.StudentProfile.SchoolId != request.SchoolId ||
             request.StudentProfile.CurriculumAdoptionId != request.CurriculumAdoptionId))
        {
            throw new InvalidOperationException("Student learning profile is outside adaptive assessment scope.");
        }

        var selected = request.LearningOutcomeIds.ToHashSet();
        if (request.PreviousResponses.Any(x =>
                !selected.Contains(x.LearningOutcomeId) ||
                x.Sequence <= 0 ||
                x.ScorePercentage is < 0m or > 100m ||
                (x.MathematicsComplexityScore.HasValue &&
                 x.MathematicsComplexityScore.Value is < 0 or > MaximumComplexityScore) ||
                (x.QuestionFamily is not null && string.IsNullOrWhiteSpace(x.QuestionFamily)) ||
                (x.Representation is not null && string.IsNullOrWhiteSpace(x.Representation)) ||
                (x.MisconceptionId is not null && string.IsNullOrWhiteSpace(x.MisconceptionId))))
        {
            throw new InvalidOperationException("Adaptive response history is invalid or outside Outcome scope.");
        }

        if (request.PreviousResponses.Select(x => x.Sequence).Distinct().Count() != request.PreviousResponses.Count)
            throw new InvalidOperationException("Adaptive response sequence must be unique.");

        if (request.MathematicsSkillStates is not null &&
            request.MathematicsSkillStates
                .Select(x => x.LearningOutcomeId)
                .Distinct()
                .Count() != request.MathematicsSkillStates.Count)
        {
            throw new InvalidOperationException(
                "Mathematics-aware adaptive state must contain one row per LearningOutcome.");
        }
    }

    private static void ValidateMathematicsStates(
        IReadOnlyList<Guid> selected,
        IReadOnlyDictionary<Guid, StudentOutcomeLearningProfile> profileRows,
        IReadOnlyDictionary<Guid, AdaptiveMathematicsSkillState> states)
    {
        var selectedSet = selected.ToHashSet();

        foreach (var state in states.Values)
        {
            if (!selectedSet.Contains(state.LearningOutcomeId) ||
                string.IsNullOrWhiteSpace(state.OutcomeCode) ||
                string.IsNullOrWhiteSpace(state.SkillId) ||
                state.SkillMastery is < 0m or > 1m ||
                state.PrerequisiteMastery is < 0m or > 1m ||
                state.CurrentComplexityScore is < 0 or > MaximumComplexityScore ||
                state.RecentSuccessfulItems < 0)
            {
                throw new InvalidOperationException(
                    "Stage 20 mathematics-aware state contains invalid scope or signal values.");
            }

            if (!Stage19AssessmentSkillContracts.TryResolve(
                    state.OutcomeCode,
                    out var contract) ||
                contract is null ||
                !string.Equals(
                    contract.SkillId,
                    state.SkillId,
                    StringComparison.Ordinal))
            {
                throw new InvalidOperationException(
                    "Stage 20 mathematics-aware state must match an exact Stage 19 Outcome/SkillContract.");
            }

            if (profileRows.TryGetValue(state.LearningOutcomeId, out var profile))
            {
                if (!string.Equals(
                        profile.OutcomeCode,
                        state.OutcomeCode,
                        StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException(
                        "Stage 20 OutcomeCode disagrees with the student mastery profile.");
                }

                var profileMastery = profile.MasteryPercentage / 100m;
                if (Math.Abs(profileMastery - state.SkillMastery) > 0.01m)
                {
                    throw new InvalidOperationException(
                        "Stage 20 SkillMastery must agree with the authoritative student mastery profile.");
                }
            }

            ArgumentNullException.ThrowIfNull(state.MisconceptionHistory);
            ArgumentNullException.ThrowIfNull(state.RepresentationFluency);

            if (state.MisconceptionHistory
                    .Select(x => x.MisconceptionId)
                    .Distinct(StringComparer.Ordinal)
                    .Count() != state.MisconceptionHistory.Count)
            {
                throw new InvalidOperationException(
                    "Stage 20 misconception history contains duplicate identifiers.");
            }

            foreach (var misconception in state.MisconceptionHistory)
            {
                if (string.IsNullOrWhiteSpace(misconception.MisconceptionId) ||
                    misconception.Count <= 0 ||
                    misconception.LastObservedSequence < 0 ||
                    (misconception.QuestionFamily is not null &&
                     !contract.AllowedQuestionFamilies.Contains(
                         misconception.QuestionFamily,
                         StringComparer.Ordinal)))
                {
                    throw new InvalidOperationException(
                        "Stage 20 misconception history is invalid or references a disallowed question family.");
                }
            }

            if (state.RepresentationFluency.Count == 0 ||
                state.RepresentationFluency
                    .Select(x => x.Representation)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .Count() != state.RepresentationFluency.Count)
            {
                throw new InvalidOperationException(
                    "Stage 20 requires distinct representation-fluency evidence.");
            }

            var mappedFamilies = new HashSet<string>(StringComparer.Ordinal);
            foreach (var representation in state.RepresentationFluency)
            {
                if (string.IsNullOrWhiteSpace(representation.Representation) ||
                    representation.Fluency is < 0m or > 1m ||
                    representation.QuestionFamilies is null ||
                    representation.QuestionFamilies.Count == 0 ||
                    representation.QuestionFamilies.Any(
                        family => !contract.AllowedQuestionFamilies.Contains(
                            family,
                            StringComparer.Ordinal)))
                {
                    throw new InvalidOperationException(
                        "Stage 20 representation fluency is invalid or references a disallowed question family.");
                }

                foreach (var family in representation.QuestionFamilies)
                    mappedFamilies.Add(family);
            }

            if (mappedFamilies.Count == 0)
            {
                throw new InvalidOperationException(
                    "Stage 20 representation fluency must map to at least one exact question family.");
            }
        }
    }

    private static decimal AdaptivePriority(
        AdaptiveMathematicsSkillState state,
        StudentOutcomeLearningProfile? profile)
    {
        var representation = AverageRepresentationFluency(state);
        var misconceptions = Math.Min(5, state.MisconceptionHistory.Sum(x => x.Count));
        var complexityOverreach = state.CurrentComplexityScore > 68 ? 1m : 0m;
        var lowConfidence = profile is null
            ? 1m
            : Math.Max(0m, (100m - profile.ConfidencePercentage) / 100m);

        return
            (1m - state.SkillMastery) * 40m +
            (1m - state.PrerequisiteMastery) * 25m +
            (1m - representation) * 20m +
            misconceptions * 2m +
            complexityOverreach * 3m +
            lowConfidence * 2m;
    }

    private static decimal AverageRepresentationFluency(
        AdaptiveMathematicsSkillState state) =>
        state.RepresentationFluency.Average(x => x.Fluency);

    private static int AdjustComplexity(
        int observedComplexity,
        int recommendedComplexity,
        AdaptiveResponseEvidence? previous)
    {
        recommendedComplexity = Math.Clamp(
            recommendedComplexity,
            0,
            MaximumComplexityScore);

        if (observedComplexity <= 0)
            return recommendedComplexity;

        var bounded = MoveTowards(
            observedComplexity,
            recommendedComplexity,
            ComplexityStepLimit);

        if (previous is null)
            return bounded;

        if (!previous.IsCorrect)
        {
            return Math.Max(
                0,
                Math.Min(
                    bounded,
                    observedComplexity - Math.Min(10, observedComplexity)));
        }

        return Math.Min(
            MaximumComplexityScore,
            Math.Max(
                bounded,
                Math.Min(
                    recommendedComplexity,
                    observedComplexity + ComplexityStepLimit)));
    }

    private static int MoveTowards(
        int current,
        int target,
        int maxDelta)
    {
        if (current == target)
            return current;

        if (target > current)
            return Math.Min(target, current + maxDelta);

        return Math.Max(target, current - maxDelta);
    }

    private static AdaptiveRepresentationFluency? SelectWeakestRepresentation(
        AdaptiveMathematicsSkillState state,
        AdaptiveResponseEvidence? previous)
    {
        if (previous is { IsCorrect: false } &&
            !string.IsNullOrWhiteSpace(previous.Representation))
        {
            var repeated = state.RepresentationFluency
                .FirstOrDefault(x =>
                    string.Equals(
                        x.Representation,
                        previous.Representation,
                        StringComparison.OrdinalIgnoreCase));
            if (repeated is not null)
                return repeated;
        }

        return state.RepresentationFluency
            .OrderBy(x => x.Fluency)
            .ThenBy(x => x.Representation, StringComparer.OrdinalIgnoreCase)
            .FirstOrDefault();
    }

    private static AdaptiveMisconceptionEvidence? SelectMisconceptionFocus(
        AdaptiveMathematicsSkillState state,
        AdaptiveResponseEvidence? previous)
    {
        if (previous is { IsCorrect: false } &&
            !string.IsNullOrWhiteSpace(previous.MisconceptionId))
        {
            var exact = state.MisconceptionHistory
                .FirstOrDefault(x =>
                    string.Equals(
                        x.MisconceptionId,
                        previous.MisconceptionId,
                        StringComparison.Ordinal));
            if (exact is not null)
                return exact;
        }

        return state.MisconceptionHistory
            .OrderByDescending(x => x.Count)
            .ThenByDescending(x => x.LastObservedSequence)
            .ThenBy(x => x.MisconceptionId, StringComparer.Ordinal)
            .FirstOrDefault();
    }

    private static string? SelectQuestionFamily(
        AdaptiveMathematicsSkillState state,
        AdaptiveRepresentationFluency? representation,
        AdaptiveMisconceptionEvidence? misconception,
        AdaptiveResponseEvidence? previous)
    {
        var contract = Stage19AssessmentSkillContracts.All
            .Single(x =>
                string.Equals(
                    x.OutcomeCode,
                    state.OutcomeCode,
                    StringComparison.OrdinalIgnoreCase));

        if (previous is { IsCorrect: false } &&
            !string.IsNullOrWhiteSpace(previous.QuestionFamily) &&
            contract.AllowedQuestionFamilies.Contains(
                previous.QuestionFamily,
                StringComparer.Ordinal))
        {
            return previous.QuestionFamily;
        }

        if (misconception?.QuestionFamily is not null)
            return misconception.QuestionFamily;

        return representation?.QuestionFamilies
            .Where(family =>
                contract.AllowedQuestionFamilies.Contains(
                    family,
                    StringComparer.Ordinal))
            .OrderBy(x => x, StringComparer.Ordinal)
            .FirstOrDefault();
    }

    private static string BuildMathematicsReason(
        AdaptiveMathematicsSkillState state,
        decimal representationFluency,
        int misconceptionCount,
        int observedComplexity,
        int targetComplexity,
        AdaptiveRepresentationFluency? representation,
        AdaptiveMisconceptionEvidence? misconception,
        AdaptiveAssessmentMode mode)
    {
        var parts = new List<string>
        {
            $"Stage 20 {mode.ToString().ToLowerInvariant()} decision uses SkillContract {state.SkillId}.",
            $"Skill mastery={state.SkillMastery:0.00}, prerequisite mastery={state.PrerequisiteMastery:0.00}, representation fluency={representationFluency:0.00}.",
            $"Mathematical complexity moves from {observedComplexity} to {targetComplexity}.",
            $"Recent misconception count={misconceptionCount}."
        };

        if (representation is not null)
            parts.Add($"Weakest/active representation={representation.Representation} ({representation.Fluency:0.00}).");

        if (misconception is not null)
            parts.Add($"Misconception focus={misconception.MisconceptionId}.");

        return string.Join(" ", parts);
    }

    private static AssessmentItemDifficulty DiagnosticDifficulty(StudentOutcomeLearningProfile? profile)
    {
        if (profile is null || profile.EvidenceCount == 0)
            return AssessmentItemDifficulty.Medium;

        return DifficultyForMastery(profile.MasteryPercentage);
    }

    private static AssessmentItemDifficulty DifficultyForMastery(decimal mastery) =>
        mastery switch
        {
            < 45m => AssessmentItemDifficulty.Easy,
            < 80m => AssessmentItemDifficulty.Medium,
            _ => AssessmentItemDifficulty.Challenging
        };

    private static AssessmentItemDifficulty DifficultyForComplexity(int complexity) =>
        complexity switch
        {
            <= 24 => AssessmentItemDifficulty.Easy,
            <= 55 => AssessmentItemDifficulty.Medium,
            _ => AssessmentItemDifficulty.Challenging
        };

    private static AssessmentItemDifficulty Increase(AssessmentItemDifficulty value) =>
        value switch
        {
            AssessmentItemDifficulty.Easy => AssessmentItemDifficulty.Medium,
            AssessmentItemDifficulty.Medium => AssessmentItemDifficulty.Challenging,
            _ => AssessmentItemDifficulty.Challenging
        };

    private static AssessmentItemDifficulty Decrease(AssessmentItemDifficulty value) =>
        value switch
        {
            AssessmentItemDifficulty.Challenging => AssessmentItemDifficulty.Medium,
            AssessmentItemDifficulty.Medium => AssessmentItemDifficulty.Easy,
            _ => AssessmentItemDifficulty.Easy
        };

    private static int Rank(AssessmentItemDifficulty value) =>
        value switch
        {
            AssessmentItemDifficulty.Easy => 1,
            AssessmentItemDifficulty.Medium => 2,
            AssessmentItemDifficulty.Challenging => 3,
            _ => throw new InvalidOperationException("Unsupported difficulty.")
        };

    private static decimal CreditMultiplier(AssessmentItemDifficulty value) =>
        value switch
        {
            AssessmentItemDifficulty.Easy => 0.55m,
            AssessmentItemDifficulty.Medium => 0.80m,
            AssessmentItemDifficulty.Challenging => 1.00m,
            _ => throw new InvalidOperationException("Unsupported difficulty.")
        };
}
