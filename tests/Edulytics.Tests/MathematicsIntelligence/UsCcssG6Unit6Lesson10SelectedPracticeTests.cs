using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit6 Lesson 10 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit6Lesson10SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U06:L10";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0610VariableArea;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0610AreaReasoning;

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
        // usccss.g6.u06.l10.variable_distributive_values
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610VariableArea,0,"27"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610VariableArea,1,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610VariableArea,2,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610VariableArea,3,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610VariableArea,4,"45"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610VariableArea,5,"36"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610VariableArea,6,"27"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610VariableArea,7,"70"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610VariableArea,8,"2200"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610VariableArea,9,"18"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610VariableArea,10,"111"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610VariableArea,11,"877"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610VariableArea,12,"115"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610VariableArea,13,"85"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610VariableArea,14,"129"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610VariableArea,15,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610VariableArea,16,"24"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610VariableArea,17,"24"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610VariableArea,18,"63"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610VariableArea,19,"18"};
        // usccss.g6.u06.l10.variable_distributive_equivalence
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610AreaReasoning,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610AreaReasoning,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610AreaReasoning,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610AreaReasoning,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610AreaReasoning,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610AreaReasoning,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610AreaReasoning,6,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610AreaReasoning,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610AreaReasoning,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610AreaReasoning,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610AreaReasoning,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610AreaReasoning,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610AreaReasoning,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610AreaReasoning,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610AreaReasoning,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0610AreaReasoning,15,"1"};
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
            "g6-u06-l10-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
