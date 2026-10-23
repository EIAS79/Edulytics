using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage C source identity checks authored without execution.</summary>
public sealed class CommonCoreG2Unit1BarGraphsSourceTeachingCorrectionTests
{
    [Fact]
    public void ExactSourceLessonTeachesAxisScaleAndMultipleSourceQuestions()
    {
        var row=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons
                .Where(l=>CommonCoreG2Unit1BarGraphsSourceTeachingCorrection
                    .IsTarget(doc,l))
                .Select(l=>(Document:doc,Lesson:l)))
            .Single();
        Assert.Equal("PED:US-CCSS-MATH:G2:U01:L09",row.Lesson.LessonCode);
        var en=row.Lesson.Translations.Single(t=>t.CultureCode=="en");
        Assert.Contains("numbered vertical scale",en.Explanation);
        Assert.Contains("favorite-season graph",en.KeyConceptsAndRules);
        Assert.Contains("winter+fall",en.StepByStepSolutions);
        Assert.Contains("7+3+13+3=26",en.CommonMistakes);
        Assert.Equal(CommonCoreG2Unit1BarGraphsSourceTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(
                row.Document,row.Lesson));
    }

    [Fact]
    public void OriginalSourceHashAndPreviousVersionAreRequired()
    {
        var row=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons
                .Where(l=>CommonCoreG2Unit1BarGraphsSourceTeachingCorrection
                    .IsTarget(doc,l))
                .Select(l=>(Document:doc,Lesson:l))).Single();
        Assert.True(CommonCoreG2Unit1BarGraphsSourceTeachingCorrection
            .CanUpgradeExisting(row.Document,row.Lesson,
                CommonCoreRichContentCorrections.CorrectionContentVersion));
        Assert.False(CommonCoreG2Unit1BarGraphsSourceTeachingCorrection
            .CanUpgradeExisting(row.Document,row.Lesson,"unreviewed-version"));
        var before=row.Lesson.CanonicalBodySha256;
        try
        {
            row.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit1BarGraphsSourceTeachingCorrection.Apply(row.Document));
        }
        finally {row.Lesson.CanonicalBodySha256=before;}
    }
}
