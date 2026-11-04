using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit3 Lesson 2 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit4Lesson2SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U04:L02";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0402DivisionNumbers;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0402DivisionMeaningLogic;

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
        // usccss.g6.u04.l02.division_groups_and_size
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0402DivisionNumbers,0,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0402DivisionNumbers,1,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0402DivisionNumbers,2,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0402DivisionNumbers,3,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0402DivisionNumbers,4,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0402DivisionNumbers,5,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0402DivisionNumbers,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0402DivisionNumbers,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0402DivisionNumbers,8,"50"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0402DivisionNumbers,9,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0402DivisionNumbers,10,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0402DivisionNumbers,11,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0402DivisionNumbers,12,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0402DivisionNumbers,13,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0402DivisionNumbers,14,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0402DivisionNumbers,15,"75"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0402DivisionNumbers,16,"150"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0402DivisionNumbers,17,"140"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0402DivisionNumbers,18,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0402DivisionNumbers,19,"2"};
        // usccss.g6.u04.l02.division_interpretations
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0402DivisionMeaningLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0402DivisionMeaningLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0402DivisionMeaningLogic,2,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0402DivisionMeaningLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0402DivisionMeaningLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0402DivisionMeaningLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0402DivisionMeaningLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0402DivisionMeaningLogic,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0402DivisionMeaningLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0402DivisionMeaningLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0402DivisionMeaningLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0402DivisionMeaningLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0402DivisionMeaningLogic,12,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0402DivisionMeaningLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0402DivisionMeaningLogic,14,"1"};
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
            "g6-u04-l02-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
