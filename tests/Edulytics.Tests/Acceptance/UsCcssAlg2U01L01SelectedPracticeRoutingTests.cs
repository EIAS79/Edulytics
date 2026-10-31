using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.Acceptance;

/// <summary>
/// U01:L01 selected-Stage-C acceptance gate, AUTHORED_NOT_RUN.
/// EXPECTED TO FAIL until an exclusive shared-engine writer registers the
/// exact lesson contract and dispatch. This is not a claim of integration.
/// </summary>
public sealed class UsCcssAlg2U01L01SelectedPracticeRoutingTests
{
    private const string Lesson = "PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U01:L01";
    private const string Hanoi = "usccss.alg2.u01.l01.hanoi_min_moves";
    private const string Recurrence = "usccss.alg2.u01.l01.recursive_sequence";
    private const string Checker = "usccss.alg2.u01.l01.checker_jump_optional";

    [Fact]
    public void LessonSelectedContract_RegistersExactlyTheThreeSourceScopedNumericFamilies()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson, out var contract));
        Assert.NotNull(contract);
        Assert.Equal(Lesson, contract.LessonCode);
        Assert.Equal("READY_VERIFIED", contract.Readiness);
        Assert.Equal(3, contract.AllowedQuestionFamilies.Distinct(StringComparer.Ordinal).Count());
        Assert.True(new HashSet<string>(contract.AllowedQuestionFamilies, StringComparer.Ordinal)
            .SetEquals([Hanoi, Recurrence, Checker]));

        foreach (var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(UsCcssAlg2U01L01PracticeAdapter.SupportsFamily(family));
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        }
    }

    [Theory]
    [InlineData(Hanoi)]
    [InlineData(Recurrence)]
    [InlineData(Checker)]
    public void LessonSelectedEngine_GeneratesAndIndependentlyVerifiesNumericItemsOnly(
        string family)
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson, out var contract));
        Assert.NotNull(contract);
        Assert.Contains(family, contract.AllowedQuestionFamilies);

        var items = new ExactSkillContractQuestionEngine().Generate(
            Lesson, "u01-l01-exact-practice", [family],
            ExactSkillQuestionDifficulty.Standard, 1, 314159, []);
        var item = Assert.Single(items);
        Assert.Equal(family, item.Family);
        Assert.False(string.IsNullOrWhiteSpace(item.Prompt));
        Assert.False(string.IsNullOrWhiteSpace(item.CorrectAnswer));
        Assert.True(item.Parameters.ContainsKey("n"));
        Assert.True(item.Parameters.ContainsKey("variant"));
        Assert.Equal(2, item.Parameters.Count);
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            family, item.Parameters, item.CorrectAnswer));
        Assert.True(UsCcssAlg2U01L01PracticeAdapter.VerifyExactNumericQuestion(
            family, item.Parameters, item.CorrectAnswer));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            family, item.Parameters, "invalid-response;"));
        Assert.False(UsCcssAlg2U01L01PracticeAdapter.VerifyExactNumericQuestion(
            family, item.Parameters, "invalid-response;"));
    }
}
