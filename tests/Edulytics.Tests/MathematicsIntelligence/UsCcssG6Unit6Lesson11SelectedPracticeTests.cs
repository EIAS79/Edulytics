using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit6 Lesson 11 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit6Lesson11SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U06:L11";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0611AlgebraEvaluation;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0611DistributionLogic;

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
        // usccss.g6.u06.l11.expansion_factor_values
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611AlgebraEvaluation,0,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611AlgebraEvaluation,1,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611AlgebraEvaluation,2,"42"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611AlgebraEvaluation,3,"52"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611AlgebraEvaluation,4,"52"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611AlgebraEvaluation,5,"36"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611AlgebraEvaluation,6,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611AlgebraEvaluation,7,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611AlgebraEvaluation,8,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611AlgebraEvaluation,9,"68"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611AlgebraEvaluation,10,"68"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611AlgebraEvaluation,11,"68"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611AlgebraEvaluation,12,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611AlgebraEvaluation,13,"28"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611AlgebraEvaluation,14,"90"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611AlgebraEvaluation,15,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611AlgebraEvaluation,16,"49"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611AlgebraEvaluation,17,"11"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611AlgebraEvaluation,18,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611AlgebraEvaluation,19,"35"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611AlgebraEvaluation,20,"1036"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611AlgebraEvaluation,21,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611AlgebraEvaluation,22,"150"};
        // usccss.g6.u06.l11.distributive_equivalence_logic
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611DistributionLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611DistributionLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611DistributionLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611DistributionLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611DistributionLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611DistributionLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611DistributionLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611DistributionLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611DistributionLogic,8,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611DistributionLogic,9,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611DistributionLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611DistributionLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611DistributionLogic,12,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611DistributionLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611DistributionLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0611DistributionLogic,15,"1"};
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
            "g6-u06-l11-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
