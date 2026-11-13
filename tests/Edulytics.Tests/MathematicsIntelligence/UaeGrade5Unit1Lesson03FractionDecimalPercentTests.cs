using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// UAE Grade 5 original Arabic local student book 01.pdf, printed pp23-28.
// These Stage C tests are written but NOT_RUN (Stage D owns execution).
public sealed class UaeGrade5Unit1Lesson03FractionDecimalPercentTests
{
    private const string Lesson = "PED:UAE-MOE-MATH:L5:GENERAL:01:03";

    [Fact]
    public void G5ReusesDecimalFamiliesAndAddsOnlyPercentExtension()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson, out var contract));
        Assert.Equal("decimals.place_value", contract!.SkillId);
        Assert.Equal(5, contract.AllowedQuestionFamilies.Count);
        foreach (var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family, out var interaction));
            Assert.NotNull(interaction);
        }
    }

    [Fact]
    public void PercentConversionAnswersRejectWrongDenominatorOrFraction()
    {
        var p = new Dictionary<string, int>
        { ["whole"] = 0, ["tenths"] = 3, ["hundredths"] = 4 };
        const string hundredths = "uae.g5.u01.l03.hundredths_to_percent";
        Assert.True(ExactSkillContractQuestionEngine.Verify(hundredths, p, "٣٤"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(hundredths, p, "3.4"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(hundredths, p, "٣"));
        p["hundredths"] = 0;
        const string tenths = "uae.g5.u01.l03.tenths_to_percent";
        Assert.True(ExactSkillContractQuestionEngine.Verify(tenths, p, "٣٠"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(tenths, p, "3"));
    }

    [Fact]
    public void ZeroAndFullHundredPercentAndInvalidBoundaries()
    {
        const string percent = "uae.g5.u01.l03.hundredths_to_percent";
        var p = new Dictionary<string, int>
        { ["whole"] = 1, ["tenths"] = 0, ["hundredths"] = 0 };
        Assert.True(ExactSkillContractQuestionEngine.Verify(percent, p, "100"));
        p["whole"] = 0;
        Assert.True(ExactSkillContractQuestionEngine.Verify(percent, p, "0"));
        p["whole"] = 1;
        p["hundredths"] = 1;
        Assert.False(ExactSkillContractQuestionEngine.Verify(percent, p, "101"));
    }

    [Fact]
    public void ExactFamilyGeneratedAnswerVerifiesForFixedSeeds()
    {
        var engine = new ExactSkillContractQuestionEngine();
        var f = "uae.g5.u01.l03.hundredths_to_percent";
        for (int seed = 1; seed <= 8; seed++)
        foreach (var q in engine.Generate(
                     Lesson, f, [f], ExactSkillQuestionDifficulty.Standard, 2, seed, []))
        {
            Assert.True(ExactSkillContractQuestionEngine.Verify(f, q.Parameters, q.CorrectAnswer));
            Assert.DoesNotContain("result", q.Parameters.Keys);
            Assert.False(ExactSkillContractQuestionEngine.Verify(f, q.Parameters, "-1"));
        }
    }
}
