using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit7 Lesson 1 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit7Lesson1SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U07:L01";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0701SignedValues;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0701SignLogic;

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
        // usccss.g6.u07.l01.signed_temperatures_elevation
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignedValues,0,"-2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignedValues,1,"-10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignedValues,2,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignedValues,3,"-300"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignedValues,4,"-450"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignedValues,5,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignedValues,6,"-10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignedValues,7,"-10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignedValues,8,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignedValues,9,"-3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignedValues,10,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignedValues,11,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignedValues,12,"-11"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignedValues,13,"14"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignedValues,14,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignedValues,15,"13"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignedValues,16,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignedValues,17,"96"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignedValues,18,"450"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignedValues,19,"15"};
        // usccss.g6.u07.l01.signed_number_interpretation
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignLogic,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignLogic,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignLogic,9,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignLogic,11,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0701SignLogic,15,"1"};
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
            "g6-u07-l01-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
