using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit6 Lesson 6 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit6Lesson6SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U06:L06";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0606Expressions;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0606ExpressionLogic;

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
        // usccss.g6.u06.l06.expression_evaluation_contexts
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606Expressions,0,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606Expressions,1,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606Expressions,2,"19"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606Expressions,3,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606Expressions,4,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606Expressions,5,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606Expressions,6,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606Expressions,7,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606Expressions,8,"315"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606Expressions,9,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606Expressions,10,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606Expressions,11,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606Expressions,12,"35"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606Expressions,13,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606Expressions,14,"60"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606Expressions,15,"90"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606Expressions,16,"200"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606Expressions,17,"18"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606Expressions,18,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606Expressions,19,"7"};
        // usccss.g6.u06.l06.expression_interpretation
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606ExpressionLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606ExpressionLogic,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606ExpressionLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606ExpressionLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606ExpressionLogic,4,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606ExpressionLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606ExpressionLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606ExpressionLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606ExpressionLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606ExpressionLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606ExpressionLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606ExpressionLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606ExpressionLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606ExpressionLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606ExpressionLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0606ExpressionLogic,15,"1"};
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
            "g6-u06-l06-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
