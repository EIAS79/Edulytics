using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit6 Lesson 1 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit6Lesson1SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U06:L01";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0601Unknowns;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0601TapeLogic;

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
        // usccss.g6.u06.l01.tape_equation_unknown_values
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0601Unknowns,0,"13"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0601Unknowns,1,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0601Unknowns,2,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0601Unknowns,3,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0601Unknowns,4,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0601Unknowns,5,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0601Unknowns,6,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0601Unknowns,7,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0601Unknowns,8,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0601Unknowns,9,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0601Unknowns,10,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0601Unknowns,11,"56"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0601Unknowns,12,"310"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0601Unknowns,13,"98"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0601Unknowns,14,"1352"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0601Unknowns,15,"300"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0601Unknowns,16,"13"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0601Unknowns,17,"7"};
        // usccss.g6.u06.l01.tape_diagram_equation_interpretation
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0601TapeLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0601TapeLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0601TapeLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0601TapeLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0601TapeLogic,4,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0601TapeLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0601TapeLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0601TapeLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0601TapeLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0601TapeLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0601TapeLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0601TapeLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0601TapeLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0601TapeLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0601TapeLogic,14,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0601TapeLogic,15,"0"};
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
            "g6-u06-l01-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
