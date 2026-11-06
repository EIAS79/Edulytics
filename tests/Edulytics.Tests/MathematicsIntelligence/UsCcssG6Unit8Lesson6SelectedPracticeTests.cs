using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit8 Lesson 6 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit8Lesson6SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U08:L06";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0806HistogramNumerics;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0806HistogramLogic;

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
        // usccss.g6.u08.l06.histogram_bin_frequencies
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramNumerics,0,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramNumerics,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramNumerics,2,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramNumerics,3,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramNumerics,4,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramNumerics,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramNumerics,6,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramNumerics,7,"13"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramNumerics,8,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramNumerics,9,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramNumerics,10,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramNumerics,11,"40"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramNumerics,12,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramNumerics,13,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramNumerics,14,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramNumerics,15,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramNumerics,16,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramNumerics,17,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramNumerics,18,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramNumerics,19,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramNumerics,20,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramNumerics,21,"2"};
        // usccss.g6.u08.l06.histogram_vs_dotplot_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramLogic,2,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramLogic,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramLogic,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramLogic,10,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramLogic,13,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramLogic,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramLogic,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramLogic,17,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0806HistogramLogic,18,"1"};
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
            "g6-u08-l06-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
