using Edulytics.Core.Entities;
using Edulytics.Core.MathematicsGeneration;
using Edulytics.Services.MathematicsGeneration;

namespace Edulytics.Services.Assessments;

/// <summary>
/// Compatibility facade used by assessment, curriculum and private-practice
/// flows. Capability is resolved through canonical skills and a provider,
/// never by teaching the generator about curriculum-specific codes.
/// </summary>
public static class NativeMathematicsOutcomeProfileResolver
{
    private static readonly IMathematicsGenerationCapabilityProvider Provider =
        new NativeMathematicsGenerationCapabilityProvider();

    public static MathematicsOutcomeGenerationProfile? Resolve(LearningOutcome outcome)
    {
        ArgumentNullException.ThrowIfNull(outcome);

        var skills = CanonicalMathematicsSkillMapper.Resolve(
            outcome.Code,
            outcome.Description);
        var families = Provider.ResolveFamilies(skills);

        return families.Count == 0
            ? null
            : new MathematicsOutcomeGenerationProfile(
                outcome.Id,
                outcome.Code,
                families)
            {
                CanonicalSkills = skills
            };
    }

    public static bool Supports(string? code, string? description)
    {
        var skills = CanonicalMathematicsSkillMapper.Resolve(code, description);
        return Provider.ResolveFamilies(skills).Count > 0;
    }
}
