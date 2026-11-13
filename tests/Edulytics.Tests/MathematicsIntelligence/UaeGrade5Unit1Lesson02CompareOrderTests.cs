using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// UAE Grade 5 Arabic student 01.pdf, printed pp17–22.
// Written for Stage D; not executed in Stage A-B-C.
public sealed class UaeGrade5Unit1Lesson02CompareOrderTests
{
    private const string Lesson = "PED:UAE-MOE-MATH:L5:GENERAL:01:02";

    [Fact]
    public void ComparisonAndOrderingFamiliesUseExactRegisteredQuestionRouting()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson, out var contract));
        Assert.Equal("number.place_value.compare_and_order_to_millions", contract!.SkillId);
        Assert.Equal(4, contract.AllowedQuestionFamilies.Count);
        foreach (var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family, out var ui));
            Assert.NotNull(ui);
        }
    }

    [Fact]
    public void MillionPlaceValueComparisonAndOrderingWorkAcrossSeeds()
    {
        var engine = new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson, out var contract));
        foreach (var family in contract!.AllowedQuestionFamilies)
        for (int seed = 1; seed <= 8; seed++)
        foreach (var item in engine.Generate(
                     Lesson, family, [family],
                     ExactSkillQuestionDifficulty.Standard, 2, seed, []))
        {
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family, item.Parameters, item.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family, item.Parameters, "INVALID"));
            Assert.DoesNotContain("result", item.Parameters.Keys);
        }
    }

    [Fact]
    public void PairComparisonChecksCorrectSymbolAndCorruptedValues()
    {
        const string relation = "uae.g5.u01.l02.compare_to_millions";
        var p = new Dictionary<string, int>
        {
            ["a"] = 95_408_710, ["b"] = 94_804_701
        };
        Assert.True(ExactSkillContractQuestionEngine.Verify(relation, p, ">"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(relation, p, "<"));
        p["a"] = p["b"];
        Assert.True(ExactSkillContractQuestionEngine.Verify(relation, p, "="));
        p["b"] = -1;
        Assert.False(ExactSkillContractQuestionEngine.Verify(relation, p, ">"));
    }

    [Fact]
    public void FourDigitGroupsRequireExactOrderingAndAllFourValues()
    {
        const string ascending = "uae.g5.u01.l02.order_four_ascending";
        var p = new Dictionary<string, int>
        {
            ["a"] = 78_450_120, ["b"] = 78_405_210,
            ["c"] = 87_045_120, ["d"] = 78_450_102
        };
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            ascending, p, "78405210;78450102;78450120;87045120"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            ascending, p, "78450102;78405210;78450120;87045120"));
        p["d"] = p["b"];
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            ascending, p, "78405210;78450102;78450120;87045120"));
    }
}
