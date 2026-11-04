using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit5 Lesson 10 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit5Lesson10SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U05:L10";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0510LongDivisionNumbers;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0510LongDivisionLogic;

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
        // usccss.g6.u05.l10.long_division_quotient_results
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0510LongDivisionNumbers,0,"58"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0510LongDivisionNumbers,1,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0510LongDivisionNumbers,2,"131"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0510LongDivisionNumbers,3,"108"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0510LongDivisionNumbers,4,"1620"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0510LongDivisionNumbers,5,"217"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0510LongDivisionNumbers,6,"210"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0510LongDivisionNumbers,7,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0510LongDivisionNumbers,8,"900"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0510LongDivisionNumbers,9,"72"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0510LongDivisionNumbers,10,"72"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0510LongDivisionNumbers,11,"171"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0510LongDivisionNumbers,12,"10502"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0510LongDivisionNumbers,13,"600"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0510LongDivisionNumbers,14,"96"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0510LongDivisionNumbers,15,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0510LongDivisionNumbers,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0510LongDivisionNumbers,17,"696"};
        // usccss.g6.u05.l10.long_division_placevalue_interpretation
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0510LongDivisionLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0510LongDivisionLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0510LongDivisionLogic,2,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0510LongDivisionLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0510LongDivisionLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0510LongDivisionLogic,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0510LongDivisionLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0510LongDivisionLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0510LongDivisionLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0510LongDivisionLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0510LongDivisionLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0510LongDivisionLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0510LongDivisionLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0510LongDivisionLogic,13,"0"};
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
            "g6-u05-l10-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
