using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Source-specific Grade 7 U01 L06; AUTHORING_NOT_RUN.</summary>
public sealed class UsCcssG7Unit1Lesson6SelectedPracticeRouteTests
{
    private const string Lesson="PED:US-CCSS-MATH:G7:U01:L06";
    private const string Family="usccss.g7.u01.l06.scaled_area_square_factor";
    private static Dictionary<string,int> P(int kind,int units,int factor,int asked=0) =>
        new() {["kind"]=kind,["units"]=units,["factor"]=factor,["asked"]=asked};

    [Fact]
    public void OptionalLessonStillHasSelectedExactGrade7AreaContract()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        Assert.Equal(new[] {Family},c.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
    }

    [Theory]
    [InlineData(0,1,1,0,"4")]    // unit block, k2 = 4 area
    [InlineData(0,3,2,0,"27")]   // three blocks, k3
    [InlineData(0,2,3,0,"32")]   // two blocks, k4
    [InlineData(1,1,4,0,"3/2")]  // triangle area6, k1/2
    [InlineData(1,1,5,0,"2/3")]  // triangle area6, k1/3
    [InlineData(1,1,6,0,"54/25")]// triangle area6, k3/5
    [InlineData(2,1,2,0,"72")]   // parallelogram area8, k3
    [InlineData(0,1,6,1,"9/25")] // area factor for k3/5
    [InlineData(0,3,4,1,"1/4")]  // area factor for k1/2
    public void AllAreasUseTheSquareOfTheLengthFactor(
        int kind,int units,int factor,int asked,string expected)
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            Family,P(kind,units,factor,asked),expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Family,P(kind,units,factor,asked),"99/99"));
    }

    [Fact]
    public void EquivalentFractionsAndDecimalAnswersAreMathematicallyEquivalent()
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,P(1,1,4),"6/4"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,P(1,1,4),"1.5"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,P(1,1,5),"4/6"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(1,1,5),"0.67"));
    }

    [Fact]
    public void MalformedOrExcessiveInputsFailClosed()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(1,1,7),"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(3,1,0),"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(0,0,1),"4"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(0,1,1),"1/0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(0,1,1),"99999999999999999999999999999999999"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            "usccss.g7.u01.l07.foreign",P(0,1,1),"4"));
    }

    [Fact]
    public void SelectedEngineGeneratesItsOwnMathematicallyVerifiableAnswers()
    {
        var engine=new ExactSkillContractQuestionEngine();
        var q=engine.Generate("g7-u01-l06",Lesson,[Family],
            ExactSkillQuestionDifficulty.Standard,3,706,Array.Empty<string>());
        Assert.Equal(3,q.Count);
        foreach(var item in q)
        {
            Assert.Equal(Family,item.Family);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                Family,item.Parameters,item.CorrectAnswer));
            Assert.DoesNotContain("CorrectAnswer",item.Prompt);
        }
    }
}
