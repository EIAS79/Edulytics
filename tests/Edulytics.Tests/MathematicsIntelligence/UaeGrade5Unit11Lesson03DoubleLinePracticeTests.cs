using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// Source: UAE Grade5 Arabic pp. 803–808. Stage D NOT_RUN.
public sealed class UaeGrade5Unit11Lesson03DoubleLinePracticeTests
{
    [Theory]
    [InlineData("uae.g5.u11.l03.double_line_final_difference")]
    [InlineData("uae.g5.u11.l03.double_line_growth")]
    public void DualSeriesComparisonIsExactlyGraded(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
        Assert.NotNull(ui);
        var engine=new ExactSkillContractQuestionEngine();
        var cases=engine.Generate("PED:UAE-MOE-MATH:L5:GENERAL:11:03",
            family,[family],ExactSkillQuestionDifficulty.Standard,8,20261113,[]);
        foreach(var item in cases)
        {
            var p=item.Parameters;
            int correct=family.EndsWith("double_line_final_difference",StringComparison.Ordinal)
                ?Math.Abs(p["d"]-p["h"]):p["h"]-p["e"];
            Assert.Equal(correct.ToString(CultureInfo.InvariantCulture),item.CorrectAnswer);
            Assert.DoesNotContain("result",p.Keys);
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,item.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"9999"));
        }
    }
}
