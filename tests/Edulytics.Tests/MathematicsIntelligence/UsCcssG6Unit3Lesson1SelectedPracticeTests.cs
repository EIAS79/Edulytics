using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit3 Lesson 1 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit3Lesson1SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U03:L01";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0301PerOne;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0301RateMeaning;

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
        // usccss.g6.u03.l01.per_one_source_rate
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0301PerOne,0,"31"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0301PerOne,1,"372"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0301PerOne,2,"155"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0301PerOne,3,"825"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0301PerOne,4,"5775"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0301PerOne,5,"66"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0301PerOne,6,"87"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0301PerOne,7,"957"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0301PerOne,8,"65"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0301PerOne,9,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0301PerOne,10,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0301PerOne,11,"13"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0301PerOne,12,"52"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0301PerOne,13,"39"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0301PerOne,14,"138"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0301PerOne,15,"276"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0301PerOne,16,"690"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0301PerOne,17,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0301PerOne,18,"292176"};
        // usccss.g6.u03.l01.source_rate_meaning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0301RateMeaning,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0301RateMeaning,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0301RateMeaning,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0301RateMeaning,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0301RateMeaning,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0301RateMeaning,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0301RateMeaning,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0301RateMeaning,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0301RateMeaning,8,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0301RateMeaning,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0301RateMeaning,10,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0301RateMeaning,11,"1"};
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
            "g6-u03-l01-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
