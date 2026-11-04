using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit3 Lesson 8 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit3Lesson8SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U03:L08";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0308SpeedAmounts;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0308PaceMeaning;

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
        // usccss.g6.u03.l08.speed_distance_time_quantities
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0308SpeedAmounts,0,"450"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0308SpeedAmounts,1,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0308SpeedAmounts,2,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0308SpeedAmounts,3,"1600"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0308SpeedAmounts,4,"64"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0308SpeedAmounts,5,"384"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0308SpeedAmounts,6,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0308SpeedAmounts,7,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0308SpeedAmounts,8,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0308SpeedAmounts,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0308SpeedAmounts,10,"4000"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0308SpeedAmounts,11,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0308SpeedAmounts,12,"1680"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0308SpeedAmounts,13,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0308SpeedAmounts,14,"32"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0308SpeedAmounts,15,"24"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0308SpeedAmounts,16,"96"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0308SpeedAmounts,17,"8"};
        // usccss.g6.u03.l08.speed_pace_comparison
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0308PaceMeaning,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0308PaceMeaning,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0308PaceMeaning,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0308PaceMeaning,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0308PaceMeaning,4,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0308PaceMeaning,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0308PaceMeaning,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0308PaceMeaning,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0308PaceMeaning,8,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0308PaceMeaning,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0308PaceMeaning,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0308PaceMeaning,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0308PaceMeaning,12,"1"};
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
            "g6-u03-l08-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
