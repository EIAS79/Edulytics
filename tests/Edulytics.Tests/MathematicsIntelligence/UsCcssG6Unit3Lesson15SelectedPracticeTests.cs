using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit3 Lesson 15 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit3Lesson15SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U03:L15";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0315PercentAmounts;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0315PercentProperties;

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
        // usccss.g6.u03.l15.percent_of_quantity_exact
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0315PercentAmounts,0,"30"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0315PercentAmounts,1,"114"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0315PercentAmounts,2,"48"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0315PercentAmounts,3,"882"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0315PercentAmounts,4,"252"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0315PercentAmounts,5,"441"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0315PercentAmounts,6,"99"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0315PercentAmounts,7,"78"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0315PercentAmounts,8,"84"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0315PercentAmounts,9,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0315PercentAmounts,10,"24"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0315PercentAmounts,11,"14"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0315PercentAmounts,12,"45"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0315PercentAmounts,13,"70"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0315PercentAmounts,14,"21"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0315PercentAmounts,15,"88"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0315PercentAmounts,16,"85"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0315PercentAmounts,17,"2112"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0315PercentAmounts,18,"2040"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0315PercentAmounts,19,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0315PercentAmounts,20,"80"};
        // usccss.g6.u03.l15.percent_of_quantity_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0315PercentProperties,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0315PercentProperties,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0315PercentProperties,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0315PercentProperties,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0315PercentProperties,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0315PercentProperties,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0315PercentProperties,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0315PercentProperties,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0315PercentProperties,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0315PercentProperties,9,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0315PercentProperties,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0315PercentProperties,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0315PercentProperties,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0315PercentProperties,13,"1"};
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
            "g6-u03-l15-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
