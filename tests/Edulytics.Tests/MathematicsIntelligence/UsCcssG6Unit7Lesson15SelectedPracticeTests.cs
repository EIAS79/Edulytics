using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit7 Lesson 15 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit7Lesson15SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U07:L15";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0715PolygonNumbers;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0715PolygonChecks;

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
        // usccss.g6.u07.l15.polygon_perimeter_signed_coordinates
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonNumbers,0,"7"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonNumbers,1,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonNumbers,2,"24"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonNumbers,3,"35"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonNumbers,4,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonNumbers,5,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonNumbers,6,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonNumbers,7,"25"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonNumbers,8,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonNumbers,9,"6"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonNumbers,10,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonNumbers,11,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonNumbers,12,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonNumbers,13,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonNumbers,14,"22"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonNumbers,15,"465"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonNumbers,16,"120"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonNumbers,17,"240"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonNumbers,18,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonNumbers,19,"5"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonNumbers,20,"11"};
        // usccss.g6.u07.l15.polygon_coordinate_reasoning
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonChecks,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonChecks,1,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonChecks,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonChecks,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonChecks,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonChecks,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonChecks,6,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonChecks,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonChecks,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonChecks,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonChecks,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonChecks,11,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonChecks,12,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonChecks,13,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonChecks,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonChecks,15,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonChecks,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonChecks,17,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0715PolygonChecks,18,"1"};
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
            "g6-u07-l15-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
