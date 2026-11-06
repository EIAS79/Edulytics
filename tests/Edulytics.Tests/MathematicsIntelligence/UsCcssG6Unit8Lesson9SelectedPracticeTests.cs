using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit8 Lesson 9 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit8Lesson9SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U08:L09";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0809Means;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0809MeanLogic;

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
        // usccss.g6.u08.l09.mean_and_balanced_total
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809Means,0,"100"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809Means,1,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809Means,2,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809Means,3,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809Means,4,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809Means,5,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809Means,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809Means,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809Means,8,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809Means,9,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809Means,10,"27"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809Means,11,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809Means,12,"11250"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809Means,13,"3750"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809Means,14,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809Means,15,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809Means,16,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809Means,17,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809Means,18,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809Means,19,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809Means,20,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809Means,21,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809Means,22,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809Means,23,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809Means,24,"5"};
        // usccss.g6.u08.l09.mean_fair_share_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809MeanLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809MeanLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809MeanLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809MeanLogic,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809MeanLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809MeanLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809MeanLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809MeanLogic,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809MeanLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809MeanLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809MeanLogic,10,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809MeanLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809MeanLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809MeanLogic,13,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809MeanLogic,14,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809MeanLogic,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809MeanLogic,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809MeanLogic,17,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0809MeanLogic,18,"1"};
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
            "g6-u08-l09-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
