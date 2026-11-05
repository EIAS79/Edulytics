using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit5 Lesson 5 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit5Lesson5SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U05:L05";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0505DecimalProducts;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0505DecimalLogic;

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
        // usccss.g6.u05.l05.decimal_products_base_ten
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0505DecimalProducts,0,"1221"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0505DecimalProducts,1,"1180"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0505DecimalProducts,2,"13501"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0505DecimalProducts,3,"1704"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0505DecimalProducts,4,"924"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0505DecimalProducts,5,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0505DecimalProducts,6,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0505DecimalProducts,7,"60"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0505DecimalProducts,8,"99"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0505DecimalProducts,9,"30"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0505DecimalProducts,10,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0505DecimalProducts,11,"3505"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0505DecimalProducts,12,"28180"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0505DecimalProducts,13,"9681"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0505DecimalProducts,14,"1142"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0505DecimalProducts,15,"22734"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0505DecimalProducts,16,"655"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0505DecimalProducts,17,"6"};
        // usccss.g6.u05.l05.decimal_product_placevalue_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0505DecimalLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0505DecimalLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0505DecimalLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0505DecimalLogic,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0505DecimalLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0505DecimalLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0505DecimalLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0505DecimalLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0505DecimalLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0505DecimalLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0505DecimalLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0505DecimalLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0505DecimalLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0505DecimalLogic,13,"1"};
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
            "g6-u05-l05-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
