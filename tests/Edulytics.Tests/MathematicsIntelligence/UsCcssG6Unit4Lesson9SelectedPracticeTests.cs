using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit3 Lesson 9 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit4Lesson9SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U04:L09";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0409SourceWordValues;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0409ModelReasoning;

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
        // usccss.g6.u04.l09.contextual_fraction_division
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0409SourceWordValues,0,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0409SourceWordValues,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0409SourceWordValues,2,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0409SourceWordValues,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0409SourceWordValues,4,"45"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0409SourceWordValues,5,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0409SourceWordValues,6,"425"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0409SourceWordValues,7,"2125"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0409SourceWordValues,8,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0409SourceWordValues,9,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0409SourceWordValues,10,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0409SourceWordValues,11,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0409SourceWordValues,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0409SourceWordValues,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0409SourceWordValues,14,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0409SourceWordValues,15,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0409SourceWordValues,16,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0409SourceWordValues,17,"2"};
        // usccss.g6.u04.l09.division_word_model_logic
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0409ModelReasoning,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0409ModelReasoning,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0409ModelReasoning,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0409ModelReasoning,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0409ModelReasoning,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0409ModelReasoning,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0409ModelReasoning,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0409ModelReasoning,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0409ModelReasoning,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0409ModelReasoning,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0409ModelReasoning,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0409ModelReasoning,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0409ModelReasoning,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0409ModelReasoning,13,"1"};
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
            "g6-u04-l09-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
