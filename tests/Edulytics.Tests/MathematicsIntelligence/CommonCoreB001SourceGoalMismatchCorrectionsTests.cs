using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Core.Lessons;
using Edulytics.Data.Seeding;
using Edulytics.Services.LessonContent;
namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// No academic approval is implied by these technical checks.
/// Proof of exact 2021 learning goals: original teacher preparation and full activities on Our-CS.
/// </summary>
public sealed class CommonCoreB001SourceGoalMismatchCorrectionsTests
{
    private const string G2Compare = "PED:US-CCSS-MATH:G2:U02:L01";
    private const string G2Ribbon = "PED:US-CCSS-MATH:G2:U03:L12";
    private const string G3Represent = "PED:US-CCSS-MATH:G3:U01:L12";
    private const string G3Factors = "PED:US-CCSS-MATH:G3:U01:L15";

    private static IReadOnlyList<(CanonicalLessonContentPackDocument Doc,
        CanonicalLessonContentPackLesson Lesson,
        CanonicalLessonContentPackTranslation En)> Targets() =>
        MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(x=>
                CommonCoreB001SourceGoalMismatchCorrections.IsTarget(doc,x))
                .Select(x=>(Doc:doc,Lesson:x,En:x.Translations.Single(t=>t.CultureCode=="en"))))
            .OrderBy(x=>x.Lesson.LessonCode,StringComparer.Ordinal)
            .ToArray();

    [Fact]
    public void ExactFourSourceGoalRepairsAreEffectiveAndRenderAsRichLessons()
    {
        var ts=Targets();
        Assert.Equal(new[]{G2Compare,G2Ribbon,G3Represent,G3Factors},
            ts.Select(x=>x.Lesson.LessonCode).ToArray());
        foreach(var t in ts)
        {
            Assert.StartsWith("https://im.kendallhunt.com/k5/teachers/grade-",t.Lesson.SourceUrl);
            Assert.Equal("CC BY 4.0",t.Lesson.SourceRights);
            Assert.Equal(CommonCoreB001SourceGoalMismatchCorrections.CorrectedContentVersion,
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(t.Doc,t.Lesson));
            Assert.Contains("Step 5:",t.En.StepByStepSolutions);
            Assert.DoesNotContain("The source task",t.En.WorkedExamples,StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("Measurement units must be equal",t.En.KeyConceptsAndRules);
            Assert.Contains("Learning goal:",t.En.KeyConceptsAndRules);
            Assert.True(t.En.WorkedExamples.Length>300);
            Assert.True(t.En.CommonMistakes.Length>200);
            var rich=RichLessonContentV2RuntimeComposer.TryCompose(t.Lesson.LessonCode,
                new CanonicalLessonTranslationRecord(
                    t.En.CultureCode,t.En.Title,t.En.Explanation,
                    t.En.KeyConceptsAndRules,t.En.WorkedExamples,
                    t.En.StepByStepSolutions,t.En.CommonMistakes,t.En.QuickSummary));
            Assert.NotNull(rich);
            Assert.NotEmpty(rich!.WorkedExamples);
        }
        var cmp=ts[0].En;
        Assert.Contains("without making or breaking a ten",cmp.Explanation);
        Assert.Contains("46 − 22",cmp.WorkedExamples);
        Assert.Contains("34 + 23",cmp.WorkedExamples);
        Assert.DoesNotContain("32-18",cmp.WorkedExamples);
        var ribbon=ts[1].En;
        Assert.Contains("86 − 24 = 62 cm",ribbon.WorkedExamples);
        Assert.Contains("62 − 18 = 44 cm",ribbon.WorkedExamples);
        Assert.Contains("tape diagram",ribbon.StepByStepSolutions);
        var represent=ts[2].En;
        Assert.Contains("4 × 3 = 12",represent.WorkedExamples);
        Assert.Contains("group",represent.StepByStepSolutions,StringComparison.OrdinalIgnoreCase);
        var factors=ts[3].En;
        Assert.Contains("4 × [ ] = 20",factors.WorkedExamples);
        Assert.Contains("[ ] × 6 = 18",factors.WorkedExamples);
        Assert.Contains("multiplication equation",factors.StepByStepSolutions);
        Assert.DoesNotContain("Write an addition relationship",factors.StepByStepSolutions);
        Assert.DoesNotContain("Measurement units must be equal",factors.KeyConceptsAndRules);
    }

    [Fact]
    public void IndependentArithmeticAndMissingFactorChecks()
    {
        Assert.Equal(24,46-22);
        Assert.Equal(68,46+22);
        Assert.Equal(57,34+23);
        Assert.Equal(43,68-25);
        Assert.Equal(62,86-24);
        Assert.Equal(44,62-18);
        Assert.Equal(86,24+18+44);
        Assert.Equal(60,26+34);
        Assert.Equal(12,4*3);
        Assert.Equal(10,5*2);
        Assert.Equal(18,3*6);
        Assert.Equal(20,4*5);
        Assert.Equal(30,3*10);
        Assert.Equal(3,18/6);
        Assert.Equal(5,20/4);
    }

    [Fact]
    public void RepairsRemainStableAndDoNotTouchOtherLessons()
    {
        var ts=Targets();
        foreach(var t in ts)
        {
            var sha=CanonicalLessonContentMaterializer.ComputeLessonFingerprint(t.Doc,t.Lesson);
            CanonicalLessonContentMaterializer.Materialize(t.Doc);
            Assert.Equal(sha,CanonicalLessonContentMaterializer.ComputeLessonFingerprint(t.Doc,t.Lesson));
        }
        var other=Assert.Single(ts[0].Doc.Lessons,
            l=>l.LessonCode=="PED:US-CCSS-MATH:G2:U02:L02");
        Assert.False(CommonCoreB001SourceGoalMismatchCorrections.IsTarget(ts[0].Doc,other));
    }

    [Fact]
    public void UnfamiliarSourceOrPriorProductionDataFailsClosed()
    {
        foreach(var t in Targets())
        {
            var original=t.Lesson.CanonicalBodySha256;
            try
            {
                t.Lesson.CanonicalBodySha256=new string('0',64);
                Assert.Throws<InvalidOperationException>(()=>
                    CommonCoreB001SourceGoalMismatchCorrections.Apply(t.Doc));
            }
            finally{t.Lesson.CanonicalBodySha256=original;}

            Assert.True(CommonCoreB001SourceGoalMismatchCorrections.CanUpgradeExisting(
                t.Doc,t.Lesson,CommonCoreRichContentCorrections.CorrectionContentVersion));
            Assert.False(CommonCoreB001SourceGoalMismatchCorrections.CanUpgradeExisting(
                t.Doc,t.Lesson,"different-prose"));
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreB001SourceGoalMismatchCorrections.ValidateExistingBodyBeforeUpgrade(
                    t.Doc,t.Lesson,"different-prose",[]));
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreB001SourceGoalMismatchCorrections.ValidateExistingBodyBeforeUpgrade(
                    t.Doc,t.Lesson,CommonCoreRichContentCorrections.CorrectionContentVersion,
                    [new CurriculumLessonContentTranslation{
                        CultureCode="en",StepByStepSolutions="Unexpected",WorkedExamples="Unexpected"
                    }]));
            CommonCoreB001SourceGoalMismatchCorrections.ValidateExistingBodyBeforeUpgrade(
                t.Doc,t.Lesson,CommonCoreRichContentCorrections.CorrectionContentVersion,[]);
        }
    }
}
