using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Written for Stage D; not run during A-B-C.
public sealed class UaeGrade5Unit5Lesson14PowerTenDivisionTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:05:14";
    [Fact] public void ExistingDecimalSkillRoutesAllThreeFamilies()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal("supporting.decimals.core",contract!.SkillId);
        Assert.Equal(3,contract.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            foreach(var item in engine.Generate(Lesson,family,[family],
                ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer));
        }
    }
    [Theory]
    [InlineData(219500,1,"21.95")]
    [InlineData(190500,2,"1.905")]
    [InlineData(36400,2,"0.364")]
    [InlineData(12,2,"0.00012")]
    [InlineData(6720,2,"0.0672")]
    public void PowerOfTenDivisionKeepsAllRequiredDecimalPlaces(int valueThousandths,int exponent,string answer)
    {
        var p=new Dictionary<string,int>{{"thousandths",valueThousandths},{"pow",exponent}};
        const string f="uae.g5.u05.l14.decimal_divide_power_ten";
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,answer));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"-1"));
        p["pow"]=0;
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,answer));
    }
}
