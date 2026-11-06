using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit3 Lesson 16 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit3Lesson16SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U03:L16";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0316FindPercent;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0316FindPercentLogic;

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
        // usccss.g6.u03.l16.find_percent_from_two_amounts
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0316FindPercent,0,"85"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0316FindPercent,1,"135"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0316FindPercent,2,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0316FindPercent,3,"24"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0316FindPercent,4,"30"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0316FindPercent,5,"2550"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0316FindPercent,6,"1530"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0316FindPercent,7,"2108"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0316FindPercent,8,"1680"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0316FindPercent,9,"1872"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0316FindPercent,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0316FindPercent,11,"14224"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0316FindPercent,12,"14224"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0316FindPercent,13,"68"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0316FindPercent,14,"68"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0316FindPercent,15,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0316FindPercent,16,"14"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0316FindPercent,17,"56"};
        // usccss.g6.u03.l16.percent_division_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0316FindPercentLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0316FindPercentLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0316FindPercentLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0316FindPercentLogic,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0316FindPercentLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0316FindPercentLogic,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0316FindPercentLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0316FindPercentLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0316FindPercentLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0316FindPercentLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0316FindPercentLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0316FindPercentLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0316FindPercentLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0316FindPercentLogic,13,"0"};
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
            "g6-u03-l16-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
