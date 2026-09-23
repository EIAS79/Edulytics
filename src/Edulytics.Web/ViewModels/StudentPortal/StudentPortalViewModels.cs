using Edulytics.Services.Notifications;
using Edulytics.Services.LessonContent;
using Edulytics.Services.StudentPortal;

namespace Edulytics.Web.ViewModels.StudentPortal;

public sealed record StudentDashboardViewModel(
    StudentPortalWorkspace Workspace,
    IReadOnlyList<NotificationInboxItem> Notifications)
{
    public int UnreadNotifications =>
        Notifications.Count(x => !x.ReadAtUtc.HasValue);

    public IReadOnlyList<StudentResultItem> RecentResults =>
        Workspace.Results.Take(4).ToArray();

    public IReadOnlyList<StudentAssessmentItem> ToDoAssessments =>
        Workspace.Assessments
            .Where(x =>
                x.DeliveryMode == Edulytics.Core.Enums.AssessmentDeliveryMode.Online &&
                !x.IsSubmitted)
            .OrderBy(x => x.AssessmentDate)
            .ToArray();

    public IReadOnlyList<StudentAssessmentItem> SubmittedOnlineAssessments =>
        Workspace.Assessments
            .Where(x =>
                x.DeliveryMode == Edulytics.Core.Enums.AssessmentDeliveryMode.Online &&
                x.IsSubmitted)
            .OrderByDescending(x => x.AssessmentDate)
            .ToArray();

    public IReadOnlyList<StudentAssessmentItem> AwaitingOfflineResults =>
        Workspace.Assessments
            .Where(x =>
                x.DeliveryMode == Edulytics.Core.Enums.AssessmentDeliveryMode.Offline &&
                !x.IsSubmitted)
            .OrderBy(x => x.AssessmentDate)
            .ToArray();

    public decimal? RecentAverage =>
        RecentResults.Count == 0
            ? null
            : RecentResults.Average(x => x.Percentage);
}

public sealed record StudentLearningViewModel(
    StudentPortalWorkspace Workspace,
    IReadOnlyList<StudentLessonSummary> Lessons)
{
    public Guid? SelectedCurriculumAdoptionId { get; init; }
    public Guid? SelectedClassGroupId { get; init; }
    public IReadOnlyList<Guid> SelectedLearningNodeIds { get; init; } = [];
}

public sealed record StudentAssessmentsViewModel(
    StudentPortalWorkspace Workspace);

public sealed record StudentResultsViewModel(
    StudentPortalWorkspace Workspace);

public sealed record StudentNotificationsViewModel(
    StudentPortalWorkspace Workspace,
    IReadOnlyList<NotificationInboxItem> Notifications);
