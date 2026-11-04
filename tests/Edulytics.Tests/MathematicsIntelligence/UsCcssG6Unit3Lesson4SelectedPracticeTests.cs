using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit3 Lesson 4 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit3Lesson4SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U03:L04";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0304Conversions;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0304DirectionChecks;

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
        // usccss.g6.u03.l04.source_ratio_unit_conversions
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0304Conversions,0,"17"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0304Conversions,1,"30"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0304Conversions,2,"51"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0304Conversions,3,"11355"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0304Conversions,4,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0304Conversions,5,"946"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0304Conversions,6,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0304Conversions,7,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0304Conversions,8,"800"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0304Conversions,9,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0304Conversions,10,"28"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0304Conversions,11,"196"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0304Conversions,12,"1372"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0304Conversions,13,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0304Conversions,14,"32"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0304Conversions,15,"24"};
        // usccss.g6.u03.l04.conversion_direction_and_units
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0304DirectionChecks,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0304DirectionChecks,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0304DirectionChecks,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0304DirectionChecks,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0304DirectionChecks,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0304DirectionChecks,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0304DirectionChecks,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0304DirectionChecks,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0304DirectionChecks,8,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0304DirectionChecks,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0304DirectionChecks,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0304DirectionChecks,11,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0304DirectionChecks,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0304DirectionChecks,13,"1"};
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
            "g6-u03-l04-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
