using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit5 Lesson 2 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit5Lesson2SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U05:L02";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0502PlaceValueArithmetic;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0502PlaceValueLogic;

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
        // usccss.g6.u05.l02.base_ten_decimal_arithmetic
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0502PlaceValueArithmetic,0,"421"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0502PlaceValueArithmetic,1,"425"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0502PlaceValueArithmetic,2,"165"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0502PlaceValueArithmetic,3,"123"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0502PlaceValueArithmetic,4,"1232"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0502PlaceValueArithmetic,5,"216"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0502PlaceValueArithmetic,6,"304"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0502PlaceValueArithmetic,7,"137"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0502PlaceValueArithmetic,8,"284"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0502PlaceValueArithmetic,9,"42"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0502PlaceValueArithmetic,10,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0502PlaceValueArithmetic,11,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0502PlaceValueArithmetic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0502PlaceValueArithmetic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0502PlaceValueArithmetic,14,"4210"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0502PlaceValueArithmetic,15,"1650"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0502PlaceValueArithmetic,16,"65"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0502PlaceValueArithmetic,17,"1"};
        // usccss.g6.u05.l02.align_decimal_place_values
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0502PlaceValueLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0502PlaceValueLogic,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0502PlaceValueLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0502PlaceValueLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0502PlaceValueLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0502PlaceValueLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0502PlaceValueLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0502PlaceValueLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0502PlaceValueLogic,8,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0502PlaceValueLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0502PlaceValueLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0502PlaceValueLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0502PlaceValueLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0502PlaceValueLogic,13,"1"};
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
            "g6-u05-l02-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
