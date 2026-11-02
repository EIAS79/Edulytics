using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Data.Contexts;
using Edulytics.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Tests.Phase42;

public sealed class UsCcssHighSchoolCourseMaterializationTests
{
    [Fact]
    public async Task Materializer_creates_only_official_outcomes_linked_to_the_selected_HS_course()
    {
        await using var db = Database();
        var adoption = Adoption("Traditional", "ALG1");
        var now = DateTime.UtcNow;
        var algebraNode = OfficialNode(adoption.FrameworkVersionId,
            "CCSS:HSA-REI.B.3", "Algebra 1", 1);
        var geometryNode = OfficialNode(adoption.FrameworkVersionId,
            "CCSS:HSG-CO.A.1", "Geometry", 2);
        var algebraLesson = Lesson(adoption.FrameworkVersionId,
            "ALG1-LESSON", "Algebra 1", "Traditional", now);
        var geometryLesson = Lesson(adoption.FrameworkVersionId,
            "GEO-LESSON", "Geometry", "Traditional", now);
        await db.AddRangeAsync(adoption, algebraNode, geometryNode, algebraLesson,
            geometryLesson,
            new CurriculumPedagogicalLessonOutcome
            {
                PedagogicalLessonId = algebraLesson.Id,
                FrameworkVersionId = adoption.FrameworkVersionId,
                OutcomeNodeId = algebraNode.Id,
                SortOrder = 1
            },
            new CurriculumPedagogicalLessonOutcome
            {
                PedagogicalLessonId = geometryLesson.Id,
                FrameworkVersionId = adoption.FrameworkVersionId,
                OutcomeNodeId = geometryNode.Id,
                SortOrder = 1
            });
        await db.SaveChangesAsync();

        await OfficialCurriculumOutcomeMaterializer.EnsureAsync(db, adoption);
        var generated = await db.LearningOutcomes
            .Where(x => x.CurriculumAdoptionId == adoption.Id)
            .Select(x => x.OfficialContentNodeId)
            .ToListAsync();

        Assert.Equal(algebraNode.Id, Assert.Single(generated));
        Assert.DoesNotContain(geometryNode.Id, generated);
    }

    [Theory]
    [InlineData("Course/pathway mapping")]
    [InlineData("Traditional")]
    [InlineData("Fourth Courses")]
    public async Task Legacy_ambiguous_US_HS_cannot_materialize_outcomes(string pathway)
    {
        await using var db = Database();
        var adoption = Adoption(pathway, courseCode: null);
        var node = OfficialNode(adoption.FrameworkVersionId,
            "CCSS:HSA-REI.B.3", "Algebra 1", 1);
        db.SchoolCurriculumAdoptions.Add(adoption);
        db.CurriculumPackContentNodes.Add(node);
        await db.SaveChangesAsync();
        await OfficialCurriculumOutcomeMaterializer.EnsureAsync(db, adoption);
        Assert.Empty(await db.LearningOutcomes.ToListAsync());
    }

    private static EdulyticsDbContext Database() => new(
        new DbContextOptionsBuilder<EdulyticsDbContext>()
            .UseInMemoryDatabase("us-hs-course-" + Guid.NewGuid().ToString("N"))
            .Options);

    private static SchoolCurriculumAdoption Adoption(string pathway, string? courseCode) => new()
    {
        Id = Guid.NewGuid(),
        SchoolId = Guid.NewGuid(),
        AcademicYearId = Guid.NewGuid(),
        AcademicProgramId = Guid.NewGuid(),
        GradeLevelId = Guid.NewGuid(),
        SubjectId = Guid.NewGuid(),
        FrameworkVersionId = Guid.NewGuid(),
        CurriculumLevelKey = CurriculumLevelIdentityRegistry.BuildKey(
            MathematicsCurriculumPackRegistry.CommonCoreCode, 11, pathway, courseCode),
        CurriculumLogicalLevel = 11,
        CurriculumLevelLabel = "Algebra 1",
        CurriculumPathway = pathway,
        IsActive = true,
        IsPrimary = true,
        RowVersion = []
    };

    private static CurriculumPackContentNode OfficialNode(
        Guid versionId, string code, string nativeLevel, int sortOrder) => new()
    {
        Id = Guid.NewGuid(),
        FrameworkVersionId = versionId,
        FrameworkCode = MathematicsCurriculumPackRegistry.CommonCoreCode,
        VersionCode = "CCSSM-2010",
        NodeKind = "Standard",
        Code = code,
        LogicalLevelFrom = 10,
        LogicalLevelTo = 13,
        NativeLevel = nativeLevel,
        Title = code,
        OfficialText = code,
        SourceAuthority = "Test-source",
        SourceUrl = "https://example.test/ccss",
        SourceLocator = code,
        Attribution = "Test",
        IsOfficial = true,
        IsActive = true,
        SortOrder = sortOrder,
        ContentHash = code,
        RowVersion = []
    };

    private static CurriculumPedagogicalLesson Lesson(
        Guid versionId, string code, string nativeLevel, string pathway, DateTime now) => new()
    {
        Id = Guid.NewGuid(),
        FrameworkVersionId = versionId,
        Code = code,
        UnitKey = "U1",
        UnitTitle = "Test",
        Title = code,
        NativeLevel = nativeLevel,
        Pathway = pathway,
        LogicalLevelFrom = 10,
        LogicalLevelTo = 13,
        SortOrder = 1,
        CreatedAtUtc = now,
        UpdatedAtUtc = now
    };
}
