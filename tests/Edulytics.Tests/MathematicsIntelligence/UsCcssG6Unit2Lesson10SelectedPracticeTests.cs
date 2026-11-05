using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit2 Lesson 10 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit2Lesson10SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U02:L10";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L10CompareAmounts;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L10EquivalentContexts;

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
        // usccss.g6.u02.l10.rate_comparison_quantities
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L10CompareAmounts,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L10CompareAmounts,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L10CompareAmounts,2,"560"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L10CompareAmounts,3,"5600"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L10CompareAmounts,4,"1500"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L10CompareAmounts,5,"250"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L10CompareAmounts,6,"125"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L10CompareAmounts,7,"375"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L10CompareAmounts,8,"1875"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L10CompareAmounts,9,"220"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L10CompareAmounts,10,"2600"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L10CompareAmounts,11,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L10CompareAmounts,12,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L10CompareAmounts,13,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L10CompareAmounts,14,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L10CompareAmounts,15,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L10CompareAmounts,16,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L10CompareAmounts,17,"10"};
        // usccss.g6.u02.l10.compare_rates_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L10EquivalentContexts,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L10EquivalentContexts,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L10EquivalentContexts,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L10EquivalentContexts,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L10EquivalentContexts,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L10EquivalentContexts,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L10EquivalentContexts,6,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L10EquivalentContexts,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L10EquivalentContexts,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L10EquivalentContexts,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L10EquivalentContexts,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L10EquivalentContexts,11,"1"};
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
            "g6-u02-l10-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
