using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit8 Lesson 16 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit8Lesson16SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U08:L16";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0816BoxPlotValues;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0816BoxPlotLogic;

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
        // usccss.g6.u08.l16.box_plot_source_quartiles
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotValues,0,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotValues,1,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotValues,2,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotValues,3,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotValues,4,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotValues,5,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotValues,6,"13"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotValues,7,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotValues,8,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotValues,9,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotValues,10,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotValues,11,"11"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotValues,12,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotValues,13,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotValues,14,"50"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotValues,15,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotValues,16,"78"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotValues,17,"78"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotValues,18,"52"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotValues,19,"45"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotValues,20,"50"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotValues,21,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotValues,22,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotValues,23,"50"};
        // usccss.g6.u08.l16.five_number_box_plot_interpretation
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotLogic,4,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotLogic,10,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotLogic,12,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotLogic,14,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotLogic,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotLogic,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0816BoxPlotLogic,17,"1"};
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
            "g6-u08-l16-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
