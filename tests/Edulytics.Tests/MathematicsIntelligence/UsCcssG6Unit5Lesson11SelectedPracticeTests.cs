using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit5 Lesson 11 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit5Lesson11SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U05:L11";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0511DecimalQuotients;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0511DivisionReasoning;

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
        // usccss.g6.u05.l11.whole_division_decimal_results
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0511DecimalQuotients,0,"75"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0511DecimalQuotients,1,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0511DecimalQuotients,2,"28"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0511DecimalQuotients,3,"30"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0511DecimalQuotients,4,"375"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0511DecimalQuotients,5,"825"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0511DecimalQuotients,6,"432"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0511DecimalQuotients,7,"24850"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0511DecimalQuotients,8,"36"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0511DecimalQuotients,9,"36"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0511DecimalQuotients,10,"36"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0511DecimalQuotients,11,"671"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0511DecimalQuotients,12,"985"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0511DecimalQuotients,13,"137"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0511DecimalQuotients,14,"1860"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0511DecimalQuotients,15,"1860"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0511DecimalQuotients,16,"28"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0511DecimalQuotients,17,"10"};
        // usccss.g6.u05.l11.decimal_quotient_placevalue_logic
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0511DivisionReasoning,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0511DivisionReasoning,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0511DivisionReasoning,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0511DivisionReasoning,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0511DivisionReasoning,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0511DivisionReasoning,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0511DivisionReasoning,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0511DivisionReasoning,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0511DivisionReasoning,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0511DivisionReasoning,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0511DivisionReasoning,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0511DivisionReasoning,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0511DivisionReasoning,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0511DivisionReasoning,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0511DivisionReasoning,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0511DivisionReasoning,15,"0"};
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
            "g6-u05-l11-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
