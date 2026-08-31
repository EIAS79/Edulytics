using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Data.Contexts;
using Edulytics.Data.Seeding;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Tests.Phase29;

public sealed class Phase29CurriculumLevelIdentityBackfillTests
{
    private static EdulyticsDbContext Db(string name) =>
        new(
            new DbContextOptionsBuilder<EdulyticsDbContext>()
                .UseInMemoryDatabase(name)
                .Options);

    [Fact]
    public async Task CommonCoreGrade7_BackfillsLogicalLevel8_AndDependentRows()
    {
        await using var db = Db($"phase29-backfill-cc-{Guid.NewGuid():N}");

        var schoolId = Guid.NewGuid();
        var programId = Guid.NewGuid();
        var yearId = Guid.NewGuid();
        var gradeId = Guid.NewGuid();
        var subjectId = Guid.NewGuid();
        var frameworkId = Guid.NewGuid();
        var versionId = Guid.NewGuid();
        var adoptionId = Guid.NewGuid();
        var classId = Guid.NewGuid();
        var topicId = Guid.NewGuid();

        db.GradeLevels.Add(new GradeLevel
        {
            Id = gradeId,
            SchoolId = schoolId,
            Name = "Grade 7",
            Order = 7
        });
        db.CurriculumFrameworks.Add(new CurriculumFramework
        {
            Id = frameworkId,
            Code = MathematicsCurriculumPackRegistry.CommonCoreCode,
            NormalizedCode = MathematicsCurriculumPackRegistry.CommonCoreCode,
            Name = "Common Core",
            IsActive = true
        });
        db.CurriculumFrameworkVersions.Add(new CurriculumFrameworkVersion
        {
            Id = versionId,
            FrameworkId = frameworkId,
            VersionCode = "V1",
            NormalizedVersionCode = "V1",
            Name = "V1",
            IsActive = true
        });
        db.SchoolCurriculumAdoptions.Add(new SchoolCurriculumAdoption
        {
            Id = adoptionId,
            SchoolId = schoolId,
            AcademicYearId = yearId,
            AcademicProgramId = programId,
            GradeLevelId = gradeId,
            SubjectId = subjectId,
            FrameworkVersionId = versionId,
            IsPrimary = true,
            IsActive = true,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        });
        db.ClassGroups.Add(new ClassGroup
        {
            Id = classId,
            SchoolId = schoolId,
            AcademicYearId = yearId,
            AcademicProgramId = programId,
            GradeLevelId = gradeId,
            Name = "A1",
            Code = "A1",
            NormalizedCode = "A1",
            Status = AcademicStructureStatus.Active
        });
        db.CurriculumTopics.Add(new CurriculumTopic
        {
            Id = topicId,
            SchoolId = schoolId,
            AcademicProgramId = programId,
            FrameworkVersionId = versionId,
            SubjectId = subjectId,
            GradeLevelId = gradeId,
            Name = "Ratios",
            Order = 1
        });
        db.LearningOutcomes.Add(new LearningOutcome
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            AcademicProgramId = programId,
            FrameworkVersionId = versionId,
            SubjectId = subjectId,
            GradeLevelId = gradeId,
            TopicId = topicId,
            Code = "7.RP.A.1",
            Description = "Legacy outcome",
            Weight = 1m,
            Order = 1
        });
        await db.SaveChangesAsync();

        var result = await new CurriculumLevelIdentityBackfill(db).RunAsync();

        var adoption = await db.SchoolCurriculumAdoptions.SingleAsync();
        Assert.Equal(8, adoption.CurriculumLogicalLevel);
        Assert.Equal("Grade 7", adoption.CurriculumLevelLabel);
        Assert.Equal(
            CurriculumLevelIdentityRegistry.BuildKey(
                MathematicsCurriculumPackRegistry.CommonCoreCode,
                8,
                pathway: null),
            adoption.CurriculumLevelKey);

        var classGroup = await db.ClassGroups.SingleAsync();
        Assert.Equal(adoptionId, classGroup.CurriculumAdoptionId);
        Assert.Equal("A1", classGroup.NormalizedName);

        var topic = await db.CurriculumTopics.SingleAsync();
        Assert.Equal(adoptionId, topic.CurriculumAdoptionId);

        var outcome = await db.LearningOutcomes.SingleAsync();
        Assert.Equal(adoptionId, outcome.CurriculumAdoptionId);

        Assert.Equal(1, result.AdoptionRowsResolved);
        Assert.Equal(1, result.ClassRowsResolved);
        Assert.Equal(1, result.TopicRowsResolved);
        Assert.Equal(1, result.OutcomeRowsResolved);
    }

    [Fact]
    public async Task CambridgeIgcseLegacyScope_RemainsUnresolved_WhenPathwayIsAmbiguous()
    {
        await using var db = Db($"phase29-backfill-cambridge-{Guid.NewGuid():N}");

        var schoolId = Guid.NewGuid();
        var frameworkId = Guid.NewGuid();
        var versionId = Guid.NewGuid();
        var gradeId = Guid.NewGuid();

        db.GradeLevels.Add(new GradeLevel
        {
            Id = gradeId,
            SchoolId = schoolId,
            Name = "Cambridge IGCSE Mathematics (0580)",
            Order = 10
        });
        db.CurriculumFrameworks.Add(new CurriculumFramework
        {
            Id = frameworkId,
            Code = MathematicsCurriculumPackRegistry.CambridgeCode,
            NormalizedCode = MathematicsCurriculumPackRegistry.CambridgeCode,
            Name = "Cambridge International Mathematics",
            IsActive = true
        });
        db.CurriculumFrameworkVersions.Add(new CurriculumFrameworkVersion
        {
            Id = versionId,
            FrameworkId = frameworkId,
            VersionCode = "V1",
            NormalizedVersionCode = "V1",
            Name = "V1",
            IsActive = true
        });
        db.SchoolCurriculumAdoptions.Add(new SchoolCurriculumAdoption
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            AcademicYearId = Guid.NewGuid(),
            AcademicProgramId = Guid.NewGuid(),
            GradeLevelId = gradeId,
            SubjectId = Guid.NewGuid(),
            FrameworkVersionId = versionId,
            IsPrimary = true,
            IsActive = true,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        });
        await db.SaveChangesAsync();

        var result = await new CurriculumLevelIdentityBackfill(db).RunAsync();

        var adoption = await db.SchoolCurriculumAdoptions.SingleAsync();
        Assert.Null(adoption.CurriculumLevelKey);
        Assert.Null(adoption.CurriculumLogicalLevel);
        Assert.Equal(0, result.AdoptionRowsResolved);
        Assert.Equal(1, result.UnresolvedAdoptions);
    }

    [Fact]
    public async Task Backfill_IsIdempotent()
    {
        await using var db = Db($"phase29-backfill-idempotent-{Guid.NewGuid():N}");

        var schoolId = Guid.NewGuid();
        var frameworkId = Guid.NewGuid();
        var versionId = Guid.NewGuid();
        var gradeId = Guid.NewGuid();

        db.GradeLevels.Add(new GradeLevel
        {
            Id = gradeId,
            SchoolId = schoolId,
            Name = "Grade 6",
            Order = 6
        });
        db.CurriculumFrameworks.Add(new CurriculumFramework
        {
            Id = frameworkId,
            Code = MathematicsCurriculumPackRegistry.CommonCoreCode,
            NormalizedCode = MathematicsCurriculumPackRegistry.CommonCoreCode,
            Name = "Common Core",
            IsActive = true
        });
        db.CurriculumFrameworkVersions.Add(new CurriculumFrameworkVersion
        {
            Id = versionId,
            FrameworkId = frameworkId,
            VersionCode = "V1",
            NormalizedVersionCode = "V1",
            Name = "V1",
            IsActive = true
        });
        db.SchoolCurriculumAdoptions.Add(new SchoolCurriculumAdoption
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            AcademicYearId = Guid.NewGuid(),
            AcademicProgramId = Guid.NewGuid(),
            GradeLevelId = gradeId,
            SubjectId = Guid.NewGuid(),
            FrameworkVersionId = versionId,
            IsPrimary = true,
            IsActive = true,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        });
        await db.SaveChangesAsync();

        var backfill = new CurriculumLevelIdentityBackfill(db);
        var first = await backfill.RunAsync();
        var second = await backfill.RunAsync();

        Assert.Equal(1, first.AdoptionRowsResolved);
        Assert.Equal(0, second.AdoptionRowsResolved);
        Assert.Equal(0, second.ClassRowsResolved);
        Assert.Equal(0, second.TopicRowsResolved);
        Assert.Equal(0, second.OutcomeRowsResolved);
    }
}
