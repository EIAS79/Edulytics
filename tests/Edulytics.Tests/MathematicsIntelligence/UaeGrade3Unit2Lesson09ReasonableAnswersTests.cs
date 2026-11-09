using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// Written during A–B–C, deliberately NOT_RUN until Stage D.
public sealed class UaeGrade3Unit2Lesson09ReasonableAnswersTests
{
    [Fact]
    public void ReusesExistingEstimationFamilyAndAddsBooleanReasonableness()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:UAE-MOE-MATH:L3:COMMON:02:09", out var contract));
        Assert.NotNull(contract);
        Assert.Equal(2, contract!.AllowedQuestionFamilies.Count);
        Assert.Contains("uae.g3.u02.l05.estimate_sum_by_rounding",
            contract.AllowedQuestionFamilies);
        Assert.Contains("uae.g3.u02.l09.reasonable_estimate_verdict",
            contract.AllowedQuestionFamilies);
        foreach (var f in contract.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(
            "uae.g3.u02.l09.reasonable_estimate_verdict", out var interaction));
        Assert.Equal(LessonPracticeInteractionKind.StructuredEntry,
            interaction!.InteractionKind);
    }

    [Fact]
    public void GeneratedVerdictsAreReasonableAndRejectCorruptData()
    {
        const string family = "uae.g3.u02.l09.reasonable_estimate_verdict";
        var engine = new ExactSkillContractQuestionEngine();
        bool foundYes = false, foundNo = false;
        foreach (int seed in new[] { 20261109, 20261110, 20261111 })
        foreach (var item in engine.Generate(
            "g3-u02-l09", family, [family],
            ExactSkillQuestionDifficulty.Standard, 8, seed, []))
        {
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family, item.Parameters, item.CorrectAnswer));
            if (item.CorrectAnswer == "نعم") foundYes = true;
            if (item.CorrectAnswer == "لا") foundNo = true;
            var corrupted = item.Parameters.ToDictionary(
                pair => pair.Key, pair => pair.Value);
            corrupted["candidate"] += 100;
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family, corrupted, item.CorrectAnswer));
        }
        Assert.True(foundYes && foundNo);
    }
}
