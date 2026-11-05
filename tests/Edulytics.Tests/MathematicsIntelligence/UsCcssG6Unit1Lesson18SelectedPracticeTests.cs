using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Edulytics.Services.Practice;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Original OUR/IM G6 U01:L18 exact cube source tests.
/// AUTHORED_NOT_RUN in A-B-C; a teacher grades folding and nets.</summary>
public sealed class UsCcssG6Unit1Lesson18SelectedPracticeTests
{
    private const string Lesson="PED:US-CCSS-MATH:G6:U01:L18";
    private const string Measures=UsCcssG6Unit1Lesson12SurfaceAreaQuestions.L18CubeAreas;
    private const string Units=UsCcssG6Unit1Lesson12SurfaceAreaQuestions.L18Units;

    [Fact]
    public void SelectsTwoOriginalCubeOnlyFamilies()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.Equal(new[]{Measures,Units},c.AllowedQuestionFamilies);
        Assert.All(c.AllowedQuestionFamilies, f=>
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f)));
        Assert.DoesNotContain("v360",c.SourceType,StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData(0,"150")]
    [InlineData(1,"125")]
    [InlineData(2,"1734")]
    [InlineData(3,"4913")]
    [InlineData(4,"512")]
    [InlineData(5,"1/27")]
    [InlineData(6,"2")]
    [InlineData(7,"22326")]
    [InlineData(8,"226981")]
    [InlineData(9,"10")]
    [InlineData(10,"54")]
    [InlineData(11,"96")]
    [InlineData(12,"64")]
    [InlineData(13,"24")]
    [InlineData(14,"8")]
    [InlineData(15,"216")]
    public void SourceAndIndependentAreasVolumesAreExact(int v,string correct)
    {
        var p=new Dictionary<string,int>{{"variant",v}};
        Assert.Equal(correct,UsCcssG6Unit1Lesson12SurfaceAreaQuestions.Solve(Measures,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Measures,p,correct));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Measures,p,"-1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Measures,p,"1/0"));
    }

    [Fact]
    public void OneThirdCubed_IsExactlyOneOverTwentySeven_NotRoundedDecimal()
    {
        var p=new Dictionary<string,int>{{"variant",5}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Measures,p,"1/27"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Measures,p,"2/54"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Measures,p,"0.037037"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Measures,p,"1/9"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Measures,p,"0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Measures,p,"27/0"));
    }

    [Theory]
    [InlineData(0,"1")]
    [InlineData(1,"1")]
    [InlineData(2,"1")]
    [InlineData(3,"0")]
    [InlineData(4,"0")]
    [InlineData(5,"1")]
    [InlineData(6,"1")]
    [InlineData(7,"1")]
    [InlineData(8,"1")]
    [InlineData(9,"0")]
    [InlineData(10,"1")]
    public void CorrespondingSquareCubeUnitClaims(int v,string correct)
    {
        var p=new Dictionary<string,int>{{"variant",v}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Units,p,correct));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Units,p,correct=="1"?"0":"1"));
    }

    [Fact]
    public void RejectInvalidVariantForgeryAndCrossLessonFamily()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(Measures,
            new Dictionary<string,int>{{"variant",16}},"150"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Units,
            new Dictionary<string,int>{{"variant",11}},"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Measures,
            new Dictionary<string,int>{{"variant",2},{"forged",1}},"1734"));
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        var bad=LessonPracticeAlignmentValidator.ValidatePersisted(
            c.ToLegacyStage18Contract(),
            "usccss.g6.u01.l17.original_square_cube_values",
            new Dictionary<string,int>{{"variant",0}},"49");
        Assert.False(bad.IsAligned);
        Assert.Equal("QuestionFamilyNotAllowed",bad.ReasonCode);
    }

    [Fact]
    public void SelectedGenerateSolveVerifyStillRequiresTeacherDrawings()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        var qs=new ExactSkillContractQuestionEngine().Generate(
            "g6-u01-l18-original-IM",Lesson,c.AllowedQuestionFamilies,
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
