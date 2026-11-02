namespace Edulytics.Core.Mathematics.Practice;

public sealed record PracticeAssessmentBlueprintItem(
    int CandidateOrder,
    string Family,
    PracticeQuestionForm Form,
    PracticeCognitiveOperation CognitiveOperation,
    PracticeCognitiveDifficulty PlannedDifficulty,
    int PreferredVariantSlot);

public sealed record PracticeAssessmentBlueprint(
    int RequestedQuestionCount,
    IReadOnlyList<PracticeAssessmentBlueprintItem> Candidates)
{
    public IReadOnlySet<string> Families =>
        Candidates
            .Select(candidate => candidate.Family)
            .ToHashSet(StringComparer.Ordinal);

    public IReadOnlySet<PracticeQuestionForm> Forms =>
        Candidates
            .Select(candidate => candidate.Form)
            .ToHashSet();

    public IReadOnlySet<PracticeCognitiveOperation> CognitiveOperations =>
        Candidates
            .Select(candidate => candidate.CognitiveOperation)
            .ToHashSet();
}

/// <summary>
/// Builds a deterministic candidate plan before any question is generated.
/// The plan intentionally contains more candidates than the requested session:
/// generation may reject a candidate for historical exposure, exact-pool
/// exhaustion or semantic duplication without falling back to wording padding.
/// </summary>
public sealed class PracticeAssessmentComposer
{
    public PracticeAssessmentBlueprint Compose(
        IReadOnlyList<string> allowedQuestionFamilies,
        int requestedQuestionCount,
        PracticeCognitiveDifficulty requestedDifficulty)
    {
        ArgumentNullException.ThrowIfNull(allowedQuestionFamilies);

        if (requestedQuestionCount is < 1 or > 30)
            throw new InvalidOperationException(
                "Practice assessment composition requires 1-30 requested questions.");

        var families = allowedQuestionFamilies
            .Where(family => !string.IsNullOrWhiteSpace(family))
            .Select(family => family.Trim())
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        if (families.Length == 0)
            throw new InvalidOperationException(
                "Practice assessment composition requires at least one allowed family.");

        var familyCandidates = families
            .Select(family => BuildFamilyCandidates(
                family,
                requestedDifficulty))
            .Where(candidates => candidates.Count > 0)
            .ToArray();

        if (familyCandidates.Length == 0)
            throw new InvalidOperationException(
                "No allowed Practice family exposes a question-form capability for the requested difficulty.");

        // Interleave families after each family has already been internally
        // interleaved by question form. This prevents the requested count from
        // causing one family/form to dominate merely because it was listed first.
        var candidates = new List<PracticeAssessmentBlueprintItem>();
        var maxDepth = familyCandidates.Max(c => c.Count);

        for (var depth = 0; depth < maxDepth; depth++)
        {
            foreach (var queue in familyCandidates)
            {
                if (depth >= queue.Count)
                    continue;

                var item = queue[depth];
                candidates.Add(item with
                {
                    CandidateOrder = candidates.Count + 1
                });
            }
        }

        return new PracticeAssessmentBlueprint(
            requestedQuestionCount,
            candidates);
    }

    private static IReadOnlyList<PracticeAssessmentBlueprintItem>
        BuildFamilyCandidates(
            string family,
            PracticeCognitiveDifficulty requestedDifficulty)
    {
        var capabilities =
            PracticeQuestionFormCapabilityRegistry.Resolve(family)
                .Where(capability =>
                    requestedDifficulty >= capability.MinimumDifficulty &&
                    requestedDifficulty <= capability.MaximumDifficulty)
                .ToArray();

        if (capabilities.Length == 0)
            return [];

        // Each capability owns explicit variant slots. Walk capabilities in
        // round-robin order so richer forms appear early in the candidate plan:
        // form A slot 1, form B slot 1, ... then form A slot 2, etc.
        var orderedSlots = capabilities
            .Select(capability => new
            {
                Capability = capability,
                Slots = capability.VariantSlots
                    .Distinct()
                    .OrderBy(slot => slot)
                    .ToArray()
            })
            .ToArray();

        var maxSlots = orderedSlots.Max(x => x.Slots.Length);
        var result = new List<PracticeAssessmentBlueprintItem>();

        for (var depth = 0; depth < maxSlots; depth++)
        {
            foreach (var entry in orderedSlots)
            {
                if (depth >= entry.Slots.Length)
                    continue;

                var capability = entry.Capability;
                result.Add(new PracticeAssessmentBlueprintItem(
                    0,
                    family,
                    capability.Form,
                    capability.CognitiveOperation,
                    requestedDifficulty,
                    entry.Slots[depth]));
            }
        }

        return result;
    }

}
