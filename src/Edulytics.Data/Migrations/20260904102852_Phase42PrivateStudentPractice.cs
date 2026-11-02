using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edulytics.Data.Migrations
{
    /// <inheritdoc />
    /// <remarks>
    /// EF-generated migration behavior is verified by the real PostgreSQL migration/model CI gate.
    /// Excluding the generated partial type keeps line coverage focused on executable product logic.
    /// </remarks>
    [ExcludeFromCodeCoverage]
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
