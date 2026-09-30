using Edulytics.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edulytics.Data.Configurations;

public sealed class AssessmentTaskResponseConfiguration : IEntityTypeConfiguration<AssessmentTaskResponse>
{
    public void Configure(EntityTypeBuilder<AssessmentTaskResponse> builder)
    {
        builder.ToTable("AssessmentTaskResponses");
        builder.HasKey(x => x.Id);
        builder.HasAlternateKey(x => new { x.SchoolId, x.Id });

        builder.Property(x => x.ResponseText)
            .HasMaxLength(4000)
            .IsRequired();
        builder.Property(x => x.UpdatedAtUtc).IsRequired();

        builder.HasIndex(x => new
        {
            x.SchoolId,
            x.AssessmentAttemptId,
            x.AssessmentQuestionId
        }).IsUnique();

        builder.HasOne<School>()
            .WithMany()
            .HasForeignKey(x => x.SchoolId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<AssessmentAttempt>()
            .WithMany()
            .HasForeignKey(x => new { x.SchoolId, x.AssessmentAttemptId })
            .HasPrincipalKey(x => new { x.SchoolId, x.Id })
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<AssessmentQuestion>()
            .WithMany()
            .HasForeignKey(x => new { x.SchoolId, x.AssessmentQuestionId })
            .HasPrincipalKey(x => new { x.SchoolId, x.Id })
            .OnDelete(DeleteBehavior.Restrict);
    }
}
