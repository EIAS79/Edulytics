using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Edulytics.Services.Practice;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Original OUR/IM G6 U01:L06 A–B–C acceptance, authored only;
/// xUnit and authenticated learner browser work belong to Stage D.</summary>
public sealed class UsCcssG6Unit1Lesson6SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U01:L06";
    private const string D = UsCcssG6Unit1Lesson6AreaQuestions.Dimensions;
    private const string E = UsCcssG6Unit1Lesson6AreaQuestions.EqualArea;
    private const string C = UsCcssG6Unit1Lesson6AreaQuestions.Composite;

    [Fact]
    public void SourceLessonSelectsOriginalThreeFamiliesWithoutPreviousLessonLeak()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        Assert.Equal("READY_VERIFIED",contract.Readiness);
        Assert.Equal(new[] {D,E,C},contract.AllowedQuestionFamilies);
        Assert.Equal("geometry.area_tiling",contract.SkillId);
        Assert.DoesNotContain("v360",contract.SourceType,StringComparison.OrdinalIgnoreCase);
        Assert.All(contract.AllowedQuestionFamilies,f=>
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f)));
    }

    [Theory]
    [InlineData(0,"120")]
    [InlineData(1,"12")]
    [InlineData(2,"45")]
    [InlineData(3,"15")]
    [InlineData(4,"2")]
    [InlineData(5,"3")]
    [InlineData(6,"5")]
    [InlineData(7,"4730")]
    [InlineData(8,"14")]
    [InlineData(9,"16")]
    [InlineData(10,"13.5")]
    [InlineData(11,"14")]
    [InlineData(12,"18")]
    public void OriginalAreaMissingDimensions_ExactCorrectWrongInvalid(
        int variant,string expected)
    {
        var p=new Dictionary<string,int>{{"variant",variant}};
        Assert.Equal(expected,UsCcssG6Unit1Lesson6AreaQuestions.Solve(D,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(D,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(D,p,"-2"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(D,p,"1/0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(D,p," 14 "));
        Assert.False(ExactSkillContractQuestionEngine.Verify(D,p,"999999"));
    }

    [Theory]
    [InlineData(0,"1")]
    [InlineData(1,"0")]
    [InlineData(2,"1")]
    [InlineData(3,"1")]
    [InlineData(4,"1")]
    [InlineData(5,"1")]
    [InlineData(6,"1")]
    [InlineData(7,"0")]
    public void OriginalEqualArea_RejectsOppositeAnswer(int variant,string correct)
    {
        var p=new Dictionary<string,int>{{"variant",variant}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(E,p,correct));
        Assert.False(ExactSkillContractQuestionEngine.Verify(E,p,
            correct=="1"?"0":"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(E,p,"true"));
    }

    [Theory]
    [InlineData(0,"48")]
    [InlineData(1,"80")]
    [InlineData(2,"24")]
    public void SourceCompositeArea_DoesNotInventUndimensionedHole(
        int variant,string expected)
    {
        var p=new Dictionary<string,int>{{"variant",variant}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(C,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(C,p,"0"));
    }

    [Fact]
    public void EquivalentFractionsAccepted_UnknownAndForgedVariantsRejected()
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(D,
            new Dictionary<string,int>{{"variant",10}},"27/2"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(D,
            new Dictionary<string,int>{{"variant",13}},"18"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(E,
            new Dictionary<string,int>{{"variant",-1}},"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(C,
            new Dictionary<string,int>{{"variant",1},{"forged",1}},"80"));
    }

    [Fact]
    public void SelectedLessonGeneratesIndependentVerifiableQuestions()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        var questions=new ExactSkillContractQuestionEngine().Generate(
            "g6-u01-l06-original-im",Lesson,c.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.Contains(q.Family,c.AllowedQuestionFamilies);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher assesses the chosen corresponding",q.Solution);
        });
    }

    [Fact]
    public void CrossLessonSourceFamiliesAreNotAccepted()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        var legacy=c.ToLegacyStage18Contract();
        var rejected=LessonPracticeAlignmentValidator.ValidatePersisted(legacy,
            "usccss.g6.u01.l05.corresponding_height",
            new Dictionary<string,int>{{"variant",0}},"4");
        Assert.False(rejected.IsAligned);
        Assert.Equal("QuestionFamilyNotAllowed",rejected.ReasonCode);
        var accepted=LessonPracticeAlignmentValidator.ValidatePersisted(legacy,
            D,new Dictionary<string,int>{{"variant",1}},"12");
        Assert.True(accepted.IsAligned);
    }
}
