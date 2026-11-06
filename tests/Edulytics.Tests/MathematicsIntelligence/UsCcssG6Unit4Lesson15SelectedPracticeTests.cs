using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit3 Lesson 15 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit4Lesson15SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U04:L15";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0415Volumes;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0415VolumeLogic;

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
        // usccss.g6.u04.l15.fractional_prism_volume
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0415Volumes,0,"480"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0415Volumes,1,"384"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0415Volumes,2,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0415Volumes,3,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0415Volumes,4,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0415Volumes,5,"1920"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0415Volumes,6,"240"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0415Volumes,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0415Volumes,8,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0415Volumes,9,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0415Volumes,10,"125"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0415Volumes,11,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0415Volumes,12,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0415Volumes,13,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0415Volumes,14,"24"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0415Volumes,15,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0415Volumes,16,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0415Volumes,17,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0415Volumes,18,"6"};
        // usccss.g6.u04.l15.prism_unit_cube_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0415VolumeLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0415VolumeLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0415VolumeLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0415VolumeLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0415VolumeLogic,4,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0415VolumeLogic,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0415VolumeLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0415VolumeLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0415VolumeLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0415VolumeLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0415VolumeLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0415VolumeLogic,11,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0415VolumeLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0415VolumeLogic,13,"1"};
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
            "g6-u04-l15-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
