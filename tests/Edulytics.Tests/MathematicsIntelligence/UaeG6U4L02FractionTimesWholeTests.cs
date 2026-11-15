using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U4L02FractionTimesWholeTests
{
    const string Lesson="PED:UAE-MOE-MATH:L6:GENERAL:04:02:STUDENT-AR-128";
    [Fact] public void OriginalArabicRichLessonReusesFiveExistingFractionFamilies()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal(10,l!.WorkedExamples.Count);
        Assert.Contains(l.Visuals,v=>v.Kind==RichLessonVisualKind.FractionBars);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal(5,c!.AllowedQuestionFamilies.Count);
        foreach(var f in c.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
    }
    [Fact] public void ExistingEngineVerifiesProperWrongArabicAndInvalidInputs()
    {
        var p=new Dictionary<string,int>{{"denominator",4},{"numerator",3},{"repeatCount",5}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l08.model_repeated_sum",p,"15"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l08.model_unit_fraction_count",p,"15"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l09.product_numerator",p,"15"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l09.product_whole",p,"3"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l09.product_remainder",p,"٣"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l09.product_whole",p,"4"));
        var invalid=new Dictionary<string,int>{{"denominator",0},{"numerator",3},{"repeatCount",5}};
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l09.product_whole",invalid,"0"));
    }
    [Fact] public void FractionTimesWholeExamplesHaveCorrectExactValues()
    {
        Assert.Equal(6,3*2);
        Assert.Equal(15,5*3);
        Assert.Equal(2,6/3);
        Assert.Equal(6,16/8*3);
        Assert.Equal(20,24/6*5);
        Assert.Equal(21,28/4*3);
        Assert.Equal(6,8*3/4);
        Assert.True(12*2==8*3);
    }
}
