using Edulytics.Core.Constants;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;
using Edulytics.Core.Users;
using Edulytics.Data.Contexts;
using Edulytics.Data.Repositories;
using Edulytics.Services.Academics;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Tests.Phase39;

public sealed class StudentPlacementMoveTests
{
    [Fact]
    public async Task MoveStudentsAsync_InvalidSelection_DoesNotPartiallyMoveValidStudents()
    {
        await using var db = CreateDb();

        var school = NewSchool();
        var year = new AcademicYear
        {
            Id = Guid.NewGuid(),
            SchoolId = school.Id,
            Name = "2026/2027",
            StartsOn = new DateOnly(2026, 9, 1),
            EndsOn = new DateOnly(2027, 6, 30),
            Status = AcademicStructureStatus.Active,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };
        var grade = new GradeLevel
        {
            Id = Guid.NewGuid(),
            SchoolId = school.Id,
            Name = "Grade 6",
            Order = 6
        };
        var program = new AcademicProgram
        {
            Id = Guid.NewGuid(),
            SchoolId = school.Id,
            Name = "Cambridge",
            Code = "CAMBRIDGE",
            NormalizedCode = "CAMBRIDGE",
            Status = AcademicStructureStatus.Active,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };
        var adoptionId = Guid.NewGuid();
        var source = NewClass(
            school.Id,
            year.Id,
            grade.Id,
            program.Id,
            adoptionId,
            "6A");
        var target = NewClass(
            school.Id,
            year.Id,
            grade.Id,
            program.Id,
            adoptionId,
            "6B");
        var validStudent = NewStudent(school.Id, "ST-001", "Ada", "Brown");
        var staleStudent = NewStudent(school.Id, "ST-002", "Ben", "Clark");

        db.AddRange(
            school,
            year,
            grade,
            program,
            source,
            target,
            validStudent,
            staleStudent,
            new StudentEnrollment
            {
                Id = Guid.NewGuid(),
                SchoolId = school.Id,
                StudentProfileId = validStudent.Id,
                ClassGroupId = source.Id,
                AcademicYearId = year.Id,
                EnrolledAtUtc = DateTime.UtcNow
            },
            new StudentEnrollment
            {
                Id = Guid.NewGuid(),
                SchoolId = school.Id,
                StudentProfileId = staleStudent.Id,
                ClassGroupId = target.Id,
                AcademicYearId = year.Id,
                EnrolledAtUtc = DateTime.UtcNow
            });
        await db.SaveChangesAsync();

        var supervisor = new SchoolUserRecord(
            Guid.NewGuid(),
            school.Id,
            "supervisor@example.com",
            true,
            false,
            DateTime.UtcNow,
            DateTime.UtcNow,
            [RoleNames.SubjectSupervisor]);
        var users = new FakeUserRepository(supervisor);

        var service = new StudentPlacementService(
            new AcademicStructureRepository(db),
            new StudentPlacementRepository(db),
            new SchoolRepository(db),
            users);

        var result = await service.MoveStudentsAsync(
            supervisor.Id,
            source.Id,
            target.Id,
            [validStudent.Id, staleStudent.Id]);

        Assert.False(result.Succeeded);
        Assert.Equal(0, result.Moved);
        Assert.Contains(
            result.Failures,
            x => x.StudentProfileId == staleStudent.Id &&
                 x.Code == "StudentNotInSourceClass");

        var validEnrollment = await db.StudentEnrollments
            .AsNoTracking()
            .SingleAsync(x => x.StudentProfileId == validStudent.Id);

        Assert.Equal(source.Id, validEnrollment.ClassGroupId);
    }

    [Fact]
    public async Task MoveStudentsAsync_ValidReviewedSelection_MovesEverySelectedStudent()
    {
        await using var db = CreateDb();

        var school = NewSchool();
        var year = new AcademicYear
        {
            Id = Guid.NewGuid(),
            SchoolId = school.Id,
            Name = "2026/2027",
            StartsOn = new DateOnly(2026, 9, 1),
            EndsOn = new DateOnly(2027, 6, 30),
            Status = AcademicStructureStatus.Active,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };
        var grade = new GradeLevel
        {
            Id = Guid.NewGuid(),
            SchoolId = school.Id,
            Name = "Grade 6",
            Order = 6
        };
        var program = new AcademicProgram
        {
            Id = Guid.NewGuid(),
            SchoolId = school.Id,
            Name = "Cambridge",
            Code = "CAMBRIDGE",
            NormalizedCode = "CAMBRIDGE",
            Status = AcademicStructureStatus.Active,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };
        var adoptionId = Guid.NewGuid();
        var source = NewClass(
            school.Id,
            year.Id,
            grade.Id,
            program.Id,
            adoptionId,
            "6A");
        var target = NewClass(
            school.Id,
            year.Id,
            grade.Id,
            program.Id,
            adoptionId,
            "6B");
        var first = NewStudent(school.Id, "ST-011", "Cara", "Davis");
        var second = NewStudent(school.Id, "ST-012", "Drew", "Evans");

        db.AddRange(school, year, grade, program, source, target, first, second);
        db.StudentEnrollments.AddRange(
            NewEnrollment(school.Id, year.Id, source.Id, first.Id),
            NewEnrollment(school.Id, year.Id, source.Id, second.Id));
        await db.SaveChangesAsync();

        var supervisor = new SchoolUserRecord(
            Guid.NewGuid(),
            school.Id,
            "supervisor@example.com",
            true,
            false,
            DateTime.UtcNow,
            DateTime.UtcNow,
            [RoleNames.SubjectSupervisor]);

        var service = new StudentPlacementService(
            new AcademicStructureRepository(db),
            new StudentPlacementRepository(db),
            new SchoolRepository(db),
            new FakeUserRepository(supervisor));

        var result = await service.MoveStudentsAsync(
            supervisor.Id,
            source.Id,
            target.Id,
            [first.Id, second.Id]);

        Assert.True(result.Succeeded);
        Assert.Equal(2, result.Moved);
        Assert.Empty(result.Failures);

        var movedClassIds = await db.StudentEnrollments
            .AsNoTracking()
            .OrderBy(x => x.StudentProfileId)
            .Select(x => x.ClassGroupId)
            .ToArrayAsync();

        Assert.Equal([target.Id, target.Id], movedClassIds);
    }

    private static EdulyticsDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<EdulyticsDbContext>()
            .UseInMemoryDatabase($"student-move-{Guid.NewGuid():N}")
            .Options;
        return new EdulyticsDbContext(options);
    }

    private static School NewSchool() => new()
    {
        Id = Guid.NewGuid(),
        Name = "Move Test School",
        SchoolCode = "MOVE-TEST",
        NormalizedSchoolCode = "MOVE-TEST",
        Status = SchoolStatus.Active,
        CountryCode = "PL",
        City = "Warsaw",
        ContactEmail = "school@example.com",
        DefaultCulture = "en",
        TimeZoneId = "Europe/Warsaw",
        CreatedAtUtc = DateTime.UtcNow,
        UpdatedAtUtc = DateTime.UtcNow
    };

    private static ClassGroup NewClass(
        Guid schoolId,
        Guid yearId,
        Guid gradeId,
        Guid programId,
        Guid adoptionId,
        string name) => new()
    {
        Id = Guid.NewGuid(),
        SchoolId = schoolId,
        AcademicYearId = yearId,
        GradeLevelId = gradeId,
        AcademicProgramId = programId,
        CurriculumAdoptionId = adoptionId,
        Name = name,
        NormalizedName = name.ToUpperInvariant(),
        Code = $"CLS-{Guid.NewGuid():N}",
        NormalizedCode = $"CLS-{Guid.NewGuid():N}".ToUpperInvariant(),
        Status = AcademicStructureStatus.Active
    };

    private static StudentProfile NewStudent(
        Guid schoolId,
        string number,
        string firstName,
        string lastName) => new()
    {
        Id = Guid.NewGuid(),
        SchoolId = schoolId,
        StudentNumber = number,
        NormalizedStudentNumber = number.ToUpperInvariant(),
        FirstName = firstName,
        LastName = lastName,
        DisplayName = $"{firstName} {lastName}",
        Status = AcademicStructureStatus.Active,
        CreatedAtUtc = DateTime.UtcNow,
        UpdatedAtUtc = DateTime.UtcNow
    };

    private static StudentEnrollment NewEnrollment(
        Guid schoolId,
        Guid yearId,
        Guid classId,
        Guid studentId) => new()
    {
        Id = Guid.NewGuid(),
        SchoolId = schoolId,
        AcademicYearId = yearId,
        ClassGroupId = classId,
        StudentProfileId = studentId,
        EnrolledAtUtc = DateTime.UtcNow
    };

    private sealed class FakeUserRepository : ISchoolUserRepository
    {
        private readonly SchoolUserRecord _actor;

        public FakeUserRepository(SchoolUserRecord actor)
        {
            _actor = actor;
        }

        public Task<SchoolUserRecord?> GetActorAsync(
            Guid userId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<SchoolUserRecord?>(
                userId == _actor.Id ? _actor : null);

        public Task<IReadOnlyList<SchoolUserRecord>> ListBySchoolAsync(
            Guid schoolId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<SchoolUserRecord>>(
                _actor.SchoolId == schoolId ? [_actor] : []);

        public Task<SchoolUserRecord?> GetBySchoolAndIdAsync(
            Guid schoolId,
            Guid userId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult<SchoolUserRecord?>(
                _actor.SchoolId == schoolId && _actor.Id == userId
                    ? _actor
                    : null);

        public Task<SchoolUserPersistenceResult> CreateAsync(
            Guid schoolId,
            string email,
            string role,
            CancellationToken cancellationToken = default) =>
            Failure();

        public Task<SchoolUserPersistenceResult> SetActiveAsync(
            Guid schoolId,
            Guid userId,
            bool isActive,
            CancellationToken cancellationToken = default) =>
            Failure();

        public Task<SchoolUserPersistenceResult> SetLockedAsync(
            Guid schoolId,
            Guid userId,
            bool isLocked,
            CancellationToken cancellationToken = default) =>
            Failure();

        public Task<SchoolUserPersistenceResult> SetRoleAsync(
            Guid schoolId,
            Guid userId,
            string role,
            CancellationToken cancellationToken = default) =>
            Failure();

        public Task<SchoolUserPersistenceResult> GeneratePasswordSetupAsync(
            Guid schoolId,
            Guid userId,
            CancellationToken cancellationToken = default) =>
            Failure();

        public Task<SchoolUserPersistenceResult> CompletePasswordSetupAsync(
            Guid userId,
            string token,
            string newPassword,
            CancellationToken cancellationToken = default) =>
            Failure();

        private static Task<SchoolUserPersistenceResult> Failure() =>
            Task.FromResult(
                SchoolUserPersistenceResult.Failure(
                    SchoolUserPersistenceError.IdentityFailure));
    }
}
