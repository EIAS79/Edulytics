using Edulytics.Core.Entities;
using Edulytics.Data.Contexts;
using Edulytics.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Tests.Acceptance;

public sealed class Phase44ManualAcceptanceCorrectiveTests
{
    [Theory]
    [InlineData("UK:STD:7Ni.01", "7Ni.01")]
    [InlineData("CCSS:7.NS.A.1", "7.NS.A.1")]
    [InlineData("PL:REQ:VII.1", "VII.1")]
    [InlineData("UAE:STD:G7.N.1", "G7.N.1")]
    public async Task AssessmentSnapshot_MaterializesVerifiedOfficialOutcomeWithoutInventingCode(
        string sourceCode,
        string expectedOperationalCode)
    {
        await using var db = NewDb();
        var schoolId = Guid.NewGuid();
        var adoption = NewAdoption(schoolId);
        var officialNode = NewOfficialNode(adoption, sourceCode);

        db.SchoolCurriculumAdoptions.Add(adoption);
        db.CurriculumPackContentNodes.Add(officialNode);
        await db.SaveChangesAsync();

        var repository = new AssessmentRepository(db);
        var first = await repository.GetSnapshotAsync(schoolId);
        var outcome = Assert.Single(first.LearningOutcomes);

        Assert.Equal(expectedOperationalCode, outcome.Code);
        Assert.Equal(officialNode.Id, outcome.OfficialContentNodeId);
        Assert.Equal(adoption.Id, outcome.CurriculumAdoptionId);
        Assert.Equal(officialNode.OfficialText, outcome.Description);

        var second = await repository.GetSnapshotAsync(schoolId);
        Assert.Single(second.LearningOutcomes);
        Assert.Single(await db.LearningOutcomes.Where(x => x.SchoolId == schoolId).ToListAsync());
    }

    [Fact]
    public async Task AssessmentSnapshot_DoesNotMaterializeAnotherSchoolsAdoption()
    {
        await using var db = NewDb();
        var schoolA = Guid.NewGuid();
        var schoolB = Guid.NewGuid();
        var adoption = NewAdoption(schoolA);
        db.SchoolCurriculumAdoptions.Add(adoption);
        db.CurriculumPackContentNodes.Add(NewOfficialNode(adoption, "UK:STD:7Ni.01"));
        await db.SaveChangesAsync();

        var repository = new AssessmentRepository(db);
        await repository.GetSnapshotAsync(schoolB);

        Assert.Empty(await db.LearningOutcomes.ToListAsync());
        Assert.Empty(await db.CurriculumTopics.ToListAsync());
    }

    [Fact]
    public void TeacherAndStudentPaths_UseTheSameOfficialOutcomeProjection()
    {
        var builderRepository = ReadRepositoryFile(
            "src", "Edulytics.Data", "Repositories", "AssessmentBuilderRepository.cs");
        var practiceRepository = ReadRepositoryFile(
            "src", "Edulytics.Data", "Repositories", "StudentPrivatePracticeRepository.cs");
        var assessmentRepository = ReadRepositoryFile(
            "src", "Edulytics.Data", "Repositories", "AssessmentRepository.cs");
        var materializer = ReadRepositoryFile(
            "src", "Edulytics.Data", "Repositories", "OfficialCurriculumOutcomeMaterializer.cs");

        Assert.Contains("OfficialCurriculumOutcomeMaterializer.EnsureAsync", builderRepository, StringComparison.Ordinal);
        Assert.Contains("OfficialCurriculumOutcomeMaterializer.EnsureAsync", practiceRepository, StringComparison.Ordinal);
        Assert.Contains("OfficialCurriculumOutcomeMaterializer.EnsureAllActiveAsync", assessmentRepository, StringComparison.Ordinal);
        Assert.Contains("x.IsOfficial", materializer, StringComparison.Ordinal);
        Assert.Contains("x.NodeKind == \"Standard\" || x.NodeKind == \"Outcome\"", materializer, StringComparison.Ordinal);
        Assert.Contains("OfficialContentNodeId = node.Id", materializer, StringComparison.Ordinal);
        Assert.Contains("var code = DisplayCode(node.Code);", materializer, StringComparison.Ordinal);
        Assert.Contains("Code = code", materializer, StringComparison.Ordinal);
    }

    [Fact]
    public void Phase44Presentation_FixesAllConfirmedManualAcceptanceLayouts()
    {
        var css = ReadRepositoryFile(
            "src", "Edulytics.Web", "wwwroot", "css", "phase44-manual-acceptance.css");
        var appCss = ReadRepositoryFile(
            "src", "Edulytics.Web", "wwwroot", "css", "acceptance-corrective.css");
        var publicCss = ReadRepositoryFile(
            "src", "Edulytics.Web", "wwwroot", "css", "round2-product-fixes.css");

        Assert.StartsWith("@import url(\"./phase44-manual-acceptance.css\");", appCss, StringComparison.Ordinal);
        Assert.StartsWith("@import url(\"./phase44-manual-acceptance.css\");", publicCss, StringComparison.Ordinal);
        Assert.Contains(".assessment-actions", css, StringComparison.Ordinal);
        Assert.Contains("flex-wrap: nowrap", css, StringComparison.Ordinal);
        Assert.Contains("#students .academic-list-grid", css, StringComparison.Ordinal);
        Assert.Contains("grid-template-columns: minmax(0, 1fr) !important", css, StringComparison.Ordinal);
        Assert.Contains(".student-learning-panel", css, StringComparison.Ordinal);
        Assert.Contains("grid-row: auto !important", css, StringComparison.Ordinal);
        Assert.Contains(".ed-public-body .language-brand .localized-brand-logo", css, StringComparison.Ordinal);
        Assert.Contains("@media (max-width: 1180px)", css, StringComparison.Ordinal);
        Assert.Contains("@media (max-width: 430px)", css, StringComparison.Ordinal);
    }

    private static EdulyticsDbContext NewDb()
    {
        var options = new DbContextOptionsBuilder<EdulyticsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .Options;
        return new EdulyticsDbContext(options);
    }

    private static SchoolCurriculumAdoption NewAdoption(Guid schoolId) => new()
    {
        Id = Guid.NewGuid(),
        SchoolId = schoolId,
        AcademicYearId = Guid.NewGuid(),
        AcademicProgramId = Guid.NewGuid(),
        GradeLevelId = Guid.NewGuid(),
        SubjectId = Guid.NewGuid(),
        FrameworkVersionId = Guid.NewGuid(),
        CurriculumLevelKey = "CAMBRIDGE-0862-STAGE-7",
        CurriculumLogicalLevel = 7,
        CurriculumLevelLabel = "Cambridge Lower Secondary Stage 7",
        CurriculumStage = "Lower Secondary",
        CurriculumPathway = null,
        IsPrimary = true,
        IsActive = true,
        CreatedAtUtc = DateTime.UtcNow,
        UpdatedAtUtc = DateTime.UtcNow,
        RowVersion = BitConverter.GetBytes(1L)
    };

    private static CurriculumPackContentNode NewOfficialNode(
        SchoolCurriculumAdoption adoption,
        string code) => new()
    {
        Id = Guid.NewGuid(),
        FrameworkVersionId = adoption.FrameworkVersionId,
        FrameworkCode = "TEST",
        VersionCode = "V1",
        NodeKind = "Outcome",
        Code = code,
        LogicalLevelFrom = 7,
        LogicalLevelTo = 7,
        NativeLevel = "Stage 7",
        Title = "Use integers in calculations",
        OfficialText = "Use positive and negative integers in mathematical calculations.",
        SourceAuthority = "Official test authority",
        SourceUrl = "https://example.invalid/source",
        SourceLocator = "test",
        Attribution = "test",
        IsOfficial = true,
        IsActive = true,
        SortOrder = 1,
        ContentHash = Guid.NewGuid().ToString("N"),
        CreatedAtUtc = DateTime.UtcNow,
        UpdatedAtUtc = DateTime.UtcNow,
        RowVersion = BitConverter.GetBytes(1L)
    };

    private static string ReadRepositoryFile(params string[] relativeSegments)
    {
        var root = FindRoot();
        return File.ReadAllText(Path.Combine([root, .. relativeSegments]));
    }

    private static string FindRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null &&
               !File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
        {
            directory = directory.Parent;
        }

        return directory?.FullName
            ?? throw new InvalidOperationException("Repository root not found.");
    }
}
