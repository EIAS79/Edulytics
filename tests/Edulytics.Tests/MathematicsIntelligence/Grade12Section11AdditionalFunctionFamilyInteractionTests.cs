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
}
