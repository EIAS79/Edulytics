using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Edulytics.Services.Practice;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Original OUR/IM G6 U01:L10 source 10.1-10.3 and Practice 1-6.
/// Tests are authored, not executed during Stage A-B-C.
/// Student height constructions and diagrams are scored by teacher.
/// </summary>
public sealed class UsCcssG6Unit1Lesson10SelectedPracticeTests
{
    private const string Lesson="PED:US-CCSS-MATH:G6:U01:L10";
    private const string Area=UsCcssG6Unit1Lesson8TriangleAreaQuestions.L10Area;
    private const string Valid=UsCcssG6Unit1Lesson8TriangleAreaQuestions.L10Validity;

    [Fact]
    public void LessonContractReusesExactTriangleEngineAndNoPreviousGrade6Leak()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.Equal(new[]{Area,Valid},c.AllowedQuestionFamilies);
        Assert.All(c.AllowedQuestionFamilies,f=>
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f)));
        Assert.DoesNotContain("v360",c.SourceType,StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("usccss.g6.u01.l09.triangle_formula_source_area",
            c.AllowedQuestionFamilies);
    }

    [Theory]
    [InlineData(0,"22.5")] // Original source grid 10.3 A
    [InlineData(1,"44")]   // Original source grid 10.3 B
    [InlineData(2,"36")]   // Original source grid 10.3 C
    [InlineData(3,"33")]   // Original source grid 10.3 D
    [InlineData(4,"8")]    // Source Practice2 A
    [InlineData(5,"8")]    // Source Practice2 B
    [InlineData(6,"7.5")]  // Source Practice2 C, NOT equal 8
    [InlineData(7,"8")]    // Source Practice2 D
    [InlineData(8,"8")]    // Source Practice2 E
    [InlineData(9,"12")]   // Source Practice3
    [InlineData(10,"18")]  // Source Practice5
    [InlineData(11,"11")]  // Optional source extension left
    [InlineData(12,"18")]  // Optional source extension right
    [InlineData(13,"12")]  // Edulytiks independent Cool-down
    public void OriginalTriangleBaseHeightAndAreasCheckExactAnswers(
        int variant,string expected)
    {
        var p=new Dictionary<string,int>{{"variant",variant}};
        Assert.Equal(expected,UsCcssG6Unit1Lesson8TriangleAreaQuestions.Solve(Area,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Area,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,p,"-5"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,p,"1/0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,p,"999999"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,p," "+expected));
    }

    [Theory]
    [InlineData(0,"1")]
    [InlineData(1,"0")]
    [InlineData(2,"1")]
    [InlineData(3,"1")]
    [InlineData(4,"0")]
    [InlineData(5,"1")]
    [InlineData(6,"1")]
    [InlineData(7,"0")]
    public void OriginalIndexCardAndOutsideHeightLogicChecksCorrectAndWrong(
        int variant,string expected)
    {
        var p=new Dictionary<string,int>{{"variant",variant}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Valid,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Valid,p,expected=="0"?"1":"0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Valid,p,"true"));
    }

    [Fact]
    public void SourcePracticeTwo_MustExcludeTriangleC()
    {
        foreach(int variant in new[]{4,5,7,8})
            Assert.True(ExactSkillContractQuestionEngine.Verify(Area,
                new Dictionary<string,int>{{"variant",variant}},"8"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,
            new Dictionary<string,int>{{"variant",6}},"8"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Area,
            new Dictionary<string,int>{{"variant",6}},"15/2"));
    }

    [Fact]
    public void ForgeryAndCrossLessonPersistedResponsesFailClosed()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,
            new Dictionary<string,int>{{"variant",14}},"12"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Valid,
            new Dictionary<string,int>{{"variant",8}},"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,
            new Dictionary<string,int>{{"variant",10},{"override",1}},"18"));
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        var bad=LessonPracticeAlignmentValidator.ValidatePersisted(
            c.ToLegacyStage18Contract(),"usccss.g6.u01.l09.triangle_formula_source_area",
            new Dictionary<string,int>{{"variant",1}},"12");
        Assert.False(bad.IsAligned);
        Assert.Equal("QuestionFamilyNotAllowed",bad.ReasonCode);
    }

    [Fact]
    public void GenerationUsesExactSelectedFamiliesAndTeacherBoundary()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        var qs=new ExactSkillContractQuestionEngine().Generate(
            "g6-u01-l10-original-IM",Lesson,c.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,qs.Count);
        Assert.All(qs,q=>{
            Assert.Contains(q.Family,c.AllowedQuestionFamilies);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher reviews actual cuts",q.Solution);
        });
    }
}
