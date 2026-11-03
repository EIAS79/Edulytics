using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Edulytics.Services.Practice;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Original OUR/IM G6 U01:L12 original teacher/student/practice source
/// fixtures. Authored not run (per Stage A-B-C contract).
/// </summary>
public sealed class UsCcssG6Unit1Lesson12SelectedPracticeTests
{
    private const string Lesson="PED:US-CCSS-MATH:G6:U01:L12";
    private const string Prism=UsCcssG6Unit1Lesson12SurfaceAreaQuestions.Prism;
    private const string Meaning=UsCcssG6Unit1Lesson12SurfaceAreaQuestions.Meaning;

    [Fact]
    public void SelectsOnlyOriginalSurfaceAreaNumericSkillFamilies()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        Assert.Equal("READY_VERIFIED",contract.Readiness);
        Assert.Equal(new[]{Prism,Meaning},contract.AllowedQuestionFamilies);
        Assert.All(contract.AllowedQuestionFamilies,f=>
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f)));
        Assert.DoesNotContain("v360",contract.SourceType,StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData(0,"720")]  // Source original cabinet 24×12×6, bottom not covered
    [InlineData(1,"1008")] // Same cabinet, six faces
    [InlineData(2,"32")]   // Source 12-cube 2×2×3
    [InlineData(3,"40")]   // Alternative 12-cube 1×2×6
    [InlineData(4,"38")]   // Alternative 12-cube 1×3×4
    [InlineData(5,"50")]   // Alternative 12-cube 1×1×12
    [InlineData(6,"118")]  // Separate independent example
    [InlineData(7,"158")]  // Separate six-face cabinet
    [InlineData(8,"64")]   // Source practice1 2×4×4 (not32)
    [InlineData(9,"22")]   // Source practice3 L five cubes
    [InlineData(10,"22")]  // Source practice3 plus five cubes
    [InlineData(11,"88")]  // Source practice4 4×2×6
    [InlineData(12,"52")]  // Source student summary 2×3×4
    [InlineData(13,"62")]  // Independent Cool-down 2×3×5
    public void OriginalPrismAreaSourceAndIndependentExamplesDistinct(
        int variant,string expected)
    {
        var p=new Dictionary<string,int>{{"variant",variant}};
        Assert.Equal(expected,UsCcssG6Unit1Lesson12SurfaceAreaQuestions.Solve(Prism,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prism,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prism,p,"-1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prism,p,"1/0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prism,p," 32 "));
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
    public void UnitFaceExposedAreaMeaning_MatchesOriginalSource(
        int variant,string expected)
    {
        var p=new Dictionary<string,int>{{"variant",variant}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Meaning,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Meaning,p,expected=="1"?"0":"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Meaning,p,"true"));
    }

    [Fact]
    public void SourceCabinetPrismAndCubedVolumeCannotBeConfused()
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prism,
            new Dictionary<string,int>{{"variant",0}},"720"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prism,
            new Dictionary<string,int>{{"variant",0}},"118"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prism,
            new Dictionary<string,int>{{"variant",8}},"32"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prism,
            new Dictionary<string,int>{{"variant",9}},"22"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prism,
            new Dictionary<string,int>{{"variant",10}},"22"));
    }

    [Fact]
    public void RejectsUnregisteredVariantsAndCrossLessonInputs()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prism,
            new Dictionary<string,int>{{"variant",14}},"720"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Meaning,
            new Dictionary<string,int>{{"variant",8}},"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prism,
            new Dictionary<string,int>{{"variant",0},{"forged",1}},"720"));
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        var bad=LessonPracticeAlignmentValidator.ValidatePersisted(
            contract.ToLegacyStage18Contract(),
            "usccss.g6.u01.l11.source_polygon_area",
            new Dictionary<string,int>{{"variant",0}},"12");
        Assert.False(bad.IsAligned);
        Assert.Equal("QuestionFamilyNotAllowed",bad.ReasonCode);
    }

    [Fact]
    public void SelectedPracticeGeneratedNumbersHaveMatchingVerifyAndTeacherBoundary()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        var qs=new ExactSkillContractQuestionEngine().Generate(
            "g6-u01-l12-original-im",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,qs.Count);
        Assert.All(qs,q=>{
            Assert.Contains(q.Family,contract.AllowedQuestionFamilies);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher checks the actually built 3D figure",q.Solution);
        });
    }
}
