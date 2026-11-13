using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// Source pp. 809-814. Tests staged, not run.
public sealed class UaeGrade5Unit11Lesson04PredictionTests
{
    [Theory]
    [InlineData("uae.g5.u11.l04.linear_next_prediction")]
    [InlineData("uae.g5.u11.l04.linear_future_prediction")]
    [InlineData("uae.g5.u11.l04.sample_scaled_estimate")]
    public void NumericPredictionRequiresExplicitAssumptionsAndExactArithmetic(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
        Assert.NotNull(ui);
        var engine=new ExactSkillContractQuestionEngine();
        var questions=engine.Generate("PED:UAE-MOE-MATH:L5:GENERAL:11:04",
            family,[family],ExactSkillQuestionDifficulty.Standard,8,20261113,[]);
        foreach(var item in questions)
        {
            var p=item.Parameters;
            int a=p["a"],b=p["b"],m=p["m"];
            int independent=family.EndsWith("linear_next_prediction",StringComparison.Ordinal)?a+4*b:
                family.EndsWith("linear_future_prediction",StringComparison.Ordinal)?a+5*b:(a%7+1)*10*m;
            Assert.Equal(independent.ToString(CultureInfo.InvariantCulture),item.CorrectAnswer);
            Assert.DoesNotContain("result",p.Keys);
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,item.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"99999"));
        }
    }
}
