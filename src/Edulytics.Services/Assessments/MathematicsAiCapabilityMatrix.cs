using Edulytics.Core.MathematicsGeneration;
using Edulytics.Services.MathematicsGeneration;

namespace Edulytics.Services.Assessments;

public enum MathematicsAiCapabilityLevel
{
    ManualOnly = 0,
    AiAssisted = 1,
    VerifiedAi = 2
}

/// <summary>
/// One curriculum-neutral capability decision for a Mathematics learning outcome.
/// Verified AI means a reviewed provider can generate and validate the requested
/// canonical skills. AI-assisted is intentionally reserved for a configured
/// contextual draft provider; Edulytics must never emit that state merely because
/// an LLM could theoretically answer the topic. Manual-only is fail-closed.
/// </summary>
public sealed record MathematicsAiCapability(
    MathematicsAiCapabilityLevel Level,
    IReadOnlyList<CanonicalMathematicsSkill> CanonicalSkills,
    IReadOnlyList<MathematicsGeneratorFamily> VerifiedFamilies,
    string? ProviderKey,
    string ReasonCode)
{
    public bool CanGenerateVerified =>
        Level == MathematicsAiCapabilityLevel.VerifiedAi &&
        VerifiedFamilies.Count > 0 &&
        !string.IsNullOrWhiteSpace(ProviderKey);
}

/// <summary>
/// Canonical source of truth for Mathematics AI capability classification.
/// UI, assessment generation and student private practice must derive their
/// availability from this matrix instead of maintaining separate curriculum-code
/// allowlists. No AI-assisted provider is registered today, therefore this matrix
/// only emits VerifiedAi or ManualOnly until a real reviewed provider is added.
/// </summary>
public static class MathematicsAiCapabilityMatrix
{
    private static readonly IMathematicsGenerationCapabilityProvider VerifiedProvider =
        new NativeMathematicsGenerationCapabilityProvider();

    public static MathematicsAiCapability Resolve(
        string? outcomeCode,
        string? description)
    {
        var skills = CanonicalMathematicsSkillMapper.Resolve(
            outcomeCode,
            description);

        if (skills.Count == 0)
        {
            return new MathematicsAiCapability(
                MathematicsAiCapabilityLevel.ManualOnly,
                skills,
                [],
                null,
                "NoCanonicalSkillMapping");
        }

        var families = VerifiedProvider.ResolveFamilies(skills);
        if (families.Count > 0)
        {
            return new MathematicsAiCapability(
                MathematicsAiCapabilityLevel.VerifiedAi,
                skills,
                families,
                VerifiedProvider.ProviderKey,
                "ReviewedNativeProvider");
        }

        return new MathematicsAiCapability(
            MathematicsAiCapabilityLevel.ManualOnly,
            skills,
            [],
            null,
            "NoConfiguredProviderForAllSkills");
    }
}
