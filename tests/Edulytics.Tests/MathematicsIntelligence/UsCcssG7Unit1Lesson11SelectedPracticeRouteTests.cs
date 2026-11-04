using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>G7 U01 L11 source-selected authored cases; tests not run.</summary>
public sealed class UsCcssG7Unit1Lesson11SelectedPracticeRouteTests
{
    private const string Lesson="PED:US-CCSS-MATH:G7:U01:L11";
    private const string Family="usccss.g7.u01.l11.unitless_scale_and_conversions";
    private static Dictionary<string,int> P(int kind,int feature)=>
        new() {["kind"]=kind,["feature"]=feature};

    [Fact]
    public void LessonSelectsUnitlessExactPracticeAndReusesEquivalentInverseFamily()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        Assert.Equal(new[]{Family,"usccss.g7.u01.l05.factor_size_and_reciprocal"},
            c.AllowedQuestionFamilies);
        Assert.All(c.AllowedQuestionFamilies,
            f=>Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f)));
    }

    [Theory]
    [InlineData(0,0,"1200")] // 12cm at1:100 ->1200cm
    [InlineData(0,3,"10")]   // 1000 actual cm ->10cm draw
    [InlineData(1,0,"350")]  // 7cm drawing leg ->350cm real
    [InlineData(1,1,"700")]  // 14cm drawing height ->700cm real
    [InlineData(1,2,"71/50")]// 71-inch figure at1:50 ->1.42inch
    [InlineData(1,3,"3.5")]  // 350cm real ->3.5m
    [InlineData(2,0,"180")]  //1in:15ft unitless denominator
    [InlineData(2,1,"6")]    //120ft at1in:20ft ->6in
    [InlineData(2,2,"15/4")] //75ft at1in:20ft ->3.75in
    [InlineData(2,3,"5")]    //75ft at1in:15ft ->5in
    [InlineData(3,0,"48")]   //model8in at1:72 =48real feet
    [InlineData(3,1,"576")]  //real inches576
    [InlineData(3,2,"1/25")] //area factor from k5
    [InlineData(3,3,"6")]    //combined k3*k2
    [InlineData(4,0,"1")]    //1:12
    [InlineData(4,1,"1")]    //1/12 to1
    [InlineData(4,2,"0")]    //100 to0.12 not equivalent
    [InlineData(4,3,"1")]    //9:108
    public void AllSameUnitScaleRelationshipsReconstructed(
        int kind,int feature,string expected)
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            Family,P(kind,feature),expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Family,P(kind,feature),"999"));
    }

    [Fact]
    public void ZeroDivisorFalsePrecisionAndForeignValuesRejected()
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,P(1,2),"1.42"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(2,0),"15"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(1,0),"7"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(1,2),"71"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(1,2),"71/0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(5,0),"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(1,4),"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            "usccss.g7.u01.l12.foreign",P(1,2),"1.42"));
    }

    [Fact]
    public void EngineGeneratesAndVerifiesWithoutPublishingHiddenAnswer()
    {
        var engine=new ExactSkillContractQuestionEngine();
        var questions=engine.Generate("g7-u01-l11",Lesson,[Family],
            ExactSkillQuestionDifficulty.Standard,3,711,Array.Empty<string>());
        Assert.Equal(3,questions.Count);
        foreach(var q in questions)
        {
            Assert.Equal(Family,q.Family);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                Family,q.Parameters,q.CorrectAnswer));
            Assert.DoesNotContain("CorrectAnswer",q.Prompt);
        }
    }
}
