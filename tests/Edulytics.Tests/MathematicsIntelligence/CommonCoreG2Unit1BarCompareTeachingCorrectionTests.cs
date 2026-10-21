using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage C original 2021 source guards, no execution before D.</summary>
public sealed class CommonCoreG2Unit1BarCompareTeachingCorrectionTests
{
    [Fact]
    public void OriginalSourceTeachingIncludesCountOnBackAndInverseEquations()
    {
        var row=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons
                .Where(l=>CommonCoreG2Unit1BarCompareTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Document:d,Lesson:l))).Single();
        Assert.Equal("PED:US-CCSS-MATH:G2:U01:L13",row.Lesson.LessonCode);
        var en=row.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("count ON",en.Explanation);
        Assert.Contains("counting BACK",en.StepByStepSolutions);
        Assert.Contains("17−8=9",en.WorkedExamples);
        Assert.Contains("20−14=6",en.KeyConceptsAndRules);
        Assert.Equal(CommonCoreG2Unit1BarCompareTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(
                row.Document,row.Lesson));
    }
    [Fact]
    public void ProvenanceAndOriginalPriorRichVersionAreMandatory()
    {
        var row=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons
                .Where(l=>CommonCoreG2Unit1BarCompareTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Document:d,Lesson:l))).Single();
        Assert.True(CommonCoreG2Unit1BarCompareTeachingCorrection.CanUpgradeExisting(
            row.Document,row.Lesson,
            CommonCoreRichContentCorrections.CorrectionContentVersion));
        Assert.False(CommonCoreG2Unit1BarCompareTeachingCorrection.CanUpgradeExisting(
            row.Document,row.Lesson,"modified-version"));
        var prev=row.Lesson.CanonicalBodySha256;
        try
        {
            row.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit1BarCompareTeachingCorrection.Apply(row.Document));
        }
        finally {row.Lesson.CanonicalBodySha256=prev;}
    }
}
