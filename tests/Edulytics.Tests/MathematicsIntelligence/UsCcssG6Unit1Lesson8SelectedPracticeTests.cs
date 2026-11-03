using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Edulytics.Services.Practice;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Source-specific original OUR/IM G6 6.1.8, AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit1Lesson8SelectedPracticeTests
{
    private const string Lesson="PED:US-CCSS-MATH:G6:U01:L08";
    private const string A=UsCcssG6Unit1Lesson8TriangleAreaQuestions.TriangleArea;
    private const string M=UsCcssG6Unit1Lesson8TriangleAreaQuestions.Preserve;
    private const string D=UsCcssG6Unit1Lesson8TriangleAreaQuestions.Decimal;

    [Fact]
    public void OriginalSourceSelectsThreeLessonOnlyExactFamilies()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.Equal(new[]{A,M,D},c.AllowedQuestionFamilies);
        Assert.All(c.AllowedQuestionFamilies,f=>
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f)));
        Assert.DoesNotContain("v360",c.SourceType,StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData(0,"12")]
    [InlineData(1,"20")]
    [InlineData(2,"20")]
    [InlineData(3,"8")]
    [InlineData(4,"12")]
    [InlineData(5,"4.5")]
    [InlineData(6,"12")]
    public void OriginalSourceTriangleArea_DoubledVsSingleArea(int v,string expected)
    {
        var p=new Dictionary<string,int>{{"variant",v}};
        Assert.Equal(expected,UsCcssG6Unit1Lesson8TriangleAreaQuestions.Solve(A,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(A,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(A,p,"0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(A,p,"-9"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(A,p,"1/0"));
    }

    [Theory]
    [InlineData(0,"1")]
    [InlineData(1,"1")]
    [InlineData(2,"0")]
    [InlineData(3,"1")]
    [InlineData(4,"1")]
    [InlineData(5,"1")]
    [InlineData(6,"0")]
    public void DistinguishesSourceDuplicateAndRearrangementMethods(
        int v,string expected)
    {
        var p=new Dictionary<string,int>{{"variant",v}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(M,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            M,p,expected=="1"?"0":"1"));
    }

    [Theory]
    [InlineData(0,"7")]
    [InlineData(1,"0.6")]
    [InlineData(2,"5.1")]
    public void OriginalPracticeFiveDecimalDimensions(int v,string expected)
    {
        var p=new Dictionary<string,int>{{"variant",v}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(D,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(D,p,"7/0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(D,p," 0.6 "));
    }

    [Fact]
    public void ExactFractionAndOutOfRangeParametersFailClosed()
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(A,
            new Dictionary<string,int>{{"variant",5}},"9/2"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(D,
            new Dictionary<string,int>{{"variant",1}},"3/5"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(A,
            new Dictionary<string,int>{{"variant",7}},"20"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(M,
            new Dictionary<string,int>{{"variant",-1}},"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(D,
            new Dictionary<string,int>{{"variant",1},{"forged",1}},"0.6"));
    }

    [Fact]
    public void SelectedPracticeGenerationAndForbiddenCrossLessonGate()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        var qs=new ExactSkillContractQuestionEngine().Generate(
            "g6-u01-l08-original-im",Lesson,c.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,qs.Count);
        Assert.All(qs,q=>{
            Assert.Contains(q.Family,c.AllowedQuestionFamilies);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher reviews actual cuts",q.Solution);
        });
        var x=LessonPracticeAlignmentValidator.ValidatePersisted(
            c.ToLegacyStage18Contract(),
            "usccss.g6.u01.l07.parallelogram_diagonal_half",
            new Dictionary<string,int>{{"variant",0}},"20");
        Assert.False(x.IsAligned);
        Assert.Equal("QuestionFamilyNotAllowed",x.ReasonCode);
    }
}
