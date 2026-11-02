using Edulytics.Core.Entities;

namespace Edulytics.Core.Interfaces;

public interface IPersonalEntitlementRepository
{
    Task<IReadOnlyList<PersonalEntitlement>> ListActiveByStudentAsync(
        Guid studentUserId,
        DateTime asOfUtc,
        CancellationToken cancellationToken = default);
}
