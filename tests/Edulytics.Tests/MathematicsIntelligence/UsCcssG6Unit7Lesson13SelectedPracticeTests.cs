using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit7 Lesson 13 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit7Lesson13SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U07:L13";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0713CoordinatesData;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0713ContextLogic;

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
        // usccss.g6.u07.l13.coordinate_context_numeric
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713CoordinatesData,0,"-567"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713CoordinatesData,1,"-892"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713CoordinatesData,2,"-1606"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713CoordinatesData,3,"-1289"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713CoordinatesData,4,"-990"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713CoordinatesData,5,"-702"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713CoordinatesData,6,"-365"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713CoordinatesData,7,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713CoordinatesData,8,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713CoordinatesData,9,"-325"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713CoordinatesData,10,"317"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713CoordinatesData,11,"337"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713CoordinatesData,12,"-3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713CoordinatesData,13,"-1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713CoordinatesData,14,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713CoordinatesData,15,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713CoordinatesData,16,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713CoordinatesData,17,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713CoordinatesData,18,"56"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713CoordinatesData,19,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713CoordinatesData,20,"21"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713CoordinatesData,21,"32"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713CoordinatesData,22,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713CoordinatesData,23,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713CoordinatesData,24,"0"};
        // usccss.g6.u07.l13.interpret_axes_and_extrema
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713ContextLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713ContextLogic,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713ContextLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713ContextLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713ContextLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713ContextLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713ContextLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713ContextLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713ContextLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713ContextLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713ContextLogic,10,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713ContextLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713ContextLogic,12,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713ContextLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713ContextLogic,14,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713ContextLogic,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713ContextLogic,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713ContextLogic,17,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713ContextLogic,18,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0713ContextLogic,19,"1"};
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
            "g6-u07-l13-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
