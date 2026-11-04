using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Edulytics.Services.Practice;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 U01:L19 source-skill and independent tent-model practice.
/// The source capstone has no separate numeric Practice problem set, and
/// all student-drawn nets, assumptions and design trade-offs are teacher-scored.
/// Tests AUTHORED_NOT_RUN at Stage A-B-C, not a runtime or Stage D pass.</summary>
public sealed class UsCcssG6Unit1Lesson19SelectedPracticeTests
{
    private const string Lesson="PED:US-CCSS-MATH:G6:U01:L19";
    private const string Area=UsCcssG6Unit1Lesson12SurfaceAreaQuestions.L19TentAreas;
    private const string Reasons=UsCcssG6Unit1Lesson12SurfaceAreaQuestions.L19TentReasoning;

    [Fact]
    public void Lesson19SelectsTwoFirstPartyDesignPracticeFamilies()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        Assert.Equal("READY_VERIFIED",contract.Readiness);
        Assert.Equal(new[]{Area,Reasons},contract.AllowedQuestionFamilies);
        Assert.All(contract.AllowedQuestionFamilies,f=>
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f)));
        Assert.DoesNotContain("v360",contract.SourceType,StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData(0,"48")]  // floor required
    [InlineData(1,"5")]   // slope hypotenuse
    [InlineData(2,"60")]  // two roof faces
    [InlineData(3,"24")]  // two end triangles
    [InlineData(4,"132")] // independent compact model, including floor
    [InlineData(5,"64")]  // longer floor
    [InlineData(6,"80")]  // longer roof
    [InlineData(7,"168")] // independent longer model, including floor
    [InlineData(8,"36")]  // fabric increase
    [InlineData(9,"12")]  // one end panel
    [InlineData(10,"8")]  // four sleeping bag widths
    [InlineData(11,"84")] // incorrect omitted-floor subtotal (explicit prompt)
    [InlineData(12,"104")]// incorrect omitted-floor subtotal (explicit prompt)
    [InlineData(13,"136")] // 4 original 34-inch bag widths
    [InlineData(14,"74")]  // original bag length inches
    [InlineData(15,"144")] // floor width12ft in inches
    [InlineData(16,"84")]  // source-dimension-compatible floor12×7ft
    [InlineData(17,"7.5")] // roof slope from half-width6, height4.5
    [InlineData(18,"105")] // roof panels length7
    [InlineData(19,"54")]  // both end triangles
    [InlineData(20,"243")] // total area all panels including floor
    [InlineData(21,"96")]  // longer floor12×8
    [InlineData(22,"120")] // longer roof panels
    [InlineData(23,"270")] // longer all-panel total
    [InlineData(24,"27")]  // longer fabric delta
    [InlineData(25,"10")]  // remaining length clearance inches
    public void FirstPartyExampleAreasMatchStatedAssumptions(int v,string expected)
    {
        var p=new Dictionary<string,int>{{"variant",v}};
        Assert.Equal(expected,UsCcssG6Unit1Lesson12SurfaceAreaQuestions.Solve(Area,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Area,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,p,"-1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,p,"1/0"));
    }

    [Theory]
    [InlineData(0,"1")] // floor required
    [InlineData(1,"0")] // no universal single fabric number
    [InlineData(2,"0")] // capacity and feasibility needed
    [InlineData(3,"1")] // 3-4-5 slope example
    [InlineData(4,"1")] // all panels required
    [InlineData(5,"0")] // area != volume units
    [InlineData(6,"1")] // no-aisle trade-off
    [InlineData(7,"1")] // peer comparison
    [InlineData(8,"0")] // equal fabric != equal usable area/headroom
    [InlineData(9,"1")] // human teacher rubric
    [InlineData(10,"0")] // narrow floor does not fit four actual bags
    [InlineData(11,"1")] // floor144in minus four bags136in
    [InlineData(12,"1")] // 7ft floor minus 74in bag
    [InlineData(13,"0")] // width spare 8in is not an aisle
    [InlineData(14,"1")] // longer source-aligned tent adds27ft²
    public void ModelConstraintsCannotSubstituteForActualTeacherScoredDesign(
        int v,string expected)
    {
        var p=new Dictionary<string,int>{{"variant",v}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Reasons,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Reasons,p,
            expected=="0"?"1":"0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Reasons,p,"true"));
    }

    [Fact]
    public void NoFloorIsNotValidCapstoneCompletion()
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(Area,
            new Dictionary<string,int>{{"variant",4}},"132"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,
            new Dictionary<string,int>{{"variant",4}},"84"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Reasons,
            new Dictionary<string,int>{{"variant",0}},"1"));
    }

    [Fact]
    public void RealSourceBagDimensionsNotCompatibleWithOldIdealizedTent()
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(Area,
            new Dictionary<string,int>{{"variant",13}},"136"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Area,
            new Dictionary<string,int>{{"variant",15}},"144"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Area,
            new Dictionary<string,int>{{"variant",17}},"7.5"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Area,
            new Dictionary<string,int>{{"variant",20}},"243"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,
            new Dictionary<string,int>{{"variant",20}},"132"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Reasons,
            new Dictionary<string,int>{{"variant",10}},"0"));
    }

    [Fact]
    public void ForgedQuestionParametersAndWrongLessonFamilyFailClosed()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,
            new Dictionary<string,int>{{"variant",26}},"132"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Reasons,
            new Dictionary<string,int>{{"variant",-1}},"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,
            new Dictionary<string,int>{{"variant",4},{"forged",1}},"132"));
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        var result=LessonPracticeAlignmentValidator.ValidatePersisted(
            contract.ToLegacyStage18Contract(),
            "usccss.g6.u01.l18.source_cube_surface_and_volume",
            new Dictionary<string,int>{{"variant",0}},"150");
        Assert.False(result.IsAligned);
        Assert.Equal("QuestionFamilyNotAllowed",result.ReasonCode);
    }

    [Fact]
    public void SelectedPracticeGeneratesValidExamplesButNotProjectApproval()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        var questions=new ExactSkillContractQuestionEngine().Generate(
            "g6-u01-l19-first-party-tent",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.Contains(q.Family,contract.AllowedQuestionFamilies);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher checks the actually built 3D figure",q.Solution);
        });
    }
}
