using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit5 Lesson 13 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit5Lesson13SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U05:L13";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0513DecimalQuotients;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0513DecimalLogic;

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
        // usccss.g6.u05.l13.decimal_divisor_quotients
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0513DecimalQuotients,0,"182"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0513DecimalQuotients,1,"243"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0513DecimalQuotients,2,"191"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0513DecimalQuotients,3,"191"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0513DecimalQuotients,4,"191"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0513DecimalQuotients,5,"2040"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0513DecimalQuotients,6,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0513DecimalQuotients,7,"32"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0513DecimalQuotients,8,"955"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0513DecimalQuotients,9,"717"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0513DecimalQuotients,10,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0513DecimalQuotients,11,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0513DecimalQuotients,12,"92"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0513DecimalQuotients,13,"23"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0513DecimalQuotients,14,"506"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0513DecimalQuotients,15,"3159"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0513DecimalQuotients,16,"5100"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0513DecimalQuotients,17,"182"};
        // usccss.g6.u05.l13.decimal_divisor_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0513DecimalLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0513DecimalLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0513DecimalLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0513DecimalLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0513DecimalLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0513DecimalLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0513DecimalLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0513DecimalLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0513DecimalLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0513DecimalLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0513DecimalLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0513DecimalLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0513DecimalLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0513DecimalLogic,13,"1"};
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
            "g6-u05-l13-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
