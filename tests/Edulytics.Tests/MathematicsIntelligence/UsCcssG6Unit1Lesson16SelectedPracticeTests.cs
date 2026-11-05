using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Edulytics.Services.Practice;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Original optional OUR/IM G6 U01:L16 source fixtures.
/// A/B/C tests AUTHORED_NOT_RUN; 8-cube physical models teacher-scored.</summary>
public sealed class UsCcssG6Unit1Lesson16SelectedPracticeTests
{
    private const string Lesson="PED:US-CCSS-MATH:G6:U01:L16";
    private const string Measures=UsCcssG6Unit1Lesson12SurfaceAreaQuestions.L16Measures;
    private const string Concepts=UsCcssG6Unit1Lesson12SurfaceAreaQuestions.L16Concepts;

    [Fact]
    public void OriginalOptionalLessonSelectsTwoScopedSolidQuestionFamilies()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.Equal(new[]{Measures,Concepts},c.AllowedQuestionFamilies);
        Assert.All(c.AllowedQuestionFamilies,f=>
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f)));
        Assert.DoesNotContain("v360",c.SourceType,StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData(0,"46")]  // original A surface
    [InlineData(1,"11")]  // A volume
    [InlineData(2,"46")]  // original B surface
    [InlineData(3,"14")]  // B volume
    [InlineData(4,"46")]  // original C surface
    [InlineData(5,"15")]  // C volume
    [InlineData(6,"24")]  // source-skill 8 cubes compact
    [InlineData(7,"28")]  // source-skill 8 cubes flat
    [InlineData(8,"34")]  // source-skill 8 cubes elongated
    [InlineData(9,"18")]  // original practice 1×1×4
    [InlineData(10,"160")] // original 2×5×10 prior net
    [InlineData(11,"22")] // independent 1×1×5 box
    [InlineData(12,"22")] // independent 1×2×3 box
    public void ThreeOriginalEqualAreaPrismsRetainDistinctVolumes(int variant,string correct)
    {
        var p=new Dictionary<string,int>{{"variant",variant}};
        Assert.Equal(correct,UsCcssG6Unit1Lesson12SurfaceAreaQuestions.Solve(Measures,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Measures,p,correct));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Measures,p,"-1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Measures,p,"1/0"));
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
    public void UnitDimensionsAndEightCubeSourceReasoning(int variant,string correct)
    {
        var p=new Dictionary<string,int>{{"variant",variant}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Concepts,p,correct));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Concepts,p,correct=="1"?"0":"1"));
    }

    [Fact]
    public void OriginalSourceFortySixNotIndependentTwentyTwo()
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(Measures,
            new Dictionary<string,int>{{"variant",0}},"46"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Measures,
            new Dictionary<string,int>{{"variant",0}},"22"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Measures,
            new Dictionary<string,int>{{"variant",11}},"22"));
    }

    [Fact]
    public void ForgedVariantsAndCrossLessonFamilyFailClosed()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(Measures,
            new Dictionary<string,int>{{"variant",13}},"46"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Concepts,
            new Dictionary<string,int>{{"variant",9}},"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Measures,
            new Dictionary<string,int>{{"variant",1},{"forged",1}},"11"));
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        var bad=LessonPracticeAlignmentValidator.ValidatePersisted(
            c.ToLegacyStage18Contract(),
            "usccss.g6.u01.l15.source_box_nets_surface_volume",
            new Dictionary<string,int>{{"variant",0}},"42");
        Assert.False(bad.IsAligned);
        Assert.Equal("QuestionFamilyNotAllowed",bad.ReasonCode);
    }

    [Fact]
    public void SelectedGenerateSolveVerifyAndTeacherBoundaryPresent()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        var qs=new ExactSkillContractQuestionEngine().Generate(
            "g6-u01-l16-original-im",Lesson,c.AllowedQuestionFamilies,
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
