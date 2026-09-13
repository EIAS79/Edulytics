using System.IO.Compression;
using System.Text;
using System.Xml.Linq;
using Edulytics.Core.Academics;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Services.Imports;
using Edulytics.Web.Imports;

namespace Edulytics.Tests.Phase11;

public sealed class ClassesImportProductWorkflowTests
{
    private static readonly XNamespace SpreadsheetNs =
        "http://schemas.openxmlformats.org/spreadsheetml/2006/main";

    [Fact]
    public void Workbook_ContainsOnlyGradeLevelAndName()
    {
        var bytes = ClassesImportWorkbook.Create(
            ["Grade 1", "Year 2"]);

        var parsed = new ImportFileParser().Parse(
            "edulytics-Classes.xlsx",
            bytes);

        Assert.True(parsed.Succeeded);
        Assert.Equal(
            ["GradeLevel", "Name"],
            parsed.File!.Headers);
    }

    [Fact]
    public void Workbook_UsesStrictGradeLevelDropdownFromSystemValues()
    {
        var bytes = ClassesImportWorkbook.Create(
            ["Grade 1", "Grade 2"]);

        using var stream = new MemoryStream(bytes);
        using var archive = new ZipArchive(
            stream,
            ZipArchiveMode.Read,
            leaveOpen: false);

        var workbook = LoadXml(archive, "xl/workbook.xml");
        var definedNames = workbook
            .Descendants(SpreadsheetNs + "definedName")
            .ToDictionary(
                x => (string)x.Attribute("name")!,
                x => x.Value);

        Assert.DoesNotContain("AcademicYears", definedNames.Keys);
        Assert.Contains("GradeLevels", definedNames.Keys);

        var availableValuesSheet = workbook
            .Descendants(SpreadsheetNs + "sheet")
            .Single(x => string.Equals(
                (string?)x.Attribute("name"),
                "Available values",
                StringComparison.Ordinal));
        Assert.Equal("hidden", (string?)availableValuesSheet.Attribute("state"));

        var classesSheet = LoadXml(
            archive,
            "xl/worksheets/sheet1.xml");

        var validation = Assert.Single(
            classesSheet.Descendants(SpreadsheetNs + "dataValidation"));

        Assert.Equal("A2:A1001", (string?)validation.Attribute("sqref"));
        Assert.Equal("1", (string?)validation.Attribute("showErrorMessage"));
        Assert.Equal(
            "GradeLevels",
            validation.Element(SpreadsheetNs + "formula1")?.Value);
    }

    [Fact]
    public void Adapter_InjectsOneSelectedAcademicYearAndGeneratesInternalCodes()
    {
        var source = Encoding.UTF8.GetBytes(
            "GradeLevel,Name\n"
            + "Grade 1,AG1\n"
            + "Grade 2,BG1\n");

        var adapted = MathOnlyImportAdapter.NormalizeUpload(
            ImportType.Classes,
            "classes.csv",
            source,
            selectedAcademicYear: "2026-2027");

        var parsed = new ImportFileParser().Parse(
            adapted.FileName,
            adapted.Bytes);

        Assert.True(parsed.Succeeded);
        Assert.Equal(
            ["AcademicYear", "GradeLevel", "Name", "Code"],
            parsed.File!.Headers);
        Assert.Equal(2, parsed.File.Rows.Count);
        Assert.All(
            parsed.File.Rows,
            row => Assert.Equal(
                "2026-2027",
                row.Values["AcademicYear"]));
        Assert.StartsWith(
            "CLS-",
            parsed.File.Rows[0].Values["Code"]);
        Assert.StartsWith(
            "CLS-",
            parsed.File.Rows[1].Values["Code"]);
        Assert.NotEqual(
            parsed.File.Rows[0].Values["Code"],
            parsed.File.Rows[1].Values["Code"]);
    }

    [Fact]
    public void Adapter_CarriesExactAdoptionIdentityWhenOneGradeHasMultiplePathways()
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
        var program = new AcademicProgram
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            Name = "UAE Stream",
            Code = "UAE",
            NormalizedCode = "UAE",
            Status = AcademicStructureStatus.Active
        };
        var general = new SchoolCurriculumAdoption
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            AcademicYearId = year.Id,
            AcademicProgramId = program.Id,
            GradeLevelId = grade.Id,
            SubjectId = Guid.NewGuid(),
            FrameworkVersionId = Guid.NewGuid(),
            CurriculumLevelKey = "UAE:L12:GENERAL",
            CurriculumLevelLabel = "Grade 12",
            CurriculumPathway = "General",
            IsPrimary = true,
            IsActive = true
        };
        var advanced = new SchoolCurriculumAdoption
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            AcademicYearId = year.Id,
            AcademicProgramId = program.Id,
            GradeLevelId = grade.Id,
            SubjectId = general.SubjectId,
            FrameworkVersionId = general.FrameworkVersionId,
            CurriculumLevelKey = "UAE:L12:ADVANCED",
            CurriculumLevelLabel = "Grade 12",
            CurriculumPathway = "Advanced",
            IsPrimary = true,
            IsActive = true
        };
        var snapshot = new AcademicStructureSnapshot(
            [year],
            [],
            [grade],
            [],
            [],
            [],
            [],
            [])
        {
            AcademicPrograms = [program],
            CurriculumAdoptions = [general, advanced]
        };

        var options = MathOnlyImportAdapter.ClassLevelOptions(
            snapshot,
            year.Id);
        Assert.Equal(2, options.Count);
        Assert.Contains(options, x => x.DisplayName.Contains("General", StringComparison.Ordinal));
        var advancedOption = Assert.Single(
            options,
            x => x.DisplayName.Contains("Advanced", StringComparison.Ordinal));

        var source = Encoding.UTF8.GetBytes(
            $"GradeLevel,Name\n{advancedOption.DisplayName},12A\n");
        var adapted = MathOnlyImportAdapter.NormalizeUpload(
            ImportType.Classes,
            "classes.csv",
            source,
            academicStructure: snapshot,
            selectedAcademicYear: year.Name);
        var parsed = new ImportFileParser().Parse(
            adapted.FileName,
            adapted.Bytes);

        Assert.True(parsed.Succeeded);
        Assert.Contains("CurriculumAdoptionId", parsed.File!.Headers);
        var row = Assert.Single(parsed.File.Rows);
        Assert.Equal(grade.Name, row.Values["GradeLevel"]);
        Assert.Equal(
            advanced.Id.ToString("D"),
            row.Values["CurriculumAdoptionId"]);
    }

    [Fact]
    public void ClassesFile_IsRecognizedBeforeWrongTypeValidation()
    {
        var source = Encoding.UTF8.GetBytes(
            "GradeLevel,Name\n"
            + "Grade 1,AG1\n");

        Assert.True(
            MathOnlyImportAdapter.LooksLikeClassesUpload(
                "classes.csv",
                source));
    }

    [Fact]
    public void LegacyClassesTemplate_IsRecognizedForActionableRejection()
    {
        var source = Encoding.UTF8.GetBytes(
            "AcademicYear,GradeLevel,Name\n"
            + "2026-2027,Grade 1,AG1\n");

        Assert.True(
            MathOnlyImportAdapter.UsesLegacyClassesTemplate(
                "classes.csv",
                source));
    }

    private static XDocument LoadXml(
        ZipArchive archive,
        string path)
    {
        var entry = archive.GetEntry(path);
        Assert.NotNull(entry);

        using var stream = entry!.Open();
        return XDocument.Load(stream);
    }
}
