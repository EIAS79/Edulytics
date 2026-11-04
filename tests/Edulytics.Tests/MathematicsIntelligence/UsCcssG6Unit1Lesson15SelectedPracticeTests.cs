using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Edulytics.Services.Practice;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Original OUR/IM G6 U01:L15 15.3 boxes and source practice
/// A–B–C tests AUTHORED_NOT_RUN. Real folding is teacher-reviewed.</summary>
public sealed class UsCcssG6Unit1Lesson15SelectedPracticeTests
{
    private const string Lesson="PED:US-CCSS-MATH:G6:U01:L15";
    private const string Measures=UsCcssG6Unit1Lesson12SurfaceAreaQuestions.L15BoxArea;
    private const string Logic=UsCcssG6Unit1Lesson12SurfaceAreaQuestions.L15NetLogic;

    [Fact]
    public void SourceLessonSelectedPracticeUsesExistingSolidExactEngine()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.Equal(new[]{Measures,Logic},c.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Measures));
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Logic));
        Assert.DoesNotContain("v360",c.SourceType,StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData(0,"42")]  // Source box A surface
    [InlineData(1,"18")]  // Source box A volume
    [InlineData(2,"54")]  // Source box B surface
    [InlineData(3,"18")]  // Source box B volume
    [InlineData(4,"54")]  // Source box C surface
    [InlineData(5,"27")]  // Source box C volume
    [InlineData(6,"60")]  // Source practice1 Jada corrected (not72)
    [InlineData(7,"272")] // Source practice2 cereal
    [InlineData(8,"36")]  // Source practice3 steps twelve cubes
    [InlineData(9,"30")]  // Source practice3 top two removed
    [InlineData(10,"56")] // Source practice5 square pyramid
    [InlineData(11,"84")] // Independent triangular-prism example
    [InlineData(12,"72")] // Independent different prism
    [InlineData(13,"66")] // Independent different prism
    public void OriginalBoxNetAreasVolumesAndCorrectedJadaExact(
        int variant,string expected)
    {
        var p=new Dictionary<string,int>{{"variant",variant}};
        Assert.Equal(expected,UsCcssG6Unit1Lesson12SurfaceAreaQuestions.Solve(Measures,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Measures,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Measures,p,"-1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Measures,p,"1/0"));
    }

    [Theory]
    [InlineData(0,"1")]
    [InlineData(1,"0")]
    [InlineData(2,"1")]
    [InlineData(3,"1")]
    [InlineData(4,"0")]
    [InlineData(5,"1")]
    [InlineData(6,"0")]
    [InlineData(7,"1")]
    [InlineData(8,"0")]
    public void OriginalBoxAndDrawingClaimReasonsAreCorrect(int variant,string expected)
    {
        var p=new Dictionary<string,int>{{"variant",variant}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Logic,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Logic,p,expected=="0"?"1":"0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Logic,p,"true"));
    }

    [Fact]
    public void OriginalBoxAIsNotTheIndependent72SquareCentimeterBox()
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(Measures,
            new Dictionary<string,int>{{"variant",0}},"42"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Measures,
            new Dictionary<string,int>{{"variant",0}},"72"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Measures,
            new Dictionary<string,int>{{"variant",6}},"72"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Measures,
            new Dictionary<string,int>{{"variant",6}},"60"));
    }

    [Fact]
    public void RejectForgedParametersAndCrossLessonPersistedFamily()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(Measures,
            new Dictionary<string,int>{{"variant",14}},"42"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Logic,
            new Dictionary<string,int>{{"variant",9}},"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Measures,
            new Dictionary<string,int>{{"variant",6},{"forged",1}},"60"));
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        var bad=LessonPracticeAlignmentValidator.ValidatePersisted(
            c.ToLegacyStage18Contract(),
            "usccss.g6.u01.l14.source_net_surface_area",
            new Dictionary<string,int>{{"variant",0}},"82");
        Assert.False(bad.IsAligned);
        Assert.Equal("QuestionFamilyNotAllowed",bad.ReasonCode);
    }

    [Fact]
    public void SelectedSourcePracticeGeneratesAndVerifiesWithoutGradingActualNet()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        var qs=new ExactSkillContractQuestionEngine().Generate(
            "g6-u01-l15-original-IM",Lesson,c.AllowedQuestionFamilies,
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
