using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit2 Lesson 11 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit2Lesson11SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U02:L11";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L11TableAmounts;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L11TableLogic;

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
        // usccss.g6.u02.l11.table_equivalent_ratio_values
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L11TableAmounts,0,"980"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L11TableAmounts,1,"210"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L11TableAmounts,2,"450"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L11TableAmounts,3,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L11TableAmounts,4,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L11TableAmounts,5,"96"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L11TableAmounts,6,"48"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L11TableAmounts,7,"72"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L11TableAmounts,8,"150"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L11TableAmounts,9,"30"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L11TableAmounts,10,"24"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L11TableAmounts,11,"62"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L11TableAmounts,12,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L11TableAmounts,13,"18"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L11TableAmounts,14,"100"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L11TableAmounts,15,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L11TableAmounts,16,"78"};
        // usccss.g6.u02.l11.ratio_table_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L11TableLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L11TableLogic,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L11TableLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L11TableLogic,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L11TableLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L11TableLogic,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L11TableLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L11TableLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L11TableLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L11TableLogic,9,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L11TableLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L11TableLogic,11,"0"};
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
            "g6-u02-l11-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
