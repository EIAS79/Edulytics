using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit6 Lesson 15 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit6Lesson15SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U06:L15";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0615ExponentVariables;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0615ExponentVariablesLogic;

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
        // usccss.g6.u06.l15.exponential_variable_values
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariables,0,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariables,1,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariables,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariables,3,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariables,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariables,5,"29"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariables,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariables,7,"78"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariables,8,"76"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariables,9,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariables,10,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariables,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariables,12,"31"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariables,13,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariables,14,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariables,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariables,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariables,17,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariables,18,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariables,19,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariables,20,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariables,21,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariables,22,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariables,23,"24"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariables,24,"15"};
        // usccss.g6.u06.l15.exponential_expression_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariablesLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariablesLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariablesLogic,2,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariablesLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariablesLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariablesLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariablesLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariablesLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariablesLogic,8,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariablesLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariablesLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariablesLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariablesLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariablesLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariablesLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariablesLogic,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariablesLogic,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariablesLogic,17,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0615ExponentVariablesLogic,18,"1"};
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
            "g6-u06-l15-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
