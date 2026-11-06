using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit3 Lesson 13 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit3Lesson13SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U03:L13";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0313BenchmarkNumbers;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0313BenchmarkLogic;

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
        // usccss.g6.u03.l13.benchmark_percent_values
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0313BenchmarkNumbers,0,"800"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0313BenchmarkNumbers,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0313BenchmarkNumbers,2,"440"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0313BenchmarkNumbers,3,"350"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0313BenchmarkNumbers,4,"50"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0313BenchmarkNumbers,5,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0313BenchmarkNumbers,6,"60"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0313BenchmarkNumbers,7,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0313BenchmarkNumbers,8,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0313BenchmarkNumbers,9,"750"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0313BenchmarkNumbers,10,"750"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0313BenchmarkNumbers,11,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0313BenchmarkNumbers,12,"50"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0313BenchmarkNumbers,13,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0313BenchmarkNumbers,14,"75"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0313BenchmarkNumbers,15,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0313BenchmarkNumbers,16,"11"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0313BenchmarkNumbers,17,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0313BenchmarkNumbers,18,"45"};
        // usccss.g6.u03.l13.benchmark_percent_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0313BenchmarkLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0313BenchmarkLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0313BenchmarkLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0313BenchmarkLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0313BenchmarkLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0313BenchmarkLogic,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0313BenchmarkLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0313BenchmarkLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0313BenchmarkLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0313BenchmarkLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0313BenchmarkLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0313BenchmarkLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0313BenchmarkLogic,12,"1"};
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
            "g6-u03-l13-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
