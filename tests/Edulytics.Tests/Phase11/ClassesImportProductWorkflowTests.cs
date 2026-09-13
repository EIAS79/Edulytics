using System.IO.Compression;
using System.Text;
using System.Xml.Linq;
using Edulytics.Core.Enums;
using Edulytics.Services.Imports;
using Edulytics.Web.Imports;

namespace Edulytics.Tests.Phase11;

public sealed class ClassesImportProductWorkflowTests
{
    private static readonly XNamespace SpreadsheetNs =
        "http://schemas.openxmlformats.org/spreadsheetml/2006/main";

    [Fact]
    public void Workbook_KeepsAcademicYearOnEveryClassRow()
    {
        var bytes = ClassesImportWorkbook.Create(
            ["2026-2027", "2027-2028"],
            ["Grade 1", "Year 2"]);

        var parsed = new ImportFileParser().Parse(
            "edulytics-Classes.xlsx",
            bytes);

        Assert.True(parsed.Succeeded);
        Assert.Equal(
            ["AcademicYear", "GradeLevel", "Name"],
            parsed.File!.Headers);
    }

    [Fact]
    public void Workbook_UsesEditableYearSuggestionsAndStrictGradeLevelDropdown()
    {
        var bytes = ClassesImportWorkbook.Create(
            ["2026-2027", "2027-2028"],
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

        Assert.Contains("AcademicYears", definedNames.Keys);
        Assert.Contains("GradeLevels", definedNames.Keys);

        var classesSheet = LoadXml(
            archive,
            "xl/worksheets/sheet1.xml");

        var validations = classesSheet
            .Descendants(SpreadsheetNs + "dataValidation")
            .ToDictionary(
                x => (string)x.Attribute("sqref")!,
                x => x);

        var academicYear = validations["A2:A1001"];
        Assert.Equal("0", (string?)academicYear.Attribute("showErrorMessage"));
        Assert.Equal(
            "AcademicYears",
            academicYear.Element(SpreadsheetNs + "formula1")?.Value);

        var gradeLevel = validations["B2:B1001"];
        Assert.Equal("1", (string?)gradeLevel.Attribute("showErrorMessage"));
        Assert.Equal(
            "GradeLevels",
            gradeLevel.Element(SpreadsheetNs + "formula1")?.Value);
    }

    [Fact]
    public void Adapter_PreservesMultipleAcademicYearsAndGeneratesInternalCodes()
    {
        var source = Encoding.UTF8.GetBytes(
            "AcademicYear,GradeLevel,Name\n"
            + "2026-2027,Grade 1,AG1\n"
            + "2027-2028,Grade 2,BG1\n");

        var adapted = MathOnlyImportAdapter.NormalizeUpload(
            ImportType.Classes,
            "classes.csv",
            source);

        var parsed = new ImportFileParser().Parse(
            adapted.FileName,
            adapted.Bytes);

        Assert.True(parsed.Succeeded);
        Assert.Equal(
            ["AcademicYear", "GradeLevel", "Name", "Code"],
            parsed.File!.Headers);
        Assert.Equal(2, parsed.File.Rows.Count);
        Assert.Equal(
            "2026-2027",
            parsed.File.Rows[0].Values["AcademicYear"]);
        Assert.Equal(
            "2027-2028",
            parsed.File.Rows[1].Values["AcademicYear"]);
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
    public void ClassesFile_IsRecognizedBeforeWrongTypeValidation()
    {
        var source = Encoding.UTF8.GetBytes(
            "AcademicYear,GradeLevel,Name\n"
            + "2026-2027,Grade 1,AG1\n");

        Assert.True(
            MathOnlyImportAdapter.LooksLikeClassesUpload(
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
