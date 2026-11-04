using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>G6 U02:L02 exact diagram/count practice. Tests AUTHORED_NOT_RUN;
/// drawn snap-cube/paint/recipe diagrams and unit labels require teacher review.</summary>
public sealed class UsCcssG6Unit2Lesson2SelectedPracticeTests
{
    private const string Lesson="PED:US-CCSS-MATH:G6:U02:L02";
    private const string Diagrams=UsCcssG6Unit2RatioQuestions.L02Diagrams;
    private const string Cards=UsCcssG6Unit2RatioQuestions.L02Cards;
    [Fact]
    public void Lesson2SelectsOnlyItsTwoDiagramAndRecipeFamilies()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.Equal(new[]{Diagrams,Cards},c.AllowedQuestionFamilies);
        Assert.All(c.AllowedQuestionFamilies,f=>Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f)));
    }

    [Theory]
    [InlineData(0,"4:2")][InlineData(1,"2:4")]
    [InlineData(2,"2:6")][InlineData(3,"6:2")]
    [InlineData(4,"8:2")][InlineData(5,"6:3")]
    [InlineData(6,"6:4")][InlineData(7,"3:5")]
    [InlineData(8,"5:2")][InlineData(9,"7:4")]
    [InlineData(10,"4:6")][InlineData(11,"9:3")]
    public void LabeledTwoRowDiagramUsesCorrectOrderAndUnits(int variant,string expected)
    {
        var p=new Dictionary<string,int>{{"variant",variant}};
        Assert.Equal(expected,UsCcssG6Unit2RatioQuestions.Solve(Diagrams,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Diagrams,p,expected));
        var parts=expected.Split(':');
        Assert.False(ExactSkillContractQuestionEngine.Verify(Diagrams,p,
            $"{parts[1]}:{parts[0]}"));
    }

    [Theory]
    [InlineData(0,"2:4:6")][InlineData(1,"4:2:1")]
    [InlineData(2,"3:6:2")][InlineData(3,"1:3:5")]
    [InlineData(4,"4:6:2")][InlineData(5,"5:2:3")]
    [InlineData(6,"2:3:4")][InlineData(7,"6:2:4")]
    [InlineData(8,"3:5:1")][InlineData(9,"2:4:3")]
    [InlineData(10,"7:3:2")][InlineData(11,"4:1:6")]
    public void ThreeIngredientDiagramIsAnOrderedThreeTermRatio(int variant,string expected)
    {
        var p=new Dictionary<string,int>{{"variant",variant}};
        Assert.Equal(expected,UsCcssG6Unit2RatioQuestions.Solve(Cards,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Cards,p,expected));
        var parts=expected.Split(':');
        Assert.False(ExactSkillContractQuestionEngine.Verify(Cards,p,
            $"{parts[2]}:{parts[1]}:{parts[0]}"));
    }

    [Fact]
    public void IncorrectDiagramsAndForgedParametersFailClosed()
    {
        var p=new Dictionary<string,int>{{"variant",0}};
        Assert.False(ExactSkillContractQuestionEngine.Verify(Diagrams,p,"2:4"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Cards,p,"4:2:6"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Cards,p,"1:2"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Cards,
            new Dictionary<string,int>{{"variant",0},{"fake",1}},"2:4:6"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Diagrams,
            new Dictionary<string,int>{{"variant",12}},"4:2"));
    }

    [Fact]
    public void ActualSelectedPracticeGeneratesValidatedShortAnswers()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        var questions=new ExactSkillContractQuestionEngine().Generate(
            "g6-u02-l02-original-diagrams",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
            Assert.Equal(AssessmentItemType.ShortAnswer,q.ItemType);
        });
    }
}
