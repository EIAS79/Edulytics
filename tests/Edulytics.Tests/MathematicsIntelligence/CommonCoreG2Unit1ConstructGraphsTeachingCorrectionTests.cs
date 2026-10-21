using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Source and exact predecessor assertions, Stage C not executed.</summary>
public sealed class CommonCoreG2Unit1ConstructGraphsTeachingCorrectionTests
{
    [Fact]
    public void SourceLessonReplacesPublisherTemplatePlaceholdersWithActualInstruction()
    {
        var row=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons
                .Where(lesson=>CommonCoreG2Unit1ConstructGraphsTeachingCorrection
                    .IsTarget(doc,lesson))
                .Select(lesson=>(Document:doc,Lesson:lesson))).Single();
        Assert.Equal("PED:US-CCSS-MATH:G2:U01:L10",row.Lesson.LessonCode);
        var en=row.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("first picture graph",en.Explanation);
        Assert.Contains("Table A",en.WorkedExamples);
        Assert.Contains("bar graph",en.StepByStepSolutions);
        Assert.DoesNotContain("Table # ___",en.WorkedExamples);
        Assert.DoesNotContain("Caption: A",en.WorkedExamples);
        Assert.Equal(CommonCoreG2Unit1ConstructGraphsTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(
                row.Document,row.Lesson));
    }

    [Fact]
    public void SourceShaCannotDriftAndPredecessorVersionMustMatch()
    {
        var row=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons
                .Where(lesson=>CommonCoreG2Unit1ConstructGraphsTeachingCorrection
                    .IsTarget(doc,lesson))
                .Select(lesson=>(Document:doc,Lesson:lesson))).Single();
        Assert.True(CommonCoreG2Unit1ConstructGraphsTeachingCorrection.CanUpgradeExisting(
            row.Document,row.Lesson,
            CommonCoreRichContentCorrections.CorrectionContentVersion));
        Assert.False(CommonCoreG2Unit1ConstructGraphsTeachingCorrection.CanUpgradeExisting(
            row.Document,row.Lesson,"unreviewed"));
        var sha=row.Lesson.CanonicalBodySha256;
        try
        {
            row.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit1ConstructGraphsTeachingCorrection.Apply(row.Document));
        }
        finally {row.Lesson.CanonicalBodySha256=sha;}
    }
}
