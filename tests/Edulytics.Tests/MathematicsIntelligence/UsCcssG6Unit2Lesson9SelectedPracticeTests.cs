using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit2 Lesson 9 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit2Lesson9SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U02:L09";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L09SpeedDistances;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L09SpeedComparisons;

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
        // usccss.g6.u02.l09.constant_speed_quantities
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L09SpeedDistances,0,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L09SpeedDistances,1,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L09SpeedDistances,2,"45"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L09SpeedDistances,3,"60"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L09SpeedDistances,4,"75"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L09SpeedDistances,5,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L09SpeedDistances,6,"75"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L09SpeedDistances,7,"135"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L09SpeedDistances,8,"120"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L09SpeedDistances,9,"900"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L09SpeedDistances,10,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L09SpeedDistances,11,"299"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L09SpeedDistances,12,"2691"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L09SpeedDistances,13,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L09SpeedDistances,14,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L09SpeedDistances,15,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L09SpeedDistances,16,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L09SpeedDistances,17,"40"};
        // usccss.g6.u02.l09.constant_speed_equivalence
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L09SpeedComparisons,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L09SpeedComparisons,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L09SpeedComparisons,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L09SpeedComparisons,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L09SpeedComparisons,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L09SpeedComparisons,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L09SpeedComparisons,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L09SpeedComparisons,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L09SpeedComparisons,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L09SpeedComparisons,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L09SpeedComparisons,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L09SpeedComparisons,11,"0"};
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
            "g6-u02-l09-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
