using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Edulytics.Services.Practice;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Original Kendall Hunt / OUR IM 6-8 U01:L05 source A/B/C acceptance.
/// AUTHORED_NOT_RUN; Stage D runs .NET, real Practice and teacher-score E2E.
/// </summary>
public sealed class UsCcssG6Unit1Lesson5SelectedPracticeRouteTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U01:L05";
    private const string Area = UsCcssG6Unit1Lesson5BaseHeightQuestions.Area;
    private const string Height = UsCcssG6Unit1Lesson5BaseHeightQuestions.Height;
    private const string Valid = UsCcssG6Unit1Lesson5BaseHeightQuestions.ValidHeight;
    private const string Scale = UsCcssG6Unit1Lesson5BaseHeightQuestions.Scale;
    private const string Triangle = UsCcssG6Unit1Lesson5BaseHeightQuestions.SmallTriangle;

    [Fact]
    public void LessonSelectsFiveSourceSpecificMechanicsAndNotPreviousGrade6Lesson()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson, out var contract));
        Assert.NotNull(contract);
        Assert.Equal("geometry.area_tiling", contract.SkillId);
        Assert.Equal("READY_VERIFIED", contract.Readiness);
        Assert.Equal(new[] { Area, Height, Valid, Scale, Triangle },
            contract.AllowedQuestionFamilies);
        Assert.All(contract.AllowedQuestionFamilies,
            f => Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f)));
        Assert.DoesNotContain("v360", contract.SourceType,
            StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData(0, "42")] // Original 5.1: 7 x 6 source image
    [InlineData(1, "24")] // 5.3 source Figure A: 4 x 6
    [InlineData(2, "15")] // 5.3 source Figure B: 5 x 3
    [InlineData(3, "6")]  // 5.3 source Figure C: 2 x 3
    [InlineData(4, "8")]  // 5.3 source Figure D: 4 vertical x 2 horizontal
    [InlineData(5, "8")]  // Original practice 3A: 4 x 2
    [InlineData(6, "10")] // Original practice 3B: 5 x 2
    [InlineData(7, "8")]  // Original practice 3C: 2 x 4
    [InlineData(8, "36")] // Original practice 5A: 9 cm x 4 cm
    [InlineData(9, "20")] // Original practice 5B: 5 cm x 4 cm
    [InlineData(10, "24")] // 6 x 4 = 5 x 4.8 original source summary
    public void SourceAreaVariants_VerifyExactNumericArea(int variant, string expected)
    {
        var parameters = new Dictionary<string, int> { ["variant"] = variant };
        Assert.Equal(expected,
            UsCcssG6Unit1Lesson5BaseHeightQuestions.Solve(Area, parameters));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Area, parameters, expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area, parameters, "-4"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area, parameters, "0/0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area, parameters, " 24 "));
    }

    [Theory]
    [InlineData(0, "4", "4/1")]
    [InlineData(1, "4.8", "24/5")]
    [InlineData(2, "4", "8/2")]
    [InlineData(3, "6", "12/2")]
    public void MatchingHeight_AcceptsExactFractionsAndRejectsOtherPair(
        int variant, string expected, string exactFraction)
    {
        var parameters = new Dictionary<string, int> { ["variant"] = variant };
        Assert.True(ExactSkillContractQuestionEngine.Verify(Height, parameters, expected));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Height, parameters, exactFraction));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Height, parameters, "5.8"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Height, parameters, "0/0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Height, parameters, "5/0"));
    }

    [Theory]
    [InlineData(0, "0")]
    [InlineData(1, "1")]
    [InlineData(2, "0")]
    [InlineData(3, "1")]
    [InlineData(4, "0")]
    [InlineData(5, "1")]
    [InlineData(6, "0")]
    [InlineData(7, "1")]
    [InlineData(8, "0")]
    [InlineData(9, "1")]
    [InlineData(10, "1")]
    [InlineData(11, "0")]
    public void SourceHeightClaims_RequirePerpendicularity(
        int variant, string expected)
    {
        var parameters = new Dictionary<string, int> { ["variant"] = variant };
        Assert.True(ExactSkillContractQuestionEngine.Verify(Valid, parameters, expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Valid, parameters,
            expected == "1" ? "0" : "1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Valid, parameters, "true"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Valid, parameters, "1.0"));
    }

    [Theory]
    [InlineData(0, "2")]
    [InlineData(1, "3")]
    [InlineData(2, "100")]
    [InlineData(3, "4")]
    [InlineData(4, "9")]
    [InlineData(5, "10000")]
    public void Extension_HeightOnlyVsTwoDimensions_HaveCorrectScaling(
        int variant, string expected)
    {
        var p = new Dictionary<string, int> { ["variant"] = variant };
        Assert.True(ExactSkillContractQuestionEngine.Verify(Scale, p, expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Scale, p, "0"));
    }

    [Fact]
    public void OriginalPracticeSeven_ExactTriangleFractions_AndCount()
    {
        var six = new Dictionary<string, int> { ["variant"] = 0 };
        var total = new Dictionary<string, int> { ["variant"] = 1 };
        Assert.Equal("1/3", UsCcssG6Unit1Lesson5BaseHeightQuestions.Solve(Triangle, six));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Triangle, six, "1/3"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Triangle, six, "6/18"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Triangle, six, "0.333333"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Triangle, total, "27"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Triangle, total, "54/2"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Triangle, total, "18"));
    }

    [Fact]
    public void InvalidForgedCrossLessonAndOutOfRangeVariants_FailClosed()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,
            new Dictionary<string, int>(), "42"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,
            new Dictionary<string, int> { ["variant"] = 121 }, "42"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Height,
            new Dictionary<string, int> { ["variant"] = 4 }, "4"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Valid,
            new Dictionary<string, int> { ["variant"] = -1 }, "1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Scale,
            new Dictionary<string, int> { ["variant"] = 6 }, "4"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Triangle,
            new Dictionary<string, int> { ["variant"] = 2 }, "27"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,
            new Dictionary<string, int> { ["variant"] = 0, ["forged"] = 1 }, "42"));

        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson, out var contract));
        Assert.NotNull(contract);
        var legacy = contract.ToLegacyStage18Contract();
        var cross = LessonPracticeAlignmentValidator.ValidatePersisted(legacy,
            "usccss.g6.u01.l04.parallelogram_exact_area",
            new Dictionary<string, int> { ["variant"] = 0 }, "12");
        Assert.False(cross.IsAligned);
        Assert.Equal("QuestionFamilyNotAllowed", cross.ReasonCode);
        var matched = LessonPracticeAlignmentValidator.ValidatePersisted(legacy,
            Triangle, new Dictionary<string, int> { ["variant"] = 0 }, "6/18");
        Assert.True(matched.IsAligned);
    }

    [Fact]
    public void Generation_IsLessonSelectedAndAllAnswersIndependentlyVerified()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson, out var contract));
        Assert.NotNull(contract);
        var generated = new ExactSkillContractQuestionEngine().Generate(
            "g6-u01-l05-first-edition", Lesson, contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard, 12, 20261103, Array.Empty<string>());
        Assert.Equal(12, generated.Count);
        Assert.All(generated, question =>
        {
            Assert.Contains(question.Family, contract.AllowedQuestionFamilies);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                question.Family, question.Parameters, question.CorrectAnswer));
            Assert.Contains("teacher checks the student's perpendicular",
                question.Solution);
        });
    }
}
