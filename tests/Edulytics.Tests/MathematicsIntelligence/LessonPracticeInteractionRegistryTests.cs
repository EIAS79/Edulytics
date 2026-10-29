using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class LessonPracticeInteractionRegistryTests
{
    [Fact]
    public void EveryRuntimeLessonPracticeFamilyHasAnInteractionContract()
    {
        var runtimeFamilies = LessonPracticeContractRegistry.All
            .SelectMany(contract =>
                contract.AllowedQuestionFamilies)
            .Distinct(StringComparer.Ordinal)
            .OrderBy(x => x, StringComparer.Ordinal)
            .ToArray();

        Assert.NotEmpty(runtimeFamilies);

        var missing = runtimeFamilies
            .Where(family =>
                !LessonPracticeInteractionRegistry.TryResolve(
                    family,
                    out _))
            .ToArray();

        Assert.Empty(missing);
    }

    [Fact]
    public void EveryRegistryLessonPracticeFamilyHasAPresentationInteraction()
    {
        // Preserve the previous 1,070 interaction contracts while adding
        // the source-reviewed Grade 8 two-variable membership family.
        Assert.Equal(1070 + 1, LessonPracticeInteractionRegistry.All.Count);
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(
            "usccss.g8.u03.l12.ordered_pair_membership", out var grade8));
        Assert.NotNull(grade8);
        Assert.Equal("enum_text", grade8!.AnswerType);

        Assert.All(
            LessonPracticeInteractionRegistry.All,
            contract =>
            {
                Assert.False(
                    string.IsNullOrWhiteSpace(
                        contract.FamilyId));
                Assert.False(
                    string.IsNullOrWhiteSpace(
                        contract.AnswerType));
                Assert.True(
                    Enum.IsDefined(
                        contract.InteractionKind));
            });
    }

    [Theory]
    [InlineData("usccss.t001.g1.dot_pair_sum")]
    [InlineData("usccss.t001.g1.add_one_or_two")]
    [InlineData("usccss.t001.g1.subtract_one_or_two")]
    [InlineData("usccss.t001.g1.mixed_add_subtract")]
    [InlineData("usccss.t001.g1.make_ten_missing_part")]
    [InlineData("usccss.t001.g1.sort_math_tools")]
    [InlineData("usccss.t001.g1.sort_shape_cards")]
    [InlineData("usccss.t001.g1.class_survey_categories")]
    [InlineData("usccss.t001.g1.choose_dot_expression")]
    [InlineData("usccss.t001.g1.choose_tool_sort")]
    [InlineData("usccss.t001.g1.choose_shape_chart")]
    [InlineData("usccss.t001.g1.choose_survey_table")]
    public void AllTwelveT001FamiliesHaveSafeIntegerKeypadAndVisualInteraction(string family)
    {
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var contract));
        Assert.NotNull(contract);
        Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,contract.InteractionKind);
        Assert.True(contract.HasVisualRepresentation);
        Assert.Equal("exact_integer",contract.AnswerType);
    }

    [Theory]
    [InlineData("usccss.t001.g1.write_dot_expression")]
    [InlineData("usccss.t001.g1.construct_tool_sort")]
    [InlineData("usccss.t001.g1.collect_survey_graph")]
    public void NewT001AuthoringFamiliesAreContractDriven(string family)
    {
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var contract));
        Assert.NotNull(contract);
        Assert.Equal(LessonPracticeInteractionKind.ContractDriven,contract.InteractionKind);
        Assert.Equal("contract_defined",contract.AnswerType);
        Assert.True(contract.HasVisualRepresentation);
    }

    [Fact]
    public void L08ConstructedShapeGraphUsesAContractDrivenInteraction()
    {
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(
            "usccss.t001.g1.construct_shape_graph",out var contract));
        Assert.NotNull(contract);
        Assert.Equal(LessonPracticeInteractionKind.ContractDriven, contract.InteractionKind);
        Assert.Equal("contract_defined", contract.AnswerType);
        Assert.True(contract.HasVisualRepresentation);
    }

    [Theory]
    [InlineData("usccss.g1.u01.l01.count_collection_to20")]
    [InlineData("usccss.g1.u01.l01.add_dot_cubes_to10")]
    public void G1CountAndAddInteractiveQuestionsUseDotVisualsAndIntegerKeypad(string family)
    {
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family, out var contract));
        Assert.NotNull(contract);
        Assert.True(contract!.HasVisualRepresentation);
        Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad, contract.InteractionKind);
    }

    [Theory]
    [InlineData(
        "fractions.compare.unlike.common_denominator",
        LessonPracticeInteractionKind.RelationChoice)]
    [InlineData(
        "fractions.of_quantity.build",
        LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData(
        "fractions.add_subtract.common_denominator.build",
        LessonPracticeInteractionKind.FractionEntry)]
    [InlineData(
        "geometry.coordinate.evaluate_linear_rule",
        LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData(
        "algebra.linear.inequality.ax_plus_b_relation_c",
        LessonPracticeInteractionKind.StructuredEntry)]
    public void KnownFamiliesResolveExpectedInteraction(
        string family,
        LessonPracticeInteractionKind expected)
    {
        Assert.True(
            LessonPracticeInteractionRegistry.TryResolve(
                family,
                out var contract));

        Assert.NotNull(contract);
        Assert.Equal(
            expected,
            contract!.InteractionKind);
    }
}
