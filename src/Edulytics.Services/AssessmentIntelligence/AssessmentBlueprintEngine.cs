using Edulytics.Core.AssessmentIntelligence;
using Edulytics.Core.Analytics;
using Edulytics.Core.Enums;

namespace Edulytics.Services.AssessmentIntelligence;

public sealed class AssessmentBlueprintEngine
{
    public const string FormulaVersion = "phase32-v1";

    public AssessmentBlueprint Build(AssessmentBlueprintRequest request)
    {
        Validate(request);

        var outcomeIds = DistinctOutcomes(request.LearningOutcomeIds);
        var priorities = BuildOutcomePriorities(
            outcomeIds,
            request.StudentProfile,
            request.Purpose);
        var outcomeAllocations = AllocateOutcomes(
            priorities,
            request.QuestionCount);
        var difficultyAllocations = AllocateDifficulties(
            request.QuestionCount,
            request.DifficultyPolicy);
        var familyAllocations = AllocateFamilies(
            request.QuestionCount,
            request.Purpose);
        var typeAllocations = AllocateItemTypes(
            request.QuestionCount,
            request.Purpose);
        var requiredEvidence = outcomeAllocations
            .Where(x => x.ItemCount > 0)
            .Select(x => new OutcomeEvidenceRequirement(
                x.LearningOutcomeId,
                x.ItemCount,
                RequiresScoredResponse: true,
                RequiresValidSolution: true))
            .ToArray();
        var excluded = (request.ExcludedExposureFingerprints ?? [])
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim())
            .Distinct(StringComparer.Ordinal)
            .OrderBy(x => x, StringComparer.Ordinal)
            .ToArray();

        return new AssessmentBlueprint(
            request.SchoolId,
            request.CurriculumAdoptionId,
            request.CurriculumLevelKey.Trim(),
            request.CurriculumTopicId,
            request.CurriculumPedagogicalLessonId,
            request.Purpose,
            request.QuestionCount,
            outcomeAllocations,
            difficultyAllocations,
            familyAllocations,
            typeAllocations,
            requiredEvidence,
            excluded,
            FormulaVersion);
    }

    private static void Validate(AssessmentBlueprintRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.SchoolId == Guid.Empty ||
            request.CurriculumAdoptionId == Guid.Empty ||
            string.IsNullOrWhiteSpace(request.CurriculumLevelKey))
        {
            throw new InvalidOperationException(
                "Assessment blueprint requires explicit school, curriculum adoption and curriculum level identity.");
        }

        if (request.QuestionCount is < 1 or > 100)
        {
            throw new InvalidOperationException(
                "Assessment blueprint question count must be between 1 and 100.");
        }

        if (request.LearningOutcomeIds is null ||
            request.LearningOutcomeIds.Count == 0 ||
            request.LearningOutcomeIds.Any(x => x == Guid.Empty))
        {
            throw new InvalidOperationException(
                "Assessment blueprint requires at least one explicit Learning Outcome.");
        }

        ValidateDifficultyPolicy(request.DifficultyPolicy);

        var profile = request.StudentProfile;
        if (profile is not null &&
            (profile.SchoolId != request.SchoolId ||
             profile.CurriculumAdoptionId != request.CurriculumAdoptionId))
        {
            throw new InvalidOperationException(
                "Student Learning Profile is outside the requested school or curriculum adoption scope.");
        }
    }

    private static void ValidateDifficultyPolicy(AssessmentDifficultyPolicy policy)
    {
        ArgumentNullException.ThrowIfNull(policy);

        if (policy.EasyPercent < 0 ||
            policy.MediumPercent < 0 ||
            policy.ChallengingPercent < 0 ||
            policy.EasyPercent > 100 ||
            policy.MediumPercent > 100 ||
            policy.ChallengingPercent > 100 ||
            policy.EasyPercent + policy.MediumPercent + policy.ChallengingPercent != 100)
        {
            throw new InvalidOperationException(
                "Assessment difficulty policy percentages must be within 0..100 and total 100.");
        }
    }

    private static Guid[] DistinctOutcomes(IReadOnlyList<Guid> outcomeIds)
    {
        var seen = new HashSet<Guid>();
        var result = new List<Guid>();

        foreach (var outcomeId in outcomeIds)
        {
            if (seen.Add(outcomeId))
                result.Add(outcomeId);
        }

        return result.ToArray();
    }

    private static OutcomePriority[] BuildOutcomePriorities(
        IReadOnlyList<Guid> outcomeIds,
        StudentLearningProfile? profile,
        AssessmentPurpose purpose)
    {
        var profileRows = (profile?.Outcomes ?? [])
            .GroupBy(x => x.LearningOutcomeId)
            .ToDictionary(x => x.Key, x => x.Single());

        return outcomeIds
            .Select((outcomeId, index) =>
            {
                if (!profileRows.TryGetValue(outcomeId, out var row))
                {
                    return new OutcomePriority(
                        outcomeId,
                        index,
                        purpose == AssessmentPurpose.Diagnostic ? 150m : 125m,
                        "NoEvidence");
                }

                var weakness = Math.Clamp(100m - row.MasteryPercentage, 0m, 100m);
                var uncertainty = Math.Clamp(100m - row.ConfidencePercentage, 0m, 100m);
                var score = weakness + uncertainty *
                    (purpose == AssessmentPurpose.Diagnostic ? 0.75m : 0.25m);

                if (purpose == AssessmentPurpose.EquivalentReassessment)
                    score += weakness * 0.25m;

                return new OutcomePriority(
                    outcomeId,
                    index,
                    Round2(score),
                    row.EvidenceCount == 0
                        ? "NoEvidence"
                        : row.MasteryPercentage < 60m
                            ? "WeakMastery"
                            : row.ConfidencePercentage < 60m
                                ? "LowConfidence"
                                : "Coverage");
            })
            .ToArray();
    }

    private static OutcomeBlueprintAllocation[] AllocateOutcomes(
        IReadOnlyList<OutcomePriority> priorities,
        int questionCount)
    {
        var counts = priorities.ToDictionary(x => x.LearningOutcomeId, _ => 0);

        if (questionCount >= priorities.Count)
        {
            foreach (var priority in priorities)
                counts[priority.LearningOutcomeId] = 1;
        }
        else
        {
            foreach (var priority in priorities
                         .OrderByDescending(x => x.Score)
                         .ThenBy(x => x.RequestOrder)
                         .Take(questionCount))
            {
                counts[priority.LearningOutcomeId] = 1;
            }
        }

        var assigned = counts.Values.Sum();
        while (assigned < questionCount)
        {
            var next = priorities
                .OrderByDescending(x =>
                    x.Score / (counts[x.LearningOutcomeId] + 1m))
                .ThenBy(x => x.RequestOrder)
                .First();

            counts[next.LearningOutcomeId]++;
            assigned++;
        }

        return priorities
            .OrderBy(x => x.RequestOrder)
            .Select(x => new OutcomeBlueprintAllocation(
                x.LearningOutcomeId,
                counts[x.LearningOutcomeId],
                x.Score,
                x.Reason))
            .ToArray();
    }

    private static DifficultyBlueprintAllocation[] AllocateDifficulties(
        int questionCount,
        AssessmentDifficultyPolicy policy)
    {
        var allocations = AllocatePercentages(
            questionCount,
            [
                new WeightedBucket<AssessmentItemDifficulty>(AssessmentItemDifficulty.Easy, policy.EasyPercent, 0),
                new WeightedBucket<AssessmentItemDifficulty>(AssessmentItemDifficulty.Medium, policy.MediumPercent, 1),
                new WeightedBucket<AssessmentItemDifficulty>(AssessmentItemDifficulty.Challenging, policy.ChallengingPercent, 2)
            ]);

        return allocations
            .Select(x => new DifficultyBlueprintAllocation(x.Key, x.Count))
            .ToArray();
    }

    private static QuestionFamilyBlueprintAllocation[] AllocateFamilies(
        int questionCount,
        AssessmentPurpose purpose)
    {
        var percentages = purpose switch
        {
            AssessmentPurpose.Practice => new[] { 40, 35, 20, 5 },
            AssessmentPurpose.TeacherAssessment => new[] { 25, 30, 25, 20 },
            AssessmentPurpose.StudentPersonalTest => new[] { 30, 30, 25, 15 },
            AssessmentPurpose.Diagnostic => new[] { 35, 35, 20, 10 },
            AssessmentPurpose.EquivalentReassessment => new[] { 25, 35, 25, 15 },
            _ => throw new InvalidOperationException("Unsupported assessment purpose.")
        };

        var allocations = AllocatePercentages(
            questionCount,
            [
                new WeightedBucket<AssessmentQuestionFamily>(AssessmentQuestionFamily.DirectComputation, percentages[0], 0),
                new WeightedBucket<AssessmentQuestionFamily>(AssessmentQuestionFamily.StructuredMethod, percentages[1], 1),
                new WeightedBucket<AssessmentQuestionFamily>(AssessmentQuestionFamily.AppliedProblem, percentages[2], 2),
                new WeightedBucket<AssessmentQuestionFamily>(AssessmentQuestionFamily.MathematicalReasoning, percentages[3], 3)
            ]);

        return allocations
            .Select(x => new QuestionFamilyBlueprintAllocation(x.Key, x.Count))
            .ToArray();
    }

    private static ItemTypeBlueprintAllocation[] AllocateItemTypes(
        int questionCount,
        AssessmentPurpose purpose)
    {
        var percentages = purpose switch
        {
            AssessmentPurpose.Practice => new[] { 45, 35, 20 },
            AssessmentPurpose.TeacherAssessment => new[] { 40, 35, 25 },
            AssessmentPurpose.StudentPersonalTest => new[] { 35, 30, 35 },
            AssessmentPurpose.Diagnostic => new[] { 30, 25, 45 },
            AssessmentPurpose.EquivalentReassessment => new[] { 40, 35, 25 },
            _ => throw new InvalidOperationException("Unsupported assessment purpose.")
        };

        var allocations = AllocatePercentages(
            questionCount,
            [
                new WeightedBucket<AssessmentItemType>(AssessmentItemType.Numeric, percentages[0], 0),
                new WeightedBucket<AssessmentItemType>(AssessmentItemType.ShortAnswer, percentages[1], 1),
                new WeightedBucket<AssessmentItemType>(AssessmentItemType.MultipleChoice, percentages[2], 2)
            ]);

        return allocations
            .Select(x => new ItemTypeBlueprintAllocation(x.Key, x.Count))
            .ToArray();
    }

    private static Allocation<T>[] AllocatePercentages<T>(
        int total,
        IReadOnlyList<WeightedBucket<T>> buckets)
        where T : struct, Enum
    {
        var rows = buckets
            .Select(x =>
            {
                var exact = total * x.Percent / 100m;
                var floor = (int)decimal.Floor(exact);
                return new AllocationRemainder<T>(
                    x.Key,
                    floor,
                    exact - floor,
                    x.Order);
            })
            .ToArray();

        var remaining = total - rows.Sum(x => x.Count);
        foreach (var row in rows
                     .OrderByDescending(x => x.Remainder)
                     .ThenBy(x => x.Order)
                     .Take(remaining))
        {
            row.Count++;
        }

        return rows
            .OrderBy(x => x.Order)
            .Select(x => new Allocation<T>(x.Key, x.Count))
            .ToArray();
    }

    private static decimal Round2(decimal value) =>
        decimal.Round(value, 2, MidpointRounding.AwayFromZero);

    private sealed record OutcomePriority(
        Guid LearningOutcomeId,
        int RequestOrder,
        decimal Score,
        string Reason);

    private sealed record WeightedBucket<T>(T Key, int Percent, int Order)
        where T : struct, Enum;

    private sealed record Allocation<T>(T Key, int Count)
        where T : struct, Enum;

    private sealed class AllocationRemainder<T>(
        T key,
        int count,
        decimal remainder,
        int order)
        where T : struct, Enum
    {
        public T Key { get; } = key;
        public int Count { get; set; } = count;
        public decimal Remainder { get; } = remainder;
        public int Order { get; } = order;
    }
}
