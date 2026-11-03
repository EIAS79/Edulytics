using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit5 Lesson 8 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit5Lesson8SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U05:L08";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0508ProductArithmetic;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0508ProductLogic;

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
        // usccss.g6.u05.l08.multiplication_calculation_values
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0508ProductArithmetic,0,"2052"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0508ProductArithmetic,1,"432"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0508ProductArithmetic,2,"162"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0508ProductArithmetic,3,"195"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0508ProductArithmetic,4,"1296"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0508ProductArithmetic,5,"5845"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0508ProductArithmetic,6,"2388"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0508ProductArithmetic,7,"4482"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0508ProductArithmetic,8,"687"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0508ProductArithmetic,9,"73978"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0508ProductArithmetic,10,"73888"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0508ProductArithmetic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0508ProductArithmetic,12,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0508ProductArithmetic,13,"140"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0508ProductArithmetic,14,"20520"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0508ProductArithmetic,15,"195"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0508ProductArithmetic,16,"1296"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0508ProductArithmetic,17,"58450"};
        // usccss.g6.u05.l08.decimal_product_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0508ProductLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0508ProductLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0508ProductLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0508ProductLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0508ProductLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0508ProductLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0508ProductLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0508ProductLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0508ProductLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0508ProductLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0508ProductLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0508ProductLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0508ProductLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0508ProductLogic,13,"0"};
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
            "g6-u05-l08-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
