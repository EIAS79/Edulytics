using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit3 Lesson 9 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit3Lesson9SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U03:L09";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0309RateValues;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0309ComparisonLogic;

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
        // usccss.g6.u03.l09.source_unit_rate_calculations
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0309RateValues,0,"120"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0309RateValues,1,"1200"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0309RateValues,2,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0309RateValues,3,"18000"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0309RateValues,4,"12100"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0309RateValues,5,"990"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0309RateValues,6,"17600"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0309RateValues,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0309RateValues,8,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0309RateValues,9,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0309RateValues,10,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0309RateValues,11,"60"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0309RateValues,12,"85"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0309RateValues,13,"150"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0309RateValues,14,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0309RateValues,15,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0309RateValues,16,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0309RateValues,17,"10"};
        // usccss.g6.u03.l09.solve_rate_comparison
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0309ComparisonLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0309ComparisonLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0309ComparisonLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0309ComparisonLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0309ComparisonLogic,4,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0309ComparisonLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0309ComparisonLogic,6,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0309ComparisonLogic,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0309ComparisonLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0309ComparisonLogic,9,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0309ComparisonLogic,10,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0309ComparisonLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0309ComparisonLogic,12,"1"};
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
            "g6-u03-l09-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
