using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// UAE G5 Arabic Student 01.pdf, printed pp. 11-16 (PDF pp. 30-35).
// Stage A-B-C test source written for Stage D; NOT EXECUTED by this change.
public sealed class UaeGrade5Unit1Lesson01PlaceValueTests
{
    private const string Lesson = "PED:UAE-MOE-MATH:L5:GENERAL:01:01";

    [Fact]
    public void G5UsesThreeExactFamiliesWithRegisteredNumericInteraction()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson, out var contract));
        Assert.Equal("number.place_value.to_billions", contract!.SkillId);
        Assert.Equal(3, contract.AllowedQuestionFamilies.Count);

        foreach (var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family, out var ui));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad, ui!.InteractionKind);
        }
    }

    [Fact]
    public void G5BillionsAreSolvedAndVerifiedAcrossDeterministicSeeds()
    {
        var engine = new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson, out var contract));

        foreach (var family in contract!.AllowedQuestionFamilies)
        for (int seed = 1; seed <= 10; seed++)
        foreach (var item in engine.Generate(Lesson, family, [family],
                     ExactSkillQuestionDifficulty.Standard, 2, seed, []))
        {
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family, item.Parameters, item.CorrectAnswer));
            Assert.DoesNotContain("result", item.Parameters.Keys);
            Assert.DoesNotContain("number", item.Parameters.Keys);
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family, item.Parameters, "-1"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family, item.Parameters, "1.5"));
            var arabic = new string(item.CorrectAnswer.Select(ch =>
                ch is >= '0' and <= '9' ? (char)('٠' + ch - '0') : ch).ToArray());
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family, item.Parameters, arabic));
        }
    }

    [Fact]
    public void HighestBillionsPlaceAndInternalZerosUseInt64WithoutOverflow()
    {
        const string family = "uae.g5.u01.l01.digit_value_to_billions";
        var p = new Dictionary<string, int>
        {
            ["pos"] = 0, ["d0"] = 9, ["d1"] = 0, ["d2"] = 0, ["d3"] = 0,
            ["d4"] = 0, ["d5"] = 0, ["d6"] = 0, ["d7"] = 0, ["d8"] = 0, ["d9"] = 5
        };
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            family, p, "٩٬٠٠٠٬٠٠٠٬٠٠٠"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            family, p, "900000000"));
        p["pos"] = 1;
        Assert.True(ExactSkillContractQuestionEngine.Verify(family, p, "0"));
        p["d0"] = 0;
        Assert.False(ExactSkillContractQuestionEngine.Verify(family, p, "0"));
    }

    [Fact]
    public void BadDigitParametersAreRejectedInsteadOfProducingQuestions()
    {
        const string family = "uae.g5.u01.l01.compose_ten_digits";
        var p = new Dictionary<string, int> { ["pos"] = -1 };
        for (int i = 0; i < 10; i++)
            p[$"d{i}"] = i == 0 ? 5 : 0;

        Assert.True(ExactSkillContractQuestionEngine.Verify(
            family, p, "٥٬٠٠٠٬٠٠٠٬٠٠٠"));
        p["d8"] = 12;
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            family, p, "5000000000"));
        p.Remove("d8");
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            family, p, "5000000000"));
    }
}
