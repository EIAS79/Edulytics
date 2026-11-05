using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U02 L12 A-C authoring fixture; intentionally not executed during A-C.</summary>
public sealed class UsCcssG7Unit2Lesson12GraphComparisonTests
{
    const string L="PED:US-CCSS-MATH:G7:U02:L12";
    const string F="usccss.g7.u02.l12.compare_same_axes_rates";
    static Dictionary<string,int> P(int kind,int feature)=>new()
    {["kind"]=kind,["feature"]=feature};

    [Fact]
    public void ContractUsesExtendedGraphEngineAndPreservesL10()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(L,out var c));
        Assert.NotNull(c);Assert.Contains(F,c.AllowedQuestionFamilies);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(F));
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(
            "usccss.g7.u02.l10.graphs_origin_slope"));
    }

    [Theory]
    [InlineData(0,0,"5")]
    [InlineData(0,1,"3/2")]
    [InlineData(0,2,"7/4")]
    [InlineData(0,3,"5/2")]
    [InlineData(1,0,"1")]
    [InlineData(1,1,"2")]
    [InlineData(2,0,"5/3")]
    [InlineData(2,1,"3")]
    [InlineData(2,2,"2")]
    [InlineData(2,3,"5/2")]
    [InlineData(3,2,"1")]
    [InlineData(3,3,"2")]
    [InlineData(4,0,"1")]
    [InlineData(4,1,"0")]
    [InlineData(4,2,"1")]
    [InlineData(4,3,"0")]
    public void SameAxesComparisonExactWitnesses(int kind,int feature,string answer)
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(F,P(kind,feature),answer));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,P(kind,feature),"999"));
    }

    [Fact]
    public void MalformedComparisonParametersFailClosed()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,P(5,0),"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,P(0,4),"1"));
        var extra=P(1,0);extra["extra"]=1;
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,extra,"1"));
    }
}
