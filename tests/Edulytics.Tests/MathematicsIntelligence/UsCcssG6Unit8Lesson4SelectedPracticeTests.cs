using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit8 Lesson 4 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit8Lesson4SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U08:L04";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0804PlotValues;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0804InterpretPlot;

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
        // usccss.g6.u08.l04.source_dotplot_comparisons
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804PlotValues,0,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804PlotValues,1,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804PlotValues,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804PlotValues,3,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804PlotValues,4,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804PlotValues,5,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804PlotValues,6,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804PlotValues,7,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804PlotValues,8,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804PlotValues,9,"18"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804PlotValues,10,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804PlotValues,11,"19"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804PlotValues,12,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804PlotValues,13,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804PlotValues,14,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804PlotValues,15,"41"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804PlotValues,16,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804PlotValues,17,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804PlotValues,18,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804PlotValues,19,"5111"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804PlotValues,20,"2343"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804PlotValues,21,"24048"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804PlotValues,22,"104375"};
        // usccss.g6.u08.l04.dotplot_center_spread_logic
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804InterpretPlot,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804InterpretPlot,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804InterpretPlot,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804InterpretPlot,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804InterpretPlot,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804InterpretPlot,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804InterpretPlot,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804InterpretPlot,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804InterpretPlot,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804InterpretPlot,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804InterpretPlot,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804InterpretPlot,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804InterpretPlot,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804InterpretPlot,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804InterpretPlot,14,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804InterpretPlot,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804InterpretPlot,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804InterpretPlot,17,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804InterpretPlot,18,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0804InterpretPlot,19,"1"};
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
            "g6-u08-l04-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
