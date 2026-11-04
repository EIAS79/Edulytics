using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit2 Lesson 15 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit2Lesson15SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U02:L15";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L15TapeParts;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L15TapeLogic;

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
        // usccss.g6.u02.l15.tape_diagram_part_whole
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L15TapeParts,0,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L15TapeParts,1,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L15TapeParts,2,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L15TapeParts,3,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L15TapeParts,4,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L15TapeParts,5,"24"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L15TapeParts,6,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L15TapeParts,7,"500"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L15TapeParts,8,"725"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L15TapeParts,9,"2175"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L15TapeParts,10,"133"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L15TapeParts,11,"1125"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L15TapeParts,12,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L15TapeParts,13,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L15TapeParts,14,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L15TapeParts,15,"10"};
        // usccss.g6.u02.l15.part_to_whole_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L15TapeLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L15TapeLogic,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L15TapeLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L15TapeLogic,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L15TapeLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L15TapeLogic,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L15TapeLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L15TapeLogic,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L15TapeLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L15TapeLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L15TapeLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L15TapeLogic,11,"0"};
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
            "g6-u02-l15-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
