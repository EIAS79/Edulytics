using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Core.Lessons;
using Edulytics.Data.Seeding;
using Edulytics.Services.LessonContent;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssB001U01L02L03SourceTeachingTests
{
    private const string Prefix="PED:US-CCSS-MATH:G1:U01:L";
    private static IReadOnlyList<(CanonicalLessonContentPackDocument Doc,
        CanonicalLessonContentPackLesson Lesson,
        CanonicalLessonContentPackTranslation En)> Targets() =>
        MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(x=>
                CommonCoreG1Unit1L02L03SourceTeachingCorrections.IsTarget(doc,x))
                .Select(x=>(Doc:doc,Lesson:x,En:x.Translations.Single(t=>t.CultureCode=="en"))))
            .OrderBy(x=>x.Lesson.LessonCode,StringComparer.Ordinal).ToArray();

    [Fact]
    public void OnlyHistoricalL02AndL03AreSourceBoundRewrittenAndRichComposable()
    {
        var targets=Targets();
        Assert.Equal(new[]{Prefix+"02",Prefix+"03"},
            targets.Select(x=>x.Lesson.LessonCode));
        foreach(var t in targets)
        {
            Assert.Equal(CommonCoreG1Unit1L02L03SourceTeachingCorrections.CorrectedContentVersion,
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(t.Doc,t.Lesson));
            Assert.StartsWith("https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/",
                t.Lesson.SourceUrl);
            Assert.Equal(new[]{"CCSS:1.OA.C.5","CCSS:1.OA.C.6"},t.Lesson.OutcomeCodes);
            Assert.Contains("Step 5:",t.En.StepByStepSolutions);
            Assert.DoesNotContain("The source task combines",t.En.WorkedExamples);
            Assert.True(t.En.WorkedExamples.Length>400);
            Assert.True(t.En.CommonMistakes.Length>190);
            var rich=RichLessonContentV2RuntimeComposer.TryCompose(t.Lesson.LessonCode,
                new CanonicalLessonTranslationRecord(t.En.CultureCode,t.En.Title,
                    t.En.Explanation,t.En.KeyConceptsAndRules,t.En.WorkedExamples,
                    t.En.StepByStepSolutions,t.En.CommonMistakes,t.En.QuickSummary));
            Assert.NotNull(rich);
            Assert.NotEmpty(rich!.WorkedExamples);
            var fingerprint=CanonicalLessonContentMaterializer.ComputeLessonFingerprint(t.Doc,t.Lesson);
            CanonicalLessonContentMaterializer.Materialize(t.Doc);
            Assert.Equal(fingerprint,CanonicalLessonContentMaterializer.ComputeLessonFingerprint(t.Doc,t.Lesson));
            var previousSha=t.Lesson.CanonicalBodySha256;
            try
            {
                t.Lesson.CanonicalBodySha256=new string('0',64);
                Assert.Throws<InvalidOperationException>(()=>
                    CommonCoreG1Unit1L02L03SourceTeachingCorrections.Apply(t.Doc));
            }
            finally {t.Lesson.CanonicalBodySha256=previousSha;}
            Assert.True(CommonCoreG1Unit1L02L03SourceTeachingCorrections.CanUpgradeExisting(
                t.Doc,t.Lesson,CommonCoreRichContentCorrections.CorrectionContentVersion));
            Assert.False(CommonCoreG1Unit1L02L03SourceTeachingCorrections.CanUpgradeExisting(
                t.Doc,t.Lesson,"unknown-persisted-version"));
        }
        Assert.Contains("Match the dots",targets[0].En.WorkedExamples,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Draw what is missing",targets[0].En.WorkedExamples);
        Assert.Contains("6 + 4 = 10",targets[0].En.WorkedExamples);
        Assert.Contains("Five in a Row",targets[1].En.Explanation);
        Assert.Contains("7 + 2 = 9",targets[1].En.WorkedExamples);
        Assert.Contains("Step 5:",targets[1].En.StepByStepSolutions);
    }

    [Fact]
    public void UnsafeHistoricalVersionOrPersistedLearnerTextMustBeRejected()
    {
        foreach(var t in Targets())
        {
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG1Unit1L02L03SourceTeachingCorrections
                    .ValidateExistingBodyBeforeUpgrade(t.Doc,t.Lesson,"unrecognized-version",[]));
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG1Unit1L02L03SourceTeachingCorrections
                    .ValidateExistingBodyBeforeUpgrade(t.Doc,t.Lesson,
                        CommonCoreRichContentCorrections.CorrectionContentVersion,
                        [new CurriculumLessonContentTranslation {
                            CultureCode="en",
                            WorkedExamples="Incorrect existing learner text",
                            StepByStepSolutions="Unexpected historic content" }]));
            CommonCoreG1Unit1L02L03SourceTeachingCorrections
                .ValidateExistingBodyBeforeUpgrade(t.Doc,t.Lesson,
                    CommonCoreRichContentCorrections.CorrectionContentVersion,[]);
        }
    }
}
