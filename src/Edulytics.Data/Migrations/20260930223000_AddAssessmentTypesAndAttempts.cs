using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edulytics.Data.Migrations;

public partial class AddAssessmentTypesAndAttempts : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "AssessmentType",
            table: "Assessments",
            type: "integer",
            nullable: false,
            defaultValue: 1);

        migrationBuilder.AddColumn<DateTime>(
            name: "AvailableFromUtc",
            table: "Assessments",
            type: "timestamp with time zone",
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "DueAtUtc",
            table: "Assessments",
            type: "timestamp with time zone",
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "AttemptTimeLimitMinutes",
            table: "Assessments",
            type: "integer",
            nullable: true);

        migrationBuilder.CreateTable(
            name: "AssessmentAttempts",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                AssessmentId = table.Column<Guid>(type: "uuid", nullable: false),
                StudentProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                Status = table.Column<int>(type: "integer", nullable: false),
                StartedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                SubmittedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                CompletedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                RowVersion = table.Column<byte[]>(type: "bytea", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AssessmentAttempts", x => x.Id);
                table.UniqueConstraint(
                    "AK_AssessmentAttempts_SchoolId_Id",
                    x => new { x.SchoolId, x.Id });
                table.ForeignKey(
                    name: "FK_AssessmentAttempts_Assessments_SchoolId_AssessmentId",
                    columns: x => new { x.SchoolId, x.AssessmentId },
                    principalTable: "Assessments",
                    principalColumns: new[] { "SchoolId", "Id" },
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_AssessmentAttempts_Schools_SchoolId",
                    column: x => x.SchoolId,
                    principalTable: "Schools",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_AssessmentAttempts_StudentProfiles_SchoolId_StudentProfileId",
                    columns: x => new { x.SchoolId, x.StudentProfileId },
                    principalTable: "StudentProfiles",
                    principalColumns: new[] { "SchoolId", "Id" },
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "AssessmentTaskResponses",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                AssessmentAttemptId = table.Column<Guid>(type: "uuid", nullable: false),
                AssessmentQuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                ResponseText = table.Column<string>(
                    type: "character varying(4000)",
                    maxLength: 4000,
                    nullable: false),
                UpdatedAtUtc = table.Column<DateTime>(
                    type: "timestamp with time zone",
                    nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AssessmentTaskResponses", x => x.Id);
                table.UniqueConstraint(
                    "AK_AssessmentTaskResponses_SchoolId_Id",
                    x => new { x.SchoolId, x.Id });
                table.ForeignKey(
                    name: "FK_AssessmentTaskResponses_AssessmentAttempts_SchoolId_AssessmentAttemptId",
                    columns: x => new { x.SchoolId, x.AssessmentAttemptId },
                    principalTable: "AssessmentAttempts",
                    principalColumns: new[] { "SchoolId", "Id" },
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_AssessmentTaskResponses_AssessmentQuestions_SchoolId_AssessmentQuestionId",
                    columns: x => new { x.SchoolId, x.AssessmentQuestionId },
                    principalTable: "AssessmentQuestions",
                    principalColumns: new[] { "SchoolId", "Id" },
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_AssessmentTaskResponses_Schools_SchoolId",
                    column: x => x.SchoolId,
                    principalTable: "Schools",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_AssessmentAttempts_SchoolId_AssessmentId_StudentProfileId",
            table: "AssessmentAttempts",
            columns: new[] { "SchoolId", "AssessmentId", "StudentProfileId" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_AssessmentAttempts_SchoolId_StudentProfileId",
            table: "AssessmentAttempts",
            columns: new[] { "SchoolId", "StudentProfileId" });

        migrationBuilder.CreateIndex(
            name: "IX_AssessmentTaskResponses_SchoolId_AssessmentQuestionId",
            table: "AssessmentTaskResponses",
            columns: new[] { "SchoolId", "AssessmentQuestionId" });

        migrationBuilder.CreateIndex(
            name: "IX_AssessmentTaskResponses_SchoolId_AssessmentAttemptId_AssessmentQuestionId",
            table: "AssessmentTaskResponses",
            columns: new[] { "SchoolId", "AssessmentAttemptId", "AssessmentQuestionId" },
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "AssessmentTaskResponses");

        migrationBuilder.DropTable(
            name: "AssessmentAttempts");

        migrationBuilder.DropColumn(
            name: "AssessmentType",
            table: "Assessments");

        migrationBuilder.DropColumn(
            name: "AvailableFromUtc",
            table: "Assessments");

        migrationBuilder.DropColumn(
            name: "DueAtUtc",
            table: "Assessments");

        migrationBuilder.DropColumn(
            name: "AttemptTimeLimitMinutes",
            table: "Assessments");
    }
}
