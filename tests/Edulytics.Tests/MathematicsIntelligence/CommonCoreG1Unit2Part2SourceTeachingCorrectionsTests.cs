using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Core.Lessons;
using Edulytics.Data.Seeding;
using Edulytics.Services.LessonContent;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class CommonCoreG1Unit2Part2SourceTeachingCorrectionsTests
{
    private const string Prefix="PED:US-CCSS-MATH:G1:U02:L";
    private static IReadOnlyList<(CanonicalLessonContentPackDocument Doc,
        CanonicalLessonContentPackLesson Lesson,CanonicalLessonContentPackTranslation En)>
        Targets()=>MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons
                .Where(l=>CommonCoreG1Unit2Part2SourceTeachingCorrections.IsTarget(doc,l))
                .Select(l=>(Doc:doc,Lesson:l,En:l.Translations.Single(t=>t.CultureCode=="en"))))
            .OrderBy(x=>x.Lesson.LessonCode,StringComparer.Ordinal).ToArray();

    [Fact]
    public void FourSourceLockedAndDistinctGradeOneLearningGoalsRenderInRichLesson()
    {
        var targets=Targets();
        Assert.Equal(new[]{Prefix+"04",Prefix+"05",Prefix+"06",Prefix+"07"},
            targets.Select(x=>x.Lesson.LessonCode));
        foreach(var t in targets)
        {
            Assert.Equal(CommonCoreG1Unit2Part2SourceTeachingCorrections.CorrectedContentVersion,
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(t.Doc,t.Lesson));
            Assert.Equal("CC BY 4.0",t.Lesson.SourceRights);
            Assert.StartsWith("https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/",
                t.Lesson.SourceUrl);
            Assert.Contains("Step 5:",t.En.StepByStepSolutions);
            Assert.DoesNotContain("The source task",t.En.WorkedExamples);
            Assert.True(t.En.WorkedExamples.Length>310);
            Assert.True(t.En.CommonMistakes.Length>175);
            var rich=RichLessonContentV2RuntimeComposer.TryCompose(t.Lesson.LessonCode,
                new CanonicalLessonTranslationRecord(t.En.CultureCode,t.En.Title,
                    t.En.Explanation,t.En.KeyConceptsAndRules,t.En.WorkedExamples,
                    t.En.StepByStepSolutions,t.En.CommonMistakes,t.En.QuickSummary));
            Assert.NotNull(rich);
            Assert.NotEmpty(rich!.WorkedExamples);
        }
        Assert.Contains("4 + [3] = 7",targets[0].En.WorkedExamples);
        Assert.Contains("9 - 2 = [7]",targets[0].En.WorkedExamples);
        Assert.Contains("Sort and Display",targets[1].En.KeyConceptsAndRules);
        Assert.Contains("Math Stories",targets[1].En.Explanation);
        Assert.Contains("Find the Pair",targets[1].En.KeyConceptsAndRules);
        Assert.Contains("4 + 3 = 7",targets[2].En.WorkedExamples);
        Assert.Contains("3 + 4 = 7",targets[2].En.WorkedExamples);
        Assert.Contains("no joining or leaving action",targets[2].En.KeyConceptsAndRules,
            StringComparison.OrdinalIgnoreCase);
        Assert.Contains("9 = 7 + 2",targets[3].En.WorkedExamples);
        Assert.Contains("9 = 5 + 4",targets[3].En.WorkedExamples);
        Assert.Contains("total before the equals sign",targets[3].En.KeyConceptsAndRules);
        Assert.Equal(3,7-4);
        Assert.Equal(7,9-2);
        Assert.Equal(5,3+2);
        Assert.Equal(9,7+2);
        Assert.Equal(9,5+4);
        Assert.Equal(10,8+2);
    }

    [Fact]
    public void FourRepairsAreIdempotentAndRefuseUnexpectedOriginalSourceOrVersions()
    {
        var targets=Targets();
        foreach(var t in targets)
        {
            var before=CanonicalLessonContentMaterializer.ComputeLessonFingerprint(t.Doc,t.Lesson);
            CanonicalLessonContentMaterializer.Materialize(t.Doc);
            Assert.Equal(before,CanonicalLessonContentMaterializer.ComputeLessonFingerprint(t.Doc,t.Lesson));

            var sha=t.Lesson.CanonicalBodySha256;
            try
            {
                t.Lesson.CanonicalBodySha256=new string('0',64);
                Assert.Throws<InvalidOperationException>(()=>
                    CommonCoreG1Unit2Part2SourceTeachingCorrections.Apply(t.Doc));
            }
            finally {t.Lesson.CanonicalBodySha256=sha;}
            Assert.True(CommonCoreG1Unit2Part2SourceTeachingCorrections.CanUpgradeExisting(
                t.Doc,t.Lesson,CommonCoreRichContentCorrections.CorrectionContentVersion));
            Assert.False(CommonCoreG1Unit2Part2SourceTeachingCorrections.CanUpgradeExisting(
                t.Doc,t.Lesson,"not-the-source-content"));
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG1Unit2Part2SourceTeachingCorrections.ValidateExistingBodyBeforeUpgrade(
                    t.Doc,t.Lesson,"not-the-source-content",[]));
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG1Unit2Part2SourceTeachingCorrections.ValidateExistingBodyBeforeUpgrade(
                    t.Doc,t.Lesson,CommonCoreRichContentCorrections.CorrectionContentVersion,
                    [new CurriculumLessonContentTranslation
                    {
                        CultureCode="en",StepByStepSolutions="Unsafe overwrite",
                        WorkedExamples="Unsafe overwrite"
                    }]));
        }
    }
}
