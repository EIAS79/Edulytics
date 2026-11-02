using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edulytics.Data.Migrations
{
    /// <inheritdoc />
    public partial class Phase41OnlineAssessmentDelivery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResponseText",
                table: "StudentAnswers",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryMode",
                table: "Assessments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DifficultyBand",
                table: "Assessments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "TargetStudentProfileId",
                table: "Assessments",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TargetType",
                table: "Assessments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_SchoolId_TargetStudentProfileId",
                table: "Assessments",
                columns: new[] { "SchoolId", "TargetStudentProfileId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Assessments_SchoolId_TargetStudentProfileId",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "ResponseText",
                table: "StudentAnswers");

            migrationBuilder.DropColumn(
                name: "DeliveryMode",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "DifficultyBand",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "TargetStudentProfileId",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "TargetType",
                table: "Assessments");
        }
    }
}
