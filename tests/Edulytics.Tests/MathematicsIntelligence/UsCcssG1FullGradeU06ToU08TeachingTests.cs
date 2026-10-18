using Edulytics.Core.Curriculum;
using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Whole remaining grade content integration. Mathematical/student-task
/// acceptance, original licensed figures and teacher assessed activities are
/// explicitly separate, mandatory academic release checks.</summary>
public sealed class UsCcssG1FullGradeU06ToU08TeachingTests
{
    [Fact]
    public void ThirtySourceBoundRepairsHaveUniqueNewLessonIdentifiers()
    {
        var documents=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        var ids=documents.SelectMany(doc=>doc.Lessons
            .Where(x=>CommonCoreG1FullGradeUnit6To8ContentCorrections.IsTarget(doc,x))
            .Select(x=>x.LessonCode)).ToArray();
        Assert.Equal(30,ids.Length);
        Assert.Equal(30,ids.Distinct(StringComparer.Ordinal).Count());
        Assert.All(ids,id=>Assert.StartsWith("PED:US-CCSS-MATH:G1:",id));
        Assert.DoesNotContain("PED:US-CCSS-MATH:G1:U06:L02",ids);
        Assert.DoesNotContain("PED:US-CCSS-MATH:G1:U07:L14",ids);
        for(var n=1;n<=10;n++)
            Assert.Contains($"PED:US-CCSS-MATH:G1:U08:L{n:00}",ids);
    }
    [Fact]
    public void OriginalSourceBoundMaterializationPreservesMeasurementGeometryTimeAndRiddleGoals()
    {
        var docs=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        var doc=docs.Single(d=>d.Lessons.Any(x=>x.LessonCode=="PED:US-CCSS-MATH:G1:U08:L10"));
        CanonicalLessonContentMaterializer.Materialize(doc);
        string text(string code)
        {
            var lesson=doc.Lessons.Single(x=>x.LessonCode==code);
            Assert.True(CommonCoreG1FullGradeUnit6To8ContentCorrections.IsTarget(doc,lesson));
            Assert.Equal(CommonCoreG1FullGradeUnit6To8ContentCorrections.Version,
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(doc,lesson));
            var tr=lesson.Translations.Single(t=>t.CultureCode=="en");
            return tr.Explanation+"\n"+tr.WorkedExamples+"\n"+tr.StepByStepSolutions+"\n"+tr.CommonMistakes;
        }
        Assert.Contains("no gaps or overlaps",text("PED:US-CCSS-MATH:G1:U06:L06"));
        Assert.Contains("two equal rectangles has halves",text("PED:US-CCSS-MATH:G1:U07:L09"));
        Assert.Contains("9+8=9+1+7=17",text("PED:US-CCSS-MATH:G1:U08:L03"));
        Assert.Contains("10 less than 75",text("PED:US-CCSS-MATH:G1:U08:L09"));
    }
}
