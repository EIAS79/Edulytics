using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit7 Lesson 7 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit7Lesson7SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U07:L07";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0707OrderMagnitude;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0707OrderLogic;

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
        // usccss.g6.u07.l07.signed_comparisons_absolute
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderMagnitude,0,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderMagnitude,1,"63"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderMagnitude,2,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderMagnitude,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderMagnitude,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderMagnitude,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderMagnitude,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderMagnitude,7,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderMagnitude,8,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderMagnitude,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderMagnitude,10,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderMagnitude,11,"14"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderMagnitude,12,"-10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderMagnitude,13,"-3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderMagnitude,14,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderMagnitude,15,"-45"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderMagnitude,16,"-37"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderMagnitude,17,"-2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderMagnitude,18,"-36"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderMagnitude,19,"-17"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderMagnitude,20,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderMagnitude,21,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderMagnitude,22,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderMagnitude,23,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderMagnitude,24,"8"};
        // usccss.g6.u07.l07.order_and_absolute_value_inequalities
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderLogic,6,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderLogic,10,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderLogic,14,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderLogic,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderLogic,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderLogic,17,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0707OrderLogic,18,"1"};
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
            "g6-u07-l07-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
