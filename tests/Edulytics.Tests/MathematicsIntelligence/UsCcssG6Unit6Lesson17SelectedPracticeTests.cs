using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit6 Lesson 17 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit6Lesson17SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U06:L17";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0617SpeedTables;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0617SpeedLogic;

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
        // usccss.g6.u06.l17.constant_speed_numeric
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedTables,0,"100"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedTables,1,"75"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedTables,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedTables,3,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedTables,4,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedTables,5,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedTables,6,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedTables,7,"200"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedTables,8,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedTables,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedTables,10,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedTables,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedTables,12,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedTables,13,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedTables,14,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedTables,15,"60"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedTables,16,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedTables,17,"123"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedTables,18,"56"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedTables,19,"64"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedTables,20,"1241"};
        // usccss.g6.u06.l17.constant_speed_equation_interpretation
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedLogic,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedLogic,6,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedLogic,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedLogic,16,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0617SpeedLogic,17,"1"};
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
            "g6-u06-l17-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
