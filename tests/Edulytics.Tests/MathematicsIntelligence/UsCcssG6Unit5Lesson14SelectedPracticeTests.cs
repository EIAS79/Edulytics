using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit5 Lesson 14 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit5Lesson14SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U05:L14";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0514DecimalContext;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0514DecimalReasoning;

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
        // usccss.g6.u05.l14.decimal_context_calculations
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0514DecimalContext,0,"84"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0514DecimalContext,1,"14"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0514DecimalContext,2,"54"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0514DecimalContext,3,"400"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0514DecimalContext,4,"450"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0514DecimalContext,5,"800"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0514DecimalContext,6,"492"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0514DecimalContext,7,"550"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0514DecimalContext,8,"495"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0514DecimalContext,9,"95"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0514DecimalContext,10,"123"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0514DecimalContext,11,"745"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0514DecimalContext,12,"9706"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0514DecimalContext,13,"22"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0514DecimalContext,14,"265"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0514DecimalContext,15,"4257"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0514DecimalContext,16,"840"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0514DecimalContext,17,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0514DecimalContext,18,"495"};
        // usccss.g6.u05.l14.decimal_application_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0514DecimalReasoning,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0514DecimalReasoning,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0514DecimalReasoning,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0514DecimalReasoning,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0514DecimalReasoning,4,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0514DecimalReasoning,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0514DecimalReasoning,6,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0514DecimalReasoning,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0514DecimalReasoning,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0514DecimalReasoning,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0514DecimalReasoning,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0514DecimalReasoning,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0514DecimalReasoning,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0514DecimalReasoning,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0514DecimalReasoning,14,"1"};
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
            "g6-u05-l14-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
