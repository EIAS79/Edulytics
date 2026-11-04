using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
// Source-specific authored cases; NOT_RUN during A-B-C.
public sealed class UsCcssG7Unit1Lesson12EquivalentScaleSourceTests
{
    const string Family="usccss.g7.u01.l12.equivalent_scales_area";
    const string Lesson="PED:US-CCSS-MATH:G7:U01:L12";
    static Dictionary<string,int> P(int k,int f)=>new(){["kind"]=k,["feature"]=f};
    [Fact]
    public void SourceFamilySelectedAndPreviousFamilyRetained()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.Contains(Family,c.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
        Assert.True(LessonPracticeContractRegistry.TryResolve(
            "PED:US-CCSS-MATH:G7:U01:L11",out _));
    }
    [Theory]
    [InlineData(0,0,"160934.4")]
    [InlineData(0,1,"63360")]
    [InlineData(1,0,"1")]
    [InlineData(1,1,"1")]
    [InlineData(1,2,"1")]
    [InlineData(1,3,"0")]
    [InlineData(2,0,"19.8")]
    [InlineData(2,1,"264")]
    [InlineData(2,2,"4.95")]
    [InlineData(2,3,"4000000")]
    [InlineData(3,0,"500")]
    [InlineData(3,1,"80")]
    [InlineData(3,2,"800")]
    [InlineData(3,3,"25")]
    [InlineData(4,0,"104544")]
    [InlineData(4,1,"261.36")]
    [InlineData(4,2,"4000000")]
    [InlineData(4,3,"5")]
    public void ExactMathCases(int kind,int feature,string value)
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,P(kind,feature),value));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(kind,feature),"999"));
    }
    [Fact]
    public void FalseAreaFactorAndInvalidInputsRejected()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(2,3),"2000"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(3,1),"3.2"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(3,1),"1/0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(2,4),"1"));
        var extra=P(0,0);extra["foreign"]=1;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,extra,"160934.4"));
    }
    [Fact]
    public void GeneratedProblemsAreSourceSelectedAndExactVerified()
    {
        var e=new ExactSkillContractQuestionEngine();
        var items=e.Generate("g7-u01-l12",Lesson,new[]{Family},
            ExactSkillQuestionDifficulty.Standard,4,712,Array.Empty<string>());
        Assert.Equal(4,items.Count);
        foreach(var q in items)
        {
            Assert.Equal(Family,q.Family);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                Family,q.Parameters,q.CorrectAnswer));
        }
    }
}
