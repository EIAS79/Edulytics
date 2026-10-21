using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Original textbook task and canonical predecessor checks — Stage C only.</summary>
public sealed class CommonCoreG2Unit1QuestionsAboutDataTeachingCorrectionTests
{
    [Fact]
    public void SourceLessonRequiresFourStudentAuthoredQuestionsAndPriorGraphs()
    {
        var row=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons
                .Where(l=>CommonCoreG2Unit1QuestionsAboutDataTeachingCorrection
                    .IsTarget(doc,l))
                .Select(l=>(Document:doc,Lesson:l))).Single();
        Assert.Equal("PED:US-CCSS-MATH:G2:U01:L11",row.Lesson.LessonCode);
        var en=row.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("four new questions",en.Explanation);
        Assert.Contains("TWO graphs",en.StepByStepSolutions);
        Assert.Contains("at least ONE",en.Explanation);
        Assert.Contains("partner",en.QuickSummary);
        Assert.DoesNotContain(@"\underline{\hspace",en.WorkedExamples);
        Assert.Equal(CommonCoreG2Unit1QuestionsAboutDataTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(
                row.Document,row.Lesson));
    }
    [Fact]
    public void OnlyExactOriginalBodyAndPriorCorrectionVersionCanUpgrade()
    {
        var row=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons
                .Where(l=>CommonCoreG2Unit1QuestionsAboutDataTeachingCorrection
                    .IsTarget(doc,l))
                .Select(l=>(Document:doc,Lesson:l))).Single();
        Assert.True(CommonCoreG2Unit1QuestionsAboutDataTeachingCorrection.CanUpgradeExisting(
            row.Document,row.Lesson,
            CommonCoreRichContentCorrections.CorrectionContentVersion));
        Assert.False(CommonCoreG2Unit1QuestionsAboutDataTeachingCorrection.CanUpgradeExisting(
            row.Document,row.Lesson,"unreviewed"));
        var sha=row.Lesson.CanonicalBodySha256;
        try
        {
            row.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit1QuestionsAboutDataTeachingCorrection.Apply(row.Document));
        }
        finally {row.Lesson.CanonicalBodySha256=sha;}
    }
}
