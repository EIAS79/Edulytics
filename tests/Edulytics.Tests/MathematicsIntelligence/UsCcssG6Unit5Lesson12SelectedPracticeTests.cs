using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit5 Lesson 12 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit5Lesson12SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U05:L12";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0512DecimalDivision;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0512DecimalScaleLogic;

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
        // usccss.g6.u05.l12.decimal_divided_by_whole
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0512DecimalDivision,0,"50"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0512DecimalDivision,1,"50"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0512DecimalDivision,2,"50"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0512DecimalDivision,3,"50"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0512DecimalDivision,4,"3945"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0512DecimalDivision,5,"1318"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0512DecimalDivision,6,"352"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0512DecimalDivision,7,"438"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0512DecimalDivision,8,"2794"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0512DecimalDivision,9,"21590"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0512DecimalDivision,10,"935"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0512DecimalDivision,11,"6237"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0512DecimalDivision,12,"6032246"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0512DecimalDivision,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0512DecimalDivision,14,"32"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0512DecimalDivision,15,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0512DecimalDivision,16,"21"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0512DecimalDivision,17,"50"};
        // usccss.g6.u05.l12.division_scale_equivalence
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0512DecimalScaleLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0512DecimalScaleLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0512DecimalScaleLogic,2,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0512DecimalScaleLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0512DecimalScaleLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0512DecimalScaleLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0512DecimalScaleLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0512DecimalScaleLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0512DecimalScaleLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0512DecimalScaleLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0512DecimalScaleLogic,10,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0512DecimalScaleLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0512DecimalScaleLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0512DecimalScaleLogic,13,"1"};
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
            "g6-u05-l12-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
