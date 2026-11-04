using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>U02 L04 original OUR/IM color mixtures; AUTHORED_NOT_RUN.
/// Physical mixing, learner diagrams and changing shade require teacher review.</summary>
public sealed class UsCcssG6Unit2Lesson4SelectedPracticeTests
{
    private const string Lesson="PED:US-CCSS-MATH:G6:U02:L04";
    private const string Amounts=UsCcssG6Unit2RatioQuestions.L04ColorAmounts;
    private const string Shades=UsCcssG6Unit2RatioQuestions.L04EqualShade;

    [Fact]
    public void SelectedPracticeUsesColorScaleAndShadeEquivalence()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        Assert.Equal(new[]{Amounts,Shades},c.AllowedQuestionFamilies);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.All(c.AllowedQuestionFamilies,f=>Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f)));
    }

    [Theory]
    [InlineData(0,"10")][InlineData(1,"30")]
    [InlineData(2,"15")][InlineData(3,"3")]
    [InlineData(4,"75")][InlineData(5,"40")]
    [InlineData(6,"38")][InlineData(7,"4")]
    [InlineData(8,"2")][InlineData(9,"20")]
    [InlineData(10,"4")][InlineData(11,"9")]
    [InlineData(12,"5")][InlineData(13,"6")]
    [InlineData(14,"12")][InlineData(15,"2")]
    [InlineData(16,"8")][InlineData(17,"25")]
    public void ColorQuantitiesAndRecipeReviewsSolveCorrectly(int variant,string expected)
    {
        var p=new Dictionary<string,int>{{"variant",variant}};
        Assert.Equal(expected,UsCcssG6Unit2RatioQuestions.Solve(Amounts,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Amounts,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Amounts,p,"999"));
    }

    [Theory]
    [InlineData(0,"1")][InlineData(1,"0")][InlineData(2,"1")]
    [InlineData(3,"0")][InlineData(4,"1")][InlineData(5,"0")]
    [InlineData(6,"1")][InlineData(7,"0")][InlineData(8,"1")]
    [InlineData(9,"0")][InlineData(10,"1")][InlineData(11,"0")]
    public void SourcePaintProportionsMustKeepIngredientOrder(int variant,string expected)
    {
        var p=new Dictionary<string,int>{{"variant",variant}};
        Assert.Equal(expected,UsCcssG6Unit2RatioQuestions.Solve(Shades,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Shades,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Shades,p,expected=="1"?"0":"1"));
    }

    [Fact]
    public void InvalidAndForeignVariantsFailClosed()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(Amounts,
            new Dictionary<string,int>{{"variant",18}},"10"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Shades,
            new Dictionary<string,int>{{"variant",-1}},"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Amounts,
            new Dictionary<string,int>{{"variant",0},{"forged",2}},"10"));
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        var result=LessonPracticeAlignmentValidator.ValidatePersisted(
            c.ToLegacyStage18Contract(),UsCcssG6Unit2RatioQuestions.L03RecipeBatches,
            new Dictionary<string,int>{{"variant",0}},"3");
        Assert.False(result.IsAligned);
        Assert.Equal("QuestionFamilyNotAllowed",result.ReasonCode);
    }

    [Fact]
    public void SourceScopedSelectedPracticeGeneratesAndVerifiesWithoutApprovingDrawings()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        var questions=new ExactSkillContractQuestionEngine().Generate(
            "g6-u02-l04-color-mixtures",Lesson,c.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
