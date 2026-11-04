using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit7 Lesson 8 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit7Lesson8SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U07:L08";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0708InequalityExamples;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0708InequalityLogic;

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
        // usccss.g6.u07.l08.inequality_bound_examples
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityExamples,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityExamples,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityExamples,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityExamples,3,"499"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityExamples,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityExamples,5,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityExamples,6,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityExamples,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityExamples,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityExamples,9,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityExamples,10,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityExamples,11,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityExamples,12,"27"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityExamples,13,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityExamples,14,"-9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityExamples,15,"28"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityExamples,16,"11"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityExamples,17,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityExamples,18,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityExamples,19,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityExamples,20,"1"};
        // usccss.g6.u07.l08.inequality_constraints_logic
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityLogic,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityLogic,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityLogic,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityLogic,6,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityLogic,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityLogic,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0708InequalityLogic,17,"1"};
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
            "g6-u07-l08-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
