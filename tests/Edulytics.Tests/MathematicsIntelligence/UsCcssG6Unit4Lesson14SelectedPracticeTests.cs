using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit3 Lesson 14 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit4Lesson14SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U04:L14";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0414GeometryNumbers;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0414GeometryLogic;

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
        // usccss.g6.u04.l14.triangle_height_and_cube_counts
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0414GeometryNumbers,0,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0414GeometryNumbers,1,"512"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0414GeometryNumbers,2,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0414GeometryNumbers,3,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0414GeometryNumbers,4,"60"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0414GeometryNumbers,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0414GeometryNumbers,6,"200"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0414GeometryNumbers,7,"5400"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0414GeometryNumbers,8,"24"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0414GeometryNumbers,9,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0414GeometryNumbers,10,"17"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0414GeometryNumbers,11,"14"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0414GeometryNumbers,12,"60"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0414GeometryNumbers,13,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0414GeometryNumbers,14,"50"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0414GeometryNumbers,15,"75"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0414GeometryNumbers,16,"50"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0414GeometryNumbers,17,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0414GeometryNumbers,18,"63"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0414GeometryNumbers,19,"64"};
        // usccss.g6.u04.l14.cube_and_triangle_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0414GeometryLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0414GeometryLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0414GeometryLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0414GeometryLogic,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0414GeometryLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0414GeometryLogic,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0414GeometryLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0414GeometryLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0414GeometryLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0414GeometryLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0414GeometryLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0414GeometryLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0414GeometryLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0414GeometryLogic,13,"1"};
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
            "g6-u04-l14-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
