using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit6 Lesson 2 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit6Lesson2SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U06:L02";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0602EquationSolutions;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0602EquationTruth;

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
        // usccss.g6.u06.l02.variable_equation_solutions
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationSolutions,0,"14"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationSolutions,1,"69"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationSolutions,2,"23"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationSolutions,3,"26"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationSolutions,4,"92"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationSolutions,5,"66"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationSolutions,6,"13"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationSolutions,7,"24"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationSolutions,8,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationSolutions,9,"3375"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationSolutions,10,"18"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationSolutions,11,"72"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationSolutions,12,"80"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationSolutions,13,"35"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationSolutions,14,"2623"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationSolutions,15,"73224"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationSolutions,16,"193362"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationSolutions,17,"14"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationSolutions,18,"69"};
        // usccss.g6.u06.l02.equation_truth_and_model_logic
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationTruth,0,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationTruth,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationTruth,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationTruth,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationTruth,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationTruth,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationTruth,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationTruth,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationTruth,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationTruth,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationTruth,10,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationTruth,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationTruth,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationTruth,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationTruth,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationTruth,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0602EquationTruth,16,"0"};
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
            "g6-u06-l02-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
