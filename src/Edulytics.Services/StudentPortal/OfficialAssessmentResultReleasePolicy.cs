using Edulytics.Core.Enums;

namespace Edulytics.Services.StudentPortal;

public static class OfficialAssessmentResultReleasePolicy
{
    public static bool CanStudentView(
        AssessmentStatus status,
        AssessmentResultReleaseStatus releaseStatus) =>
        status == AssessmentStatus.Closed &&
        releaseStatus == AssessmentResultReleaseStatus.Published;
}
