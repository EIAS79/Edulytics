using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit3 Lesson 10 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit4Lesson10SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U04:L10";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0410AlgorithmValues;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0410AlgorithmLogic;

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
        // usccss.g6.u04.l10.fraction_division_steps
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0410AlgorithmValues,0,"48"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0410AlgorithmValues,1,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0410AlgorithmValues,2,"40"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0410AlgorithmValues,3,"180"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0410AlgorithmValues,4,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0410AlgorithmValues,5,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0410AlgorithmValues,6,"50"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0410AlgorithmValues,7,"50"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0410AlgorithmValues,8,"50"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0410AlgorithmValues,9,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0410AlgorithmValues,10,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0410AlgorithmValues,11,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0410AlgorithmValues,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0410AlgorithmValues,13,"100"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0410AlgorithmValues,14,"45"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0410AlgorithmValues,15,"150"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0410AlgorithmValues,16,"800"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0410AlgorithmValues,17,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0410AlgorithmValues,18,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0410AlgorithmValues,19,"5"};
        // usccss.g6.u04.l10.division_reciprocal_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0410AlgorithmLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0410AlgorithmLogic,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0410AlgorithmLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0410AlgorithmLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0410AlgorithmLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0410AlgorithmLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0410AlgorithmLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0410AlgorithmLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0410AlgorithmLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0410AlgorithmLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0410AlgorithmLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0410AlgorithmLogic,11,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0410AlgorithmLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0410AlgorithmLogic,13,"1"};
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
            "g6-u04-l10-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
