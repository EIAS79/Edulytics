using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit3 Lesson 11 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit4Lesson11SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U04:L11";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0411DivisionFluency;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0411DivisionLogic;

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
        // usccss.g6.u04.l11.divide_fractions_exact
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0411DivisionFluency,0,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0411DivisionFluency,1,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0411DivisionFluency,2,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0411DivisionFluency,3,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0411DivisionFluency,4,"36"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0411DivisionFluency,5,"18"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0411DivisionFluency,6,"27"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0411DivisionFluency,7,"50"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0411DivisionFluency,8,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0411DivisionFluency,9,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0411DivisionFluency,10,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0411DivisionFluency,11,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0411DivisionFluency,12,"150"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0411DivisionFluency,13,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0411DivisionFluency,14,"11"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0411DivisionFluency,15,"33"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0411DivisionFluency,16,"6"};
        // usccss.g6.u04.l11.fraction_algorithm_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0411DivisionLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0411DivisionLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0411DivisionLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0411DivisionLogic,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0411DivisionLogic,4,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0411DivisionLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0411DivisionLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0411DivisionLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0411DivisionLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0411DivisionLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0411DivisionLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0411DivisionLogic,11,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0411DivisionLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0411DivisionLogic,13,"1"};
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
            "g6-u04-l11-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
