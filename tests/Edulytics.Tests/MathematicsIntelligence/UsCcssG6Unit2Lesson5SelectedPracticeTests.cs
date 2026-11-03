using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>G6 U02:L05 selected equivalence Practice. Tests AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit2Lesson5SelectedPracticeTests
{
    private const string Lesson="PED:US-CCSS-MATH:G6:U02:L05";
    private const string Equiv=UsCcssG6Unit2RatioQuestions.L05Equivalence;
    private const string Missing=UsCcssG6Unit2RatioQuestions.L05ScaleMissing;
    [Fact] public void LessonFiveHasExplicitPairAndRecipeFamilies()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.Equal(new[]{Equiv,Missing},c.AllowedQuestionFamilies);
        Assert.All(c.AllowedQuestionFamilies,f=>Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f)));
    }
    // Independent arithmetic cross-check: 6:4 equals 18:12 (variant 12);
    // a 4:6 ratio scaled by 3 gives 12:18, and 6:4 reduced by 2 is 3:2.
    [Theory]
    [InlineData(0,"1")][InlineData(1,"1")][InlineData(2,"1")]
    [InlineData(3,"0")][InlineData(4,"0")][InlineData(5,"1")]
    [InlineData(6,"0")][InlineData(7,"1")][InlineData(8,"0")]
    [InlineData(9,"1")][InlineData(10,"0")][InlineData(11,"1")]
    [InlineData(12,"1")][InlineData(13,"1")][InlineData(14,"0")]
    public void AllOriginalAndFirstPartyRatioComparisonVariants(int v,string correct)
    {
        var p=new Dictionary<string,int>{{"variant",v}};
        Assert.Equal(correct,UsCcssG6Unit2RatioQuestions.Solve(Equiv,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Equiv,p,correct));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Equiv,p,correct=="1"?"0":"1"));
    }
    [Theory]
    [InlineData(0,"8")][InlineData(1,"10")][InlineData(2,"6")]
    [InlineData(3,"5")][InlineData(4,"15")][InlineData(5,"30")]
    [InlineData(6,"50")][InlineData(7,"162")][InlineData(8,"324")]
    [InlineData(9,"81")][InlineData(10,"12")][InlineData(11,"18")]
    [InlineData(12,"18")][InlineData(13,"20")][InlineData(14,"2")]
    [InlineData(15,"2")]
    public void RatioAndCasseroleScaleSourceMathVariants(int v,string correct)
    {
        var p=new Dictionary<string,int>{{"variant",v}};
        Assert.Equal(correct,UsCcssG6Unit2RatioQuestions.Solve(Missing,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Missing,p,correct));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Missing,p,"999"));
    }
    [Fact] public void InvalidAndForeignSourceQuestionFailClosed()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(Equiv,new Dictionary<string,int>{{"variant",15}},"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Missing,new Dictionary<string,int>{{"variant",-1}},"8"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Equiv,new Dictionary<string,int>{{"variant",0},{"extra",1}},"1"));
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));Assert.NotNull(c);
        var wrong=LessonPracticeAlignmentValidator.ValidatePersisted(c.ToLegacyStage18Contract(),
            UsCcssG6Unit2RatioQuestions.L04ColorAmounts,new Dictionary<string,int>{{"variant",0}},"10");
        Assert.False(wrong.IsAligned);
    }
    [Fact] public void GeneratedSelectedQuestionsRetainTeacherReviewBoundary()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));Assert.NotNull(c);
        var qs=new ExactSkillContractQuestionEngine().Generate(
            "g6-u02-l05-equivalent-ratios",Lesson,c.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,qs.Count);
        Assert.All(qs,q=>{Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));Assert.Contains("Teacher",q.Solution);});
    }
}
