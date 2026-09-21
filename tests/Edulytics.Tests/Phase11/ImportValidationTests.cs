using Edulytics.Core.Constants;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Imports;
using Edulytics.Services.Imports;

namespace Edulytics.Tests.Phase11;

public sealed class ImportValidationTests
{
    [Fact]
    public void SixImportSchemas_AreExact()
    {
        var validator = new ImportValidationEngine();

        Assert.Equal(
            new[] { "StudentNumber", "FirstName", "LastName", "AcademicYear", "ClassCode" },
            validator.RequiredHeaders(ImportType.Students));

        Assert.Equal(
            new[] { "Email", "AcademicYear", "ClassCode", "SubjectCode" },
            validator.RequiredHeaders(ImportType.Teachers));

        Assert.Equal(
            new[] { "AcademicYear", "GradeLevel", "Code", "Name" },
            validator.RequiredHeaders(ImportType.Classes));

        Assert.Equal(
            new[] { "Code", "Name" },
            validator.RequiredHeaders(ImportType.Subjects));

        Assert.Equal(
            new[] { "AssessmentId", "StudentNumber", "QuestionOrder", "Score" },
            validator.RequiredHeaders(ImportType.AssessmentResults));

        Assert.Equal(
            new[] { "AssessmentId", "QuestionOrder", "OutcomeCode" },
            validator.RequiredHeaders(ImportType.CurriculumMappings));
    }

    [Fact]
    public void MissingColumn_IsValidationError()
    {
        var file = new ParsedImportFile(
            ["Code"],
            [Row(2, ("Code", "MATH"))]);

        var result = new ImportValidationEngine().Validate(
            ImportType.Subjects,
            file,
            new ImportDataSnapshot(),
            [],
            Guid.NewGuid(),
            RoleNames.SchoolAdmin);

        Assert.Contains(
            result,
            x => x.Code == "MissingColumn" && x.ColumnName == "Name");
    }

    [Fact]
    public void ExistingSubject_IsConflict()
    {
        var snapshot = new ImportDataSnapshot
        {
            Subjects =
            [
                new Subject
                {
                    Id = Guid.NewGuid(),
                    SchoolId = Guid.NewGuid(),
                    Name = "Mathematics",
                    Code = "MATH",
                    NormalizedCode = "MATH",
                    Status = AcademicStructureStatus.Active
                }
            ]
        };

        var result = new ImportValidationEngine().Validate(
            ImportType.Subjects,
            new ParsedImportFile(
                ["Code", "Name"],
                [Row(2, ("Code", "MATH"), ("Name", "Math"))]),
            snapshot,
            [],
            Guid.NewGuid(),
            RoleNames.SchoolAdmin);

        Assert.Contains(result, x => x.Code == "ExistingConflict");
    }

    [Fact]
    public void LegacyClassesRow_WithSingleAdoption_RemainsValid()
    {
        var schoolId = Guid.NewGuid();
        var year = new AcademicYear
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            Name = "2026-2027",
            Status = AcademicStructureStatus.Active
        };
        var grade = new GradeLevel
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            Name = "Cambridge Primary Stage 6",
            Order = 6
        };
        var adoption = new SchoolCurriculumAdoption
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            AcademicYearId = year.Id,
            AcademicProgramId = Guid.NewGuid(),
            GradeLevelId = grade.Id,
            SubjectId = Guid.NewGuid(),
            FrameworkVersionId = Guid.NewGuid(),
            IsPrimary = true,
            IsActive = true
        };

        var result = new ImportValidationEngine().Validate(
            ImportType.Classes,
            new ParsedImportFile(
                ["AcademicYear", "GradeLevel", "Code", "Name"],
                [Row(
                    2,
                    ("AcademicYear", year.Name),
                    ("GradeLevel", grade.Name),
                    ("Code", "CLS-ABC123"),
                    ("Name", "BG4"))]),
            new ImportDataSnapshot
            {
                AcademicYears = [year],
                GradeLevels = [grade],
                CurriculumAdoptions = [adoption]
            },
            [],
            Guid.NewGuid(),
            RoleNames.SubjectSupervisor);

        Assert.Empty(result);
    }

    [Fact]
    public void LegacyClassesRow_WithMultipleAdoptions_IsAmbiguousInsteadOfThrowing()
    {
        var schoolId = Guid.NewGuid();
        var year = new AcademicYear
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            Name = "2026-2027",
            Status = AcademicStructureStatus.Active
        };
        var grade = new GradeLevel
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            Name = "Grade 12",
            Order = 12
        };
        var general = new SchoolCurriculumAdoption
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            AcademicYearId = year.Id,
            AcademicProgramId = Guid.NewGuid(),
            GradeLevelId = grade.Id,
            SubjectId = Guid.NewGuid(),
            FrameworkVersionId = Guid.NewGuid(),
            CurriculumPathway = "General",
            IsPrimary = true,
            IsActive = true
        };
        var advanced = new SchoolCurriculumAdoption
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            AcademicYearId = year.Id,
            AcademicProgramId = general.AcademicProgramId,
            GradeLevelId = grade.Id,
            SubjectId = general.SubjectId,
            FrameworkVersionId = general.FrameworkVersionId,
            CurriculumPathway = "Advanced",
            IsPrimary = true,
            IsActive = true
        };

        var result = new ImportValidationEngine().Validate(
            ImportType.Classes,
            new ParsedImportFile(
                ["AcademicYear", "GradeLevel", "Code", "Name"],
                [Row(
                    2,
                    ("AcademicYear", year.Name),
                    ("GradeLevel", grade.Name),
                    ("Code", "CLS-UAE12"),
                    ("Name", "12A"))]),
            new ImportDataSnapshot
            {
                AcademicYears = [year],
                GradeLevels = [grade],
                CurriculumAdoptions = [general, advanced]
            },
            [],
            Guid.NewGuid(),
            RoleNames.SubjectSupervisor);

        var issue = Assert.Single(result);
        Assert.Equal("GradeLevel", issue.ColumnName);
        Assert.Equal("AmbiguousReference", issue.Code);
    }

    [Fact]
    public void ClassesRow_WithExactAdoptionIdentity_IsValidWhenGradeIsShared()
    {
        var schoolId = Guid.NewGuid();
        var year = new AcademicYear
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            Name = "2026-2027",
            Status = AcademicStructureStatus.Active
        };
        var grade = new GradeLevel
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            Name = "Grade 12",
            Order = 12
        };
        var general = new SchoolCurriculumAdoption
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            AcademicYearId = year.Id,
            AcademicProgramId = Guid.NewGuid(),
            GradeLevelId = grade.Id,
            SubjectId = Guid.NewGuid(),
            FrameworkVersionId = Guid.NewGuid(),
            CurriculumPathway = "General",
            IsActive = true
        };
        var advanced = new SchoolCurriculumAdoption
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            AcademicYearId = year.Id,
            AcademicProgramId = general.AcademicProgramId,
            GradeLevelId = grade.Id,
            SubjectId = general.SubjectId,
            FrameworkVersionId = general.FrameworkVersionId,
            CurriculumPathway = "Advanced",
            IsActive = true
        };

        var result = new ImportValidationEngine().Validate(
            ImportType.Classes,
            new ParsedImportFile(
                ["AcademicYear", "GradeLevel", "Name", "CurriculumAdoptionId", "Code"],
                [Row(
                    2,
                    ("AcademicYear", year.Name),
                    ("GradeLevel", grade.Name),
                    ("Name", "12A"),
                    ("CurriculumAdoptionId", advanced.Id.ToString("D")),
                    ("Code", "CLS-UAE12"))]),
            new ImportDataSnapshot
            {
                AcademicYears = [year],
                GradeLevels = [grade],
                CurriculumAdoptions = [general, advanced]
            },
            [],
            Guid.NewGuid(),
            RoleNames.SubjectSupervisor);

        Assert.Empty(result);
    }

    [Fact]
    public void ImportOwnership_IsRoleAndTypeSpecific()
    {
        Assert.True(DataImportService.CanImportType(RoleNames.SubjectSupervisor, ImportType.Students));
        Assert.True(DataImportService.CanImportType(RoleNames.SubjectSupervisor, ImportType.Teachers));
        Assert.True(DataImportService.CanImportType(RoleNames.SubjectSupervisor, ImportType.Classes));
        Assert.False(DataImportService.CanImportType(RoleNames.SubjectSupervisor, ImportType.AssessmentResults));
        Assert.False(DataImportService.CanImportType(RoleNames.SubjectSupervisor, ImportType.Subjects));
        Assert.False(DataImportService.CanImportType(RoleNames.SubjectSupervisor, ImportType.CurriculumMappings));

        Assert.False(DataImportService.CanImportType(RoleNames.Teacher, ImportType.AssessmentResults));
        Assert.False(DataImportService.CanImportType(RoleNames.Teacher, ImportType.Students));
        Assert.False(DataImportService.CanImportType(RoleNames.Teacher, ImportType.Teachers));
        Assert.False(DataImportService.CanImportType(RoleNames.Teacher, ImportType.Classes));

        foreach (var type in Enum.GetValues<ImportType>())
        {
            Assert.False(DataImportService.CanImportType(RoleNames.SchoolAdmin, type));
        }
    }

    private static ImportFileRow Row(
        int number,
        params (string Key, string Value)[] values) =>
        new(
            number,
            values.ToDictionary(
                x => x.Key,
                x => x.Value,
                StringComparer.OrdinalIgnoreCase));
}
