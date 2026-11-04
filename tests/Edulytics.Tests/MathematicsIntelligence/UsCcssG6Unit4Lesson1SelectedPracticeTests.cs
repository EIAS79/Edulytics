using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit3 Lesson 1 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit4Lesson1SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U04:L01";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0401SourceNumerics;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0401QuotientScale;

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
        // usccss.g6.u04.l01.division_and_source_quantities
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0401SourceNumerics,0,"40"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0401SourceNumerics,1,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0401SourceNumerics,2,"4000"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0401SourceNumerics,3,"100000"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0401SourceNumerics,4,"55"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0401SourceNumerics,5,"190"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0401SourceNumerics,6,"57"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0401SourceNumerics,7,"36"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0401SourceNumerics,8,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0401SourceNumerics,9,"27"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0401SourceNumerics,10,"18"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0401SourceNumerics,11,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0401SourceNumerics,12,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0401SourceNumerics,13,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0401SourceNumerics,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0401SourceNumerics,15,"1"};
        // usccss.g6.u04.l01.predict_quotient_size
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0401QuotientScale,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0401QuotientScale,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0401QuotientScale,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0401QuotientScale,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0401QuotientScale,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0401QuotientScale,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0401QuotientScale,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0401QuotientScale,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0401QuotientScale,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0401QuotientScale,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0401QuotientScale,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0401QuotientScale,11,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0401QuotientScale,12,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0401QuotientScale,13,"1"};
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
            "g6-u04-l01-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
