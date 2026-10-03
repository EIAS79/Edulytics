using Edulytics.Core.DirectStudents;
using Edulytics.Core.Entities;

namespace Edulytics.Core.Interfaces;

public interface IDirectStudentAccountRepository
{
    Task<bool> ExistsAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<DirectStudentProfile?> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<DirectStudentRegistrationResult> RegisterAsync(
        DirectStudentRegistrationRequest request,
        CancellationToken cancellationToken = default);
}
