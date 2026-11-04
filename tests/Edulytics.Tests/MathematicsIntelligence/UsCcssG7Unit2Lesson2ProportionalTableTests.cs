using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Grade7 U02 L02 source exact test cases, NOT_RUN during A-C.</summary>
public sealed class UsCcssG7Unit2Lesson2ProportionalTableTests
{
    private const string Family="usccss.g7.u02.l02.proportional_tables";
    private const string Lesson="PED:US-CCSS-MATH:G7:U02:L02";
    static Dictionary<string,int> P(int k,int f)=>new()
    {["kind"]=k,["feature"]=f};
    [Fact]
    public void L02HasNewSelectedFamilyAndRetainsL01()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.Equal(new[]{Family},c.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
        Assert.True(LessonPracticeContractRegistry.TryResolve(
            "PED:US-CCSS-MATH:G7:U02:L01",out _));
    }
    [Theory]
    [InlineData(0,0,"2")]
    [InlineData(0,1,"24")]
    [InlineData(0,2,"12")]
    [InlineData(0,3,"0")]
    [InlineData(1,0,"30")]
    [InlineData(1,1,"15")]
    [InlineData(1,2,"3")]
    [InlineData(2,0,"15")]
    [InlineData(2,1,"20")]
    [InlineData(2,2,"56")]
    [InlineData(2,3,"1/2")]
    [InlineData(3,0,"25")]
    [InlineData(3,1,"65/4")]
    [InlineData(3,2,"16")]
    [InlineData(3,3,"5/4")]
    [InlineData(4,0,"15")]
    [InlineData(4,1,"35")]
    [InlineData(4,2,"2.5")]
    [InlineData(5,1,"12")]
    [InlineData(5,3,"3/2")]
    [InlineData(6,0,"3/7")]
    [InlineData(6,3,"9")]
    [InlineData(7,0,"3")]
    [InlineData(7,1,"1")]
    [InlineData(7,2,"0")]
    public void IndependentlySupportedSourceTableArithmetic(int kind,int f,string ans)
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,P(kind,f),ans));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(kind,f),"999"));
    }
    [Fact]
    public void PreventsAnomalyFalseProportionalityAndForeignParameters()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(0,3),"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(3,1),"16"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(8,0),"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(1,4),"1"));
        var extra=P(1,0);extra["wrong"]=12;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,extra,"30"));
    }
    [Fact]
    public void ExactSelectedEngineGeneratesAndSelfVerifies()
    {
        var engine=new ExactSkillContractQuestionEngine();
        var questions=engine.Generate("g7-u02-l02",Lesson,new[]{Family},
            ExactSkillQuestionDifficulty.Standard,4,7202,Array.Empty<string>());
        Assert.Equal(4,questions.Count);
        foreach(var q in questions)
        {
            Assert.Equal(Family,q.Family);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                Family,q.Parameters,q.CorrectAnswer));
        }
    }
}
