using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit6 Lesson 18 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit6Lesson18SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U06:L18";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0618RelationValues;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0618RelationLogic;

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
        // usccss.g6.u06.l18.related_area_volume_growth
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationValues,0,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationValues,1,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationValues,2,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationValues,3,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationValues,4,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationValues,5,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationValues,6,"80"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationValues,7,"640"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationValues,8,"270"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationValues,9,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationValues,10,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationValues,11,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationValues,12,"128"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationValues,13,"1024"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationValues,14,"4096"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationValues,15,"32768"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationValues,16,"65536"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationValues,17,"268435456"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationValues,18,"249"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationValues,19,"28"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationValues,20,"420"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationValues,21,"560"};
        // usccss.g6.u06.l18.inverse_and_exponential_relationships
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationLogic,12,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationLogic,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationLogic,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationLogic,17,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0618RelationLogic,18,"1"};
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
            "g6-u06-l18-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
