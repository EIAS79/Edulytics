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
        Assert.Equal(new[]{G3Represent,G3Factors},
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
        // The two Grade2 B001 identities were superseded by later SHA-guarded
        // original-source corrections. Preserve the actual source math while
        // asserting they no longer route through this legacy B001 compatibility layer.
        var g2=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(l=>l.LessonCode==G2Compare||l.LessonCode==G2Ribbon)
                .Select(l=>(Doc:doc,Lesson:l,En:l.Translations.Single(t=>t.CultureCode=="en"))))
            .OrderBy(x=>x.Lesson.LessonCode,StringComparer.Ordinal).ToArray();
        Assert.Equal(new[]{G2Compare,G2Ribbon},g2.Select(x=>x.Lesson.LessonCode));
        foreach(var row in g2)
        {
            Assert.False(CommonCoreB001SourceGoalMismatchCorrections.IsTarget(row.Doc,row.Lesson));
            Assert.NotEqual(CommonCoreB001SourceGoalMismatchCorrections.CorrectedContentVersion,
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(row.Doc,row.Lesson));
            Assert.StartsWith("https://im.kendallhunt.com/k5/teachers/grade-",row.Lesson.SourceUrl);
        }
        var g2CompareMath=string.Join(" ",g2[0].En.Explanation,g2[0].En.WorkedExamples,g2[0].En.KeyConceptsAndRules);
        Assert.Contains("46",g2CompareMath);
        Assert.Contains("34",g2CompareMath);
        var g2RibbonMath=string.Join(" ",g2[1].En.Explanation,g2[1].En.WorkedExamples,g2[1].En.StepByStepSolutions);
        // U03 L12 Friendship Bracelets: Lin's ACTUAL 92 cm, 35 cm and 28 cm
        // produce 29 cm remaining. The historic B001 86−24=62 is not the
        // publisher original; 86 belongs to separately authored first-party work.
        Assert.Contains("92",g2RibbonMath);
        Assert.Contains("35",g2RibbonMath);
        Assert.Contains("28",g2RibbonMath);
        Assert.Contains("29",g2RibbonMath);
        Assert.Contains("FIRST-PARTY",g2RibbonMath);
        var represent=ts[0].En;
        Assert.Contains("4 × 3 = 12",represent.WorkedExamples);
        Assert.Contains("group",represent.StepByStepSolutions,StringComparison.OrdinalIgnoreCase);
        var factors=ts[1].En;
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
        var gradeTwo=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Single(doc=>doc.Lessons.Any(l=>l.LessonCode==G2Compare));
        var other=Assert.Single(gradeTwo.Lessons,
            l=>l.LessonCode=="PED:US-CCSS-MATH:G2:U02:L02");
        Assert.False(CommonCoreB001SourceGoalMismatchCorrections.IsTarget(gradeTwo,other));
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
