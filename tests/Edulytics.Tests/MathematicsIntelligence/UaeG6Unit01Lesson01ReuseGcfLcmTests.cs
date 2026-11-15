using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// Source-selected UAE G6 General U01:L01 targets. Reuses independently verified
// G5 GCF/LCM question logic rather than introducing a redundant new engine.
// Written during A-B-C, not executed in this phase.
public sealed class UaeG6Unit01Lesson01ReuseGcfLcmTests
{
    private const string SourceLesson = "PED:UAE-MOE-MATH:L6:GENERAL:01:01:FACTORS-AND-MULTIPLES";

    [Fact]
    public void ArabicLessonIsEmbeddedAndUsesExistingGcfLcmPracticeFamilies()
    {
        var content = RichLessonContentV2Registry.Find(SourceLesson, "ar");
        Assert.NotNull(content);
        Assert.Equal("العوامل والمضاعفات", content!.Title);
        Assert.True(content.KeyConcepts.Count >= 4);
        Assert.True(content.WorkedExamples.Count >= 4);
        Assert.Contains(content.Visuals, v => v.Kind == RichLessonVisualKind.NumberLine);
        Assert.Contains(content.Visuals, v => v.Kind == RichLessonVisualKind.EquationSet);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(SourceLesson, out var contract));
        Assert.NotNull(contract);
        Assert.Equal(6, contract!.AllowedQuestionFamilies.Count);
        Assert.Contains("number.lcm", contract.SkillIds);
        foreach (var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family, out _));
        }
    }

    [Theory]
    [InlineData("uae.g5.u07.l02.gcf_two_numbers", 18, 30, 30, "6")]
    [InlineData("uae.g5.u07.l02.gcf_three_numbers", 24, 36, 60, "12")]
    [InlineData("uae.g5.u07.l05.lcm_two_numbers", 4, 10, 1, "20")]
    [InlineData("uae.g5.u07.l05.lcm_three_numbers", 4, 6, 8, "24")]
    public void ExistingEngineVerifiesExactG6SkillTargetsAndRejectsIncorrectAnswers(
        string family, int a, int b, int c, string expected)
    {
        var inputs = new Dictionary<string, int>
        {
            ["a"] = a, ["b"] = b, ["c"] = c
        };
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family, out var interaction));
        Assert.NotNull(interaction);
        Assert.True(ExactSkillContractQuestionEngine.Verify(family, inputs, expected));
        var arabicDigits = new string(expected.Select(ch =>
            ch >= '0' && ch <= '9' ? (char)('٠' + ch - '0') : ch).ToArray());
        Assert.True(ExactSkillContractQuestionEngine.Verify(family, inputs, arabicDigits));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family, inputs, "-1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family, inputs, "abc"));
    }

    [Fact]
    public void CommonFactorAndCommonMultipleReusePreservePositiveAndNegativeCases()
    {
        const string commonFactor = "uae.g5.u07.l02.test_common_factor";
        Assert.True(ExactSkillContractQuestionEngine.Verify(commonFactor,
            new Dictionary<string, int> { ["a"] = 24, ["b"] = 36, ["c"] = 6 }, "1"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(commonFactor,
            new Dictionary<string, int> { ["a"] = 24, ["b"] = 36, ["c"] = 5 }, "0"));

        const string commonMultiple = "uae.g5.u07.l05.test_common_multiple";
        Assert.True(ExactSkillContractQuestionEngine.Verify(commonMultiple,
            new Dictionary<string, int> { ["a"] = 4, ["b"] = 6, ["c"] = 24 }, "1"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(commonMultiple,
            new Dictionary<string, int> { ["a"] = 4, ["b"] = 6, ["c"] = 18 }, "0"));
    }
}
