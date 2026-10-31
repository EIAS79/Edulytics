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

    // AUTHORED_NOT_RUN: these use the selected shared engine, not only the
    // standalone Algebra 2 adapter. The grader must retain exact BigInteger
    // and reject any teacher-only or forged numeric variants.
    [Fact]
    public void SharedSelectedHanoi_Handles64DiscExactEquivalentsAndRejectsMalformed()
    {
        var issued = new Dictionary<string, int> { ["n"] = 64, ["variant"] = 0 };
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            Hanoi, issued, "18446744073709551615"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            Hanoi, issued, "2^64-1"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            Hanoi, issued, "2 ^ (64) - 1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Hanoi, issued, "18446744073709552000"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Hanoi, issued, "2^(6 4)-1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Hanoi, issued, "2^(63)-1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Hanoi, issued, "2^(64-1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Hanoi, new Dictionary<string, int> { ["n"] = 65, ["variant"] = 0 },
            "36893488147419103231"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Hanoi, new Dictionary<string, int> { ["n"] = 1, ["variant"] = 6 },
            "1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Checker, new Dictionary<string, int> { ["n"] = 1, ["variant"] = 7 },
            "3"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Recurrence, new Dictionary<string, int> { ["n"] = 1, ["variant"] = 8 },
            "3"));
    }

    [Fact]
    public void SharedSelectedRecurrence_GeneratesFourMachineOnlyVariantsWithSafeHints()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson, out var contract));
        Assert.NotNull(contract);
        var items = new ExactSkillContractQuestionEngine().Generate(
            Lesson, "u01-l01-recursive-four-variants", [Recurrence],
            ExactSkillQuestionDifficulty.Standard, 4, 271828, [], preferredVariant: 0);
        Assert.Equal(4, items.Count);
        Assert.Equal(4, items.Select(x => x.Parameters["variant"]).Distinct().Count());
        foreach (var item in items)
        {
            Assert.True(item.Parameters["variant"] is >= 2 and <= 5);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                item.Family, item.Parameters, item.CorrectAnswer));
            Assert.True(UsCcssAlg2U01L01PracticeAdapter.VerifyExactNumericQuestion(
                item.Family, item.Parameters, item.CorrectAnswer));
            Assert.DoesNotContain("Teacher scores", item.Solution, StringComparison.Ordinal);
            Assert.DoesNotContain("TeacherRubric", item.Solution, StringComparison.Ordinal);
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                item.Family, item.Parameters, "1/0"));
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
