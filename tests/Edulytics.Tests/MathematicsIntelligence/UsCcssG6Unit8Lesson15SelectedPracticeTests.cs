using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit8 Lesson 15 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit8Lesson15SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U08:L15";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0815QuartileValues;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0815QuartileLogic;

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
        // usccss.g6.u08.l15.quartiles_five_number_summary
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileValues,0,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileValues,1,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileValues,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileValues,3,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileValues,4,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileValues,5,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileValues,6,"11"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileValues,7,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileValues,8,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileValues,9,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileValues,10,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileValues,11,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileValues,12,"35"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileValues,13,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileValues,14,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileValues,15,"11"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileValues,16,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileValues,17,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileValues,18,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileValues,19,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileValues,20,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileValues,21,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileValues,22,"40"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileValues,23,"30"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileValues,24,"125"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileValues,25,"8"};
        // usccss.g6.u08.l15.quartile_spread_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileLogic,11,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileLogic,15,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileLogic,16,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0815QuartileLogic,17,"1"};
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
            "g6-u08-l15-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
