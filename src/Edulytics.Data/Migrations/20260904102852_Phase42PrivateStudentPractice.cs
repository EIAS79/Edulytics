using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edulytics.Data.Migrations
{
    /// <inheritdoc />
    public partial class Phase42PrivateStudentPractice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "PracticeAttempts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_PracticeAttempts_SchoolId_IsPrivate_StudentProfileId_Submit~",
                table: "PracticeAttempts",
                columns: new[] { "SchoolId", "IsPrivate", "StudentProfileId", "SubmittedAtUtc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PracticeAttempts_SchoolId_IsPrivate_StudentProfileId_Submit~",
                table: "PracticeAttempts");

            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "PracticeAttempts");
        }
    }
}
