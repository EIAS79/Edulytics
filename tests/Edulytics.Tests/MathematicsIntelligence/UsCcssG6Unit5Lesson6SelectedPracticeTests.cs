using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit5 Lesson 6 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit5Lesson6SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U05:L06";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0506Products;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0506MethodsLogic;

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
        // usccss.g6.u05.l06.decimal_multiplication_methods
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0506Products,0,"132"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0506Products,1,"68"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0506Products,2,"240"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0506Products,3,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0506Products,4,"229"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0506Products,5,"346"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0506Products,6,"117"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0506Products,7,"140"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0506Products,8,"1602"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0506Products,9,"3950"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0506Products,10,"1540"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0506Products,11,"21"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0506Products,12,"294"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0506Products,13,"36"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0506Products,14,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0506Products,15,"3458"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0506Products,16,"150"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0506Products,17,"15"};
        // usccss.g6.u05.l06.decimal_product_scale_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0506MethodsLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0506MethodsLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0506MethodsLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0506MethodsLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0506MethodsLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0506MethodsLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0506MethodsLogic,6,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0506MethodsLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0506MethodsLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0506MethodsLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0506MethodsLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0506MethodsLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0506MethodsLogic,12,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0506MethodsLogic,13,"1"};
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
            "g6-u05-l06-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
