using Edulytics.Core.Enums;

namespace Edulytics.Core.Assessments;

public enum StudentAssessmentAvailabilityState
{
    Offline = 0,
    Scheduled = 1,
    Available = 2,
    Closed = 3,
    Submitted = 4
}

public readonly record struct StudentAssessmentAvailability(
    StudentAssessmentAvailabilityState State,
    bool CanStart,
    DateTime? NextStateChangeAtUtc);

public static class StudentAssessmentAvailabilityPolicy
{
    public static StudentAssessmentAvailability Evaluate(
        AssessmentType assessmentType,
        AssessmentDeliveryMode deliveryMode,
        AssessmentStatus status,
        DateTime? availableFromUtc,
        DateTime? dueAtUtc,
        bool isSubmitted,
        DateTime nowUtc)
    {
        if (isSubmitted)
        {
            return new StudentAssessmentAvailability(
                StudentAssessmentAvailabilityState.Submitted,
                false,
                null);
        }

        if (deliveryMode != AssessmentDeliveryMode.Online)
        {
            return new StudentAssessmentAvailability(
                StudentAssessmentAvailabilityState.Offline,
                false,
                null);
        }

        if (status != AssessmentStatus.Open)
        {
            return new StudentAssessmentAvailability(
                StudentAssessmentAvailabilityState.Closed,
                false,
                null);
        }

        if (assessmentType == AssessmentType.Exam)
        {
            if (availableFromUtc.HasValue && nowUtc < availableFromUtc.Value)
            {
                return new StudentAssessmentAvailability(
                    StudentAssessmentAvailabilityState.Scheduled,
                    false,
                    availableFromUtc.Value);
            }

            if (dueAtUtc.HasValue && nowUtc >= dueAtUtc.Value)
            {
                return new StudentAssessmentAvailability(
                    StudentAssessmentAvailabilityState.Closed,
                    false,
                    null);
            }

            return new StudentAssessmentAvailability(
                StudentAssessmentAvailabilityState.Available,
                true,
                dueAtUtc);
        }

        if (assessmentType == AssessmentType.Homework)
        {
            if (!dueAtUtc.HasValue || nowUtc >= dueAtUtc.Value)
            {
                return new StudentAssessmentAvailability(
                    StudentAssessmentAvailabilityState.Closed,
                    false,
                    null);
            }

            return new StudentAssessmentAvailability(
                StudentAssessmentAvailabilityState.Available,
                true,
                null);
        }

        return new StudentAssessmentAvailability(
            StudentAssessmentAvailabilityState.Available,
            true,
            null);
    }
}
