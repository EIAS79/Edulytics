using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade5Unit5Lesson13DecimalDivisionTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:05:13";
    [Fact] public void ThreeFamiliesRouteThroughReusedDivisionEngine()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal("number.division_model_word_problem",contract!.SkillId);
        Assert.Equal(3,contract.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            foreach(var item in engine.Generate(Lesson,f,[f],
                ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,item.Parameters,item.CorrectAnswer));
                Assert.DoesNotContain("answer",item.Parameters.Keys);
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,item.Parameters,"-999"));
            }
        }
    }
    [Theory]
    [InlineData(6890,130,"5.3")]
    [InlineData(60,150,"0.04")]
    [InlineData(462,20,"2.31")]
    [InlineData(16500,3,"550")]
    [InlineData(18600,12,"155")]
    public void ExactDivisionPreservesDecimalPosition(int dividendThousandths,int divisorHundredths,string result)
    {
        var p=new Dictionary<string,int>{{"dividendThousandths",dividendThousandths},{"divisorHundredths",divisorHundredths}};
        const string family="uae.g5.u05.l13.exact_decimal_divide";
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,result));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"-1"));
        p["divisorHundredths"]=0;
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,result));
    }
}
