using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit8 Lesson 17 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit8Lesson17SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U08:L17";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0817BoxPlotValues;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0817BoxPlotLogic;

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
        // usccss.g6.u08.l17.boxplot_athlete_pineapple_statistics
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotValues,0,"785"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotValues,1,"738"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotValues,2,"731"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotValues,3,"728"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotValues,4,"42"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotValues,5,"50"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotValues,6,"30"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotValues,7,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotValues,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotValues,9,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotValues,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotValues,11,"40"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotValues,12,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotValues,13,"41"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotValues,14,"135"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotValues,15,"100"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotValues,16,"80"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotValues,17,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotValues,18,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotValues,19,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotValues,20,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotValues,21,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotValues,22,"75"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotValues,23,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotValues,24,"26"};
        // usccss.g6.u08.l17.interpret_center_and_spread_boxplots
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotLogic,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotLogic,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotLogic,8,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotLogic,12,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotLogic,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotLogic,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotLogic,17,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0817BoxPlotLogic,18,"1"};
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
            "g6-u08-l17-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
