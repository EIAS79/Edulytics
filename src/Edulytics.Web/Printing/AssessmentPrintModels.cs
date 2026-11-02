using Edulytics.Services.Assessments;

namespace Edulytics.Web.Printing;

public sealed record StudentAssessmentPaper(
    string Title,
    DateOnly AssessmentDate,
    decimal MaxScore,
    IReadOnlyList<StudentAssessmentPaperQuestion> Questions);

public sealed record StudentAssessmentPaperQuestion(
    int Order,
    string Prompt,
    decimal MaxScore);

public sealed record TeacherAssessmentAnswerKey(
    string Title,
    DateOnly AssessmentDate,
    decimal MaxScore,
    IReadOnlyList<TeacherAssessmentAnswerKeyQuestion> Questions);

public sealed record TeacherAssessmentAnswerKeyQuestion(
    int Order,
    string Prompt,
    decimal MaxScore,
    string CorrectAnswer,
    string Solution);

public sealed record AssessmentPdfLabels(
    string StudentPaperTitle,
    string TeacherAnswerKeyTitle,
    string AssessmentMaxScore,
    string StudentName,
    string Date,
    string Marks,
    string CorrectAnswer,
    string Solution);

public static class AssessmentPrintDocumentFactory
{
    public static StudentAssessmentPaper CreateStudentPaper(AssessmentBuilderWorkspace workspace)
    {
        ArgumentNullException.ThrowIfNull(workspace);
        var assessment = workspace.Details.Assessment;
        var questions = workspace.Questions
            .OrderBy(question => question.Order)
            .Select(question => new StudentAssessmentPaperQuestion(
                question.Order,
                question.Prompt,
                question.MaxScore))
            .ToArray();

        return new StudentAssessmentPaper(
            assessment.Title,
            assessment.AssessmentDate,
            assessment.MaxScore,
            questions);
    }

    public static TeacherAssessmentAnswerKey CreateTeacherAnswerKey(AssessmentBuilderWorkspace workspace)
    {
        ArgumentNullException.ThrowIfNull(workspace);
        var assessment = workspace.Details.Assessment;
        var questions = workspace.Questions
            .OrderBy(question => question.Order)
            .Select(question => new TeacherAssessmentAnswerKeyQuestion(
                question.Order,
                question.Prompt,
                question.MaxScore,
                question.CorrectAnswer,
                question.Solution))
            .ToArray();

        return new TeacherAssessmentAnswerKey(
            assessment.Title,
            assessment.AssessmentDate,
            assessment.MaxScore,
            questions);
    }
}
