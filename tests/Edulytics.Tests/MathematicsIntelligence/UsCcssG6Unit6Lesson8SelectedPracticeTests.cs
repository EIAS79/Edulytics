using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit6 Lesson 8 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit6Lesson8SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U06:L08";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0608ExpressionValues;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0608EquivalentLogic;

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
        // usccss.g6.u06.l08.equivalent_expression_evaluation
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608ExpressionValues,0,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608ExpressionValues,1,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608ExpressionValues,2,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608ExpressionValues,3,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608ExpressionValues,4,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608ExpressionValues,5,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608ExpressionValues,6,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608ExpressionValues,7,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608ExpressionValues,8,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608ExpressionValues,9,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608ExpressionValues,10,"24"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608ExpressionValues,11,"21"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608ExpressionValues,12,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608ExpressionValues,13,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608ExpressionValues,14,"125"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608ExpressionValues,15,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608ExpressionValues,16,"13"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608ExpressionValues,17,"322"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608ExpressionValues,18,"4472"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608ExpressionValues,19,"364"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608ExpressionValues,20,"18"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608ExpressionValues,21,"567"};
        // usccss.g6.u06.l08.equivalent_for_all_values_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608EquivalentLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608EquivalentLogic,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608EquivalentLogic,2,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608EquivalentLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608EquivalentLogic,4,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608EquivalentLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608EquivalentLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608EquivalentLogic,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608EquivalentLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608EquivalentLogic,9,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608EquivalentLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608EquivalentLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608EquivalentLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608EquivalentLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608EquivalentLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0608EquivalentLogic,15,"1"};
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
            "g6-u06-l08-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
