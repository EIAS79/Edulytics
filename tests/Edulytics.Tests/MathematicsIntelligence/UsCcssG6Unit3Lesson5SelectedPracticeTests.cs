using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit3 Lesson 5 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit3Lesson5SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U03:L05";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0305Rates;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0305Comparisons;

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
        // usccss.g6.u03.l05.source_speed_price_values
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0305Rates,0,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0305Rates,1,"22"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0305Rates,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0305Rates,3,"62"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0305Rates,4,"60"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0305Rates,5,"59"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0305Rates,6,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0305Rates,7,"1200"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0305Rates,8,"231000"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0305Rates,9,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0305Rates,10,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0305Rates,11,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0305Rates,12,"170"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0305Rates,13,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0305Rates,14,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0305Rates,15,"540000"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0305Rates,16,"11000"};
        // usccss.g6.u03.l05.same_rate_price_speed_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0305Comparisons,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0305Comparisons,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0305Comparisons,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0305Comparisons,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0305Comparisons,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0305Comparisons,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0305Comparisons,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0305Comparisons,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0305Comparisons,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0305Comparisons,9,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0305Comparisons,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0305Comparisons,11,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0305Comparisons,12,"1"};
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
            "g6-u03-l05-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
