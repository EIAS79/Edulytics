using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit6 Lesson 3 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit6Lesson3SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U06:L03";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0603HangerValues;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0603HangerLogic;

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
        // usccss.g6.u06.l03.hanger_equation_unknowns
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0603HangerValues,0,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0603HangerValues,1,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0603HangerValues,2,"14"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0603HangerValues,3,"17"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0603HangerValues,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0603HangerValues,5,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0603HangerValues,6,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0603HangerValues,7,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0603HangerValues,8,"48"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0603HangerValues,9,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0603HangerValues,10,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0603HangerValues,11,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0603HangerValues,12,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0603HangerValues,13,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0603HangerValues,14,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0603HangerValues,15,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0603HangerValues,16,"170"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0603HangerValues,17,"2"};
        // usccss.g6.u06.l03.balance_equation_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0603HangerLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0603HangerLogic,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0603HangerLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0603HangerLogic,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0603HangerLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0603HangerLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0603HangerLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0603HangerLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0603HangerLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0603HangerLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0603HangerLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0603HangerLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0603HangerLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0603HangerLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0603HangerLogic,14,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0603HangerLogic,15,"1"};
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
            "g6-u06-l03-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
