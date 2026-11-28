using Edulytics.Core.Curriculum;
using Edulytics.Services.LessonContent;
using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UaeG11UnifiedArabicScopeTests
{
    private const string Framework = "UAE-MOE-MATH";
    private const string Lesson = "PED:UAE-MOE-MATH:L11:UNIFIED:01:01";

    [Fact]
    public void NewUnifiedGrade11HasOneScopeAndExactArabicCatalogue()
    {
        MathematicsCurriculumPackRegistry.Validate();
        var uae = Assert.Single(MathematicsCurriculumPackRegistry.All, x => x.Code == Framework);
        var level = Assert.Single(uae.Levels, x => x.LogicalLevel == 11);
        Assert.Equal("Unified", level.Pathway);
        var doc = Assert.Single(PedagogicalLessonBlueprintRegistry.LoadEmbeddedDocuments(),
            x => x.PackCode == Framework && x.LogicalLevel == 11);
        Assert.Equal(19, doc.Units.Count);
        Assert.Equal(130, doc.Lessons.Count);
        Assert.Equal(Enumerable.Range(1,130), doc.Lessons.Select(x => x.SortOrder));
        Assert.All(doc.Lessons, x =>
        {
            Assert.True(LessonContentPolicy.IsUaeG11ArabicLesson(Framework,x.LessonCode));
            Assert.Empty(x.OutcomeCodes);
            Assert.Null(x.OfficialReferenceCode);
            Assert.Contains(x.Title, ch => ch is >= '\u0600' and <= '\u06ff');
        });
        Assert.False(LessonContentPolicy.IsUaeG11ArabicLesson(Framework,
            "PED:UAE-MOE-MATH:L11:UNIFIED:99:99"));
        Assert.False(LessonContentPolicy.IsUaeG11ArabicLesson(Framework,
            "PED:UAE-MOE-MATH:L11:ADVANCED:01:01"));
        Assert.False(LessonContentPolicy.IsUaeG11ArabicLesson("US-CCSS-MATH",Lesson));
    }

    [Fact]
    public void FirstLessonHasExactArabicRichAndCanonicalBody()
    {
        var pack = Assert.Single(MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments(),
            x => x.Lessons.Any(y => y.LessonCode == Lesson));
        Assert.Equal("ar", pack.AcademicLanguage);
        var body = Assert.Single(pack.Lessons, x => x.LessonCode == Lesson);
        Assert.Equal("ar", Assert.Single(body.Translations).CultureCode);
        var rich = RichLessonContentV2Registry.Find(Lesson, "ar");
        Assert.NotNull(rich);
        Assert.Equal("ar", rich!.CultureCode);
        Assert.Equal(Lesson, rich.LessonCode);
        Assert.False(LessonContentPolicy.CanExposeCanonicalBody(pack.Status));
    }
}
