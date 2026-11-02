using Edulytics.Core.Academics;
using Edulytics.Core.Entities;
using Edulytics.Core.Interfaces;
using Edulytics.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Data.Repositories;

public sealed class StudentPlacementRepository : IStudentPlacementRepository
{
    private readonly EdulyticsDbContext _db;

    public StudentPlacementRepository(EdulyticsDbContext db)
    {
        _db = db;
    }

    public Task<StudentEnrollment?> GetEnrollmentAsync(
        Guid schoolId,
        Guid academicYearId,
        Guid studentProfileId,
        CancellationToken cancellationToken = default) =>
        _db.StudentEnrollments.SingleOrDefaultAsync(
            x =>
                x.SchoolId == schoolId &&
                x.AcademicYearId == academicYearId &&
                x.StudentProfileId == studentProfileId,
            cancellationToken);

    public Task AddEnrollmentAsync(
        StudentEnrollment enrollment,
        CancellationToken cancellationToken = default) =>
        _db.StudentEnrollments.AddAsync(enrollment, cancellationToken).AsTask();

    public async Task<AcademicPersistenceResult> SaveAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _db.SaveChangesAsync(cancellationToken);
            return AcademicPersistenceResult.Success();
        }
        catch (DbUpdateConcurrencyException)
        {
            return AcademicPersistenceResult.Failure(AcademicPersistenceError.Conflict);
        }
        catch (DbUpdateException)
        {
            return AcademicPersistenceResult.Failure(AcademicPersistenceError.Constraint);
        }
    }
}
