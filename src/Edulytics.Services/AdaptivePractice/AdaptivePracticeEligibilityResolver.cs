using Edulytics.Core.AdaptivePractice;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Services.AdaptivePractice;

/// <summary>
/// Fail-closed product gate for Adaptive Practice V2.
///
/// This resolver never changes existing Practice behaviour. Callers may route
/// to V2 only when this method returns IsLearnerFacing=true. Shadow mode may
/// compute decisions but must leave current Practice V1 authoritative.
/// </summary>
public sealed class AdaptivePracticeEligibilityResolver(
    AdaptivePracticeV2Policy policy)
    : IAdaptivePracticeEligibilityResolver
{
    public AdaptivePracticeEligibilityDecision Resolve(
        AdaptivePracticeEligibilityRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (!policy.Enabled)
            return Reject(request, AdaptivePracticeEligibilityReasonCodes.FeatureDisabled);

        if (policy.Mode == AdaptivePracticeV2Mode.Off)
            return Reject(request, AdaptivePracticeEligibilityReasonCodes.ModeOff);

        if (request.SchoolId == Guid.Empty ||
            !request.IsMathematics ||
            string.IsNullOrWhiteSpace(request.CurriculumLevelKey) ||
            string.IsNullOrWhiteSpace(request.LessonCode))
        {
            return Reject(request, AdaptivePracticeEligibilityReasonCodes.InvalidScope);
        }

        if (!policy.AllowsSchool(request.SchoolId))
            return Reject(request, AdaptivePracticeEligibilityReasonCodes.SchoolNotAllowed);

        var level = CurriculumLevelIdentityRegistry.Find(
            request.CurriculumLevelKey);

        if (level is null)
            return Reject(request, AdaptivePracticeEligibilityReasonCodes.CurriculumLevelUnknown);

        // Preserve the reviewed Primary rollout unless the explicit
        // full-catalogue READY_VERIFIED policy is enabled. Full-catalogue
        // routing still fails closed on unknown levels and on lessons without
        // a READY_VERIFIED Practice contract below.
        if (!policy.RouteAllReadyVerifiedCatalogue &&
            level.LogicalLevel is < 1 or > 6)
        {
            return Reject(
                request,
                AdaptivePracticeEligibilityReasonCodes.OutsidePrimaryRollout,
                level);
        }

        if (!policy.AllowsCurriculumLevel(level.Key))
        {
            return Reject(
                request,
                AdaptivePracticeEligibilityReasonCodes.CurriculumLevelNotAllowed,
                level);
        }

        if (!policy.AllowsLesson(request.LessonCode))
        {
            return Reject(
                request,
                AdaptivePracticeEligibilityReasonCodes.LessonNotAllowed,
                level);
        }

        if (!LessonPracticeCapabilityResolver.TryResolve(
                request.LessonCode,
                out var contract) ||
            contract is null)
        {
            return Reject(
                request,
                AdaptivePracticeEligibilityReasonCodes.PracticeCapabilityMissing,
                level);
        }

        if (!string.Equals(
                contract.Readiness,
                LessonPracticeCapabilityResolver.ReadyVerified,
                StringComparison.Ordinal))
        {
            return Reject(
                request,
                AdaptivePracticeEligibilityReasonCodes.PracticeCapabilityNotReady,
                level);
        }

        return new AdaptivePracticeEligibilityDecision(
            true,
            policy.Mode,
            AdaptivePracticeEligibilityReasonCodes.Eligible,
            level.Key,
            level.LogicalLevel,
            contract.LessonCode,
            contract.SkillId,
            contract.ContractVersion);
    }

    private AdaptivePracticeEligibilityDecision Reject(
        AdaptivePracticeEligibilityRequest request,
        string reasonCode,
        CurriculumLevelIdentity? level = null) =>
        new(
            false,
            policy.Mode,
            reasonCode,
            level?.Key ?? request.CurriculumLevelKey,
            level?.LogicalLevel,
            request.LessonCode);
}
