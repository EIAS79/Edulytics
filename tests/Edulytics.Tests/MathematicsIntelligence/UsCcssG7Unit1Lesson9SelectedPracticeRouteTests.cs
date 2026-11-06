using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>G7 U01:L09 scoped first-party source tasks; AUTHORING_NOT_RUN.</summary>
public sealed class UsCcssG7Unit1Lesson9SelectedPracticeRouteTests
{
    private const string Lesson="PED:US-CCSS-MATH:G7:U01:L09";
    private const string Family="usccss.g7.u01.l09.create_drawing_scale_lengths";
    private static Dictionary<string,int> P(int kind,int feature,int style)=>
        new() {["kind"]=kind,["feature"]=feature,["style"]=style};

    [Fact]
    public void LessonSelectsDrawingCreationAndReusesPriorDrawingVsActualDistance()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        Assert.Equal(new[]{Family,"usccss.g7.u01.l07.drawing_actual_distance_units"},
            c.AllowedQuestionFamilies);
        Assert.All(c.AllowedQuestionFamilies,
            f=>Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f)));
    }

    [Theory]
    [InlineData(0,0,0,"16")]    // 4m wall ->16cm
    [InlineData(0,1,0,"10")]    // 2.5m ->10cm
    [InlineData(0,2,0,"11")]    // 2.75m ->11cm
    [InlineData(0,3,0,"15")]    // 3.75m ->15cm
    [InlineData(0,2,1,"5.5")]   // reduced plan ->5.5cm
    [InlineData(1,0,0,"5.4")]   // Utah 270mi at 50mi/cm
    [InlineData(1,1,0,"7")]     // 350mi at 50mi/cm
    [InlineData(1,1,1,"14/3")]  // 350mi at 75mi/cm
    [InlineData(1,2,1,"22/15")] // 110mi at 75mi/cm
    [InlineData(2,0,0,"18")]    // book height real
    [InlineData(2,1,1,"3")]     // 1inch draw=3inch real
    [InlineData(3,0,0,"3")]     // flag6ft width at2ft/cm
    [InlineData(3,1,0,"1")]     // flag2ft stripe
    [InlineData(3,2,0,"1/2")]   // flag1ft stripe
    [InlineData(3,0,1,"12")]    // flag width at2cm/ft
    public void SourcePlansPreserveScaleAcrossEveryPart(
        int kind,int feature,int style,string correct)
    {
        var p=P(kind,feature,style);
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,p,correct));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,"999"));
    }

    [Fact]
    public void EquivalentFractionsAndInvalidScalesFailClosed()
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,P(1,0,0),"27/5"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,P(0,2,1),"11/2"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(1,1,1),"4.67"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(3,2,0),"0.25"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(1,1,1),"14/0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(4,0,0),"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(0,4,0),"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(0,0,2),"16"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            "usccss.g7.u01.l10.unapproved",P(0,0,0),"16"));
    }

    [Fact]
    public void ExactSelectedPracticeGeneratesAndVerifiesWithoutExpectedAnswerLeaks()
    {
        var engine=new ExactSkillContractQuestionEngine();
        var items=engine.Generate("g7-u01-l09",Lesson,[Family],
            ExactSkillQuestionDifficulty.Standard,3,709,Array.Empty<string>());
        Assert.Equal(3,items.Count);
        foreach(var item in items)
        {
            Assert.Equal(Family,item.Family);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                Family,item.Parameters,item.CorrectAnswer));
            Assert.DoesNotContain("CorrectAnswer",item.Prompt);
        }
    }
}
