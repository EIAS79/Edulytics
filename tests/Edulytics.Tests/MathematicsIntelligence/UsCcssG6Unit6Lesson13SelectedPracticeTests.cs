using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit6 Lesson 13 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit6Lesson13SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U06:L13";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0613ExponentNumbers;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0613ExponentTruth;

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
        // usccss.g6.u06.l13.exponents_with_rational_bases
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentNumbers,0,"243"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentNumbers,1,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentNumbers,2,"625"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentNumbers,3,"243"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentNumbers,4,"243"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentNumbers,5,"21"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentNumbers,6,"21"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentNumbers,7,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentNumbers,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentNumbers,9,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentNumbers,10,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentNumbers,11,"18"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentNumbers,12,"3516"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentNumbers,13,"89"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentNumbers,14,"89"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentNumbers,15,"45"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentNumbers,16,"343"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentNumbers,17,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentNumbers,18,"216"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentNumbers,19,"243"};
        // usccss.g6.u06.l13.exponent_equation_truth
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentTruth,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentTruth,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentTruth,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentTruth,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentTruth,4,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentTruth,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentTruth,6,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentTruth,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentTruth,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentTruth,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentTruth,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentTruth,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentTruth,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentTruth,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentTruth,14,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentTruth,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0613ExponentTruth,16,"1"};
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
            "g6-u06-l13-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
