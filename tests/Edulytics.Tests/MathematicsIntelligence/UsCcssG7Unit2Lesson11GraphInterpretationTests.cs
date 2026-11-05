using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U02 L11 A-C authoring fixture; intentionally not executed during A-C.</summary>
public sealed class UsCcssG7Unit2Lesson11GraphInterpretationTests
{
    const string L="PED:US-CCSS-MATH:G7:U02:L11";
    const string Graph="usccss.g7.u02.l10.graphs_origin_slope";
    const string Equation="usccss.g7.u02.l04.proportional_equations";
    const string Rate="usccss.g7.u02.l03.unit_rates_reciprocal";

    [Fact]
    public void ContractReusesExistingExactGraphEquationAndRateFamilies()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(L,out var c));
        Assert.NotNull(c);
        Assert.Contains(Graph,c.AllowedQuestionFamilies);
        Assert.Contains(Equation,c.AllowedQuestionFamilies);
        Assert.Contains(Rate,c.AllowedQuestionFamilies);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Graph));
    }

    [Fact]
    public void ExistingGraphFamilyContainsExactPointToRateWitness()
    {
        var p=new Dictionary<string,int>{{"kind",2},{"feature",2}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Graph,p,"3/2"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Graph,p,"2/3"));
    }

    [Fact]
    public void PriorLessonContractRemainsResolvable()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(
            "PED:US-CCSS-MATH:G7:U02:L10",out _));
    }
}
