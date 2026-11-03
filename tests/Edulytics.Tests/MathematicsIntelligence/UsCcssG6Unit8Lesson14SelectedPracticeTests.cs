using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit8 Lesson 14 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit8Lesson14SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U08:L14";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0814CenterValues;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0814CenterLogic;

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
        // usccss.g6.u08.l14.mean_median_outlier_changes
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterValues,0,"21"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterValues,1,"21"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterValues,2,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterValues,3,"31"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterValues,4,"31"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterValues,5,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterValues,6,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterValues,7,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterValues,8,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterValues,9,"785"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterValues,10,"785"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterValues,11,"100"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterValues,12,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterValues,13,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterValues,14,"100"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterValues,15,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterValues,16,"120"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterValues,17,"24"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterValues,18,"19"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterValues,19,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterValues,20,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterValues,21,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterValues,22,"1"};
        // usccss.g6.u08.l14.mean_median_choice_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterLogic,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterLogic,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterLogic,10,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterLogic,12,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterLogic,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterLogic,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0814CenterLogic,17,"1"};
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
            "g6-u08-l14-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
