using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit8 Lesson 7 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit8Lesson7SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U08:L07";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0807HistogramCompare;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0807HistogramLogic;

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
        // usccss.g6.u08.l07.histogram_center_spread_numeric
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramCompare,0,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramCompare,1,"110"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramCompare,2,"50"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramCompare,3,"60"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramCompare,4,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramCompare,5,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramCompare,6,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramCompare,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramCompare,8,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramCompare,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramCompare,10,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramCompare,11,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramCompare,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramCompare,13,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramCompare,14,"-5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramCompare,15,"-7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramCompare,16,"-4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramCompare,17,"40"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramCompare,18,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramCompare,19,"40"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramCompare,20,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramCompare,21,"15"};
        // usccss.g6.u08.l07.center_spread_and_inequality_interpretation
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramLogic,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramLogic,9,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramLogic,11,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramLogic,13,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramLogic,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramLogic,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0807HistogramLogic,17,"1"};
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
            "g6-u08-l07-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
