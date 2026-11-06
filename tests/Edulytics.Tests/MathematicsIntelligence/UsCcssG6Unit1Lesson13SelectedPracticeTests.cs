using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Edulytics.Services.Practice;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Original OUR/IM Grade6 U01:L13 polyhedra, source numeric
/// and classification practice. AUTHORED_NOT_RUN in Stage A-B-C.
/// A teacher must assess actual nets, folded shapes, and reasoning.</summary>
public sealed class UsCcssG6Unit1Lesson13SelectedPracticeTests
{
    private const string Lesson="PED:US-CCSS-MATH:G6:U01:L13";
    private const string Logic=UsCcssG6Unit1Lesson12SurfaceAreaQuestions.L13Polyhedra;
    private const string Counts=UsCcssG6Unit1Lesson12SurfaceAreaQuestions.L13Measures;

    [Fact]
    public void ExactLessonPracticeSelectsOriginalSolidFamiliesOnly()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.Equal(new[]{Logic,Counts},c.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Logic));
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Counts));
        Assert.DoesNotContain("v360",c.SourceType,StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData(0,"1")]  // source practice1 A pyramid
    [InlineData(1,"1")]  // source practice1 B polyhedron
    [InlineData(2,"0")]  // source practice1 C curved cone
    [InlineData(3,"1")]  // source practice1 D hexagonal prism
    [InlineData(4,"0")]  // source practice1 E curved sphere
    [InlineData(5,"0")]  // source practice3 Tyler false
    [InlineData(6,"1")]
    [InlineData(7,"0")]
    [InlineData(8,"0")]
    [InlineData(9,"1")]  // original source net3 invalid
    [InlineData(10,"1")]
    [InlineData(11,"0")]
    [InlineData(12,"1")] // source tetrahedron net1
    [InlineData(13,"1")] // source tetrahedron net2
    [InlineData(14,"0")] // source tetrahedron net3 NOT foldable
    public void OriginalSolidClassificationAndNetLogic(int v,string expected)
    {
        var p=new Dictionary<string,int>{{"variant",v}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Logic,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Logic,p,expected=="0"?"1":"0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Logic,p,"true"));
    }

    [Theory]
    [InlineData(0,"5")] // triangular prism F
    [InlineData(1,"5")] // square pyramid F
    [InlineData(2,"4")] // tetrahedron F
    [InlineData(3,"6")] // tetrahedron E
    [InlineData(4,"4")] // tetrahedron V
    [InlineData(5,"10")] // original practice2 octagonal prism F
    [InlineData(6,"24")] // original practice2 octagonal prism E
    [InlineData(7,"16")] // original practice2 octagonal prism V
    [InlineData(8,"9")] // source prior-area practice4
    [InlineData(9,"31")] // original practice5 cutout
    [InlineData(10,"6")]
    [InlineData(11,"12")]
    [InlineData(12,"8")]
    public void SourceNumericCountsAndCutoutAreas(int v,string expected)
    {
        var p=new Dictionary<string,int>{{"variant",v}};
        Assert.Equal(expected,UsCcssG6Unit1Lesson12SurfaceAreaQuestions.Solve(Counts,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Counts,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Counts,p,"-1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Counts,p,"1/0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Counts,p," "+expected));
    }

    [Fact]
    public void InvalidVariantsAndWrongLessonFailClosed()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(Logic,
            new Dictionary<string,int>{{"variant",15}},"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Counts,
            new Dictionary<string,int>{{"variant",-1}},"5"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Counts,
            new Dictionary<string,int>{{"variant",5},{"forged",1}},"10"));
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        var x=LessonPracticeAlignmentValidator.ValidatePersisted(
            c.ToLegacyStage18Contract(),
            "usccss.g6.u01.l12.prism_exposed_face_area",
            new Dictionary<string,int>{{"variant",0}},"720");
        Assert.False(x.IsAligned);
        Assert.Equal("QuestionFamilyNotAllowed",x.ReasonCode);
    }

    [Fact]
    public void SelectedGenerationSolvesAndVerifiesButNeedsPhysicalTeacherReview()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        var questions=new ExactSkillContractQuestionEngine().Generate(
            "g6-u01-l13-original-IM",Lesson,c.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.Contains(q.Family,c.AllowedQuestionFamilies);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher checks the actually built 3D figure",q.Solution);
        });
    }
}
