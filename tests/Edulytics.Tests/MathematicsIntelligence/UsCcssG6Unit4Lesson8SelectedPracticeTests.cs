using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit3 Lesson 8 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit4Lesson8SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U04:L08";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0408GroupSizeNumbers;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0408GroupSizeLogic;

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
        // usccss.g6.u04.l08.fraction_group_size
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0408GroupSizeNumbers,0,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0408GroupSizeNumbers,1,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0408GroupSizeNumbers,2,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0408GroupSizeNumbers,3,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0408GroupSizeNumbers,4,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0408GroupSizeNumbers,5,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0408GroupSizeNumbers,6,"24"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0408GroupSizeNumbers,7,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0408GroupSizeNumbers,8,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0408GroupSizeNumbers,9,"7000000"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0408GroupSizeNumbers,10,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0408GroupSizeNumbers,11,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0408GroupSizeNumbers,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0408GroupSizeNumbers,13,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0408GroupSizeNumbers,14,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0408GroupSizeNumbers,15,"2"};
        // usccss.g6.u04.l08.fraction_group_size_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0408GroupSizeLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0408GroupSizeLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0408GroupSizeLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0408GroupSizeLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0408GroupSizeLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0408GroupSizeLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0408GroupSizeLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0408GroupSizeLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0408GroupSizeLogic,8,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0408GroupSizeLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0408GroupSizeLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0408GroupSizeLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0408GroupSizeLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0408GroupSizeLogic,13,"0"};
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
            "g6-u04-l08-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
