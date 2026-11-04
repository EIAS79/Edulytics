using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit2 Lesson 14 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit2Lesson14SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U02:L14";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L14InfoGapNumbers;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L14InfoGapReasoning;

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
        // usccss.g6.u02.l14.info_gap_ratio_quantities
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L14InfoGapNumbers,0,"150"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L14InfoGapNumbers,1,"2250"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L14InfoGapNumbers,2,"4500"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L14InfoGapNumbers,3,"150"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L14InfoGapNumbers,4,"3150"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L14InfoGapNumbers,5,"329"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L14InfoGapNumbers,6,"1335"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L14InfoGapNumbers,7,"35"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L14InfoGapNumbers,8,"11"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L14InfoGapNumbers,9,"525"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L14InfoGapNumbers,10,"90"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L14InfoGapNumbers,11,"40"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L14InfoGapNumbers,12,"128"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L14InfoGapNumbers,13,"96"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L14InfoGapNumbers,14,"224"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L14InfoGapNumbers,15,"70"};
        // usccss.g6.u02.l14.rate_information_requests
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L14InfoGapReasoning,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L14InfoGapReasoning,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L14InfoGapReasoning,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L14InfoGapReasoning,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L14InfoGapReasoning,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L14InfoGapReasoning,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L14InfoGapReasoning,6,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L14InfoGapReasoning,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L14InfoGapReasoning,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L14InfoGapReasoning,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L14InfoGapReasoning,10,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L14InfoGapReasoning,11,"1"};
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
            "g6-u02-l14-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
