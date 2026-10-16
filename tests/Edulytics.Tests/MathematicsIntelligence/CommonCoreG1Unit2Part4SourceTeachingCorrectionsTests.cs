using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Core.Lessons;
using Edulytics.Data.Seeding;
using Edulytics.Services.LessonContent;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class CommonCoreG1Unit2Part4SourceTeachingCorrectionsTests
{
    private static IReadOnlyList<(CanonicalLessonContentPackDocument Doc,
        CanonicalLessonContentPackLesson Lesson,CanonicalLessonContentPackTranslation En)>
        Targets() =>
        MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons
                .Where(l=>CommonCoreG1Unit2Part4SourceTeachingCorrections.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l,En:l.Translations.Single(t=>t.CultureCode=="en"))))
            .OrderBy(x=>x.Lesson.LessonCode,StringComparer.Ordinal).ToArray();

    [Fact]
    public void FiveOriginalSourceGoalsReplaceGenericAdditionWithExactCompareAndStoryTeaching()
    {
        var selected=Targets();
        Assert.Equal(new[]{
            "PED:US-CCSS-MATH:G1:U02:L11",
            "PED:US-CCSS-MATH:G1:U02:L12",
            "PED:US-CCSS-MATH:G1:U02:L13",
            "PED:US-CCSS-MATH:G1:U02:L14",
            "PED:US-CCSS-MATH:G1:U02:L15"
        },selected.Select(x=>x.Lesson.LessonCode));
        foreach(var row in selected)
        {
            Assert.Equal(CommonCoreG1Unit2Part4SourceTeachingCorrections.CorrectedContentVersion,
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(row.Doc,row.Lesson));
            Assert.Equal("CC BY 4.0",row.Lesson.SourceRights);
            Assert.StartsWith("https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/",
                row.Lesson.SourceUrl);
            Assert.Contains("Step 5:",row.En.StepByStepSolutions);
            Assert.DoesNotContain("The source task",row.En.WorkedExamples);
            Assert.True(row.En.WorkedExamples.Length>400);
            Assert.True(row.En.CommonMistakes.Length>160);
            var rendered=RichLessonContentV2RuntimeComposer.TryCompose(row.Lesson.LessonCode,
                new CanonicalLessonTranslationRecord(row.En.CultureCode,row.En.Title,
                    row.En.Explanation,row.En.KeyConceptsAndRules,row.En.WorkedExamples,
                    row.En.StepByStepSolutions,row.En.CommonMistakes,row.En.QuickSummary));
            Assert.NotNull(rendered);
            Assert.NotEmpty(rendered!.WorkedExamples);
        }
        Assert.Contains("5 + [4] = 9",selected[0].En.WorkedExamples);
        Assert.Contains("10 - 6 = 4",selected[1].En.WorkedExamples);
        Assert.Contains("7 - 4 = 3",selected[2].En.WorkedExamples);
        Assert.Contains("8 - 3 = [5]",selected[3].En.WorkedExamples);
        Assert.Contains("8 - 6 = [2]",selected[4].En.WorkedExamples);
        Assert.Contains("not a real classroom survey",selected[2].En.KeyConceptsAndRules);
        Assert.Equal(4,9-5);
        Assert.Equal(4,10-6);
        Assert.Equal(3,7-4);
        Assert.Equal(5,8-3);
        Assert.Equal(2,8-6);
    }

    [Fact]
    public void CorrectionsAreIdempotentAndRejectSourceHashAndVersionDrift()
    {
        foreach(var item in Targets())
        {
            var before=CanonicalLessonContentMaterializer.ComputeLessonFingerprint(item.Doc,item.Lesson);
            CanonicalLessonContentMaterializer.Materialize(item.Doc);
            Assert.Equal(before,CanonicalLessonContentMaterializer.ComputeLessonFingerprint(item.Doc,item.Lesson));

            var original=item.Lesson.CanonicalBodySha256;
            try
            {
                item.Lesson.CanonicalBodySha256=new string('0',64);
                Assert.Throws<InvalidOperationException>(()=>
                    CommonCoreG1Unit2Part4SourceTeachingCorrections.Apply(item.Doc));
            }
            finally {item.Lesson.CanonicalBodySha256=original;}
            Assert.True(CommonCoreG1Unit2Part4SourceTeachingCorrections.CanUpgradeExisting(
                item.Doc,item.Lesson,CommonCoreRichContentCorrections.CorrectionContentVersion));
            Assert.False(CommonCoreG1Unit2Part4SourceTeachingCorrections.CanUpgradeExisting(
                item.Doc,item.Lesson,"unrelated-content-version"));
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG1Unit2Part4SourceTeachingCorrections.ValidateExistingBodyBeforeUpgrade(
                    item.Doc,item.Lesson,"unrelated-content-version",[]));
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG1Unit2Part4SourceTeachingCorrections.ValidateExistingBodyBeforeUpgrade(
                    item.Doc,item.Lesson,CommonCoreRichContentCorrections.CorrectionContentVersion,
                    [new CurriculumLessonContentTranslation {
                        CultureCode="en",StepByStepSolutions="Unexpected persisted learner text",
                        WorkedExamples="Unexpected persisted worked example"
                    }]));
        }
    }
}
