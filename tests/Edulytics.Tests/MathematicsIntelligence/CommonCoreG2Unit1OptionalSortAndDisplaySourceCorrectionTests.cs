using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Source and canonical prose locks authored only, Stage D unrun.</summary>
public sealed class CommonCoreG2Unit1OptionalSortAndDisplaySourceCorrectionTests
{
    [Fact]
    public void OptionalLessonNeedsActual20To30ObjectsAndThreeCategories()
    {
        var row=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons
                .Where(l=>CommonCoreG2Unit1OptionalSortAndDisplaySourceCorrection
                    .IsTarget(doc,l))
                .Select(l=>(Document:doc,Lesson:l))).Single();
        Assert.Equal("PED:US-CCSS-MATH:G2:U01:L12",row.Lesson.LessonCode);
        var en=row.Lesson.Translations.Single(t=>t.CultureCode=="en");
        Assert.Contains("20–30 REAL objects",en.Explanation);
        Assert.Contains("THREE",en.Explanation);
        Assert.Contains("Sort and Display Stage 2",en.KeyConceptsAndRules);
        Assert.Contains("17−9=8",en.KeyConceptsAndRules);
        Assert.Contains("teacher",en.QuickSummary);
        Assert.Equal(CommonCoreG2Unit1OptionalSortAndDisplaySourceCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(
                row.Document,row.Lesson));
    }
    [Fact]
    public void GuardsCanonicalBodyAndExactlyOneExpectedPredecessor()
    {
        var row=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons
                .Where(l=>CommonCoreG2Unit1OptionalSortAndDisplaySourceCorrection
                    .IsTarget(doc,l))
                .Select(l=>(Document:doc,Lesson:l))).Single();
        Assert.True(CommonCoreG2Unit1OptionalSortAndDisplaySourceCorrection.CanUpgradeExisting(
            row.Document,row.Lesson,
            CommonCoreRichContentCorrections.CorrectionContentVersion));
        Assert.False(CommonCoreG2Unit1OptionalSortAndDisplaySourceCorrection.CanUpgradeExisting(
            row.Document,row.Lesson,"unverified"));
        var sha=row.Lesson.CanonicalBodySha256;
        try
        {
            row.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit1OptionalSortAndDisplaySourceCorrection.Apply(row.Document));
        }
        finally {row.Lesson.CanonicalBodySha256=sha;}
    }
}
