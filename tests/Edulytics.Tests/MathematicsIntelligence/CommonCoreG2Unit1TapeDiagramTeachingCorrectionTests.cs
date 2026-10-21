using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage C only; source and persistence provenance tests authored.</summary>
public sealed class CommonCoreG2Unit1TapeDiagramTeachingCorrectionTests
{
    [Fact]
    public void OriginalBarToTapePedagogyUsesAlignedUnknownDifference()
    {
        var row=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit1TapeDiagramTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Document:d,Lesson:l))).Single();
        Assert.Equal("PED:US-CCSS-MATH:G2:U01:L14",row.Lesson.LessonCode);
        Assert.Equal(new[]{"CCSS:2.MD.D.10"},row.Lesson.OutcomeCodes);
        var en=row.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("TAPE DIAGRAMS",en.Explanation);
        Assert.Contains("pizza 19",en.Explanation);
        Assert.Contains("19−14=5",en.KeyConceptsAndRules);
        Assert.Contains("dashed section",en.StepByStepSolutions);
        Assert.Equal(CommonCoreG2Unit1TapeDiagramTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(
                row.Document,row.Lesson));
    }

    [Fact]
    public void UnexpectedSourceBodyHashOrPredecessorMustFailClosed()
    {
        var row=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit1TapeDiagramTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Document:d,Lesson:l))).Single();
        Assert.True(CommonCoreG2Unit1TapeDiagramTeachingCorrection.CanUpgradeExisting(
            row.Document,row.Lesson,
            CommonCoreRichContentCorrections.CorrectionContentVersion));
        Assert.False(CommonCoreG2Unit1TapeDiagramTeachingCorrection.CanUpgradeExisting(
            row.Document,row.Lesson,"unknown-version"));
        var previous=row.Lesson.CanonicalBodySha256;
        try
        {
            row.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit1TapeDiagramTeachingCorrection.Apply(row.Document));
        }
        finally {row.Lesson.CanonicalBodySha256=previous;}
    }
}
