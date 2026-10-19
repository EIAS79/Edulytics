using Edulytics.Core.Curriculum;
using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG1FullGradeFinalUnit4TeachingTests
{
    [Fact]
    public void AllFiveOriginalGoalTargetsExistWithoutOverwritingEarlierSourceCorrections()
    {
        var doc=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Single(x=>x.Lessons.Any(l=>l.LessonCode=="PED:US-CCSS-MATH:G1:U04:L16"));
        var targets=doc.Lessons.Where(l=>CommonCoreG1FullGradeFinalUnit4Corrections.IsTarget(doc,l)).ToArray();
        Assert.Equal(5,targets.Length);
        Assert.Equal(new[]{16,17,18,19,20}.Select(n=>$"PED:US-CCSS-MATH:G1:U04:L{n:00}"),
            targets.Select(x=>x.LessonCode));
        Assert.False(CommonCoreG1FullGradeFinalUnit4Corrections.IsTarget(doc,
            doc.Lessons.Single(l=>l.LessonCode=="PED:US-CCSS-MATH:G1:U04:L21")));
        CanonicalLessonContentMaterializer.Materialize(doc);
        var comparison=doc.Lessons.Single(l=>l.LessonCode=="PED:US-CCSS-MATH:G1:U04:L16");
        var representation=doc.Lessons.Single(l=>l.LessonCode=="PED:US-CCSS-MATH:G1:U04:L19");
        Assert.Contains("85 > 58",comparison.Translations.Single(x=>x.CultureCode=="en").WorkedExamples);
        Assert.Contains("3 tens and 13 ones",representation.Translations.Single(x=>x.CultureCode=="en").WorkedExamples);
        var noBreak=doc.Lessons.Single(l=>l.LessonCode=="PED:US-CCSS-MATH:G1:U04:L19")
            .Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("CANNOT break a tower or build a new tower",noBreak.Explanation);
        Assert.Contains("65 = 5 pre-existing ten-towers and 15 loose ones",noBreak.WorkedExamples);
        var bags=doc.Lessons.Single(l=>l.LessonCode=="PED:US-CCSS-MATH:G1:U04:L20")
            .Translations.Single(x=>x.CultureCode=="en").StepByStepSolutions;
        Assert.Contains("Bag A has five tens and two ones (52)",bags);
        Assert.Contains("Bag B has four tens and twenty-five ones (65)",bags);
        Assert.Contains("Bag C has 49 cubes with 29 ones",bags);
        Assert.Contains("Bag D has 36 cubes with two tens",bags);
        Assert.Equal(CommonCoreG1FullGradeFinalUnit4Corrections.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(doc,comparison));
    }
}
