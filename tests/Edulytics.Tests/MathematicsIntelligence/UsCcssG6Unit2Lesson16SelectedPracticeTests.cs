using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit2 Lesson 16 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit2Lesson16SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U02:L16";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L16MultistepRatio;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L16RepresentationChoice;

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
        // usccss.g6.u02.l16.multistep_ratio_totals
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L16MultistepRatio,0,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L16MultistepRatio,1,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L16MultistepRatio,2,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L16MultistepRatio,3,"80"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L16MultistepRatio,4,"120"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L16MultistepRatio,5,"300"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L16MultistepRatio,6,"9100"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L16MultistepRatio,7,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L16MultistepRatio,8,"160"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L16MultistepRatio,9,"140"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L16MultistepRatio,10,"52"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L16MultistepRatio,11,"26"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L16MultistepRatio,12,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L16MultistepRatio,13,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L16MultistepRatio,14,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L16MultistepRatio,15,"4"};
        // usccss.g6.u02.l16.ratio_tool_selection
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L16RepresentationChoice,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L16RepresentationChoice,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L16RepresentationChoice,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L16RepresentationChoice,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L16RepresentationChoice,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L16RepresentationChoice,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L16RepresentationChoice,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L16RepresentationChoice,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L16RepresentationChoice,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L16RepresentationChoice,9,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L16RepresentationChoice,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L16RepresentationChoice,11,"1"};
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
            "g6-u02-l16-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
