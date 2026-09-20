namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Single authoritative capability decision for lesson-scoped Practice.
/// A lesson is available for Practice only when the runtime registry resolves
/// an explicitly READY_VERIFIED contract. Game routing is a presentation layer
/// and must never independently authorize Practice.
/// </summary>
public static class LessonPracticeCapabilityResolver
{
    public const string ReadyVerified = "READY_VERIFIED";

    public static bool TryResolve(
        string? lessonCode,
        out LessonPracticeContract? contract)
    {
        if (!LessonPracticeContractRegistry.TryResolve(
                lessonCode,
                out contract) ||
            contract is null ||
            !string.Equals(
                contract.Readiness,
                ReadyVerified,
                StringComparison.Ordinal))
        {
            contract = null;
            return false;
        }

        return true;
    }
}
