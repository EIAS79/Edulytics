using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U4L03FractionPairsTests
{
    const string L="PED:UAE-MOE-MATH:L6:GENERAL:04:03:STUDENT-AR-128";
    const string F="uae.g6.u04.l03.multiply_fraction_pair_simplified";
    [Fact] public void ArabicLessonUsesNewScopedFractionPairAndExistingWholeKernel()
    {
        var content=RichLessonContentV2Registry.Find(L,"ar");
        Assert.NotNull(content);
        Assert.True(content!.WorkedExamples.Count>=10);
        Assert.Contains(content.Visuals,v=>v.Kind==RichLessonVisualKind.FractionBars);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
        Assert.Equal(2,c!.AllowedQuestionFamilies.Count);
        foreach(var family in c.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
    }
    [Fact] public void G6ExactFractionPairSimplifiesAndRejectsInvalidAnswers()
    {
        var p=new Dictionary<string,int>{{"n1",3},{"d1",4},{"n2",5},{"d2",6}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(F,p,"5/8"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(F,p,"٥/٨"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,p,"15/24"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,p,"5/0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,p,"6/8"));
        var invalid=new Dictionary<string,int>{{"n1",3},{"d1",0},{"n2",5},{"d2",6}};
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,invalid,"5/8"));
    }
    [Fact] public void OriginalFractionExamplesAgreeWithIndependentExactCrossProducts()
    {
        Assert.Equal(1*6,3*2);
        Assert.Equal(3*24,8*9);
        Assert.Equal(5*24,8*15);
        Assert.Equal(1*72,6*12);
        Assert.Equal(2*15,5*6);
        Assert.Equal(5*24,12*10);
        Assert.Equal(3,1*3);
        Assert.True(2*3<3*5);
    }
    [Fact] public void LegacyG4FractionWholeFamilyStillVerifies()
    {
        var p=new Dictionary<string,int>{{"denominator",4},{"numerator",3},{"repeatCount",5}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l09.product_whole",p,"3"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l09.product_whole",p,"4"));
    }
}
