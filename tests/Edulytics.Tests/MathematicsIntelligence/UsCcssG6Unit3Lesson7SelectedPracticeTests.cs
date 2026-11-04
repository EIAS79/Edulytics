using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit3 Lesson 7 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit3Lesson7SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U03:L07";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0307UnitRateValues;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0307RateInvariance;

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
        // usccss.g6.u03.l07.unit_rate_table_quantities
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0307UnitRateValues,0,"110"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0307UnitRateValues,1,"55"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0307UnitRateValues,2,"165"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0307UnitRateValues,3,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0307UnitRateValues,4,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0307UnitRateValues,5,"120"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0307UnitRateValues,6,"125"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0307UnitRateValues,7,"72"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0307UnitRateValues,8,"75"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0307UnitRateValues,9,"200"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0307UnitRateValues,10,"300"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0307UnitRateValues,11,"350"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0307UnitRateValues,12,"500"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0307UnitRateValues,13,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0307UnitRateValues,14,"33"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0307UnitRateValues,15,"560"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0307UnitRateValues,16,"555"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0307UnitRateValues,17,"525"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0307UnitRateValues,18,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0307UnitRateValues,19,"132"};
        // usccss.g6.u03.l07.unit_rate_equivalence
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0307RateInvariance,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0307RateInvariance,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0307RateInvariance,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0307RateInvariance,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0307RateInvariance,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0307RateInvariance,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0307RateInvariance,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0307RateInvariance,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0307RateInvariance,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0307RateInvariance,9,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0307RateInvariance,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0307RateInvariance,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0307RateInvariance,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0307RateInvariance,13,"0"};
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
            "g6-u03-l07-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
