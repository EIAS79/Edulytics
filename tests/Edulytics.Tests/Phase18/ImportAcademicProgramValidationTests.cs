using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Data.Contexts;
using Edulytics.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Tests.Phase18;

public sealed class ImportAcademicProgramValidationTests
{
    [Fact]
    public async Task Snapshot_ExcludesCurriculumAdoption_WhenAcademicProgramIsMissing()
    {
        var schoolId = Guid.NewGuid();
        await using var db = CreateContext();
        db.SchoolCurriculumAdoptions.Add(Adoption(schoolId, Guid.NewGuid()));
        await db.SaveChangesAsync();

        var snapshot = await new ImportRepository(db).GetSnapshotAsync(schoolId);

        Assert.Empty(snapshot.CurriculumAdoptions);
    }

    [Fact]
    public async Task Snapshot_ExcludesCurriculumAdoption_WhenAcademicProgramIsInactive()
    {
        var schoolId = Guid.NewGuid();
        var program = Program(schoolId, AcademicStructureStatus.Inactive);
        await using var db = CreateContext();
        db.AcademicPrograms.Add(program);
        db.SchoolCurriculumAdoptions.Add(Adoption(schoolId, program.Id));
        await db.SaveChangesAsync();

        var snapshot = await new ImportRepository(db).GetSnapshotAsync(schoolId);

        Assert.Empty(snapshot.CurriculumAdoptions);
    }

    [Fact]
    public async Task Snapshot_IncludesCurriculumAdoption_WhenAcademicProgramIsActiveInSameSchool()
    {
        var schoolId = Guid.NewGuid();
        var program = Program(schoolId, AcademicStructureStatus.Active);
        var adoption = Adoption(schoolId, program.Id);
        await using var db = CreateContext();
        db.AcademicPrograms.Add(program);
        db.SchoolCurriculumAdoptions.Add(adoption);
        await db.SaveChangesAsync();

        var snapshot = await new ImportRepository(db).GetSnapshotAsync(schoolId);

        var actual = Assert.Single(snapshot.CurriculumAdoptions);
        Assert.Equal(adoption.Id, actual.Id);
        Assert.Equal(program.Id, actual.AcademicProgramId);
    }

    [Fact]
    public async Task Snapshot_ExcludesCurriculumAdoption_WhenProgramBelongsToAnotherSchool()
    {
        var schoolId = Guid.NewGuid();
        var program = Program(Guid.NewGuid(), AcademicStructureStatus.Active);
        await using var db = CreateContext();
        db.AcademicPrograms.Add(program);
        db.SchoolCurriculumAdoptions.Add(Adoption(schoolId, program.Id));
        await db.SaveChangesAsync();

        var snapshot = await new ImportRepository(db).GetSnapshotAsync(schoolId);

        Assert.Empty(snapshot.CurriculumAdoptions);
    }

    private static EdulyticsDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<EdulyticsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .Options;
        return new EdulyticsDbContext(options);
    }

    private static AcademicProgram Program(Guid schoolId, AcademicStructureStatus status) =>
        new()
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            Name = "Mathematics",
            Code = "MATH",
            NormalizedCode = "MATH",
            Status = status,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };

    private static SchoolCurriculumAdoption Adoption(Guid schoolId, Guid programId) =>
        new()
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            AcademicYearId = Guid.NewGuid(),
            AcademicProgramId = programId,
            GradeLevelId = Guid.NewGuid(),
            SubjectId = Guid.NewGuid(),
            FrameworkVersionId = Guid.NewGuid(),
            IsPrimary = true,
            IsActive = true,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };
}
