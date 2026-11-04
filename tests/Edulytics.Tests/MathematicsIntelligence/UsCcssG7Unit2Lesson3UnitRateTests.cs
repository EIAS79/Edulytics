using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U02 L03 source arithmetic acceptance cases, AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG7Unit2Lesson3UnitRateTests
{
    const string Family="usccss.g7.u02.l03.unit_rates_reciprocal";
    const string Lesson="PED:US-CCSS-MATH:G7:U02:L03";
    static Dictionary<string,int> P(int k,int f)=>new()
    {["kind"]=k,["feature"]=f};
    [Fact]
    public void ExactSourceLessonBindingPreservesEarlierLesson()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.Equal(new[]{Family},c.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
        Assert.True(LessonPracticeContractRegistry.TryResolve(
            "PED:US-CCSS-MATH:G7:U02:L02",out _));
    }
    [Theory]
    [InlineData(0,0,"90")]
    [InlineData(0,1,"125")]
    [InlineData(0,3,"884.9")]
    [InlineData(1,0,"7")]
    [InlineData(1,1,"24.5")]
    [InlineData(1,2,"0.4")]
    [InlineData(1,3,"69.91")]
    [InlineData(2,0,"10")]
    [InlineData(2,1,"1/10")]
    [InlineData(2,2,"100")]
    [InlineData(2,3,"1/100")]
    [InlineData(3,0,"550")]
    [InlineData(3,1,"935")]
    [InlineData(3,2,"36")]
    [InlineData(3,3,"55/6")]
    [InlineData(4,0,"6")]
    [InlineData(4,2,"8")]
    [InlineData(4,3,"3/2")]
    [InlineData(5,1,"1/4")]
    [InlineData(5,2,"12000")]
    [InlineData(6,1,"1/12")]
    [InlineData(6,2,"54")]
    [InlineData(7,0,"2500")]
    [InlineData(7,1,"50")]
    [InlineData(7,3,"2")]
    public void PreciseDirectionalConversions(int kind,int feature,string expected)
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,P(kind,feature),expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(kind,feature),"999"));
    }
    [Fact]
    public void InverseConstantAndAreaErrorsAreRejected()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(2,1),"10"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(2,2),"10"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(3,2),"0.6"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(8,0),"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(0,4),"1"));
        var extra=P(1,0);extra["other"]=1;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,extra,"7"));
    }
    [Fact]
    public void GeneratedExactFamilyIsVerifiableWithNoTeacherScoreClaim()
    {
        var e=new ExactSkillContractQuestionEngine();
        var a=e.Generate("g7-u02-l03",Lesson,new[]{Family},
            ExactSkillQuestionDifficulty.Standard,4,7203,Array.Empty<string>());
        Assert.Equal(4,a.Count);
        foreach(var q in a)
        {
            Assert.Equal(Family,q.Family);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                Family,q.Parameters,q.CorrectAnswer));
        }
    }
}
