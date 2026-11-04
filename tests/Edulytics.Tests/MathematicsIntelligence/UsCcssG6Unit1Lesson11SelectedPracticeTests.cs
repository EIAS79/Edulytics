using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Edulytics.Services.Practice;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Grade 6 original OUR/IM 6.1.11 polygon classification and source area
/// tests. Stage A–B–C AUTHORED_NOT_RUN. Drawing / proofs need human review.
/// </summary>
public sealed class UsCcssG6Unit1Lesson11SelectedPracticeTests
{
    private const string Lesson="PED:US-CCSS-MATH:G6:U01:L11";
    private const string Classification=UsCcssG6Unit1Lesson11PolygonQuestions.Classification;
    private const string Area=UsCcssG6Unit1Lesson11PolygonQuestions.Area;

    [Fact]
    public void OriginalPolygonsSelectedPracticeReusesExistingEngine()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.Equal(new[]{Classification,Area},c.AllowedQuestionFamilies);
        Assert.All(c.AllowedQuestionFamilies,f=>
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f)));
        Assert.DoesNotContain("v360",c.SourceType,StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData(0,"1")] // closed arrow A
    [InlineData(1,"0")] // rounded corners B
    [InlineData(2,"1")] // simple concave star C
    [InlineData(3,"0")] // open triangle D
    [InlineData(4,"0")] // curved heart E
    [InlineData(5,"0")] // 3D prism F
    [InlineData(6,"1")]
    [InlineData(7,"0")]
    [InlineData(8,"1")]
    [InlineData(9,"0")]
    public void SourcePolygonClassification_DistinguishesBoundaryFromNonpolygon(
        int variant,string expected)
    {
        var p=new Dictionary<string,int>{{"variant",variant}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Classification,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Classification,p,
            expected=="1"?"0":"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Classification,p,"true"));
    }

    [Theory]
    [InlineData(0,"12")]    // source quadrilateral A
    [InlineData(1,"18")]    // source quadrilateral B
    [InlineData(2,"28")]    // source quadrilateral C
    [InlineData(3,"14")]    // source quadrilateral D
    [InlineData(4,"15")]    // source quadrilateral E
    [InlineData(5,"18")]    // source quadrilateral F
    [InlineData(6,"18")]    // source trapezoid
    [InlineData(7,"93.6")]  // source hexagon
    [InlineData(8,"11")]    // source P4 triangle
    [InlineData(9,"40")]    // ORIGINAL source pinwheel
    [InlineData(10,"12")]   // independent example not original
    [InlineData(11,"32")]   // independently authored Cool-down
    public void OriginalPolygonAreaFixturesAndDifferentPinwheelNumerics(
        int variant,string expected)
    {
        var p=new Dictionary<string,int>{{"variant",variant}};
        Assert.Equal(expected,UsCcssG6Unit1Lesson11PolygonQuestions.Solve(Area,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Area,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,p,"-1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,p,"1/0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,p," "+expected));
    }

    [Fact]
    public void OriginalPinwheelIsForty_NotTheIndependentlyAuthoredTwelve()
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(Area,
            new Dictionary<string,int>{{"variant",9}},"40"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,
            new Dictionary<string,int>{{"variant",9}},"12"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Area,
            new Dictionary<string,int>{{"variant",10}},"12"));
    }

    [Fact]
    public void EquivalentFractionAndForgedVariantRejectedAsExpected()
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(Area,
            new Dictionary<string,int>{{"variant",7}},"468/5"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,
            new Dictionary<string,int>{{"variant",12}},"12"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Classification,
            new Dictionary<string,int>{{"variant",-1}},"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,
            new Dictionary<string,int>{{"variant",6},{"forged",1}},"18"));
    }

    [Fact]
    public void LessonScopedGenerationAndPersistedWrongFamilyFailClosed()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        var questions=new ExactSkillContractQuestionEngine().Generate(
            "g6-u01-l11-original-IM",Lesson,c.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.Contains(q.Family,c.AllowedQuestionFamilies);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher separately checks polygon outline",q.Solution);
        });
        var bad=LessonPracticeAlignmentValidator.ValidatePersisted(
            c.ToLegacyStage18Contract(),
            "usccss.g6.u01.l10.triangle_corresponding_height_area",
            new Dictionary<string,int>{{"variant",0}},"22.5");
        Assert.False(bad.IsAligned);
        Assert.Equal("QuestionFamilyNotAllowed",bad.ReasonCode);
    }
}
