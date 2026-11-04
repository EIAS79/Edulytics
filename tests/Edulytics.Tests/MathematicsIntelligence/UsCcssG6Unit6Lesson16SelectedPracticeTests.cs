using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit6 Lesson 16 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit6Lesson16SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U06:L16";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0616RatesAndTables;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0616RelatedLogic;

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
        // usccss.g6.u06.l16.related_quantities_numeric
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RatesAndTables,0,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RatesAndTables,1,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RatesAndTables,2,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RatesAndTables,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RatesAndTables,4,"450"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RatesAndTables,5,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RatesAndTables,6,"270"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RatesAndTables,7,"80"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RatesAndTables,8,"400"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RatesAndTables,9,"720"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RatesAndTables,10,"800"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RatesAndTables,11,"14"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RatesAndTables,12,"66"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RatesAndTables,13,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RatesAndTables,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RatesAndTables,15,"17"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RatesAndTables,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RatesAndTables,17,"8638"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RatesAndTables,18,"6042"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RatesAndTables,19,"1215"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RatesAndTables,20,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RatesAndTables,21,"4"};
        // usccss.g6.u06.l16.two_variable_equation_interpretation
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RelatedLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RelatedLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RelatedLogic,2,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RelatedLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RelatedLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RelatedLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RelatedLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RelatedLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RelatedLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RelatedLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RelatedLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RelatedLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RelatedLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RelatedLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RelatedLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RelatedLogic,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0616RelatedLogic,16,"1"};
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
            "g6-u06-l16-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
