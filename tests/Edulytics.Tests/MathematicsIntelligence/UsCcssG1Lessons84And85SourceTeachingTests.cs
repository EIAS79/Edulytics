using Edulytics.Core.Curriculum;
using Edulytics.Data.Seeding;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG1Lessons84And85SourceTeachingTests
{
    [Fact]
    public void Original2021CentersAndEstimationAreInMaterializedTeaching()
    {
        var doc=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Single(x=>x.Lessons.Any(l=>l.LessonCode=="PED:US-CCSS-MATH:G1:U04:L19"));
        CanonicalLessonContentMaterializer.Materialize(doc);
        var lesson19=doc.Lessons.Single(l=>l.LessonCode=="PED:US-CCSS-MATH:G1:U04:L19");
        var lesson20=doc.Lessons.Single(l=>l.LessonCode=="PED:US-CCSS-MATH:G1:U04:L20");
        var l19=lesson19.Translations.Single(t=>t.CultureCode=="en");
        var l20=lesson20.Translations.Single(t=>t.CultureCode=="en");
        Assert.Contains("Greatest of Them All, Get Your Numbers in Order, or Grab and Count",
            l19.WorkedExamples);
        Assert.Contains("FIVE existing ten-towers and FIFTEEN loose ones",l19.Explanation);
        Assert.Contains("TOO LOW, ABOUT RIGHT and TOO HIGH",l20.Explanation);
        Assert.Contains("76 cubes",l20.Explanation);
        Assert.Contains("Bag A has five tens and two ones (52)",l20.StepByStepSolutions);
        Assert.Equal(CommonCoreG1FullGradeFinalUnit4Corrections.OriginalLesson19SourceContentVersion,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(doc,lesson19));
        Assert.Equal(CommonCoreG1FullGradeFinalUnit4Corrections.OriginalLesson20SourceContentVersion,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(doc,lesson20));
        Assert.True(CommonCoreG1FullGradeFinalUnit4Corrections.CanUpgradeExisting(
            doc,lesson19,CommonCoreG1FullGradeFinalUnit4Corrections.Version));
        Assert.True(CommonCoreG1FullGradeFinalUnit4Corrections.CanUpgradeExisting(
            doc,lesson20,CommonCoreG1FullGradeFinalUnit4Corrections.Version));
        Assert.False(CommonCoreG1FullGradeFinalUnit4Corrections.CanUpgradeExisting(
            doc,lesson19,"invalid-upgrade"));
    }

    [Fact]
    public void Original2021FourMysteryBagsAndFlexible37RemainGradeable()
    {
        const string f="usccss.g1.u04.l20.equivalent_tens_ones";
        foreach(var (mode,a,answer) in new[]
        {
            (4,52,"NUMBER=52"),
            (5,65,"NUMBER=65"),
            (6,49,"TENS=2"),
            (7,36,"ONES=16")
        })
        {
            var p=new Dictionary<string,int>{{"a",a},{"b",0},{"c",0},{"mode",mode}};
            Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Valid(f,p));
            Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Verify(f,p,answer));
            Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(f,p,answer+";FALSE=1"));
            p["a"]++;
            Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Valid(f,p));
        }
        const string regroup="usccss.g1.u04.l19.regroup_one_ten";
        var task=new Dictionary<string,int>{{"a",37},{"b",0},{"c",0},{"mode",1}};
        Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Verify(regroup,task,
            "TENS=0;ONES=37;ALT_TENS=2;ALT_ONES=17"));
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(regroup,task,
            "TENS=0;ONES=37;ALT_TENS=0;ALT_ONES=37"));
    }
}
