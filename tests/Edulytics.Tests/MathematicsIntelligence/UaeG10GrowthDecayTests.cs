using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Edulytics.Core.Enums;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG10GrowthDecayTests
{
    const string P="uae.g10.general.u05.l09."; const string Code="PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U05:L09:C050";
    [Theory]
    [InlineData("growth_decay_value",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("half_life_value",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("rate_from_factor",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("mixed_linear_exponential_value",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("first_target_period",LessonPracticeInteractionKind.IntegerKeypad)]
    public void FamilyDispatch(string suffix,LessonPracticeInteractionKind kind)
    {
        string family=P+suffix; Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var contract));Assert.Equal(kind,contract!.InteractionKind);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(int seed in new[]{20261125,20261126,20261127})foreach(var task in engine.Generate(Code,Code,[family],ExactSkillQuestionDifficulty.Standard,4,seed,[]))
        { Assert.True(ExactSkillContractQuestionEngine.Verify(family,task.Parameters,task.CorrectAnswer));Assert.False(ExactSkillContractQuestionEngine.Verify(family,task.Parameters,"wrong")); }
    }
    [Fact] public void IndependentGoldens()
    {
        var p=new Dictionary<string,int>{{"initial",800},{"rate",12},{"direction",0},{"time",2},{"linear",500}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(P+"growth_decay_value",p,"٦١٩٫٥٢"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(P+"growth_decay_value",p,"619.53"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(P+"mixed_linear_exponential_value",p,"1619.52"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(P+"first_target_period",p,"3"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(P+"first_target_period",p,"2"));
        var h=new Dictionary<string,int>{{"initial",160},{"half",8},{"time",12}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(P+"half_life_value",h,"56.57"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(P+"half_life_value",h,"56.56"));
    }
}
