using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit9 Lesson 1 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit9Lesson1SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U09:L01";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0901EstimateNumbers;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0901AssumptionLogic;

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
        // usccss.g6.u09.l01.first_party_fermi_source_rates
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901EstimateNumbers,0,"1080"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901EstimateNumbers,1,"64800"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901EstimateNumbers,2,"648"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901EstimateNumbers,3,"3944"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901EstimateNumbers,4,"3944"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901EstimateNumbers,5,"36"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901EstimateNumbers,6,"1800"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901EstimateNumbers,7,"1555200"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901EstimateNumbers,8,"1555"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901EstimateNumbers,9,"2536"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901EstimateNumbers,10,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901EstimateNumbers,11,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901EstimateNumbers,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901EstimateNumbers,13,"1000000"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901EstimateNumbers,14,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901EstimateNumbers,15,"15000"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901EstimateNumbers,16,"60000"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901EstimateNumbers,17,"60000"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901EstimateNumbers,18,"15000"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901EstimateNumbers,19,"150000"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901EstimateNumbers,20,"16500"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901EstimateNumbers,21,"1000"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901EstimateNumbers,22,"1000000"};
        // usccss.g6.u09.l01.first_party_fermi_modeling_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901AssumptionLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901AssumptionLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901AssumptionLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901AssumptionLogic,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901AssumptionLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901AssumptionLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901AssumptionLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901AssumptionLogic,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901AssumptionLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901AssumptionLogic,9,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901AssumptionLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901AssumptionLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901AssumptionLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901AssumptionLogic,13,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901AssumptionLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901AssumptionLogic,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901AssumptionLogic,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901AssumptionLogic,17,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0901AssumptionLogic,18,"1"};
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
            "g6-u09-l01-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
