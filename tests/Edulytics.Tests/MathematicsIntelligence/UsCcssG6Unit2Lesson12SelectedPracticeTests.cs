using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit2 Lesson 12 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit2Lesson12SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U02:L12";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L12TableCalculations;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L12TableStrategies;

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
        // usccss.g6.u02.l12.table_missing_quantity
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L12TableCalculations,0,"960"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L12TableCalculations,1,"960"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L12TableCalculations,2,"44"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L12TableCalculations,3,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L12TableCalculations,4,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L12TableCalculations,5,"75"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L12TableCalculations,6,"18"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L12TableCalculations,7,"50"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L12TableCalculations,8,"40"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L12TableCalculations,9,"40"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L12TableCalculations,10,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L12TableCalculations,11,"14"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L12TableCalculations,12,"150"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L12TableCalculations,13,"60"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L12TableCalculations,14,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L12TableCalculations,15,"15"};
        // usccss.g6.u02.l12.table_equivalence_methods
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L12TableStrategies,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L12TableStrategies,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L12TableStrategies,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L12TableStrategies,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L12TableStrategies,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L12TableStrategies,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L12TableStrategies,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L12TableStrategies,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L12TableStrategies,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L12TableStrategies,9,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L12TableStrategies,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L12TableStrategies,11,"1"};
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
            "g6-u02-l12-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
