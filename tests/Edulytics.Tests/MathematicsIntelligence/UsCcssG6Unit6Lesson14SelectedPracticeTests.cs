using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit6 Lesson 14 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit6Lesson14SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U06:L14";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0614ExponentEvaluation;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0614ExponentOrderLogic;

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
        // usccss.g6.u06.l14.exponent_order_numeric
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentEvaluation,0,"72"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentEvaluation,1,"576"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentEvaluation,2,"64"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentEvaluation,3,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentEvaluation,4,"27"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentEvaluation,5,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentEvaluation,6,"72"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentEvaluation,7,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentEvaluation,8,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentEvaluation,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentEvaluation,10,"8000"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentEvaluation,11,"500"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentEvaluation,12,"1250"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentEvaluation,13,"1500"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentEvaluation,14,"24"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentEvaluation,15,"40"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentEvaluation,16,"275"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentEvaluation,17,"345"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentEvaluation,18,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentEvaluation,19,"27"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentEvaluation,20,"81"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentEvaluation,21,"16"};
        // usccss.g6.u06.l14.exponent_evaluation_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentOrderLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentOrderLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentOrderLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentOrderLogic,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentOrderLogic,4,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentOrderLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentOrderLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentOrderLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentOrderLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentOrderLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentOrderLogic,10,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentOrderLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentOrderLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentOrderLogic,13,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentOrderLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentOrderLogic,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentOrderLogic,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentOrderLogic,17,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0614ExponentOrderLogic,18,"1"};
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
            "g6-u06-l14-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
