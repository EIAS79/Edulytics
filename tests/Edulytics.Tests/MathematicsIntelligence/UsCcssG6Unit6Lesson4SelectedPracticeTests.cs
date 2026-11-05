using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit6 Lesson 4 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit6Lesson4SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U06:L04";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0604SolveValues;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0604EquationReasoning;

    [Fact] public void BothFamiliesAreLessonScopedAndSupported()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        Assert.Equal("READY_VERIFIED",contract.Readiness);
        Assert.Equal(new[]{FamilyA,FamilyB},contract.AllowedQuestionFamilies);
        Assert.All(contract.AllowedQuestionFamilies,
            f=>Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f)));
    }

    public static IEnumerable<object[]> SourceMathVariants()
    {
        // usccss.g6.u06.l04.equation_context_solutions
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0604SolveValues,0,"11"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0604SolveValues,1,"300"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0604SolveValues,2,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0604SolveValues,3,"129"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0604SolveValues,4,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0604SolveValues,5,"11"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0604SolveValues,6,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0604SolveValues,7,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0604SolveValues,8,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0604SolveValues,9,"11"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0604SolveValues,10,"424"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0604SolveValues,11,"82"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0604SolveValues,12,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0604SolveValues,13,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0604SolveValues,14,"240"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0604SolveValues,15,"14"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0604SolveValues,16,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0604SolveValues,17,"13"};
        // usccss.g6.u06.l04.equation_choice_and_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0604EquationReasoning,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0604EquationReasoning,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0604EquationReasoning,2,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0604EquationReasoning,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0604EquationReasoning,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0604EquationReasoning,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0604EquationReasoning,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0604EquationReasoning,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0604EquationReasoning,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0604EquationReasoning,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0604EquationReasoning,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0604EquationReasoning,11,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0604EquationReasoning,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0604EquationReasoning,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0604EquationReasoning,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0604EquationReasoning,15,"1"};
    }

    [Theory]
    [MemberData(nameof(SourceMathVariants))]
    public void OriginalMathAndIndependentVariantsCheckExpectedAnswers(string family,int variant,string expected)
    {
        var parameters=new Dictionary<string,int>{{"variant",variant}};
        Assert.Equal(expected,UsCcssG6Unit2RatioQuestions.Solve(family,parameters));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,parameters,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,parameters,"999999"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,
           new Dictionary<string,int>{{"variant",variant},{"injected",1}},expected));
    }

    [Fact] public void NoForeignFamilyCanBeAccepted()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        var result=LessonPracticeAlignmentValidator.ValidatePersisted(
            contract.ToLegacyStage18Contract(),
            UsCcssG6Unit2RatioQuestions.L05Equivalence,
            new Dictionary<string,int>{{"variant",0}},"1");
        Assert.False(result.IsAligned);
        Assert.Equal("QuestionFamilyNotAllowed",result.ReasonCode);
    }

    [Fact] public void SelectedQuestionGenerationAndVerificationPreserveTeacherBoundary()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        var questions=new ExactSkillContractQuestionEngine().Generate(
            "g6-u06-l04-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
