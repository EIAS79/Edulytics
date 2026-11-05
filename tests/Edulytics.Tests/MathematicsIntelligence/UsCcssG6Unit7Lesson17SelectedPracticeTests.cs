using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit7 Lesson 17 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit7Lesson17SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U07:L17";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0717Multiples;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0717LcmLogic;

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
        // usccss.g6.u07.l17.least_common_multiple_source_cycles
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717Multiples,0,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717Multiples,1,"36"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717Multiples,2,"30"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717Multiples,3,"24"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717Multiples,4,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717Multiples,5,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717Multiples,6,"24"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717Multiples,7,"30"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717Multiples,8,"40"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717Multiples,9,"72"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717Multiples,10,"24"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717Multiples,11,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717Multiples,12,"36"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717Multiples,13,"35"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717Multiples,14,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717Multiples,15,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717Multiples,16,"11"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717Multiples,17,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717Multiples,18,"500"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717Multiples,19,"4375"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717Multiples,20,"1875"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717Multiples,21,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717Multiples,22,"4"};
        // usccss.g6.u07.l17.multiple_cycle_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717LcmLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717LcmLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717LcmLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717LcmLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717LcmLogic,4,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717LcmLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717LcmLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717LcmLogic,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717LcmLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717LcmLogic,9,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717LcmLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717LcmLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717LcmLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717LcmLogic,13,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717LcmLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717LcmLogic,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717LcmLogic,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0717LcmLogic,17,"1"};
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
            "g6-u07-l17-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
