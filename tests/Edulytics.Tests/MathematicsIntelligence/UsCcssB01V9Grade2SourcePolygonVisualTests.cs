using Edulytics.Core.Curriculum;
using Edulytics.Core.Lessons;
using Edulytics.Data.Seeding;
using Edulytics.Services.LessonContent;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// B01 grade 2 U06 L02 original teacher polygons: shape-count task needs
/// 3,4,5,6-sided closed polygons, not an EquationSet without spatial math.
/// Independently authored SVG geometry and source/standards guards.
/// </summary>
public sealed class UsCcssB01V9Grade2SourcePolygonVisualTests
{
    private const string Code = "PED:US-CCSS-MATH:G2:U06:L02";

    [Fact]
    public void DrawShapesOriginalLessonRendersFiveIndependentPolygonExamples()
    {
        var pair = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Where(d=>d.PackCode=="US-CCSS-MATH"&&d.VersionCode=="CCSSM-2010")
            .SelectMany(d=>d.Lessons.Select(l=>(Document:d,Lesson:l)))
            .Single(p=>p.Lesson.LessonCode==Code);
        Assert.Equal("b13ea7b593153778a94d099465a30b96153cd9c5bdb29fadabd711498e1e9d7e",
            pair.Lesson.CanonicalBodySha256);
        Assert.Equal("https://im.kendallhunt.com/k5/teachers/grade-2/unit-6/lesson-2/preparation.html",
            pair.Lesson.SourceUrl);
        Assert.Equal(new[]{"CCSS:2.G.A.1"},pair.Lesson.OutcomeCodes);

        var english=pair.Lesson.Translations.Single(t=>t.CultureCode=="en");
        var content=RichLessonContentV2RuntimeComposer.TryCompose(Code,
            new CanonicalLessonTranslationRecord(
                english.CultureCode,english.Title,english.Explanation,
                english.KeyConceptsAndRules,english.WorkedExamples,
                english.StepByStepSolutions,english.CommonMistakes,
                english.QuickSummary));
        Assert.NotNull(content);
        var polygons=content.Visuals
            .Where(v=>v.Kind==RichLessonVisualKind.PolygonDiagram)
            .ToArray();
        Assert.Equal(5,polygons.Length);
        Assert.Equal(new[]{"triangle","quadrilateral","square","pentagon","hexagon"},
            polygons.Select(x=>Assert.Single(x.Items)).ToArray());
        Assert.Contains(polygons,v=>v.Title.Contains("3 sides"));
        Assert.Contains(polygons,v=>v.Title.Contains("4 sides"));
        Assert.Contains(polygons,v=>v.Title.Contains("5 sides"));
        Assert.Contains(polygons,v=>v.Title.Contains("6 sides"));
        Assert.Contains(polygons,v=>v.Description.Contains("OPEN chain"));
        Assert.Contains(content.Visuals,v=>v.Kind==RichLessonVisualKind.EquationSet);
        Assert.Equal(6,content.Visuals.Count);
        Assert.DoesNotContain(content.Visuals,v=>v.Kind==RichLessonVisualKind.ClockFace);
    }
}