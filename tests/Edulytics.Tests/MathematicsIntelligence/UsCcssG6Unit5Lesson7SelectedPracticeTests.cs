using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit5 Lesson 7 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit5Lesson7SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U05:L07";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0507ProductValues;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0507PartialLogic;

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
        // usccss.g6.u05.l07.decimal_partial_product_values
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0507ProductValues,0,"18"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0507ProductValues,1,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0507ProductValues,2,"24"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0507ProductValues,3,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0507ProductValues,4,"434"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0507ProductValues,5,"300"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0507ProductValues,6,"120"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0507ProductValues,7,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0507ProductValues,8,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0507ProductValues,9,"1908"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0507ProductValues,10,"35"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0507ProductValues,11,"5184"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0507ProductValues,12,"35"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0507ProductValues,13,"140"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0507ProductValues,14,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0507ProductValues,15,"104"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0507ProductValues,16,"414"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0507ProductValues,17,"884"};
        // usccss.g6.u05.l07.partial_product_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0507PartialLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0507PartialLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0507PartialLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0507PartialLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0507PartialLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0507PartialLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0507PartialLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0507PartialLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0507PartialLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0507PartialLogic,9,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0507PartialLogic,10,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0507PartialLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0507PartialLogic,12,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0507PartialLogic,13,"1"};
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
            "g6-u05-l07-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
