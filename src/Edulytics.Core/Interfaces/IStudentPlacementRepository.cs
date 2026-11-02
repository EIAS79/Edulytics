using Edulytics.Core.Academics;
using Edulytics.Core.Entities;

namespace Edulytics.Core.Interfaces;

public interface IStudentPlacementRepository
{
    Task<StudentEnrollment?> GetEnrollmentAsync(
        Guid schoolId,
        Guid academicYearId,
        Guid studentProfileId,
        CancellationToken cancellationToken = default);

    Task AddEnrollmentAsync(
        StudentEnrollment enrollment,
        CancellationToken cancellationToken = default);

    Task<AcademicPersistenceResult> SaveAsync(
        CancellationToken cancellationToken = default);
}
