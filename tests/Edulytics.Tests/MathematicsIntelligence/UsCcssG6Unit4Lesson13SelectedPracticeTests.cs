using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit3 Lesson 13 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit4Lesson13SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U04:L13";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0413AreaValues;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0413AreaLogic;

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
        // usccss.g6.u04.l13.rectangle_area_fractional_sides
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0413AreaValues,0,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0413AreaValues,1,"28"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0413AreaValues,2,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0413AreaValues,3,"180"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0413AreaValues,4,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0413AreaValues,5,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0413AreaValues,6,"45"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0413AreaValues,7,"28"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0413AreaValues,8,"210"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0413AreaValues,9,"35"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0413AreaValues,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0413AreaValues,11,"11"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0413AreaValues,12,"13"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0413AreaValues,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0413AreaValues,14,"1250"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0413AreaValues,15,"11"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0413AreaValues,16,"462"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0413AreaValues,17,"9"};
        // usccss.g6.u04.l13.rectangle_area_diagram_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0413AreaLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0413AreaLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0413AreaLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0413AreaLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0413AreaLogic,4,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0413AreaLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0413AreaLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0413AreaLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0413AreaLogic,8,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0413AreaLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0413AreaLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0413AreaLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0413AreaLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0413AreaLogic,13,"1"};
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
            "g6-u04-l13-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
