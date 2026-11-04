using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit7 Lesson 12 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit7Lesson12SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U07:L12";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0712PlotAndScale;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0712ScaleReasoning;

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
        // usccss.g6.u07.l12.four_quadrant_plot_and_axis_scale
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712PlotAndScale,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712PlotAndScale,1,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712PlotAndScale,2,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712PlotAndScale,3,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712PlotAndScale,4,"-6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712PlotAndScale,5,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712PlotAndScale,6,"-3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712PlotAndScale,7,"-3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712PlotAndScale,8,"-250"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712PlotAndScale,9,"350"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712PlotAndScale,10,"-100"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712PlotAndScale,11,"100"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712PlotAndScale,12,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712PlotAndScale,13,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712PlotAndScale,14,"36"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712PlotAndScale,15,"350"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712PlotAndScale,16,"875"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712PlotAndScale,17,"1750"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712PlotAndScale,18,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712PlotAndScale,19,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712PlotAndScale,20,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712PlotAndScale,21,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712PlotAndScale,22,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712PlotAndScale,23,"-4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712PlotAndScale,24,"-10"};
        // usccss.g6.u07.l12.axis_precision_and_plot_logic
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712ScaleReasoning,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712ScaleReasoning,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712ScaleReasoning,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712ScaleReasoning,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712ScaleReasoning,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712ScaleReasoning,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712ScaleReasoning,6,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712ScaleReasoning,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712ScaleReasoning,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712ScaleReasoning,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712ScaleReasoning,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712ScaleReasoning,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712ScaleReasoning,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712ScaleReasoning,13,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712ScaleReasoning,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0712ScaleReasoning,15,"1"};
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
            "g6-u07-l12-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
