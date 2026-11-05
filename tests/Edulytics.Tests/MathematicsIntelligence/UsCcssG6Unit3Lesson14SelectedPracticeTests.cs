using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit3 Lesson 14 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit3Lesson14SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U03:L14";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0314PercentComputations;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0314PercentInterpretation;

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
        // usccss.g6.u03.l14.percent_unknowns
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0314PercentComputations,0,"400"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0314PercentComputations,1,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0314PercentComputations,2,"64"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0314PercentComputations,3,"65"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0314PercentComputations,4,"75"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0314PercentComputations,5,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0314PercentComputations,6,"120"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0314PercentComputations,7,"75"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0314PercentComputations,8,"75"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0314PercentComputations,9,"160"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0314PercentComputations,10,"60"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0314PercentComputations,11,"2000"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0314PercentComputations,12,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0314PercentComputations,13,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0314PercentComputations,14,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0314PercentComputations,15,"8281"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0314PercentComputations,16,"75"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0314PercentComputations,17,"20"};
        // usccss.g6.u03.l14.percent_compare_and_inverse
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0314PercentInterpretation,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0314PercentInterpretation,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0314PercentInterpretation,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0314PercentInterpretation,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0314PercentInterpretation,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0314PercentInterpretation,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0314PercentInterpretation,6,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0314PercentInterpretation,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0314PercentInterpretation,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0314PercentInterpretation,9,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0314PercentInterpretation,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0314PercentInterpretation,11,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0314PercentInterpretation,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0314PercentInterpretation,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0314PercentInterpretation,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0314PercentInterpretation,15,"0"};
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
            "g6-u03-l14-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
