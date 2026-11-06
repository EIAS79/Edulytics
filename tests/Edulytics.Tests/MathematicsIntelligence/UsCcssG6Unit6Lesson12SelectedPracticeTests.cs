using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit6 Lesson 12 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit6Lesson12SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U06:L12";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0612ExponentValues;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0612ExponentReasoning;

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
        // usccss.g6.u06.l12.exponents_numeric_values
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentValues,0,"64"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentValues,1,"64"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentValues,2,"64"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentValues,3,"256"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentValues,4,"81"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentValues,5,"81"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentValues,6,"1024"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentValues,7,"4096"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentValues,8,"256"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentValues,9,"1024"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentValues,10,"216"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentValues,11,"216"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentValues,12,"216"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentValues,13,"751"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentValues,14,"349"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentValues,15,"1371"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentValues,16,"1371"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentValues,17,"40"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentValues,18,"40"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentValues,19,"324"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentValues,20,"125"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentValues,21,"42"};
        // usccss.g6.u06.l12.exponent_and_distributive_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentReasoning,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentReasoning,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentReasoning,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentReasoning,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentReasoning,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentReasoning,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentReasoning,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentReasoning,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentReasoning,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentReasoning,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentReasoning,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentReasoning,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentReasoning,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentReasoning,13,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentReasoning,14,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentReasoning,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentReasoning,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0612ExponentReasoning,17,"1"};
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
            "g6-u06-l12-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
