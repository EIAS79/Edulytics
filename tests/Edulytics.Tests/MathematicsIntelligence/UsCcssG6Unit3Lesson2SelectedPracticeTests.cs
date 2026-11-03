using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit3 Lesson 2 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit3Lesson2SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U03:L02";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0302UnitMagnitudes;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0302UnitCategories;

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
        // usccss.g6.u03.l02.unit_magnitude_and_conversion
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0302UnitMagnitudes,0,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0302UnitMagnitudes,1,"72"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0302UnitMagnitudes,2,"36"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0302UnitMagnitudes,3,"100"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0302UnitMagnitudes,4,"1000"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0302UnitMagnitudes,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0302UnitMagnitudes,6,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0302UnitMagnitudes,7,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0302UnitMagnitudes,8,"750"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0302UnitMagnitudes,9,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0302UnitMagnitudes,10,"18"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0302UnitMagnitudes,11,"300000"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0302UnitMagnitudes,12,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0302UnitMagnitudes,13,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0302UnitMagnitudes,14,"3000"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0302UnitMagnitudes,15,"200"};
        // usccss.g6.u03.l02.choose_appropriate_unit_category
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0302UnitCategories,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0302UnitCategories,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0302UnitCategories,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0302UnitCategories,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0302UnitCategories,4,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0302UnitCategories,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0302UnitCategories,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0302UnitCategories,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0302UnitCategories,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0302UnitCategories,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0302UnitCategories,10,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0302UnitCategories,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0302UnitCategories,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0302UnitCategories,13,"1"};
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
            "g6-u03-l02-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
