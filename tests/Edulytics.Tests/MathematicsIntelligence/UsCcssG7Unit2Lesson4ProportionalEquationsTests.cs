using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>OUR/IM G7 U02 L04 A-B-C authored fixtures; NOT_RUN in this stage.</summary>
public sealed class UsCcssG7Unit2Lesson4ProportionalEquationsTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G7:U02:L04";
    private const string Family = "usccss.g7.u02.l04.proportional_equations";
    private static Dictionary<string,int> P(int mode,int feature,int variant=0)=>new()
    {
        ["mode"]=mode,["feature"]=feature,["variant"]=variant
    };
    [Fact]
    public void SourceSelectedExactFamiliesAndPreviousLessonsRemainIntact()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.Contains(Family,c.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
        Assert.True(LessonPracticeContractRegistry.TryResolve(
            "PED:US-CCSS-MATH:G7:U02:L03",out _));
    }

    [Theory]
    [InlineData(0,0,0,"3")]
    [InlineData(0,1,0,"1/2")]
    [InlineData(0,2,0,"610")]
    [InlineData(0,3,0,"5/4")]
    [InlineData(1,3,0,"129")]
    [InlineData(2,3,0,"25/2")]
    [InlineData(3,2,0,"1525")]
    [InlineData(4,2,0,"75/4")]
    [InlineData(5,0,0,"1/3")]
    [InlineData(5,1,0,"2")]
    [InlineData(5,2,0,"1/610")]
    [InlineData(5,3,0,"4/5")]
    [InlineData(6,1,0,"1")]
    [InlineData(6,1,1,"0")]
    public void DirectionalUnitRateAndEquationCheckingIsExact(
        int mode,int feature,int variant,string correct)
    {
        var parameters=P(mode,feature,variant);
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            Family,parameters,correct));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Family,parameters,"999"));
    }
    [Fact]
    public void MalformedVariantsAndApproximateAnswersFailClosed()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(7,0),"0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(0,4),"0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(0,0,2),"3"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(5,2),"0.0016"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(0,1),"0/0"));
        var extra=P(0,1);extra["foreign"]=1;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,extra,"1/2"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            "usccss.g7.u02.l05.foreign",P(0,1),"1/2"));
    }
    [Fact]
    public void ScopedQuestionsGeneratedWithMathReverification()
    {
        var engine=new ExactSkillContractQuestionEngine();
        var questions=engine.Generate("g7-u02-l04-authored",
            Lesson,[Family],ExactSkillQuestionDifficulty.Standard,
            4,7204,Array.Empty<string>());
        Assert.Equal(4,questions.Count);
        foreach(var item in questions)
        {
            Assert.Equal(Family,item.Family);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                item.Family,item.Parameters,item.CorrectAnswer));
        }
    }
}
