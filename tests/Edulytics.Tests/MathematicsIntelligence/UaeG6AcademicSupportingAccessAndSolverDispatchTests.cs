using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UaeG6AcademicSupportingAccessAndSolverDispatchTests
{
    private const string G6First =
        "PED:UAE-MOE-MATH:L6:GENERAL:01:01:FACTORS-AND-MULTIPLES";
    private const string G6Area =
        "PED:UAE-MOE-MATH:L6:GENERAL:09:01:STUDENT-AR-128";

    [Fact]
    public void SupportingExceptionIsConstrainedToOwnerApprovedG6GeneralArabic()
    {
        const string uae = "UAE-MOE-MATH";
        Assert.True(LessonContentPolicy.CanExposeStudentLesson(uae, true, G6First));
        Assert.True(LessonContentPolicy.CanExposeStudentLesson(uae, true, G6Area));
        Assert.False(LessonContentPolicy.CanExposeStudentLesson(uae, true));
        Assert.False(LessonContentPolicy.CanExposeStudentLesson(uae, true,
            "PED:UAE-MOE-MATH:L5:GENERAL:01:01:STUDENT-AR-128"));
        Assert.False(LessonContentPolicy.CanExposeStudentLesson(uae, true,
            "PED:UAE-MOE-MATH:L6:ADVANCED:01:01:STUDENT-AR-128"));
        Assert.False(LessonContentPolicy.CanExposeStudentLesson(uae, true,
            "PED:UAE-MOE-MATH:L6:GENERAL:01:02:UNAPPROVED"));
        Assert.False(LessonContentPolicy.CanExposeStudentLesson(MathematicsCurriculumPackRegistry.CambridgeCode, true, G6Area));
        Assert.True(LessonContentPolicy.CanExposeStudentLesson(uae, false));
    }

    [Theory]
    [InlineData("uae.g6.u09.l01.area_conservation")]
    [InlineData("uae.g6.u09.l02.parallelogram_area")]
    [InlineData("uae.g6.u10.l02.rect_prism_volume")]
    [InlineData("uae.g6.u11.l02.mean")]
    [InlineData("uae.g6.u12.l01.dot_plot_frequency")]
    public void NewFirstPartyFamiliesGenerateIndependentlyVerifiableAnswers(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var question = Assert.Single(new ExactSkillContractQuestionEngine().Generate(
            G6Area, family, [family], ExactSkillQuestionDifficulty.Standard,
            1, 20261115, []));
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            family, question.Parameters, question.CorrectAnswer));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            family, question.Parameters, "definitely-incorrect-answer"));
    }

    [Fact]
    public void OrderOfOperationsArabicAnswerWithSpacesMustVerify()
    {
        const string family = "uae.g6.u06.l03.first_operation";
        var question = Assert.Single(new ExactSkillContractQuestionEngine().Generate(
            G6Area, family, [family], ExactSkillQuestionDifficulty.Standard,
            1, 20261115, []));
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            family, question.Parameters, question.CorrectAnswer));
    }
}
