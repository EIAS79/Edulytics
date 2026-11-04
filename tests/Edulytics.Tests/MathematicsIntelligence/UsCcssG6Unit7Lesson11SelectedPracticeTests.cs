using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit7 Lesson 11 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit7Lesson11SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U07:L11";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0711Coordinates;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0711CoordinateLogic;

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
        // usccss.g6.u07.l11.coordinates_and_axis_locations
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711Coordinates,0,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711Coordinates,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711Coordinates,2,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711Coordinates,3,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711Coordinates,4,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711Coordinates,5,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711Coordinates,6,"24"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711Coordinates,7,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711Coordinates,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711Coordinates,9,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711Coordinates,10,"-3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711Coordinates,11,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711Coordinates,12,"-3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711Coordinates,13,"-5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711Coordinates,14,"-1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711Coordinates,15,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711Coordinates,16,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711Coordinates,17,"29"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711Coordinates,18,"174"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711Coordinates,19,"3000"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711Coordinates,20,"210"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711Coordinates,21,"8"};
        // usccss.g6.u07.l11.coordinate_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711CoordinateLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711CoordinateLogic,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711CoordinateLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711CoordinateLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711CoordinateLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711CoordinateLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711CoordinateLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711CoordinateLogic,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711CoordinateLogic,8,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711CoordinateLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711CoordinateLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711CoordinateLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711CoordinateLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711CoordinateLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711CoordinateLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711CoordinateLogic,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0711CoordinateLogic,16,"0"};
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
            "g6-u07-l11-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
