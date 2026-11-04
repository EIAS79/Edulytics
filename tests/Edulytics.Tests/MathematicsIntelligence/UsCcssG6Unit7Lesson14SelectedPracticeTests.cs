using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit7 Lesson 14 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit7Lesson14SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U07:L14";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0714Distances;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0714DistanceLogic;

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
        // usccss.g6.u07.l14.horizontal_vertical_coordinate_distances
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714Distances,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714Distances,1,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714Distances,2,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714Distances,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714Distances,4,"-1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714Distances,5,"-5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714Distances,6,"-1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714Distances,7,"-2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714Distances,8,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714Distances,9,"-2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714Distances,10,"-4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714Distances,11,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714Distances,12,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714Distances,13,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714Distances,14,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714Distances,15,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714Distances,16,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714Distances,17,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714Distances,18,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714Distances,19,"18"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714Distances,20,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714Distances,21,"506"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714Distances,22,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714Distances,23,"6"};
        // usccss.g6.u07.l14.distance_same_axis_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714DistanceLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714DistanceLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714DistanceLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714DistanceLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714DistanceLogic,4,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714DistanceLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714DistanceLogic,6,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714DistanceLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714DistanceLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714DistanceLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714DistanceLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714DistanceLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714DistanceLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714DistanceLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714DistanceLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714DistanceLogic,15,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714DistanceLogic,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0714DistanceLogic,17,"1"};
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
            "g6-u07-l14-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
