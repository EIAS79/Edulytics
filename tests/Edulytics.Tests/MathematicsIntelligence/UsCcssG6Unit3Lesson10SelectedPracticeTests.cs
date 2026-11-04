using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit3 Lesson 10 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit3Lesson10SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U03:L10";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0310PercentNumbers;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0310PercentLogic;

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
        // usccss.g6.u03.l10.percent_coin_values
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0310PercentNumbers,0,"40"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0310PercentNumbers,1,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0310PercentNumbers,2,"135"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0310PercentNumbers,3,"30"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0310PercentNumbers,4,"30"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0310PercentNumbers,5,"140"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0310PercentNumbers,6,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0310PercentNumbers,7,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0310PercentNumbers,8,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0310PercentNumbers,9,"250"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0310PercentNumbers,10,"500"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0310PercentNumbers,11,"55"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0310PercentNumbers,12,"11"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0310PercentNumbers,13,"1045"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0310PercentNumbers,14,"576"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0310PercentNumbers,15,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0310PercentNumbers,16,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0310PercentNumbers,17,"24"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0310PercentNumbers,18,"30"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0310PercentNumbers,19,"36"};
        // usccss.g6.u03.l10.percent_comparison_logic
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0310PercentLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0310PercentLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0310PercentLogic,2,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0310PercentLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0310PercentLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0310PercentLogic,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0310PercentLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0310PercentLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0310PercentLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0310PercentLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0310PercentLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0310PercentLogic,11,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0310PercentLogic,12,"1"};
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
            "g6-u03-l10-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
