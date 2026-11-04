using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade 7 L07 source-specific scope; A-C AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG7Unit1Lesson7SelectedPracticeRouteTests
{
    private const string Lesson="PED:US-CCSS-MATH:G7:U01:L07";
    private const string Family="usccss.g7.u01.l07.drawing_actual_distance_units";
    private static Dictionary<string,int> P(int scale,int twice,int direction)=>
        new() {["scale"]=scale,["drawingTwice"]=twice,["direction"]=direction};

    [Fact]
    public void LessonSelectsUnitAwareFamilyAndExistingShapeCounterexample()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        Assert.Equal(new[]{Family,"usccss.g7.u01.l04.angle_and_distance_scaling"},
            c.AllowedQuestionFamilies);
        Assert.All(c.AllowedQuestionFamilies,
            f=>Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f)));
    }

    [Theory]
    [InlineData(0,28,0,"28")]  // 14cm ->28m, NOT 0.14m
    [InlineData(0,9,0,"9")]    // 4.5cm ->9m
    [InlineData(0,9,1,"4.5")]  // 9m bench ->4.5cm
    [InlineData(1,3,0,"7.5")]  // 1.5cm ->7.5m
    [InlineData(2,6,0,"30")]   // 3cm ->30m
    [InlineData(3,12,0,"120")]// 6cm tall structure ->120m
    [InlineData(3,8,1,"4")]   // 80m ->4cm
    public void GivenScaleConvertsDrawingAndActualWithCorrectUnits(
        int scale,int twice,int direction,string expected)
    {
        var parameters=P(scale,twice,direction);
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            Family,parameters,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Family,parameters,expected+"0"));
    }

    [Fact]
    public void EquivalentFractionsAndInvalidParameters()
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,P(0,9,1),"9/2"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(0,28,0),"0.14"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(0,28,0),"14"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(4,28,0),"28"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(0,0,0),"0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(0,41,0),"41"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(0,9,1),"9/0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(0,9,1),"4.5cm"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            "usccss.g7.u01.l08.foreign",P(0,9,1),"4.5"));
    }

    [Fact]
    public void ExactFamilyProducesVerifiableSourceUnitTaskWithoutExpectedAnswer()
    {
        var engine=new ExactSkillContractQuestionEngine();
        var questions=engine.Generate("g7-u01-l07",Lesson,[Family],
            ExactSkillQuestionDifficulty.Standard,3,707,Array.Empty<string>());
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
