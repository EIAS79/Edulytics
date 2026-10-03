namespace Edulytics.Services.Entitlements;

public interface IStudentEntitlementService
{
    Task<StudentEntitlementSnapshot> GetAsync(
        Guid studentUserId,
        DateTime? asOfUtc = null,
        CancellationToken cancellationToken = default);
}
