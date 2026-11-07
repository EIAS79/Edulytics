using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class Grade12Section11AdditionalFunctionFamilyInteractionTests
{
    public static IEnumerable<object[]> FourNewlyRegisteredFamilies()
    {
        yield return [UsCcssAlg1Unit4FunctionQuestions.FunctionOneToOneFamily];
        yield return [UsCcssAlg1Unit4FunctionQuestions.FunctionImplicitClassificationFamily];
        yield return [UsCcssAlg1Unit4FunctionQuestions.FunctionTableLookupFamily];
        yield return [UsCcssAlg1Unit4FunctionQuestions.FunctionQuadraticRootsFamily];
    }

    [Theory]
    [MemberData(nameof(FourNewlyRegisteredFamilies))]
    public void Grade12Section11CanGenerateDisplayAndVerifyNewFunctionFamilies(string family)
    {
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family, out var interaction));
        Assert.NotNull(interaction);
        Assert.Equal(LessonPracticeInteractionKind.ContractDriven, interaction.InteractionKind);
        Assert.True(UsCcssAlg1Unit4FunctionQuestions.Supports(family));

        for (var seed = 0; seed < 20; seed++)
        {
            var problem = UsCcssAlg1Unit4FunctionQuestions.Build(family, new Random(seed));
            Assert.Equal(family, problem.Family);
            Assert.False(string.IsNullOrWhiteSpace(problem.Prompt));
            var correct = UsCcssAlg1Unit4FunctionQuestions.Solve(family, problem.Parameters);
            Assert.True(UsCcssAlg1Unit4FunctionQuestions.Verify(family, problem.Parameters, correct));
            Assert.False(UsCcssAlg1Unit4FunctionQuestions.Verify(family, problem.Parameters,
                "NOT_A_VALID_ANSWER"));
        }
    }

    [Fact]
    public void ImplicitFunctionPoolCoversSixteenDistinctMathematicalRelations()
    {
        const string family = UsCcssAlg1Unit4FunctionQuestions.FunctionImplicitClassificationFamily;
        var yesVariants = new HashSet<int> { 0, 3, 4, 6, 7, 9, 11, 13, 15 };
        for (var variant = 0; variant < 16; variant++)
        {
            var parameters = new Dictionary<string, int> { ["variant"] = variant };
            var expected = yesVariants.Contains(variant) ? "yes" : "no";
            Assert.Equal(expected, UsCcssAlg1Unit4FunctionQuestions.Solve(family, parameters));
            Assert.True(UsCcssAlg1Unit4FunctionQuestions.Verify(family, parameters, expected));
            Assert.False(UsCcssAlg1Unit4FunctionQuestions.Verify(family, parameters,
                expected == "yes" ? "no" : "yes"));
        }
        Assert.False(UsCcssAlg1Unit4FunctionQuestions.Verify(family,
            new Dictionary<string, int> { ["variant"] = -1 }, "yes"));
        Assert.False(UsCcssAlg1Unit4FunctionQuestions.Verify(family,
            new Dictionary<string, int> { ["variant"] = 16 }, "yes"));
        var generated = Enumerable.Range(1, 256)
            .Select(seed => UsCcssAlg1Unit4FunctionQuestions.Build(family, new Random(seed)).Prompt)
            .Distinct(StringComparer.Ordinal)
            .Count();
        Assert.True(generated >= 10, "Family must support a meaningful ten-question session.");
    }
}
