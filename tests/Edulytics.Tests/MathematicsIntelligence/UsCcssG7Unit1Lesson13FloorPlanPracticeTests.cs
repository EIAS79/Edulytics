using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
// Unit 1 capstone A-C authoring fixtures. NEVER RUN in A-C stage.
public sealed class UsCcssG7Unit1Lesson13FloorPlanPracticeTests
{
    const string Family="usccss.g7.u01.l13.floor_plan_scale_fit";
    const string Lesson="PED:US-CCSS-MATH:G7:U01:L13";
    static Dictionary<string,int> P(int kind,int feature,int style)=>new()
    {["kind"]=kind,["feature"]=feature,["style"]=style};
    [Fact]
    public void ExactCapstoneContractReusesExistingFamilies()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.Equal(new[] {Family,
            "usccss.g7.u01.l09.create_drawing_scale_lengths",
            "usccss.g7.u01.l06.scaled_area_square_factor"},c.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
        Assert.True(LessonPracticeContractRegistry.TryResolve(
            "PED:US-CCSS-MATH:G7:U01:L12",out _));
    }
    [Theory]
    [InlineData(0,0,0,"8")]
    [InlineData(0,0,1,"4")]
    [InlineData(0,1,0,"6")]
    [InlineData(0,1,1,"3")]
    [InlineData(0,2,1,"1/2")]
    [InlineData(0,3,1,"1.5")]
    [InlineData(1,0,0,"0")]
    [InlineData(1,1,0,"1")]
    [InlineData(1,2,0,"0")]
    [InlineData(1,3,1,"1")]
    [InlineData(2,0,0,"1/4")]
    [InlineData(2,3,1,"4")]
    [InlineData(3,0,1,"48")]
    [InlineData(3,1,1,"12")]
    [InlineData(3,2,1,"4")]
    [InlineData(3,3,0,"28")]
    [InlineData(3,3,1,"14")]
    public void FloorPlanNumericWitnesses(int kind,int feature,int style,string answer)
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            Family,P(kind,feature,style),answer));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Family,P(kind,feature,style),"999"));
    }
    [Fact]
    public void MalformedBoundsOrWrongAreaFailClosed()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(4,0,0),"8"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(0,4,0),"8"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(0,0,2),"8"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(2,0,0),"1/2"));
        var extra=P(2,0,0);extra["unverified"]=1;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,extra,"1/4"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            "usccss.g7.u02.l01.foreign",P(0,0,0),"8"));
    }
    [Fact]
    public void ExactCapstoneGeneratorIsSelectedAndSelfVerified()
    {
        var e=new ExactSkillContractQuestionEngine();
        var items=e.Generate("g7-u01-l13",Lesson,new[]{Family},
            ExactSkillQuestionDifficulty.Standard,4,713,Array.Empty<string>());
        Assert.Equal(4,items.Count);
        foreach(var q in items)
        {
            Assert.Equal(Family,q.Family);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                Family,q.Parameters,q.CorrectAnswer));
        }
    }
}
