using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit6 Lesson 5 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit6Lesson5SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U06:L05";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0605FractionBarNumeric;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0605FractionBarLogic;

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
        // usccss.g6.u06.l05.rational_fraction_bar_division
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0605FractionBarNumeric,0,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0605FractionBarNumeric,1,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0605FractionBarNumeric,2,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0605FractionBarNumeric,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0605FractionBarNumeric,4,"26"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0605FractionBarNumeric,5,"26"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0605FractionBarNumeric,6,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0605FractionBarNumeric,7,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0605FractionBarNumeric,8,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0605FractionBarNumeric,9,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0605FractionBarNumeric,10,"40"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0605FractionBarNumeric,11,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0605FractionBarNumeric,12,"48"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0605FractionBarNumeric,13,"390"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0605FractionBarNumeric,14,"75"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0605FractionBarNumeric,15,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0605FractionBarNumeric,16,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0605FractionBarNumeric,17,"5"};
        // usccss.g6.u06.l05.fraction_bar_meaning_and_solutions
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0605FractionBarLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0605FractionBarLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0605FractionBarLogic,2,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0605FractionBarLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0605FractionBarLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0605FractionBarLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0605FractionBarLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0605FractionBarLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0605FractionBarLogic,8,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0605FractionBarLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0605FractionBarLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0605FractionBarLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0605FractionBarLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0605FractionBarLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0605FractionBarLogic,14,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0605FractionBarLogic,15,"1"};
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
            "g6-u06-l05-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
