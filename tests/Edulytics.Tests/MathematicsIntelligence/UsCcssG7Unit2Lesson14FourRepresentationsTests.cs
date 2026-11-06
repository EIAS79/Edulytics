using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U02 L14 A-C authoring fixture; intentionally not executed during A-C.</summary>
public sealed class UsCcssG7Unit2Lesson14FourRepresentationsTests
{
    const string L="PED:US-CCSS-MATH:G7:U02:L14";
    const string Equation="usccss.g7.u02.l08.classify_equations";
    const string Table="usccss.g7.u02.l07.table_proportional_or_not";
    const string Graph="usccss.g7.u02.l10.graphs_origin_slope";

    [Fact]
    public void ContractReusesClosedClassificationFamilies()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(L,out var c));
        Assert.NotNull(c);
        Assert.Contains(Equation,c.AllowedQuestionFamilies);
        Assert.Contains(Table,c.AllowedQuestionFamilies);
        Assert.Contains(Graph,c.AllowedQuestionFamilies);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Equation));
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Table));
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Graph));
    }

    [Fact]
    public void PreviousLessonRemainsResolvable()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(
            "PED:US-CCSS-MATH:G7:U02:L13",out _));
    }
}
