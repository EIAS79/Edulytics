using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit3 Lesson 7 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit4Lesson7SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U04:L07";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0407FractionGroups;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0407FractionLogic;

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
        // usccss.g6.u04.l07.fraction_of_group_numeric
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0407FractionGroups,0,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0407FractionGroups,1,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0407FractionGroups,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0407FractionGroups,3,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0407FractionGroups,4,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0407FractionGroups,5,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0407FractionGroups,6,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0407FractionGroups,7,"11"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0407FractionGroups,8,"26"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0407FractionGroups,9,"14"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0407FractionGroups,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0407FractionGroups,11,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0407FractionGroups,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0407FractionGroups,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0407FractionGroups,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0407FractionGroups,15,"4"};
        // usccss.g6.u04.l07.fractional_group_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0407FractionLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0407FractionLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0407FractionLogic,2,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0407FractionLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0407FractionLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0407FractionLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0407FractionLogic,6,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0407FractionLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0407FractionLogic,8,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0407FractionLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0407FractionLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0407FractionLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0407FractionLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0407FractionLogic,13,"0"};
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
            "g6-u04-l07-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
