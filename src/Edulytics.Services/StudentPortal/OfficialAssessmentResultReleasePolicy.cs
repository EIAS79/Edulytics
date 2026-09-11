using Edulytics.Core.Enums;

namespace Edulytics.Services.StudentPortal;

public static class OfficialAssessmentResultReleasePolicy
{
    public static bool CanStudentView(AssessmentStatus status) =>
        status == AssessmentStatus.Closed;
}
