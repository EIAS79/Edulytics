namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Independent lesson-alignment gate. Mathematical verification proves that an
/// item is correct; this validator proves that the item is authorized for the
/// exact lesson contract. The two checks intentionally remain separate.
/// </summary>
public static class LessonPracticeAlignmentValidator
{
    public static LessonPracticeAlignmentDecision Validate(
        LessonPracticeContract contract,
        string? skillId,
        string? questionFamily)
    {
        ArgumentNullException.ThrowIfNull(contract);

        var reasons = new List<string>();
        if (!string.Equals(contract.Readiness, "READY_VERIFIED", StringComparison.Ordinal))
            reasons.Add("LessonPracticeContract is not READY_VERIFIED.");

        if (string.IsNullOrWhiteSpace(skillId) ||
            !contract.PrimarySkillIds.Contains(skillId.Trim(), StringComparer.Ordinal))
        {
            reasons.Add("Generated item does not target an authorized Primary Skill.");
        }

        if (string.IsNullOrWhiteSpace(questionFamily) ||
            !contract.AllowedQuestionFamilies.Contains(questionFamily.Trim(), StringComparer.Ordinal))
        {
            reasons.Add("Question family is not explicitly allowed by the lesson contract.");
        }

        if (!string.IsNullOrWhiteSpace(questionFamily) &&
            contract.ForbiddenQuestionFamilies.Contains(questionFamily.Trim(), StringComparer.Ordinal))
        {
            reasons.Add("Question family is explicitly forbidden by the lesson contract.");
        }

        if (contract.PrimaryCoverage.Count == 0 ||
            contract.PrimaryCoverage.Values.Any(value => value <= 0) ||
            contract.PrimaryCoverage.Values.Sum() != 100)
        {
            reasons.Add("Primary-skill coverage must contain positive percentages totalling 100.");
        }

        foreach (var primary in contract.PrimarySkillIds)
        {
            if (!contract.PrimaryCoverage.ContainsKey(primary))
                reasons.Add($"Primary Skill {primary} has no coverage allocation.");
        }

        return new LessonPracticeAlignmentDecision(
            reasons.Count == 0,
            reasons);
    }
}

public sealed record LessonPracticeAlignmentDecision(
    bool IsAligned,
    IReadOnlyList<string> Reasons);
