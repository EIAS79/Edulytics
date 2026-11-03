using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit8 Lesson 5 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit8Lesson5SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U08:L05";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0805DotPlotValues;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0805DotPlotLogic;

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
        // usccss.g6.u08.l05.dotplot_source_comparisons
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotValues,0,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotValues,1,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotValues,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotValues,3,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotValues,4,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotValues,5,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotValues,6,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotValues,7,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotValues,8,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotValues,9,"18"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotValues,10,"29"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotValues,11,"40"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotValues,12,"21"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotValues,13,"55"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotValues,14,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotValues,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotValues,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotValues,17,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotValues,18,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotValues,19,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotValues,20,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotValues,21,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotValues,22,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotValues,23,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotValues,24,"600"};
        // usccss.g6.u08.l05.dotplot_center_spread_interpretation
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotLogic,4,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotLogic,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotLogic,12,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotLogic,14,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotLogic,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotLogic,16,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0805DotPlotLogic,17,"1"};
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
            "g6-u08-l05-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
