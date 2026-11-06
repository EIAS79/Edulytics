using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit7 Lesson 9 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit7Lesson9SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U07:L09";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0709BoundedSolutions;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0709SolutionLogic;

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
        // usccss.g6.u07.l09.inequality_solution_values
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709BoundedSolutions,0,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709BoundedSolutions,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709BoundedSolutions,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709BoundedSolutions,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709BoundedSolutions,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709BoundedSolutions,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709BoundedSolutions,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709BoundedSolutions,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709BoundedSolutions,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709BoundedSolutions,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709BoundedSolutions,10,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709BoundedSolutions,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709BoundedSolutions,12,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709BoundedSolutions,13,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709BoundedSolutions,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709BoundedSolutions,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709BoundedSolutions,16,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709BoundedSolutions,17,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709BoundedSolutions,18,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709BoundedSolutions,19,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709BoundedSolutions,20,"60"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709BoundedSolutions,21,"750"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709BoundedSolutions,22,"1"};
        // usccss.g6.u07.l09.inequality_solution_set_logic
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709SolutionLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709SolutionLogic,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709SolutionLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709SolutionLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709SolutionLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709SolutionLogic,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709SolutionLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709SolutionLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709SolutionLogic,8,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709SolutionLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709SolutionLogic,10,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709SolutionLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709SolutionLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709SolutionLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709SolutionLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709SolutionLogic,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709SolutionLogic,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709SolutionLogic,17,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0709SolutionLogic,18,"1"};
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
            "g6-u07-l09-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
