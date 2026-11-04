using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit3 Lesson 6 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit3Lesson6SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U03:L06";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0306Rates;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0306InterpretRate;

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
        // usccss.g6.u03.l06.source_reciprocal_rate_values
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0306Rates,0,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0306Rates,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0306Rates,2,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0306Rates,3,"32"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0306Rates,4,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0306Rates,5,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0306Rates,6,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0306Rates,7,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0306Rates,8,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0306Rates,9,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0306Rates,10,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0306Rates,11,"52"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0306Rates,12,"57"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0306Rates,13,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0306Rates,14,"32"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0306Rates,15,"36"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0306Rates,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0306Rates,17,"4"};
        // usccss.g6.u03.l06.reciprocal_rate_meaning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0306InterpretRate,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0306InterpretRate,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0306InterpretRate,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0306InterpretRate,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0306InterpretRate,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0306InterpretRate,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0306InterpretRate,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0306InterpretRate,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0306InterpretRate,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0306InterpretRate,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0306InterpretRate,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0306InterpretRate,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0306InterpretRate,12,"0"};
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
            "g6-u03-l06-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
