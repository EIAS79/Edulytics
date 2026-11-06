using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Edulytics.Services.Practice;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Original OUR/IM G6 6.1.14 net surface-area source tests.
/// Stage A–B–C AUTHORED_NOT_RUN, geometry constructions teacher-scored.</summary>
public sealed class UsCcssG6Unit1Lesson14SelectedPracticeTests
{
    private const string Lesson="PED:US-CCSS-MATH:G6:U01:L14";
    private const string Net=UsCcssG6Unit1Lesson12SurfaceAreaQuestions.L14NetArea;
    private const string Logic=UsCcssG6Unit1Lesson12SurfaceAreaQuestions.L14NetLogic;

    [Fact]
    public void ContractUsesExactlyTwoOriginalNetFamiliesOnReusedSolidEngine()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.Equal(new[]{Net,Logic},c.AllowedQuestionFamilies);
        Assert.All(c.AllowedQuestionFamilies,f=>
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f)));
        Assert.DoesNotContain("v360",c.SourceType,StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData(0,"82")] // original 14.2 net A, NOT 52
    [InlineData(1,"48")] // original 14.2 net B square pyramid
    [InlineData(2,"48")] // original 14.2 net C triangular prism
    [InlineData(3,"72")] // original practice2
    [InlineData(4,"52")] // original lesson summary 4×3×2
    [InlineData(5,"40")] // independently authored DIFFERENT pyramid
    [InlineData(6,"5")]
    [InlineData(7,"6")]
    [InlineData(8,"5")]
    [InlineData(9,"12")]
    public void SourceNetAreasAndFacesExactlyVerified(int variant,string expected)
    {
        var p=new Dictionary<string,int>{{"variant",variant}};
        Assert.Equal(expected,UsCcssG6Unit1Lesson12SurfaceAreaQuestions.Solve(Net,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Net,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Net,p,"-2"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Net,p,"1/0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Net,p," "+expected));
    }

    [Fact]
    public void PublisherSquarePyramidBHasDifferentFaceHeightThanIndependentExample()
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(Net,
            new Dictionary<string,int>{{"variant",1}},"48"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Net,
            new Dictionary<string,int>{{"variant",1}},"40"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Net,
            new Dictionary<string,int>{{"variant",5}},"40"));
    }

    [Theory]
    [InlineData(0,"1")]
    [InlineData(1,"1")]
    [InlineData(2,"1")]
    [InlineData(3,"0")]
    [InlineData(4,"1")]
    [InlineData(5,"0")]
    [InlineData(6,"1")]
    [InlineData(7,"1")]
    [InlineData(8,"0")]
    [InlineData(9,"1")]
    [InlineData(10,"0")]
    public void NetFoldAndUnitLogicRejectsFalseClaims(int v,string expected)
    {
        var p=new Dictionary<string,int>{{"variant",v}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Logic,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Logic,p,
            expected=="0"?"1":"0"));
    }

    [Fact]
    public void TamperingAndCrossLessonFamiliesFailClosed()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(Net,
            new Dictionary<string,int>{{"variant",10}},"82"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Logic,
            new Dictionary<string,int>{{"variant",-1}},"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Net,
            new Dictionary<string,int>{{"variant",2},{"forged",1}},"48"));
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        var bad=LessonPracticeAlignmentValidator.ValidatePersisted(
            c.ToLegacyStage18Contract(),
            "usccss.g6.u01.l13.source_faces_edges_vertices",
            new Dictionary<string,int>{{"variant",0}},"5");
        Assert.False(bad.IsAligned);
        Assert.Equal("QuestionFamilyNotAllowed",bad.ReasonCode);
    }

    [Fact]
    public void SelectedGenerationIsSolvableButTeacherChecksFolding()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        var qs=new ExactSkillContractQuestionEngine().Generate(
            "g6-u01-l14-original-im",Lesson,c.AllowedQuestionFamilies,
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
