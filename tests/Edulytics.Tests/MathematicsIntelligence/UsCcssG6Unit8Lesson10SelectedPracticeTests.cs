using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit8 Lesson 10 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit8Lesson10SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U08:L10";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0810BalanceNumbers;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0810BalanceLogic;

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
        // usccss.g6.u08.l10.mean_balance_source_data
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceNumbers,0,"70"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceNumbers,1,"14"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceNumbers,2,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceNumbers,3,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceNumbers,4,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceNumbers,5,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceNumbers,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceNumbers,7,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceNumbers,8,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceNumbers,9,"120"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceNumbers,10,"70"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceNumbers,11,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceNumbers,12,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceNumbers,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceNumbers,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceNumbers,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceNumbers,16,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceNumbers,17,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceNumbers,18,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceNumbers,19,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceNumbers,20,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceNumbers,21,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceNumbers,22,"0"};
        // usccss.g6.u08.l10.balance_point_interpretation
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceLogic,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceLogic,6,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceLogic,8,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceLogic,12,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceLogic,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceLogic,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0810BalanceLogic,17,"1"};
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
            "g6-u08-l10-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
