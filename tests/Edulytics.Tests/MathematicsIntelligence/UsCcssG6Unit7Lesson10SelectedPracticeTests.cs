using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit7 Lesson 10 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit7Lesson10SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U07:L10";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0710ContextSolutions;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0710ConstraintLogic;

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
        // usccss.g6.u07.l10.inequality_context_solutions
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ContextSolutions,0,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ContextSolutions,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ContextSolutions,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ContextSolutions,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ContextSolutions,4,"11"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ContextSolutions,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ContextSolutions,6,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ContextSolutions,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ContextSolutions,8,"55"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ContextSolutions,9,"59"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ContextSolutions,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ContextSolutions,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ContextSolutions,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ContextSolutions,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ContextSolutions,14,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ContextSolutions,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ContextSolutions,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ContextSolutions,17,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ContextSolutions,18,"60"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ContextSolutions,19,"54"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ContextSolutions,20,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ContextSolutions,21,"11"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ContextSolutions,22,"1"};
        // usccss.g6.u07.l10.inequality_context_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ConstraintLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ConstraintLogic,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ConstraintLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ConstraintLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ConstraintLogic,4,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ConstraintLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ConstraintLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ConstraintLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ConstraintLogic,8,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ConstraintLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ConstraintLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ConstraintLogic,11,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ConstraintLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ConstraintLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ConstraintLogic,14,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ConstraintLogic,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ConstraintLogic,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0710ConstraintLogic,17,"1"};
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
            "g6-u07-l10-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
