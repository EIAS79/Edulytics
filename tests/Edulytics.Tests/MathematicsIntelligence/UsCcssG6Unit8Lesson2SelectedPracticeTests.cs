using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit8 Lesson 2 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit8Lesson2SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U08:L02";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0802StatisticalClassifications;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0802StatsLogic;

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
        // usccss.g6.u08.l02.statistical_question_classification
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatisticalClassifications,0,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatisticalClassifications,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatisticalClassifications,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatisticalClassifications,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatisticalClassifications,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatisticalClassifications,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatisticalClassifications,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatisticalClassifications,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatisticalClassifications,8,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatisticalClassifications,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatisticalClassifications,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatisticalClassifications,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatisticalClassifications,12,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatisticalClassifications,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatisticalClassifications,14,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatisticalClassifications,15,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatisticalClassifications,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatisticalClassifications,17,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatisticalClassifications,18,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatisticalClassifications,19,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatisticalClassifications,20,"17"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatisticalClassifications,21,"18"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatisticalClassifications,22,"-18"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatisticalClassifications,23,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatisticalClassifications,24,"5"};
        // usccss.g6.u08.l02.expected_data_variation_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatsLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatsLogic,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatsLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatsLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatsLogic,4,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatsLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatsLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatsLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatsLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatsLogic,9,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatsLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatsLogic,11,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatsLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatsLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatsLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatsLogic,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatsLogic,16,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0802StatsLogic,17,"1"};
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
            "g6-u08-l02-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
