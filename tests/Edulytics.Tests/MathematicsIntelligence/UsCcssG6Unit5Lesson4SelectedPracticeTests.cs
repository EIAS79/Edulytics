using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit5 Lesson 4 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit5Lesson4SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U05:L04";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0504PreciseArithmetic;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0504DecimalReasoning;

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
        // usccss.g6.u05.l04.precise_decimal_add_subtract
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0504PreciseArithmetic,0,"353"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0504PreciseArithmetic,1,"146"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0504PreciseArithmetic,2,"8022"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0504PreciseArithmetic,3,"42"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0504PreciseArithmetic,4,"2964"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0504PreciseArithmetic,5,"262"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0504PreciseArithmetic,6,"4863"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0504PreciseArithmetic,7,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0504PreciseArithmetic,8,"64"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0504PreciseArithmetic,9,"14"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0504PreciseArithmetic,10,"90"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0504PreciseArithmetic,11,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0504PreciseArithmetic,12,"27"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0504PreciseArithmetic,13,"4200"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0504PreciseArithmetic,14,"1320"};
        // usccss.g6.u05.l04.placevalue_high_precision_logic
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0504DecimalReasoning,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0504DecimalReasoning,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0504DecimalReasoning,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0504DecimalReasoning,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0504DecimalReasoning,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0504DecimalReasoning,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0504DecimalReasoning,6,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0504DecimalReasoning,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0504DecimalReasoning,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0504DecimalReasoning,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0504DecimalReasoning,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0504DecimalReasoning,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0504DecimalReasoning,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0504DecimalReasoning,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0504DecimalReasoning,14,"1"};
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
            "g6-u05-l04-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
