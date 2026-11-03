using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Edulytics.Services.Practice;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Original OUR/IM G6 U01:L07 source tasks, authored only;
/// NOT_RUN in Stage A–B–C; drawing and teacher scoring require Stage D.</summary>
public sealed class UsCcssG6Unit1Lesson7SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U01:L07";
    private const string H = UsCcssG6Unit1Lesson7TriangleQuestions.Half;
    private const string F = UsCcssG6Unit1Lesson7TriangleQuestions.Fraction;
    private const string S = UsCcssG6Unit1Lesson7TriangleQuestions.Shape;

    [Fact]
    public void SelectsExactOriginalTriangleAndFractionFamilies()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.Equal(new[] {H,F,S},c.AllowedQuestionFamilies);
        Assert.All(c.AllowedQuestionFamilies, f=>
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f)));
        Assert.DoesNotContain("v360",c.SourceType,StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData(0,"20")]
    [InlineData(1,"12")]
    [InlineData(2,"9")]
    [InlineData(3,"24")]
    [InlineData(4,"7.5")]
    [InlineData(5,"6")]
    [InlineData(6,"3")]
    public void ExactDiagonalSourceHalfAreas_AndSideCount(int variant,string result)
    {
        var p=new Dictionary<string,int>{{"variant",variant}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(H,p,result));
        Assert.False(ExactSkillContractQuestionEngine.Verify(H,p,"-1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(H,p," 1 "));
        Assert.False(ExactSkillContractQuestionEngine.Verify(H,p,"1/0"));
    }

    [Theory]
    [InlineData(0,"6","12/2")]
    [InlineData(1,"4/3","8/6")]
    [InlineData(2,"28","56/2")]
    public void OriginalPracticeFive_ExactFractionsAndNonterminatingHeight(
        int variant,string correct,string equivalent)
    {
        var p=new Dictionary<string,int>{{"variant",variant}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(F,p,correct));
        Assert.True(ExactSkillContractQuestionEngine.Verify(F,p,equivalent));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,p,"1/0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,p,"-2"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,p,"0.333333"));
    }

    [Theory]
    [InlineData(0,"1")]
    [InlineData(1,"0")]
    [InlineData(2,"1")]
    [InlineData(3,"0")]
    [InlineData(4,"1")]
    [InlineData(5,"0")]
    [InlineData(6,"1")]
    [InlineData(7,"0")]
    [InlineData(8,"1")]
    public void CompositionAndCongruence_RejectsWrongBinaryClaim(
        int variant,string answer)
    {
        var p=new Dictionary<string,int>{{"variant",variant}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(S,p,answer));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            S,p,answer=="1"?"0":"1"));
    }

    [Fact]
    public void OutOfRangeForgedAndCrossLessonResponsesFailClosed()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(H,
            new Dictionary<string,int>{{"variant",7}},"3"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,
            new Dictionary<string,int>{{"variant",3}},"28"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(S,
            new Dictionary<string,int>{{"variant",9}},"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,
            new Dictionary<string,int>{{"variant",1},{"forged",1}},"4/3"));
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        var legacy=c.ToLegacyStage18Contract();
        var wrong=LessonPracticeAlignmentValidator.ValidatePersisted(legacy,
            "usccss.g6.u01.l06.equal_area_reasoning",
            new Dictionary<string,int>{{"variant",0}},"1");
        Assert.False(wrong.IsAligned);
        Assert.Equal("QuestionFamilyNotAllowed",wrong.ReasonCode);
    }

    [Fact]
    public void SelectedGenerationProducesVerifiableQuestionsWithTeacherBoundary()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        var qs=new ExactSkillContractQuestionEngine().Generate(
            "g6-u01-l07-original-im",Lesson,c.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,qs.Count);
        Assert.All(qs,q=>{
            Assert.Contains(q.Family,c.AllowedQuestionFamilies);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher reviews source-matched diagonal",q.Solution);
        });
    }
}
