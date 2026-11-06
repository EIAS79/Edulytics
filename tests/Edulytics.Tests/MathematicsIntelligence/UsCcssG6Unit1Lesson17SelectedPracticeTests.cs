using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Edulytics.Services.Practice;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Original OUR/IM G6 U01:L17 first-edition exponent2/3 source.
/// Stage A–B–C authored NOT_RUN. Student cube building needs teacher score.</summary>
public sealed class UsCcssG6Unit1Lesson17SelectedPracticeTests
{
    private const string Lesson="PED:US-CCSS-MATH:G6:U01:L17";
    private const string Values=UsCcssG6Unit1Lesson12SurfaceAreaQuestions.L17Measures;
    private const string Meaning=UsCcssG6Unit1Lesson12SurfaceAreaQuestions.L17Concepts;

    [Fact]
    public void SelectsOnlySourceSquaresAndCubes()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.Equal(new[]{Values,Meaning},c.AllowedQuestionFamilies);
        Assert.All(c.AllowedQuestionFamilies,f=>
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f)));
        Assert.DoesNotContain("v360",c.SourceType,StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData(0,"49")]
    [InlineData(1,"8")]
    [InlineData(2,"3")]
    [InlineData(3,"27")]
    [InlineData(4,"5")]
    [InlineData(5,"9")]
    [InlineData(6,"27")]
    [InlineData(7,"4")]
    [InlineData(8,"16")]
    [InlineData(9,"64")]
    [InlineData(10,"1000")]
    [InlineData(11,"100")]
    [InlineData(12,"7")]
    [InlineData(13,"16")]
    [InlineData(14,"7")]
    [InlineData(15,"27")]
    [InlineData(16,"22")]
    [InlineData(17,"26")]
    [InlineData(18,"6")]
    public void SourcePerfectSquareCubeAndPrismValues(int v,string expected)
    {
        var p=new Dictionary<string,int>{{"variant",v}};
        Assert.Equal(expected,UsCcssG6Unit1Lesson12SurfaceAreaQuestions.Solve(Values,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Values,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Values,p,"-1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Values,p,"1/0"));
    }

    [Theory]
    [InlineData(0,"1")]
    [InlineData(1,"0")]
    [InlineData(2,"1")]
    [InlineData(3,"0")]
    [InlineData(4,"1")]
    [InlineData(5,"0")]
    [InlineData(6,"1")]
    [InlineData(7,"1")]
    [InlineData(8,"0")]
    [InlineData(9,"1")]
    public void ExponentsCountFactorsNotMultiplicationByExponent(int v,string answer)
    {
        var p=new Dictionary<string,int>{{"variant",v}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Meaning,p,answer));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Meaning,p,answer=="0"?"1":"0"));
    }

    [Fact]
    public void Source32UnitCubesDoNotAllowFourByFourByFour()
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(Values,
            new Dictionary<string,int>{{"variant",2}},"3"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Values,
            new Dictionary<string,int>{{"variant",4}},"5"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Values,
            new Dictionary<string,int>{{"variant",2}},"4"));
    }

    [Fact]
    public void RejectsUnselectedVariantsAndCrossLessonPersistedAnswers()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(Values,
            new Dictionary<string,int>{{"variant",19}},"3"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Meaning,
            new Dictionary<string,int>{{"variant",10}},"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Values,
            new Dictionary<string,int>{{"variant",0},{"forged",1}},"49"));
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        var bad=LessonPracticeAlignmentValidator.ValidatePersisted(
            c.ToLegacyStage18Contract(),
            "usccss.g6.u01.l16.source_equal_surface_different_volume",
            new Dictionary<string,int>{{"variant",0}},"46");
        Assert.False(bad.IsAligned);
        Assert.Equal("QuestionFamilyNotAllowed",bad.ReasonCode);
    }

    [Fact]
    public void SelectedGeneratorSolvesAndRetainsTeacherReview()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        var qs=new ExactSkillContractQuestionEngine().Generate(
            "g6-u01-l17-original-im",Lesson,c.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,qs.Count);
        Assert.All(qs,q=>{
            Assert.Contains(q.Family,c.AllowedQuestionFamilies);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher checks the actually built 3D figure",q.Solution);
        });
    }
}
