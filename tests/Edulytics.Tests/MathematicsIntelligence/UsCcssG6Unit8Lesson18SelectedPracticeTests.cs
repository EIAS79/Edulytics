using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit8 Lesson 18 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit8Lesson18SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U08:L18";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0818SourceStatistics;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0818SelectionLogic;

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
        // usccss.g6.u08.l18.first_party_statistics_context
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SourceStatistics,0,"30"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SourceStatistics,1,"21"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SourceStatistics,2,"56"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SourceStatistics,3,"9"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SourceStatistics,4,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SourceStatistics,5,"41"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SourceStatistics,6,"26"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SourceStatistics,7,"34"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SourceStatistics,8,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SourceStatistics,9,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SourceStatistics,10,"630"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SourceStatistics,11,"154"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SourceStatistics,12,"266"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SourceStatistics,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SourceStatistics,14,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SourceStatistics,15,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SourceStatistics,16,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SourceStatistics,17,"12"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SourceStatistics,18,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SourceStatistics,19,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SourceStatistics,20,"20"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SourceStatistics,21,"10"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SourceStatistics,22,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SourceStatistics,23,"10"};
        // usccss.g6.u08.l18.first_party_statistical_report_selection
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SelectionLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SelectionLogic,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SelectionLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SelectionLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SelectionLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SelectionLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SelectionLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SelectionLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SelectionLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SelectionLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SelectionLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SelectionLogic,11,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SelectionLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SelectionLogic,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SelectionLogic,14,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SelectionLogic,15,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SelectionLogic,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SelectionLogic,17,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SelectionLogic,18,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0818SelectionLogic,19,"1"};
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
            "g6-u08-l18-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
