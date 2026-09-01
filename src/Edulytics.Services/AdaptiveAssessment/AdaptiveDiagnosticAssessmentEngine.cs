using Edulytics.Core.AdaptiveAssessment;
using Edulytics.Core.Analytics;
using Edulytics.Core.AssessmentIntelligence;
using Edulytics.Core.Enums;

namespace Edulytics.Services.AdaptiveAssessment;

public sealed class AdaptiveDiagnosticAssessmentEngine
{
    public const string EngineVersion = "phase35-v1";
    private const decimal DiagnosticConfidenceThreshold = 45m;
    private const int DiagnosticEvidenceThreshold = 2;

    public AdaptiveAssessmentDecision DecideNext(AdaptiveAssessmentRequest request)
    {
        Validate(request);

        var selected = request.LearningOutcomeIds.Distinct().ToArray();
        var profileRows = request.StudentProfile?.Outcomes
            .Where(x => selected.Contains(x.LearningOutcomeId))
            .ToDictionary(x => x.LearningOutcomeId)
            ?? new Dictionary<Guid, StudentOutcomeLearningProfile>();

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
                x.ScorePercentage is < 0m or > 100m))
        {
            throw new InvalidOperationException("Adaptive response history is invalid or outside Outcome scope.");
        }

        if (request.PreviousResponses.Select(x => x.Sequence).Distinct().Count() != request.PreviousResponses.Count)
            throw new InvalidOperationException("Adaptive response sequence must be unique.");
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
