using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Coder 8 M05/M06/M07/M08/M10 source-case technical checks; not academic
/// explanation certification, selected student Practice or Stage D acceptance.
/// </summary>
public sealed class UsCcssHsMatrixInverseEligibilityQuestionsTests
{
    private const string Family = UsCcssHsMatrixInverseEligibilityQuestions.Family;

    [Theory]
    [InlineData(0, "singular")]             // M07: det = 0, inverse unavailable
    [InlineData(1, "dimension_mismatch")]    // M08: 2x2 inverse times 3-vector
    [InlineData(2, "not_square")]            // M10: 2x3 matrix lacks inverse
    [InlineData(3, "invertible")]            // Positive control
    public void ThreeDistinctInvalidMatrixTasksAndPositiveControlGradeWithoutGuessing(
        int mode, string answer)
    {
        var p = UsCcssHsMatrixInverseEligibilityQuestions.ParametersForCase(mode);
        Assert.Equal(answer, UsCcssHsMatrixInverseEligibilityQuestions.Solve(Family, p));
        Assert.Equal(answer, UsCcssHsMatrixInverseEligibilityQuestions.Evaluate(p).Classification);
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family, p, answer.ToUpperInvariant()));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, "undefined"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, "1,0;0,1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, "0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, "<script>"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, new string('x', 50)));
        foreach (var other in new[] { "singular", "dimension_mismatch", "not_square", "invertible" })
            if (other != answer)
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, other));
    }

    [Fact]
    public void MatrixValidatorRejectsMalformedShapesAndAmbiguousReasons()
    {
        var source = UsCcssHsMatrixInverseEligibilityQuestions.ParametersForCase(1);
        var wrongShape = new Dictionary<string, int>(source) { ["columns"] = 5 };
        var ambiguous = new Dictionary<string, int>(source)
        {
            ["a00"] = 1, ["a01"] = 2, ["a10"] = 2, ["a11"] = 4
        };
        var hiddenNonzero = new Dictionary<string, int>(source) { ["a02"] = 3 };
        var impossibleVector = new Dictionary<string, int>(source) { ["rhsLength"] = 2 };
        foreach (var invalid in new[] { wrongShape, ambiguous, hiddenNonzero, impossibleVector })
        {
            Assert.Equal("MALFORMED", UsCcssHsMatrixInverseEligibilityQuestions.Evaluate(invalid).Status);
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family, invalid, "dimension_mismatch"));
        }
    }

    [Fact]
    public void NewCasesAreGeneratedThroughSharedEngineButOnlyAuditVisible()
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolvePendingForAudit(Family, out var interaction));
        Assert.NotNull(interaction);
        Assert.False(LessonPracticeInteractionRegistry.TryResolve(Family, out _));
        var lesson = "PED:US-CCSS-MATH:HS:FOURTH:ADV-ALG-FUNC:U01:L01";
        Assert.True(LessonPracticeContractRegistry.TryResolve(lesson, out var contract));
        Assert.NotNull(contract);
        Assert.Equal("INTEGRATION_PENDING", contract!.Readiness);
        Assert.Contains(Family, contract.AllowedQuestionFamilies);
        Assert.False(LessonPracticeCapabilityResolver.TryResolve(lesson, out _));

        var seen = new HashSet<string>(StringComparer.Ordinal);
        for (var seed = 0; seed < 120; seed++)
        {
            var sample = UsCcssHsMatrixInverseEligibilityQuestions.Build(Family, new Random(seed));
            Assert.Contains("A^-1 b", sample.Prompt);
            Assert.Contains("Choose exactly one", sample.Prompt);
            var correct = UsCcssHsMatrixInverseEligibilityQuestions.Solve(Family, sample.Parameters);
            Assert.True(ExactSkillContractQuestionEngine.Verify(Family, sample.Parameters, correct));
            seen.Add(correct);
        }
        Assert.Equal(4, seen.Count);
    }

    [Fact]
    public void M05M06ClassificationDependsOnAugmentedSystemNotJustDeterminant()
    {
        var dependent = new Dictionary<string, int>
        {
            ["a"] = 1, ["b"] = 1, ["c"] = 2, ["d"] = 2,
            ["e"] = 2, ["f"] = 4, ["mode"] = 1
        };
        var inconsistent = new Dictionary<string, int>(dependent) { ["f"] = 5, ["mode"] = 2 };
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            UsCcssHsFourthCourseQuestions.MatrixClassification, dependent, "infinite"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            UsCcssHsFourthCourseQuestions.MatrixClassification, inconsistent, "none"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            UsCcssHsFourthCourseQuestions.MatrixClassification, inconsistent, "infinite"));
    }
}
