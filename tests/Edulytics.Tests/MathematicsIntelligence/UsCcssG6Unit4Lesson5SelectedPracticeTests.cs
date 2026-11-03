using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit3 Lesson 5 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit4Lesson5SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U04:L05";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0405FractionValues;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0405FractionMeaning;

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
        // usccss.g6.u04.l05.fraction_group_quotients
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0405FractionValues,0,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0405FractionValues,1,"14"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0405FractionValues,2,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0405FractionValues,3,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0405FractionValues,4,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0405FractionValues,5,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0405FractionValues,6,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0405FractionValues,7,"190"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0405FractionValues,8,"180"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0405FractionValues,9,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0405FractionValues,10,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0405FractionValues,11,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0405FractionValues,12,"35"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0405FractionValues,13,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0405FractionValues,14,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0405FractionValues,15,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0405FractionValues,16,"760"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0405FractionValues,17,"95"};
        // usccss.g6.u04.l05.fractional_group_interpretation
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0405FractionMeaning,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0405FractionMeaning,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0405FractionMeaning,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0405FractionMeaning,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0405FractionMeaning,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0405FractionMeaning,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0405FractionMeaning,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0405FractionMeaning,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0405FractionMeaning,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0405FractionMeaning,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0405FractionMeaning,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0405FractionMeaning,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0405FractionMeaning,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0405FractionMeaning,13,"1"};
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
            "g6-u04-l05-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
