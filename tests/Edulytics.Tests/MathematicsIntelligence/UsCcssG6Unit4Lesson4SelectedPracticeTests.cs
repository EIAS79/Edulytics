using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit3 Lesson 4 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit4Lesson4SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U04:L04";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0404FractionGroups;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0404FractionModels;

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
        // usccss.g6.u04.l04.fraction_groups_exact
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0404FractionGroups,0,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0404FractionGroups,1,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0404FractionGroups,2,"24"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0404FractionGroups,3,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0404FractionGroups,4,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0404FractionGroups,5,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0404FractionGroups,6,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0404FractionGroups,7,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0404FractionGroups,8,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0404FractionGroups,9,"14"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0404FractionGroups,10,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0404FractionGroups,11,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0404FractionGroups,12,"1250"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0404FractionGroups,13,"50"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0404FractionGroups,14,"8000"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0404FractionGroups,15,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0404FractionGroups,16,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0404FractionGroups,17,"3000"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0404FractionGroups,18,"1440"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0404FractionGroups,19,"25"};
        // usccss.g6.u04.l04.fraction_division_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0404FractionModels,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0404FractionModels,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0404FractionModels,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0404FractionModels,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0404FractionModels,4,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0404FractionModels,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0404FractionModels,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0404FractionModels,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0404FractionModels,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0404FractionModels,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0404FractionModels,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0404FractionModels,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0404FractionModels,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0404FractionModels,13,"1"};
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
            "g6-u04-l04-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
