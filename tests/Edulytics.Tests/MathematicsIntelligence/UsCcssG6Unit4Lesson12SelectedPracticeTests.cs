using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit3 Lesson 12 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit4Lesson12SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U04:L12";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0412LengthValues;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0412LengthReasoning;

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
        // usccss.g6.u04.l12.fractional_length_comparisons
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0412LengthValues,0,"153"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0412LengthValues,1,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0412LengthValues,2,"200"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0412LengthValues,3,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0412LengthValues,4,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0412LengthValues,5,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0412LengthValues,6,"13"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0412LengthValues,7,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0412LengthValues,8,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0412LengthValues,9,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0412LengthValues,10,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0412LengthValues,11,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0412LengthValues,12,"75"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0412LengthValues,13,"765"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0412LengthValues,14,"21"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0412LengthValues,15,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0412LengthValues,16,"4"};
        // usccss.g6.u04.l12.fractional_comparison_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0412LengthReasoning,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0412LengthReasoning,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0412LengthReasoning,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0412LengthReasoning,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0412LengthReasoning,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0412LengthReasoning,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0412LengthReasoning,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0412LengthReasoning,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0412LengthReasoning,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0412LengthReasoning,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0412LengthReasoning,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0412LengthReasoning,11,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0412LengthReasoning,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0412LengthReasoning,13,"0"};
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
            "g6-u04-l12-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
