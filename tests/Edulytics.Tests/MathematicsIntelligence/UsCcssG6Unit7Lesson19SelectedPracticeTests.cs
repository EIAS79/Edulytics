using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade6 Unit7 Lesson 19 first-edition selected Practice; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG6Unit7Lesson19SelectedPracticeTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U07:L19";
    private const string FamilyA = UsCcssG6Unit2RatioQuestions.L0719FirstPartyPlotting;
    private const string FamilyB = UsCcssG6Unit2RatioQuestions.L0719FirstPartyPictureLogic;

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
        // usccss.g6.u07.l19.first_party_plotting_coordinates
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPlotting,0,"3"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPlotting,1,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPlotting,2,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPlotting,3,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPlotting,4,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPlotting,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPlotting,6,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPlotting,7,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPlotting,8,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPlotting,9,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPlotting,10,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPlotting,11,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPlotting,12,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPlotting,13,"2"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPlotting,14,"-4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPlotting,15,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPlotting,16,"4"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPlotting,17,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPlotting,18,"16"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPlotting,19,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPlotting,20,"8"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPlotting,21,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPlotting,22,"0"};
        // usccss.g6.u07.l19.first_party_picture_precision_rules
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPictureLogic,0,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPictureLogic,1,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPictureLogic,2,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPictureLogic,3,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPictureLogic,4,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPictureLogic,5,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPictureLogic,6,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPictureLogic,7,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPictureLogic,8,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPictureLogic,9,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPictureLogic,10,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPictureLogic,11,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPictureLogic,12,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPictureLogic,13,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPictureLogic,14,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPictureLogic,15,"0"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPictureLogic,16,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPictureLogic,17,"1"};
        yield return new object[]{UsCcssG6Unit2RatioQuestions.L0719FirstPartyPictureLogic,18,"1"};
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
            "g6-u07-l19-original-selected-practice",Lesson,contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,questions.Count);
        Assert.All(questions,q=>{
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher",q.Solution);
        });
    }
}
