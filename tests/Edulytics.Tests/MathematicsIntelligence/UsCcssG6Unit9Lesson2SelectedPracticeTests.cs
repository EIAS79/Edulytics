using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit9 Lesson 2 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit9Lesson2SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U09:L02";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0902PopulationMath;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0902PopulationLogic;

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
        // usccss.g6.u09.l02.first_party_population_ratio_quantities
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationMath,0,"30"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationMath,1,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationMath,2,"40"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationMath,3,"17"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationMath,4,"1233"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationMath,5,"7400"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationMath,6,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationMath,7,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationMath,8,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationMath,9,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationMath,10,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationMath,11,"40"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationMath,12,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationMath,13,"47"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationMath,14,"53"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationMath,15,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationMath,16,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationMath,17,"40"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationMath,18,"1110"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationMath,19,"1850"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationMath,20,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationMath,21,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationMath,22,"12"};
        // usccss.g6.u09.l02.first_party_population_model_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationLogic,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationLogic,10,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationLogic,14,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationLogic,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationLogic,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationLogic,17,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0902PopulationLogic,18,"1"};
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
            "g6-u09-l02-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
