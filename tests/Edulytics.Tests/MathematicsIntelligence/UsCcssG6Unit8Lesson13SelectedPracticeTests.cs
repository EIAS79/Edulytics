using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit8 Lesson 13 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit8Lesson13SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U08:L13";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0813Medians;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0813MedianLogic;

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
        // usccss.g6.u08.l13.median_sorted_samples
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813Medians,0,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813Medians,1,"140"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813Medians,2,"150"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813Medians,3,"145"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813Medians,4,"120"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813Medians,5,"190"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813Medians,6,"70"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813Medians,7,"1470"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813Medians,8,"147"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813Medians,9,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813Medians,10,"13"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813Medians,11,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813Medians,12,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813Medians,13,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813Medians,14,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813Medians,15,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813Medians,16,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813Medians,17,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813Medians,18,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813Medians,19,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813Medians,20,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813Medians,21,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813Medians,22,"6"};
        // usccss.g6.u08.l13.median_interpretations
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813MedianLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813MedianLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813MedianLogic,2,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813MedianLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813MedianLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813MedianLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813MedianLogic,6,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813MedianLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813MedianLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813MedianLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813MedianLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813MedianLogic,11,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813MedianLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813MedianLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813MedianLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813MedianLogic,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813MedianLogic,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0813MedianLogic,17,"1"};
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
            "g6-u08-l13-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
