using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit3 Lesson 17 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit4Lesson17SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U04:L17";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0417BoundedPacking;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0417PackingModelLogic;

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
        // usccss.g6.u04.l17.first_party_fixed_orientation_packing_math
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417BoundedPacking,0,"18"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417BoundedPacking,1,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417BoundedPacking,2,"10200"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417BoundedPacking,3,"132"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417BoundedPacking,4,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417BoundedPacking,5,"4035"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417BoundedPacking,6,"144"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417BoundedPacking,7,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417BoundedPacking,8,"2690"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417BoundedPacking,9,"240"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417BoundedPacking,10,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417BoundedPacking,11,"3750"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417BoundedPacking,12,"288"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417BoundedPacking,13,"18"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417BoundedPacking,14,"276"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417BoundedPacking,15,"2690"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417BoundedPacking,16,"372"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417BoundedPacking,17,"3220"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417BoundedPacking,18,"189"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417BoundedPacking,19,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417BoundedPacking,20,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417BoundedPacking,21,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417BoundedPacking,22,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417BoundedPacking,23,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417BoundedPacking,24,"3"};
        // usccss.g6.u04.l17.first_party_orientation_model_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417PackingModelLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417PackingModelLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417PackingModelLogic,2,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417PackingModelLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417PackingModelLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417PackingModelLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417PackingModelLogic,6,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417PackingModelLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417PackingModelLogic,8,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417PackingModelLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417PackingModelLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417PackingModelLogic,11,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417PackingModelLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417PackingModelLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0417PackingModelLogic,14,"1"};
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
            "g6-u04-l17-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
