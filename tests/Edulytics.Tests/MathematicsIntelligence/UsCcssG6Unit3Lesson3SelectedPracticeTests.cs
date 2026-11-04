using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit3 Lesson 3 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit3Lesson3SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U03:L03";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0303SourceAmounts;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0303AttributeClassification;

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
        // usccss.g6.u03.l03.measurement_source_quantity
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0303SourceAmounts,0,"145"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0303SourceAmounts,1,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0303SourceAmounts,2,"19"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0303SourceAmounts,3,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0303SourceAmounts,4,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0303SourceAmounts,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0303SourceAmounts,6,"60"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0303SourceAmounts,7,"1800"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0303SourceAmounts,8,"720"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0303SourceAmounts,9,"45"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0303SourceAmounts,10,"38"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0303SourceAmounts,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0303SourceAmounts,12,"1100"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0303SourceAmounts,13,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0303SourceAmounts,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0303SourceAmounts,15,"3000"};
        // usccss.g6.u03.l03.measurement_attribute_identification
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0303AttributeClassification,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0303AttributeClassification,1,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0303AttributeClassification,2,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0303AttributeClassification,3,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0303AttributeClassification,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0303AttributeClassification,5,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0303AttributeClassification,6,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0303AttributeClassification,7,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0303AttributeClassification,8,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0303AttributeClassification,9,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0303AttributeClassification,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0303AttributeClassification,11,"2"};
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
            "g6-u03-l03-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
