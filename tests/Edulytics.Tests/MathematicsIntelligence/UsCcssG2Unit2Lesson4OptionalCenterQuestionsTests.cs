using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Both composing and non-composing within100 family generation.</summary>
public sealed class UsCcssG2Unit2Lesson4OptionalCenterQuestionsTests
{
    [Fact]
    public void FourSourceFamiliesHaveExactIntegerPresentation()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U02:L04",out var contract));
        Assert.NotNull(contract);
        Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var interaction));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
        }
    }
    [Fact]
    public void VariantArithmeticRespectsTwoDifferentStageRules()
    {
        var engine=new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U02:L04",out var contract));
        foreach(var f in contract!.AllowedQuestionFamilies)
        foreach(var seed in new[]{20261022,20261023})
        {
            var questions=engine.Generate("im2021-g2-u02-l04",f,[f],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,questions.Count);
            foreach(var q in questions)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    f,q.Parameters,"-1"));
                var changed=new Dictionary<string,int>(q.Parameters,StringComparer.Ordinal);
                if(f.EndsWith("subtract_place_value",StringComparison.Ordinal))
                {
                    Assert.True(q.Parameters["left"]%10>=q.Parameters["right"]%10);
                    changed["left"]=9;
                }
                else if(f.EndsWith("capture_squares_add_within20",StringComparison.Ordinal))
                {
                    Assert.InRange(q.Parameters["spinner"],6,10);
                    Assert.InRange(q.Parameters["card"],0,10);
                    changed["spinner"]=11;
                }
                else
                {
                    var ones=q.Parameters["top"]%10+q.Parameters["bottom"]%10;
                    if(f.EndsWith("five_in_row_composing_ten",StringComparison.Ordinal))
                        Assert.True(ones>=10);
                    else Assert.True(ones<10);
                    changed["bottom"]=101;
                }
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    f,changed,q.CorrectAnswer));
            }
        }
    }
}
