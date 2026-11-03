using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit3 Lesson 12 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit3Lesson12SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U03:L12";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0312TapePercent;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0312TapeStructure;

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
        // usccss.g6.u03.l12.tape_diagram_percent_amounts
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0312TapePercent,0,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0312TapePercent,1,"30"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0312TapePercent,2,"28"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0312TapePercent,3,"14"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0312TapePercent,4,"40"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0312TapePercent,5,"28"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0312TapePercent,6,"50"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0312TapePercent,7,"60"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0312TapePercent,8,"250"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0312TapePercent,9,"252"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0312TapePercent,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0312TapePercent,11,"3375"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0312TapePercent,12,"96"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0312TapePercent,13,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0312TapePercent,14,"60"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0312TapePercent,15,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0312TapePercent,16,"25"};
        // usccss.g6.u03.l12.percent_tape_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0312TapeStructure,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0312TapeStructure,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0312TapeStructure,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0312TapeStructure,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0312TapeStructure,4,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0312TapeStructure,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0312TapeStructure,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0312TapeStructure,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0312TapeStructure,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0312TapeStructure,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0312TapeStructure,10,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0312TapeStructure,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0312TapeStructure,12,"1"};
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
            "g6-u03-l12-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
