using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit3 Lesson 11 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit3Lesson11SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U03:L11";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0311PercentValues;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0311PercentReasoning;

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
        // usccss.g6.u03.l11.percent_scaled_amounts
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0311PercentValues,0,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0311PercentValues,1,"18"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0311PercentValues,2,"80"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0311PercentValues,3,"250"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0311PercentValues,4,"30"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0311PercentValues,5,"60"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0311PercentValues,6,"25908"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0311PercentValues,7,"27432"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0311PercentValues,8,"4800"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0311PercentValues,9,"3000"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0311PercentValues,10,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0311PercentValues,11,"45"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0311PercentValues,12,"100"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0311PercentValues,13,"480"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0311PercentValues,14,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0311PercentValues,15,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0311PercentValues,16,"40"};
        // usccss.g6.u03.l11.percent_of_quantity_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0311PercentReasoning,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0311PercentReasoning,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0311PercentReasoning,2,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0311PercentReasoning,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0311PercentReasoning,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0311PercentReasoning,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0311PercentReasoning,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0311PercentReasoning,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0311PercentReasoning,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0311PercentReasoning,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0311PercentReasoning,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0311PercentReasoning,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0311PercentReasoning,12,"1"};
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
            "g6-u03-l11-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
