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
        Assert.Equal(
            290, // +2 exact source-bound G1 U01 L01 practice families
            LessonPracticeInteractionRegistry.All.Count);

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
