using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U02 L07 source A-B-C acceptance tests. NOT_RUN in authoring.</summary>
public sealed class UsCcssG7Unit2Lesson7ProportionalityTests
{
    const string L="PED:US-CCSS-MATH:G7:U02:L07",F="usccss.g7.u02.l07.table_proportional_or_not";
    static Dictionary<string,int> P(int kind,int feature)=>new()
    {["kind"]=kind,["feature"]=feature};
    [Fact]
    public void SelectedG7ContractAndPriorContractsPreserved()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(L,out var c));
        Assert.NotNull(c);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.Contains(F,c.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(F));
        Assert.True(LessonPracticeContractRegistry.TryResolve(
            "PED:US-CCSS-MATH:G7:U02:L06",out _));
    }
    [Theory]
    [InlineData(0,0,"1")]
    [InlineData(0,1,"1")]
    [InlineData(0,2,"0")]
    [InlineData(1,0,"10")]
    [InlineData(1,3,"106")]
    [InlineData(2,0,"0")]
    [InlineData(2,1,"1")]
    [InlineData(3,0,"0")]
    [InlineData(3,1,"1")]
    [InlineData(4,0,"1")]
    [InlineData(4,1,"0")]
    [InlineData(5,0,"9/5")]
    [InlineData(5,2,"4")]
    [InlineData(6,0,"1")]
    [InlineData(6,1,"0")]
    [InlineData(7,0,"7")]
    [InlineData(7,1,"120")]
    [InlineData(7,2,"1/25")]
    [InlineData(7,3,"5/2")]
    public void ClassifiesSourceEquivalentRatiosCorrectly(int k,int i,string correct)
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(F,P(k,i),correct));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,P(k,i),"999"));
    }
    [Fact]
    public void InvalidInputsAndCrossLessonFamilyFailClosed()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,P(8,0),"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,P(0,4),"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,P(5,0),"1.81"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,P(5,0),"1/0"));
        var q=P(4,0);q["other"]=0;
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,q,"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            "usccss.g7.u02.l08.unapproved",P(0,0),"1"));
    }
    [Fact]
    public void SharedGeneratorOutputsOnlyVerifiedSourceFamily()
    {
        var engine=new ExactSkillContractQuestionEngine();
        var rows=engine.Generate("g7-u02-l07-source",L,[F],
            ExactSkillQuestionDifficulty.Standard,4,7207,Array.Empty<string>());
        Assert.Equal(4,rows.Count);
        foreach(var row in rows)
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                row.Family,row.Parameters,row.CorrectAnswer));
    }
}
