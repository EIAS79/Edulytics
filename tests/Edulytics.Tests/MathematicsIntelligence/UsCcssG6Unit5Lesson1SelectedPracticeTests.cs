using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit5 Lesson 1 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit5Lesson1SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U05:L01";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0501DecimalArithmetic;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0501DecimalEstimation;

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
        // usccss.g6.u05.l01.shopping_decimal_arithmetic
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0501DecimalArithmetic,0,"490"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0501DecimalArithmetic,1,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0501DecimalArithmetic,2,"3750"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0501DecimalArithmetic,3,"1100"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0501DecimalArithmetic,4,"1275"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0501DecimalArithmetic,5,"2375"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0501DecimalArithmetic,6,"125"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0501DecimalArithmetic,7,"960"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0501DecimalArithmetic,8,"1377"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0501DecimalArithmetic,9,"417"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0501DecimalArithmetic,10,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0501DecimalArithmetic,11,"32"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0501DecimalArithmetic,12,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0501DecimalArithmetic,13,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0501DecimalArithmetic,14,"14"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0501DecimalArithmetic,15,"40"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0501DecimalArithmetic,16,"320"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0501DecimalArithmetic,17,"490"};
        // usccss.g6.u05.l01.shopping_reasonableness
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0501DecimalEstimation,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0501DecimalEstimation,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0501DecimalEstimation,2,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0501DecimalEstimation,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0501DecimalEstimation,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0501DecimalEstimation,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0501DecimalEstimation,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0501DecimalEstimation,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0501DecimalEstimation,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0501DecimalEstimation,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0501DecimalEstimation,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0501DecimalEstimation,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0501DecimalEstimation,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0501DecimalEstimation,13,"1"};
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
            "g6-u05-l01-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
