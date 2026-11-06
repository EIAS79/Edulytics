using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit3 Lesson 16 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit4Lesson16SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U04:L16";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0416AllOperations;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0416OperationsLogic;

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
        // usccss.g6.u04.l16.fraction_operations_context
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0416AllOperations,0,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0416AllOperations,1,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0416AllOperations,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0416AllOperations,3,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0416AllOperations,4,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0416AllOperations,5,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0416AllOperations,6,"55"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0416AllOperations,7,"11"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0416AllOperations,8,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0416AllOperations,9,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0416AllOperations,10,"80"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0416AllOperations,11,"93"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0416AllOperations,12,"156"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0416AllOperations,13,"51"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0416AllOperations,14,"22"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0416AllOperations,15,"33"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0416AllOperations,16,"352"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0416AllOperations,17,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0416AllOperations,18,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0416AllOperations,19,"27"};
        // usccss.g6.u04.l16.fraction_ops_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0416OperationsLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0416OperationsLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0416OperationsLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0416OperationsLogic,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0416OperationsLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0416OperationsLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0416OperationsLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0416OperationsLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0416OperationsLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0416OperationsLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0416OperationsLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0416OperationsLogic,11,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0416OperationsLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0416OperationsLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0416OperationsLogic,14,"1"};
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
            "g6-u04-l16-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
