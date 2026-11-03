using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit2 Lesson 6 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit2Lesson6SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U02:L06";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L06DoubleLineAmounts;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L06DoubleLineLogic;

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
        // usccss.g6.u02.l06.double_number_line_amounts
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L06DoubleLineAmounts,0,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L06DoubleLineAmounts,1,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L06DoubleLineAmounts,2,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L06DoubleLineAmounts,3,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L06DoubleLineAmounts,4,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L06DoubleLineAmounts,5,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L06DoubleLineAmounts,6,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L06DoubleLineAmounts,7,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L06DoubleLineAmounts,8,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L06DoubleLineAmounts,9,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L06DoubleLineAmounts,10,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L06DoubleLineAmounts,11,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L06DoubleLineAmounts,12,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L06DoubleLineAmounts,13,"24"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L06DoubleLineAmounts,14,"24"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L06DoubleLineAmounts,15,"6"};
        // usccss.g6.u02.l06.double_number_line_structure
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L06DoubleLineLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L06DoubleLineLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L06DoubleLineLogic,2,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L06DoubleLineLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L06DoubleLineLogic,4,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L06DoubleLineLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L06DoubleLineLogic,6,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L06DoubleLineLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L06DoubleLineLogic,8,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L06DoubleLineLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L06DoubleLineLogic,10,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L06DoubleLineLogic,11,"1"};
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
            "g6-u02-l06-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
