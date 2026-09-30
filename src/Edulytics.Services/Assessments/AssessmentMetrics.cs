using System.Diagnostics.Metrics;

namespace Edulytics.Services.Assessments;

public static class AssessmentMetrics
{
    public const string MeterName = "Edulytics.Assessments";

    private static readonly Meter Meter = new(MeterName, "1.0.0");

    private static readonly Counter<long> ScheduledExamOpenedCounter =
        Meter.CreateCounter<long>("exam_scheduled_open_success");

    private static readonly Counter<long> ExamBeforeStartDeniedCounter =
        Meter.CreateCounter<long>("exam_access_denied_before_start");

    private static readonly Counter<long> HomeworkSubmittedCounter =
        Meter.CreateCounter<long>("homework_submitted_before_due");

    private static readonly Counter<long> HomeworkMissedDueCounter =
        Meter.CreateCounter<long>("homework_missed_due");

    private static readonly Counter<long> WorksheetCompletedCounter =
        Meter.CreateCounter<long>("worksheet_online_completed");

    private static readonly Counter<long> WorksheetPdfGeneratedCounter =
        Meter.CreateCounter<long>("worksheet_pdf_generated");

    private static readonly Counter<long> NonExamEvaluationBlockedCounter =
        Meter.CreateCounter<long>("non_exam_evaluation_evidence_blocked");

    public static void ScheduledExamOpened() =>
        ScheduledExamOpenedCounter.Add(1);

    public static void ExamBeforeStartDenied() =>
        ExamBeforeStartDeniedCounter.Add(1);

    public static void HomeworkSubmitted() =>
        HomeworkSubmittedCounter.Add(1);

    public static void HomeworkMissedDue() =>
        HomeworkMissedDueCounter.Add(1);

    public static void WorksheetCompleted() =>
        WorksheetCompletedCounter.Add(1);

    public static void WorksheetPdfGenerated() =>
        WorksheetPdfGeneratedCounter.Add(1);

    public static void NonExamEvaluationEvidenceBlocked(long count)
    {
        if (count > 0)
            NonExamEvaluationBlockedCounter.Add(count);
    }
}
