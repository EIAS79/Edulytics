using Edulytics.Core.Assessments;
using Edulytics.Core.Enums;

namespace Edulytics.Tests;

public sealed class AssessmentAvailabilityPolicyTests
{
    [Fact]
    public void Exam_IsScheduledBeforeStart_AndAvailableAtStart()
    {
        var start = new DateTime(2026, 10, 1, 10, 0, 0, DateTimeKind.Utc);
        var end = start.AddHours(1);

        var before = StudentAssessmentAvailabilityPolicy.Evaluate(
            AssessmentType.Exam,
            AssessmentDeliveryMode.Online,
            AssessmentStatus.Open,
            start,
            end,
            isSubmitted: false,
            start.AddTicks(-1));

        var atStart = StudentAssessmentAvailabilityPolicy.Evaluate(
            AssessmentType.Exam,
            AssessmentDeliveryMode.Online,
            AssessmentStatus.Open,
            start,
            end,
            isSubmitted: false,
            start);

        Assert.Equal(StudentAssessmentAvailabilityState.Scheduled, before.State);
        Assert.False(before.CanStart);
        Assert.Equal(start, before.NextStateChangeAtUtc);

        Assert.Equal(StudentAssessmentAvailabilityState.Available, atStart.State);
        Assert.True(atStart.CanStart);
        Assert.Equal(end, atStart.NextStateChangeAtUtc);
    }

    [Fact]
    public void Exam_HardEndIsExclusiveFinalCutoff()
    {
        var start = new DateTime(2026, 10, 1, 10, 0, 0, DateTimeKind.Utc);
        var end = start.AddHours(1);

        var beforeEnd = StudentAssessmentAvailabilityPolicy.Evaluate(
            AssessmentType.Exam,
            AssessmentDeliveryMode.Online,
            AssessmentStatus.Open,
            start,
            end,
            isSubmitted: false,
            end.AddTicks(-1));

        var atEnd = StudentAssessmentAvailabilityPolicy.Evaluate(
            AssessmentType.Exam,
            AssessmentDeliveryMode.Online,
            AssessmentStatus.Open,
            start,
            end,
            isSubmitted: false,
            end);

        Assert.Equal(StudentAssessmentAvailabilityState.Available, beforeEnd.State);
        Assert.True(beforeEnd.CanStart);

        Assert.Equal(StudentAssessmentAvailabilityState.Closed, atEnd.State);
        Assert.False(atEnd.CanStart);
        Assert.Null(atEnd.NextStateChangeAtUtc);
    }

    [Fact]
    public void SubmittedExamCannotStart()
    {
        var now = new DateTime(2026, 10, 1, 10, 30, 0, DateTimeKind.Utc);

        var state = StudentAssessmentAvailabilityPolicy.Evaluate(
            AssessmentType.Exam,
            AssessmentDeliveryMode.Online,
            AssessmentStatus.Open,
            now.AddMinutes(-30),
            now.AddMinutes(30),
            isSubmitted: true,
            now);

        Assert.Equal(StudentAssessmentAvailabilityState.Submitted, state.State);
        Assert.False(state.CanStart);
    }
}
