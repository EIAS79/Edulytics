namespace Edulytics.Core.Mathematics.Verification;

public enum MathematicsVerificationStatus
{
    Verified = 1,
    Rejected = 2,
    Unsupported = 3,
    Inconclusive = 4
}

public sealed record MathematicsVerificationEvidence(
    string Method,
    string Description);

public sealed record MathematicsVerificationResult(
    MathematicsVerificationStatus Status,
    IReadOnlyList<MathematicsVerificationEvidence> Evidence,
    IReadOnlyList<string> Diagnostics)
{
    public bool IsVerified => Status == MathematicsVerificationStatus.Verified;
}
