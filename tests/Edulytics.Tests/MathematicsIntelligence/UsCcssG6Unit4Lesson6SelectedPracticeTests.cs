using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit3 Lesson 6 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit4Lesson6SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U04:L06";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0406TapeValues;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0406TapeLogic;

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
        // usccss.g6.u04.l06.fraction_division_values
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0406TapeValues,0,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0406TapeValues,1,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0406TapeValues,2,"14"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0406TapeValues,3,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0406TapeValues,4,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0406TapeValues,5,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0406TapeValues,6,"11"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0406TapeValues,7,"40"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0406TapeValues,8,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0406TapeValues,9,"45"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0406TapeValues,10,"175"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0406TapeValues,11,"225"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0406TapeValues,12,"36"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0406TapeValues,13,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0406TapeValues,14,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0406TapeValues,15,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0406TapeValues,16,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0406TapeValues,17,"6"};
        // usccss.g6.u04.l06.fraction_tape_interpretation
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0406TapeLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0406TapeLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0406TapeLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0406TapeLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0406TapeLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0406TapeLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0406TapeLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0406TapeLogic,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0406TapeLogic,8,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0406TapeLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0406TapeLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0406TapeLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0406TapeLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0406TapeLogic,13,"1"};
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
            "g6-u04-l06-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
