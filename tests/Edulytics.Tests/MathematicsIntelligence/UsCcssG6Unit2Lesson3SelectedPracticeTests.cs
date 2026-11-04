using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Original Grade 6 Unit2 Lesson3 recipes; tests AUTHORED_NOT_RUN.
 /// Drawing, taste and explanations are teacher assessed, never auto-certified.</summary>
public sealed class UsCcssG6Unit2Lesson3SelectedPracticeTests
{
    private const string Lesson="PED:US-CCSS-MATH:G6:U02:L03";
    private const string Batches=UsCcssG6Unit2RatioQuestions.L03RecipeBatches;
    private const string Taste=UsCcssG6Unit2RatioQuestions.L03EqualTaste;

    [Fact]
    public void Lesson3RoutesRecipeScaleAndEqualTasteFamilies()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.Equal(new[]{Batches,Taste},c.AllowedQuestionFamilies);
        Assert.All(c.AllowedQuestionFamilies,f=>Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f)));
    }

    [Theory]
    [InlineData(0,"3")][InlineData(1,"4")][InlineData(2,"10")]
    [InlineData(3,"6")][InlineData(4,"3")]
    [InlineData(5,"10")][InlineData(6,"4")]
    [InlineData(7,"15")][InlineData(8,"6")]
    [InlineData(9,"25")][InlineData(10,"10")]
    [InlineData(11,"8")][InlineData(12,"2")]
    [InlineData(13,"15")][InlineData(14,"5")]
    [InlineData(15,"8")][InlineData(16,"20")]
    [InlineData(17,"4")][InlineData(18,"64")]
    [InlineData(19,"15")]
    public void RecipeAndCumulativeNumericalVariantsMatchDeclaredSourceContext(int v,string expected)
    {
        var p=new Dictionary<string,int>{{"variant",v}};
        Assert.Equal(expected,UsCcssG6Unit2RatioQuestions.Solve(Batches,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Batches,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Batches,p,"999"));
    }

    [Theory]
    [InlineData(0,"1")][InlineData(1,"0")][InlineData(2,"1")][InlineData(3,"0")]
    [InlineData(4,"1")][InlineData(5,"0")][InlineData(6,"1")][InlineData(7,"0")]
    [InlineData(8,"1")][InlineData(9,"0")]
    public void SameTasteRequiresEveryIngredientScaledBySameFactor(int v,string expected)
    {
        var p=new Dictionary<string,int>{{"variant",v}};
        Assert.Equal(expected,UsCcssG6Unit2RatioQuestions.Solve(Taste,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Taste,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Taste,p,expected=="0"?"1":"0"));
    }

    [Fact]
    public void ForgedOrOutOfRangeVariantsReject()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(Batches,
            new Dictionary<string,int>{{"variant",-1}},"3"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Batches,
            new Dictionary<string,int>{{"variant",20}},"3"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Taste,
            new Dictionary<string,int>{{"variant",10}},"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Batches,
            new Dictionary<string,int>{{"variant",0},{"extra",5}},"3"));
    }

    [Fact]
    public void SelectedPracticeGeneratesValidAnswersWithHumanReviewBoundary()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        var questions=new ExactSkillContractQuestionEngine().Generate(
            "g6-u02-l03-recipes-source",Lesson,c.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
