using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit7 Lesson 18 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit7Lesson18SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U07:L18";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0718FactorMultipleChoices;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0718DecisionLogic;

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
        // usccss.g6.u07.l18.gcf_lcm_context_problems
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718FactorMultipleChoices,0,"24"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718FactorMultipleChoices,1,"30"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718FactorMultipleChoices,2,"40"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718FactorMultipleChoices,3,"120"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718FactorMultipleChoices,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718FactorMultipleChoices,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718FactorMultipleChoices,6,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718FactorMultipleChoices,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718FactorMultipleChoices,8,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718FactorMultipleChoices,9,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718FactorMultipleChoices,10,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718FactorMultipleChoices,11,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718FactorMultipleChoices,12,"24"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718FactorMultipleChoices,13,"32"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718FactorMultipleChoices,14,"50"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718FactorMultipleChoices,15,"96"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718FactorMultipleChoices,16,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718FactorMultipleChoices,17,"24"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718FactorMultipleChoices,18,"96"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718FactorMultipleChoices,19,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718FactorMultipleChoices,20,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718FactorMultipleChoices,21,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718FactorMultipleChoices,22,"350"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718FactorMultipleChoices,23,"4995"};
        // usccss.g6.u07.l18.factor_multiple_selection_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718DecisionLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718DecisionLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718DecisionLogic,2,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718DecisionLogic,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718DecisionLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718DecisionLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718DecisionLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718DecisionLogic,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718DecisionLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718DecisionLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718DecisionLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718DecisionLogic,11,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718DecisionLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718DecisionLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718DecisionLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718DecisionLogic,15,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718DecisionLogic,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718DecisionLogic,17,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718DecisionLogic,18,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0718DecisionLogic,19,"0"};
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
            "g6-u07-l18-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
