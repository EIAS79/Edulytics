using System.Text.RegularExpressions;
using Edulytics.Core.Entities;
using Edulytics.Core.MathematicsGeneration;

namespace Edulytics.Services.Assessments;

/// <summary>
/// Compatibility facade used by assessment, curriculum and private-practice
/// flows. Capability is resolved through the canonical AI capability matrix,
/// never by teaching the generator about curriculum-specific codes. Any
/// partially unsupported outcome fails closed.
/// </summary>
public static class NativeMathematicsOutcomeProfileResolver
{
    public static MathematicsOutcomeGenerationProfile? Resolve(LearningOutcome outcome)
    {
        ArgumentNullException.ThrowIfNull(outcome);

        var semanticContext = string.IsNullOrWhiteSpace(outcome.GenerationSemanticHint)
            ? outcome.Description
            : $"{outcome.Description} {outcome.GenerationSemanticHint}";
        var capability = MathematicsAiCapabilityMatrix.Resolve(
            outcome.Code,
            semanticContext);

        return capability.CanGenerateVerified
            ? new MathematicsOutcomeGenerationProfile(
                outcome.Id,
                outcome.Code,
                capability.VerifiedFamilies)
            {
                CanonicalSkills = capability.CanonicalSkills,
                IntegerComputationMaximum = ResolveIntegerComputationMaximum(
                    semanticContext,
                    capability.CanonicalSkills)
            }
            : null;
    }

    public static bool Supports(LearningOutcome outcome) =>
        Resolve(outcome) is not null;

    public static bool Supports(string? code, string? description) =>
        MathematicsAiCapabilityMatrix.Resolve(code, description).CanGenerateVerified;

    private static int? ResolveIntegerComputationMaximum(
        string semanticContext,
        IReadOnlyList<CanonicalMathematicsSkill> skills)
    {
        if (!skills.Any(IsWholeNumberSkill))
            return null;

        var match = Regex.Match(
            semanticContext,
            @"\bWITHIN\s+([0-9]{1,6})\b",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        if (!match.Success ||
            !int.TryParse(match.Groups[1].Value, out var maximum) ||
            maximum < 1)
        {
            return null;
        }

        return maximum;
    }

    private static bool IsWholeNumberSkill(CanonicalMathematicsSkill skill) =>
        skill is CanonicalMathematicsSkill.WholeNumberAdditionAndSubtraction or
            CanonicalMathematicsSkill.WholeNumberAddition or
            CanonicalMathematicsSkill.WholeNumberSubtraction or
            CanonicalMathematicsSkill.WholeNumberMultiplication or
            CanonicalMathematicsSkill.WholeNumberDivision;
}
