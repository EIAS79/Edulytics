using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit7 Lesson 3 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit7Lesson3SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U07:L03";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0703Comparisons;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0703SignedLogic;

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
        // usccss.g6.u07.l03.signed_order_comparisons
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703Comparisons,0,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703Comparisons,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703Comparisons,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703Comparisons,3,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703Comparisons,4,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703Comparisons,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703Comparisons,6,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703Comparisons,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703Comparisons,8,"-282"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703Comparisons,9,"3350"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703Comparisons,10,"-8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703Comparisons,11,"2842"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703Comparisons,12,"3099"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703Comparisons,13,"800"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703Comparisons,14,"2000"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703Comparisons,15,"5600"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703Comparisons,16,"19"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703Comparisons,17,"87"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703Comparisons,18,"13"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703Comparisons,19,"2080"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703Comparisons,20,"274"};
        // usccss.g6.u07.l03.signed_inequality_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703SignedLogic,0,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703SignedLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703SignedLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703SignedLogic,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703SignedLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703SignedLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703SignedLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703SignedLogic,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703SignedLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703SignedLogic,9,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703SignedLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703SignedLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703SignedLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703SignedLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703SignedLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703SignedLogic,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0703SignedLogic,16,"1"};
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
            "g6-u07-l03-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
