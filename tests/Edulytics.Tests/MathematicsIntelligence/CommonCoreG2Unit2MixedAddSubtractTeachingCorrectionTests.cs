using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class CommonCoreG2Unit2MixedAddSubtractTeachingCorrectionTests
{
    [Fact]
    public void OriginalFourExpressionMixedArithmeticAndAuthenticCardBoundaries()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit2MixedAddSubtractTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        var en=pair.Lesson.Translations.Single(t=>t.CultureCode=="en");
        Assert.Contains("27+47=74",en.WorkedExamples);
        Assert.Contains("55−27=28",en.WorkedExamples);
        Assert.Contains("36+38=74",en.WorkedExamples);
        Assert.Contains("82−39=43",en.WorkedExamples);
        Assert.Contains("card",string.Join(" ",en.Explanation,en.WorkedExamples,en.CommonMistakes),StringComparison.OrdinalIgnoreCase);
        Assert.Contains("teacher",string.Join(" ",en.Explanation,en.WorkedExamples,en.CommonMistakes),StringComparison.OrdinalIgnoreCase);
        Assert.Equal(CommonCoreG2Unit2MixedAddSubtractTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(pair.Doc,pair.Lesson));
    }
    [Fact]
    public void AlteredSourceAndUnreviewedPriorVersionAreRejected()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit2MixedAddSubtractTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit2MixedAddSubtractTeachingCorrection
            .CanUpgradeExisting(pair.Doc,pair.Lesson,"unreviewed-version"));
        var sha=pair.Lesson.CanonicalBodySha256;
        try {
            pair.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit2MixedAddSubtractTeachingCorrection.Apply(pair.Doc));
        } finally {pair.Lesson.CanonicalBodySha256=sha;}
    }
}
