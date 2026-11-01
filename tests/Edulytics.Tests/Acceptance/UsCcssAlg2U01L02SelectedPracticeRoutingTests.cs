using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.Acceptance;

/// <summary>
/// ALG2 U01:L02 selected Practice integration acceptance, AUTHORED_NOT_RUN.
/// Private persisted scoring is a separate gate and is not covered by this fixture.
/// </summary>
public sealed class UsCcssAlg2U01L02SelectedPracticeRoutingTests
{
    private const string Lesson = UsCcssAlg2U01L02PracticeAdapter.LessonCode;
    private const string Geometric = UsCcssAlg2U01L02PracticeAdapter.GeometricFamily;
    private const string Paper = UsCcssAlg2U01L02PracticeAdapter.PaperFamily;
    private const string Sierpinski = UsCcssAlg2U01L02PracticeAdapter.SierpinskiFamily;

    [Fact]
    public void SelectedLesson_HasExactlyThreeMachineNumericSourceFamilies()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson, out var contract));
        Assert.NotNull(contract);
        Assert.Equal(Lesson, contract.LessonCode);
        Assert.Equal("READY_VERIFIED", contract.Readiness);
        Assert.Equal("sequences.core.recursive_nth_term", contract.SkillId);
        Assert.True(new HashSet<string>(contract.AllowedQuestionFamilies, StringComparer.Ordinal)
            .SetEquals([Geometric, Paper, Sierpinski]));

        foreach (var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(UsCcssAlg2U01L02PracticeAdapter.SupportsFamily(family));
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        }

        Assert.False(ExactSkillContractQuestionEngine.SupportsFamily(
            "usccss.alg2.u01.l02.monthly_balance"));
        Assert.DoesNotContain(
            "usccss.alg2.u01.l02.monthly_balance", contract.AllowedQuestionFamilies);
    }

    [Theory]
    [InlineData(Geometric, 6)]
    [InlineData(Paper, 2)]
    [InlineData(Sierpinski, 2)]
    public void SharedGenerateSolveVerify_UsesBoundedTrustedSourceSlotsAndPrivateFeedback(
        string family, int numberOfVariants)
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson, out var contract));
        Assert.NotNull(contract);
        Assert.Contains(family, contract.AllowedQuestionFamilies);

        for (int slot = 0; slot < numberOfVariants; slot++)
        {
            var generated = new ExactSkillContractQuestionEngine().Generate(
                Lesson, "alg2-u01-l02-slot-" + slot,
                [family], ExactSkillQuestionDifficulty.Standard, 1,
                31657 + slot, [], preferredVariant: slot);
            var item = Assert.Single(generated);
            Assert.Equal(family, item.Family);
            Assert.Equal(slot, item.Parameters["variant"]);
            Assert.Equal(2, item.Parameters.Count);
            Assert.False(string.IsNullOrWhiteSpace(item.Prompt));
            Assert.False(string.IsNullOrWhiteSpace(item.Solution));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                item.Family, item.Parameters, item.CorrectAnswer));
            Assert.True(UsCcssAlg2U01L02PracticeAdapter.VerifyExactNumericQuestion(
                item.Family, item.Parameters, item.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                item.Family, item.Parameters, "1/0"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                item.Family, item.Parameters, "not a number"));
        }
    }

    [Theory]
    [InlineData(Geometric, 1, 4, "1/16", "0.0625")]
    [InlineData(Paper, 1, 6, "5/4", "1.25")]
    [InlineData(Sierpinski, 1, 5, "1/4", "0.25")]
    public void SharedExactVerifier_CreditsEquivalentRationalsButRejectsTampering(
        string family, int slot, int n, string fraction, string decimalEquivalent)
    {
        var correct = new Dictionary<string, int>
        {
            ["n"] = n,
            ["variant"] = slot
        };
        Assert.True(ExactSkillContractQuestionEngine.Verify(family, correct, fraction));
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            family, correct, decimalEquivalent));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            family, correct, "1/0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            family, new Dictionary<string, int>
            {
                ["n"] = n,
                ["variant"] = 15
            }, fraction));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            family, new Dictionary<string, int>
            {
                ["n"] = n,
                ["variant"] = slot,
                ["forged"] = 1
            }, fraction));
    }
}
