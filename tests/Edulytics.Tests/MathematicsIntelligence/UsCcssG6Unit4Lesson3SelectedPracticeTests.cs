using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit3 Lesson 3 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit4Lesson3SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U04:L03";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0403ContextualDivision;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0403DivisionInterpretation;

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
        // usccss.g6.u04.l03.division_contextual_results
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0403ContextualDivision,0,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0403ContextualDivision,1,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0403ContextualDivision,2,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0403ContextualDivision,3,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0403ContextualDivision,4,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0403ContextualDivision,5,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0403ContextualDivision,6,"400"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0403ContextualDivision,7,"1056"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0403ContextualDivision,8,"552"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0403ContextualDivision,9,"144"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0403ContextualDivision,10,"408"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0403ContextualDivision,11,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0403ContextualDivision,12,"240"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0403ContextualDivision,13,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0403ContextualDivision,14,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0403ContextualDivision,15,"3600"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0403ContextualDivision,16,"600"};
        // usccss.g6.u04.l03.division_explanation_checks
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0403DivisionInterpretation,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0403DivisionInterpretation,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0403DivisionInterpretation,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0403DivisionInterpretation,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0403DivisionInterpretation,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0403DivisionInterpretation,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0403DivisionInterpretation,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0403DivisionInterpretation,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0403DivisionInterpretation,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0403DivisionInterpretation,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0403DivisionInterpretation,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0403DivisionInterpretation,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0403DivisionInterpretation,12,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0403DivisionInterpretation,13,"1"};
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
            "g6-u04-l03-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
