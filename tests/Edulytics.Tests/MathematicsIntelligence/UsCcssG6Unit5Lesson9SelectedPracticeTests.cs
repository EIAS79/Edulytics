using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit5 Lesson 9 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit5Lesson9SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U05:L09";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0509WholeDivision;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0509DivisionLogic;

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
        // usccss.g6.u05.l09.partial_quotient_results
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0509WholeDivision,0,"421"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0509WholeDivision,1,"219"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0509WholeDivision,2,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0509WholeDivision,3,"89"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0509WholeDivision,4,"24"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0509WholeDivision,5,"400"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0509WholeDivision,6,"105"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0509WholeDivision,7,"100"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0509WholeDivision,8,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0509WholeDivision,9,"600"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0509WholeDivision,10,"219"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0509WholeDivision,11,"219"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0509WholeDivision,12,"480"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0509WholeDivision,13,"54"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0509WholeDivision,14,"19825"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0509WholeDivision,15,"2116"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0509WholeDivision,16,"205625"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0509WholeDivision,17,"19386"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0509WholeDivision,18,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0509WholeDivision,19,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0509WholeDivision,20,"2"};
        // usccss.g6.u05.l09.partial_quotient_principles
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0509DivisionLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0509DivisionLogic,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0509DivisionLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0509DivisionLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0509DivisionLogic,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0509DivisionLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0509DivisionLogic,6,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0509DivisionLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0509DivisionLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0509DivisionLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0509DivisionLogic,10,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0509DivisionLogic,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0509DivisionLogic,12,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0509DivisionLogic,13,"1"};
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
            "g6-u05-l09-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
