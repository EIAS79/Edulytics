using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit2 Lesson 13 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit2Lesson13SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U02:L13";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L13RepresentationValues;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L13RepresentationLogic;

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
        // usccss.g6.u02.l13.table_line_ratio_values
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L13RepresentationValues,0,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L13RepresentationValues,1,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L13RepresentationValues,2,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L13RepresentationValues,3,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L13RepresentationValues,4,"36"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L13RepresentationValues,5,"108"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L13RepresentationValues,6,"216"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L13RepresentationValues,7,"234"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L13RepresentationValues,8,"36"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L13RepresentationValues,9,"46"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L13RepresentationValues,10,"50"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L13RepresentationValues,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L13RepresentationValues,12,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L13RepresentationValues,13,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L13RepresentationValues,14,"15"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L13RepresentationValues,15,"18"};
        // usccss.g6.u02.l13.representation_selection
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L13RepresentationLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L13RepresentationLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L13RepresentationLogic,2,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L13RepresentationLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L13RepresentationLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L13RepresentationLogic,5,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L13RepresentationLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L13RepresentationLogic,7,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L13RepresentationLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L13RepresentationLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L13RepresentationLogic,10,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L13RepresentationLogic,11,"1"};
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
            "g6-u02-l13-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
