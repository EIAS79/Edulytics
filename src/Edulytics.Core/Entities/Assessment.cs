using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;

namespace Edulytics.Core.Entities;

public sealed class Assessment : ISchoolScoped
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public Guid SubjectId { get; set; }
    public Guid ClassGroupId { get; set; }
    public Guid AcademicYearId { get; set; }
    public Guid TermId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateOnly AssessmentDate { get; set; }
    public decimal MaxScore { get; set; }
    public AssessmentStatus Status { get; set; }
    public AssessmentTargetType TargetType { get; set; } = AssessmentTargetType.Class;
    public Guid? TargetStudentProfileId { get; set; }
    public AssessmentDeliveryMode DeliveryMode { get; set; } = AssessmentDeliveryMode.Offline;
    public AssessmentDifficultyBand DifficultyBand { get; set; } = AssessmentDifficultyBand.AtClassLevel;
    public Guid CreatedByUserId { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
    public byte[] RowVersion { get; set; } = [];
}
