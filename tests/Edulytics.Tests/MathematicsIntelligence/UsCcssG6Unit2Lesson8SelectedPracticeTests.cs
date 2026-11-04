using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit2 Lesson 8 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit2Lesson8SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U02:L08";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L08UnitPrices;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L08PriceLogic;

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
        // usccss.g6.u02.l08.unit_prices_and_counts
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L08UnitPrices,0,"1320"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L08UnitPrices,1,"3960"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L08UnitPrices,2,"5280"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L08UnitPrices,3,"350"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L08UnitPrices,4,"700"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L08UnitPrices,5,"1050"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L08UnitPrices,6,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L08UnitPrices,7,"60"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L08UnitPrices,8,"132"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L08UnitPrices,9,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L08UnitPrices,10,"250"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L08UnitPrices,11,"50"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L08UnitPrices,12,"35"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L08UnitPrices,13,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L08UnitPrices,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L08UnitPrices,15,"180"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L08UnitPrices,16,"292"};
        // usccss.g6.u02.l08.unit_price_interpretation
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L08PriceLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L08PriceLogic,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L08PriceLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L08PriceLogic,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L08PriceLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L08PriceLogic,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L08PriceLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L08PriceLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L08PriceLogic,8,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L08PriceLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L08PriceLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L08PriceLogic,11,"0"};
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
            "g6-u02-l08-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
