using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Source archive 06_Grade_6_OUR_IM/2/1. Academic A-B-C tests are
/// AUTHORED_NOT_RUN; Stage D runs them later. Human-created diagrams are not
/// auto-certified by numerical Practice.
/// </summary>
public sealed class UsCcssG6Unit2Lesson1SelectedPracticeTests
{
    private const string Lesson="PED:US-CCSS-MATH:G6:U02:L01";
    private const string Ratios=UsCcssG6Unit2RatioQuestions.L01Collections;
    private const string Shade=UsCcssG6Unit2RatioQuestions.L01Shade;

    [Fact]
    public void SelectedLessonPracticeRoutesBothOriginalSkills()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        Assert.Equal("READY_VERIFIED",contract.Readiness);
        Assert.Equal(new[]{Ratios,Shade},contract.AllowedQuestionFamilies);
        Assert.All(contract.AllowedQuestionFamilies,
            f=>Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f)));
    }

    [Theory]
    [InlineData(0,"9:4")][InlineData(1,"3:4")]
    [InlineData(2,"4:3")][InlineData(3,"9:3")]
    [InlineData(4,"6:3")][InlineData(5,"3:6")]
    [InlineData(6,"8:12")][InlineData(7,"12:8")]
    [InlineData(8,"6:9")][InlineData(9,"5:2")]
    [InlineData(10,"10:5")][InlineData(11,"7:3")]
    public void OrderedCategoryRatiosDoNotSilentlyReverse(int variant,string expected)
    {
        var parameters=new Dictionary<string,int>{{"variant",variant}};
        Assert.Equal(expected,UsCcssG6Unit2RatioQuestions.Solve(Ratios,parameters));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Ratios,parameters,expected));
        var parts=expected.Split(':');
        Assert.False(ExactSkillContractQuestionEngine.Verify(Ratios,parameters,
            $"{parts[1]}:{parts[0]}"));
    }

    [Theory]
    [InlineData(0,"16")][InlineData(1,"8")]
    [InlineData(2,"12")][InlineData(3,"6")]
    [InlineData(4,"20")][InlineData(5,"10")]
    [InlineData(6,"18")][InlineData(7,"9")]
    [InlineData(8,"18")][InlineData(9,"6")]
    [InlineData(10,"12")][InlineData(11,"8")]
    public void EqualUnitShadingRespectsTheNamedColor(int variant,string expected)
    {
        var p=new Dictionary<string,int>{{"variant",variant}};
        Assert.Equal(expected,UsCcssG6Unit2RatioQuestions.Solve(Shade,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Shade,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Shade,p,"999"));
    }

    [Fact]
    public void EquivalentRatiosAreAcceptedButNotBackwardsOrInvalid()
    {
        var p=new Dictionary<string,int>{{"variant",0}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Ratios,p,"18 : 8"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Ratios,p,"4:9"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Ratios,p,"-9:4"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Ratios,p,"9:0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Ratios,p,"9:4:3"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Ratios,
            new Dictionary<string,int>{{"variant",0},{"forged",1}},"9:4"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Shade,
            new Dictionary<string,int>{{"variant",12}},"16"));
    }

    [Fact]
    public void Grade6ExactPracticeGeneratesAndVerifiesBothFamilies()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        var questions=new ExactSkillContractQuestionEngine().Generate(
            "g6-u02-l01-original-ratio-language",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>
        {
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
            Assert.True(q.ItemType is AssessmentItemType.ShortAnswer or AssessmentItemType.Numeric);
        });
    }

    [Fact]
    public void WrongFamilyCannotBeSubmittedForThisLesson()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        var result=LessonPracticeAlignmentValidator.ValidatePersisted(
            contract.ToLegacyStage18Contract(),
            "usccss.g6.u01.l19.first_party_tent_panel_area",
            new Dictionary<string,int>{{"variant",0}},"48");
        Assert.False(result.IsAligned);
        Assert.Equal("QuestionFamilyNotAllowed",result.ReasonCode);
    }
}
