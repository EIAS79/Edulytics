using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit8 Lesson 3 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit8Lesson3SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U08:L03";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0803Frequency;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0803PlotLogic;

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
        // usccss.g6.u08.l03.dotplot_frequency_counts
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803Frequency,0,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803Frequency,1,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803Frequency,2,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803Frequency,3,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803Frequency,4,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803Frequency,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803Frequency,6,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803Frequency,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803Frequency,8,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803Frequency,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803Frequency,10,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803Frequency,11,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803Frequency,12,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803Frequency,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803Frequency,14,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803Frequency,15,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803Frequency,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803Frequency,17,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803Frequency,18,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803Frequency,19,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803Frequency,20,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803Frequency,21,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803Frequency,22,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803Frequency,23,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803Frequency,24,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803Frequency,25,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803Frequency,26,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803Frequency,27,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803Frequency,28,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803Frequency,29,"8"};
        // usccss.g6.u08.l03.frequency_plot_interpretation
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803PlotLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803PlotLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803PlotLogic,2,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803PlotLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803PlotLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803PlotLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803PlotLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803PlotLogic,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803PlotLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803PlotLogic,9,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803PlotLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803PlotLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803PlotLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803PlotLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803PlotLogic,14,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803PlotLogic,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803PlotLogic,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0803PlotLogic,17,"1"};
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
            "g6-u08-l03-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
