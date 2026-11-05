using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit5 Lesson 15 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit5Lesson15SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U05:L15";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0515FirstPartyMeasurements;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0515MeasurementLogic;

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
        // usccss.g6.u05.l15.first_party_fixed_paper_box_scaling
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0515FirstPartyMeasurements,0,"1524"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0515FirstPartyMeasurements,1,"2032"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0515FirstPartyMeasurements,2,"3048"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0515FirstPartyMeasurements,3,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0515FirstPartyMeasurements,4,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0515FirstPartyMeasurements,5,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0515FirstPartyMeasurements,6,"36"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0515FirstPartyMeasurements,7,"144"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0515FirstPartyMeasurements,8,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0515FirstPartyMeasurements,9,"32"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0515FirstPartyMeasurements,10,"128"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0515FirstPartyMeasurements,11,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0515FirstPartyMeasurements,12,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0515FirstPartyMeasurements,13,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0515FirstPartyMeasurements,14,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0515FirstPartyMeasurements,15,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0515FirstPartyMeasurements,16,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0515FirstPartyMeasurements,17,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0515FirstPartyMeasurements,18,"64"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0515FirstPartyMeasurements,19,"2"};
        // usccss.g6.u05.l15.measurement_precision_and_area_scaling
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0515MeasurementLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0515MeasurementLogic,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0515MeasurementLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0515MeasurementLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0515MeasurementLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0515MeasurementLogic,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0515MeasurementLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0515MeasurementLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0515MeasurementLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0515MeasurementLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0515MeasurementLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0515MeasurementLogic,11,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0515MeasurementLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0515MeasurementLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0515MeasurementLogic,14,"1"};
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
            "g6-u05-l15-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
