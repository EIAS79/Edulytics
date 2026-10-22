using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage C only — original source fields and SHA locks, not executed.</summary>
public sealed class CommonCoreG2Unit1ThreeUnknownPositionsTeachingCorrectionTests
{
    [Fact]
    public void Grade2ShellStoryUsesUnknownSmallerButSupportsAllPositions()
    {
        var row=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(lesson=>
                CommonCoreG2Unit1ThreeUnknownPositionsTeachingCorrection.IsTarget(doc,lesson))
                .Select(lesson=>(Doc:doc,Lesson:lesson))).Single();
        Assert.Equal("PED:US-CCSS-MATH:G2:U01:L15",row.Lesson.LessonCode);
        var en=row.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("Diego has 55 shells",en.Explanation);
        Assert.Contains("UNKNOWN LARGER",en.KeyConceptsAndRules);
        Assert.Contains("UNKNOWN SMALLER",en.KeyConceptsAndRules);
        Assert.Contains("UNKNOWN DIFFERENCE",en.KeyConceptsAndRules);
        Assert.Contains("55−20=35",en.WorkedExamples);
        Assert.Contains("multiple equations",en.CommonMistakes,StringComparison.OrdinalIgnoreCase);
        Assert.Equal(CommonCoreG2Unit1ThreeUnknownPositionsTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(row.Doc,row.Lesson));
    }

    [Fact]
    public void ExactCanonicalSourceAndPriorRichVersionAreRequired()
    {
        var row=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(lesson=>
                CommonCoreG2Unit1ThreeUnknownPositionsTeachingCorrection.IsTarget(doc,lesson))
                .Select(lesson=>(Doc:doc,Lesson:lesson))).Single();
        Assert.True(CommonCoreG2Unit1ThreeUnknownPositionsTeachingCorrection
            .CanUpgradeExisting(row.Doc,row.Lesson,
                CommonCoreRichContentCorrections.CorrectionContentVersion));
        Assert.False(CommonCoreG2Unit1ThreeUnknownPositionsTeachingCorrection
            .CanUpgradeExisting(row.Doc,row.Lesson,"changed-version"));
        var before=row.Lesson.CanonicalBodySha256;
        try
        {
            row.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit1ThreeUnknownPositionsTeachingCorrection
                    .Apply(row.Doc));
        }
        finally {row.Lesson.CanonicalBodySha256=before;}
    }
}
