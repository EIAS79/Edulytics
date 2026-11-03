using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit7 Lesson 4 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit7Lesson4SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U07:L04";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0704Ordering;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0704SourceChecks;

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
        // usccss.g6.u07.l04.rational_order_and_temperature
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704Ordering,0,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704Ordering,1,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704Ordering,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704Ordering,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704Ordering,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704Ordering,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704Ordering,6,"-3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704Ordering,7,"-1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704Ordering,8,"-1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704Ordering,9,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704Ordering,10,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704Ordering,11,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704Ordering,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704Ordering,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704Ordering,14,"-1881"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704Ordering,15,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704Ordering,16,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704Ordering,17,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704Ordering,18,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704Ordering,19,"1"};
        // usccss.g6.u07.l04.rational_order_explanations_and_cumulative
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704SourceChecks,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704SourceChecks,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704SourceChecks,2,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704SourceChecks,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704SourceChecks,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704SourceChecks,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704SourceChecks,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704SourceChecks,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704SourceChecks,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704SourceChecks,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704SourceChecks,10,"153"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704SourceChecks,11,"145"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704SourceChecks,12,"594"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704SourceChecks,13,"1485"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704SourceChecks,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704SourceChecks,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704SourceChecks,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704SourceChecks,17,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704SourceChecks,18,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0704SourceChecks,19,"0"};
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
            "g6-u07-l04-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
