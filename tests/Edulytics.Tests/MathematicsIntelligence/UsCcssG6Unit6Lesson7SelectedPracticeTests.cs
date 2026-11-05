using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit6 Lesson 7 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit6Lesson7SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U06:L07";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0607PercentEquations;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0607PercentEquationLogic;

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
        // usccss.g6.u06.l07.percent_equation_unknown_whole
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquations,0,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquations,1,"175"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquations,2,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquations,3,"55"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquations,4,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquations,5,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquations,6,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquations,7,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquations,8,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquations,9,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquations,10,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquations,11,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquations,12,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquations,13,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquations,14,"3500"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquations,15,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquations,16,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquations,17,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquations,18,"11"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquations,19,"12"};
        // usccss.g6.u06.l07.percent_equation_interpretation
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquationLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquationLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquationLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquationLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquationLogic,4,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquationLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquationLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquationLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquationLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquationLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquationLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquationLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquationLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquationLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquationLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0607PercentEquationLogic,15,"1"};
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
            "g6-u06-l07-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
