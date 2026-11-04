using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit8 Lesson 1 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit8Lesson1SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U08:L01";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0801ClassifyData;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0801DataReasoning;

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
        // usccss.g6.u08.l01.numerical_vs_categorical_sources
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801ClassifyData,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801ClassifyData,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801ClassifyData,2,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801ClassifyData,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801ClassifyData,4,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801ClassifyData,5,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801ClassifyData,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801ClassifyData,7,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801ClassifyData,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801ClassifyData,9,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801ClassifyData,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801ClassifyData,11,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801ClassifyData,12,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801ClassifyData,13,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801ClassifyData,14,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801ClassifyData,15,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801ClassifyData,16,"18"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801ClassifyData,17,"13"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801ClassifyData,18,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801ClassifyData,19,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801ClassifyData,20,"3"};
        // usccss.g6.u08.l01.data_type_and_dotplot_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801DataReasoning,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801DataReasoning,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801DataReasoning,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801DataReasoning,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801DataReasoning,4,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801DataReasoning,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801DataReasoning,6,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801DataReasoning,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801DataReasoning,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801DataReasoning,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801DataReasoning,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801DataReasoning,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801DataReasoning,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801DataReasoning,13,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801DataReasoning,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801DataReasoning,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0801DataReasoning,16,"1"};
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
            "g6-u08-l01-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
