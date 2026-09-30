using Edulytics.Core.Interfaces;

namespace Edulytics.Core.Entities;

public sealed class AssessmentTaskResponse : ISchoolScoped
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public Guid AssessmentAttemptId { get; set; }
    public Guid AssessmentQuestionId { get; set; }
    public string ResponseText { get; set; } = string.Empty;
    public DateTime UpdatedAtUtc { get; set; }
}
