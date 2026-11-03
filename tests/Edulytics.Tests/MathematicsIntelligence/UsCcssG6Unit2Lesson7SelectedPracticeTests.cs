using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit2 Lesson 7 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit2Lesson7SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U02:L07";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L07LineValues;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L07LineLogic;

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
        // usccss.g6.u02.l07.created_double_number_line_values
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L07LineValues,0,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L07LineValues,1,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L07LineValues,2,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L07LineValues,3,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L07LineValues,4,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L07LineValues,5,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L07LineValues,6,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L07LineValues,7,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L07LineValues,8,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L07LineValues,9,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L07LineValues,10,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L07LineValues,11,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L07LineValues,12,"49"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L07LineValues,13,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L07LineValues,14,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L07LineValues,15,"44"};
        // usccss.g6.u02.l07.double_number_line_units_per
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L07LineLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L07LineLogic,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L07LineLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L07LineLogic,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L07LineLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L07LineLogic,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L07LineLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L07LineLogic,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L07LineLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L07LineLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L07LineLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L07LineLogic,11,"1"};
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
            "g6-u02-l07-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
