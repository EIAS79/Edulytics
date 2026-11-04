using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U02 L13 A-C authoring fixture; intentionally not executed during A-C.</summary>
public sealed class UsCcssG7Unit2Lesson13ReciprocalGraphTests
{
    const string L="PED:US-CCSS-MATH:G7:U02:L13";
    const string Reciprocal="usccss.g7.u02.l05.reciprocal_proportional_equations";
    const string Equation="usccss.g7.u02.l04.proportional_equations";
    const string Rate="usccss.g7.u02.l03.unit_rates_reciprocal";

    [Fact]
    public void ContractReusesReciprocalEquationAndRateFamilies()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(L,out var c));
        Assert.NotNull(c);
        Assert.Contains(Reciprocal,c.AllowedQuestionFamilies);
        Assert.Contains(Equation,c.AllowedQuestionFamilies);
        Assert.Contains(Rate,c.AllowedQuestionFamilies);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Reciprocal));
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Equation));
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Rate));
    }

    [Fact]
    public void PreviousLessonRemainsResolvable()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(
            "PED:US-CCSS-MATH:G7:U02:L12",out _));
    }
}
