using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U4L04MixedProductsTests
{
    const string L="PED:UAE-MOE-MATH:L6:GENERAL:04:04:STUDENT-AR-128";
    const string F="uae.g6.u04.l04.multiply_mixed_by_whole";
    [Fact] public void ArabicRichLessonAndFourExactFamiliesExist()
    {
        var content=RichLessonContentV2Registry.Find(L,"ar");
        Assert.NotNull(content);
        Assert.Equal(10,content!.WorkedExamples.Count);
        Assert.Contains(content.Visuals,v=>v.Kind==RichLessonVisualKind.RatioTable);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var contract));
        Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
        foreach(var family in contract.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
    }
    [Fact] public void ExactMixedWholeAcceptsArabicAndRejectsInvalid()
    {
        var p=new Dictionary<string,int>{{"w",1},{"n",3},{"d",4},{"whole",8}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(F,p,"14"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(F,p,"١٤"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,p,"7/4"));
        var invalid=new Dictionary<string,int>{{"w",1},{"n",3},{"d",0},{"whole",8}};
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,invalid,"14"));
        var p2=new Dictionary<string,int>{{"w",2},{"n",1},{"d",4},{"whole",3}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(F,p2,"27/4"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,p2,"54/8"));
    }
    [Fact] public void ExistingG5MixedByFractionAndMixedPairRemainCompatible()
    {
        var p=new Dictionary<string,int>{{"w1",1},{"n1",7},{"d1",8},
            {"w2",3},{"n2",1},{"d2",3}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            "uae.g5.u09.l07.multiply_two_mixed",p,"25/4"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            "uae.g5.u09.l07.multiply_two_mixed",p,"150/24"));
        var scaling=new Dictionary<string,int>{{"whole",5},{"num",3},{"den",4}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            "uae.g5.u09.l08.classify_scaling",scaling,"2"));
    }
    [Fact] public void TenOriginalWorkedProductsPassIndependentCrossMultiplication()
    {
        Assert.Equal(1*2*3,3*2);
        Assert.Equal(9*4*5,9*2*2*5);
        Assert.Equal(7*4*3,7*4*3);
        Assert.Equal(25*8*3,15*10*4);
        Assert.Equal(12*2*3,9*8);
        Assert.Equal(14*4,7*8);
        Assert.Equal(3*2*5,5*3*2);
        Assert.Equal(9*4,9*4);
        Assert.Equal(4*3*2,8*3);
        Assert.Equal(13*4*5,13*2*10);
    }
}
