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

        var capability = MathematicsAiCapabilityMatrix.Resolve(
            outcome.Code,
            outcome.Description);

        return capability.CanGenerateVerified
            ? new MathematicsOutcomeGenerationProfile(
                outcome.Id,
                outcome.Code,
                capability.VerifiedFamilies)
            {
                CanonicalSkills = capability.CanonicalSkills
            }
            : null;
    }

    public static bool Supports(string? code, string? description) =>
        MathematicsAiCapabilityMatrix.Resolve(code, description).CanGenerateVerified;
}
