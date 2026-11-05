using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit7 Lesson 6 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit7Lesson6SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U07:L06";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0706AbsoluteValues;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0706AbsoluteLogic;

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
        // usccss.g6.u07.l06.absolute_distance_values
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteValues,0,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteValues,1,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteValues,2,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteValues,3,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteValues,4,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteValues,5,"27"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteValues,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteValues,7,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteValues,8,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteValues,9,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteValues,10,"234"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteValues,11,"116"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteValues,12,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteValues,13,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteValues,14,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteValues,15,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteValues,16,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteValues,17,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteValues,18,"49"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteValues,19,"289"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteValues,20,"15625"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteValues,21,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteValues,22,"100"};
        // usccss.g6.u07.l06.absolute_value_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteLogic,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteLogic,8,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteLogic,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteLogic,16,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0706AbsoluteLogic,17,"1"};
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
            "g6-u07-l06-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
