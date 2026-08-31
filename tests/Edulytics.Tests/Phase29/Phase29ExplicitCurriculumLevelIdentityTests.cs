using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Data.Contexts;
using Edulytics.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Tests.Phase29;

public sealed class Phase29ExplicitCurriculumLevelIdentityTests
{
    private static EdulyticsDbContext Db(string name) =>
        new(
            new DbContextOptionsBuilder<EdulyticsDbContext>()
                .UseInMemoryDatabase(name)
                .Options);

    [Fact]
    public void ProgramToPackMapping_IsControlled()
    {
        Assert.Equal(
            MathematicsCurriculumPackRegistry.CambridgeCode,
            CurriculumLevelIdentityRegistry.PackCodeForProgramCode("BRITISH"));
        Assert.Equal(
            MathematicsCurriculumPackRegistry.CommonCoreCode,
            CurriculumLevelIdentityRegistry.PackCodeForProgramCode("AMERICAN"));
        Assert.Equal(
            MathematicsCurriculumPackRegistry.UaeCode,
            CurriculumLevelIdentityRegistry.PackCodeForProgramCode("UAE"));
        Assert.Equal(
            MathematicsCurriculumPackRegistry.PolandCode,
            CurriculumLevelIdentityRegistry.PackCodeForProgramCode("POLISH"));
        Assert.Null(CurriculumLevelIdentityRegistry.PackCodeForProgramCode("MAIN"));
    }

    [Fact]
    public void CommonCoreGrade7_ResolvesByNativeIdentity_ToLogicalLevel8()
    {
        var identity = CurriculumLevelIdentityRegistry.ResolveLegacy(
            MathematicsCurriculumPackRegistry.CommonCoreCode,
            "Grade 7",
            gradeOrder: 7);

        Assert.NotNull(identity);
        Assert.Equal(8, identity!.LogicalLevel);
        Assert.Equal("Grade 7", identity.Label);
        Assert.Null(identity.Pathway);
    }

    [Fact]
    public void CambridgeCoreAndExtended_AreDistinctStableIdentities()
    {
        var packCode = CurriculumLevelIdentityRegistry
            .PackCodeForProgramCode("BRITISH");

        Assert.Equal(
            MathematicsCurriculumPackRegistry.CambridgeCode,
            packCode);

        var levels = CurriculumLevelIdentityRegistry.ForPack(packCode);

        var core = Assert.Single(levels, x =>
            x.LogicalLevel == 10 && x.Pathway == "Core");
        var extended = Assert.Single(levels, x =>
            x.LogicalLevel == 10 && x.Pathway == "Extended");

        Assert.NotEqual(core.Key, extended.Key);
        Assert.Contains("CORE", core.Key, StringComparison.Ordinal);
        Assert.Contains("EXTENDED", extended.Key, StringComparison.Ordinal);
    }

    [Fact]
    public void LegacyCambridgeIgcseLabel_IsAmbiguous_AndFailsClosed()
    {
        var identity = CurriculumLevelIdentityRegistry.ResolveLegacy(
            MathematicsCurriculumPackRegistry.CambridgeCode,
            "Cambridge IGCSE Mathematics (0580)",
            gradeOrder: 10);

        Assert.Null(identity);
    }

    [Fact]
    public async Task OfficialOutcomeRepository_UsesExactPathway_AndSharedDoesNotLeak()
    {
        await using var db = Db($"phase29-explicit-pathway-{Guid.NewGuid():N}");
        var versionId = Guid.NewGuid();

        db.CurriculumPackContentNodes.AddRange(
            Node(versionId, "SHARED", pathway: null, sortOrder: 1),
            Node(versionId, "CORE", pathway: "Core", sortOrder: 2),
            Node(versionId, "EXTENDED", pathway: "Extended", sortOrder: 3));
        await db.SaveChangesAsync();

        var repository = new CurriculumRepository(db);

        var shared = await repository.GetOfficialOutcomeSourcesAsync(
            versionId,
            logicalLevel: 10,
            pathway: null);
        var core = await repository.GetOfficialOutcomeSourcesAsync(
            versionId,
            logicalLevel: 10,
            pathway: "Core");
        var extended = await repository.GetOfficialOutcomeSourcesAsync(
            versionId,
            logicalLevel: 10,
            pathway: "Extended");

        Assert.Equal("SHARED", Assert.Single(shared).Code);
        Assert.Equal("CORE", Assert.Single(core).Code);
        Assert.Equal("EXTENDED", Assert.Single(extended).Code);
    }

    [Fact]
    public void NormalAcademicUi_UsesExplicitCurriculumLevelFlow()
    {
        var root = FindRepositoryRoot();
        var academic = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/AcademicStructure/Index.cshtml"));
        var curriculum = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Curriculum/Index.cshtml"));

        Assert.Contains("<section id=\"grades\"", academic);
        Assert.Contains("<section id=\"subjects\"", academic);
        Assert.DoesNotContain("asp-action=\"CreateGradeLevel\"", academic);
        Assert.DoesNotContain("asp-action=\"CreateSubject\"", academic);
        Assert.Contains("asp-action=\"AdoptCurriculumLevel\"", academic);
        Assert.Contains("asp-action=\"CreateCurriculumClass\"", academic);
        Assert.Contains("asp-action=\"CreateCurriculumTeacherAssignment\"", academic);
        Assert.Contains("name=\"curriculumAdoptionId\"", academic);

        var classesStart = academic.IndexOf(
            "<section id=\"classes\"",
            StringComparison.Ordinal);
        var subjectsStart = academic.IndexOf(
            "<section id=\"subjects\"",
            StringComparison.Ordinal);
        Assert.True(classesStart >= 0 && subjectsStart > classesStart);

        Assert.DoesNotContain("asp-action=\"SelectFramework\"", curriculum);
        Assert.DoesNotContain("name=\"gradeLevelId\"", curriculum);
        Assert.DoesNotContain("name=\"subjectId\"", curriculum);
        Assert.Contains("asp-action=\"CreateCurriculumTopic\"", curriculum);
        Assert.Contains("asp-action=\"CreateCurriculumOfficialOutcome\"", curriculum);
        Assert.Contains("name=\"curriculumAdoptionId\"", curriculum);
    }

    private static CurriculumPackContentNode Node(
        Guid versionId,
        string code,
        string? pathway,
        int sortOrder) =>
        new()
        {
            Id = Guid.NewGuid(),
            FrameworkVersionId = versionId,
            FrameworkCode = MathematicsCurriculumPackRegistry.CambridgeCode,
            VersionCode = "TEST",
            NodeKind = "Outcome",
            Code = code,
            LogicalLevelFrom = 10,
            LogicalLevelTo = 10,
            NativeLevel = "IGCSE",
            Pathway = pathway,
            Title = code,
            OfficialText = code,
            SourceAuthority = "Test",
            SourceUrl = "https://example.test",
            SourceLocator = code,
            Attribution = "Test",
            IsOfficial = true,
            IsActive = true,
            SortOrder = sortOrder,
            ContentHash = code,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
            RowVersion = []
        };

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
                return directory.FullName;
            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException();
    }
}
