using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edulytics.Data.Migrations
{
    /// <inheritdoc />
    public partial class Phase29ExplicitCurriculumLevelIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SchoolCurriculumAdoptions_SchoolId_AcademicYearId_Academic~1",
                table: "SchoolCurriculumAdoptions");

            migrationBuilder.DropIndex(
                name: "IX_SchoolCurriculumAdoptions_SchoolId_AcademicYearId_AcademicP~",
                table: "SchoolCurriculumAdoptions");

            migrationBuilder.DropIndex(
                name: "IX_LearningOutcomes_SchoolId_AcademicProgramId_FrameworkVersio~",
                table: "LearningOutcomes");

            migrationBuilder.DropIndex(
                name: "IX_CurriculumTopics_SchoolId_AcademicProgramId_FrameworkVersi~1",
                table: "CurriculumTopics");

            migrationBuilder.DropIndex(
                name: "IX_CurriculumTopics_SchoolId_AcademicProgramId_FrameworkVersio~",
                table: "CurriculumTopics");

            migrationBuilder.RenameIndex(
                name: "IX_LearningOutcomes_SchoolId_AcademicProgramId_FrameworkVersi~1",
                table: "LearningOutcomes",
                newName: "IX_LearningOutcomes_SchoolId_AcademicProgramId_FrameworkVersio~");

            migrationBuilder.AddColumn<string>(
                name: "CurriculumLevelKey",
                table: "SchoolCurriculumAdoptions",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurriculumLevelLabel",
                table: "SchoolCurriculumAdoptions",
                type: "character varying(240)",
                maxLength: 240,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurriculumLogicalLevel",
                table: "SchoolCurriculumAdoptions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurriculumPathway",
                table: "SchoolCurriculumAdoptions",
                type: "character varying(240)",
                maxLength: 240,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurriculumStage",
                table: "SchoolCurriculumAdoptions",
                type: "character varying(160)",
                maxLength: 160,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CurriculumAdoptionId",
                table: "LearningOutcomes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CurriculumAdoptionId",
                table: "CurriculumTopics",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CurriculumAdoptionId",
                table: "ClassGroups",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "ClassGroups",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_SchoolCurriculumAdoptions_SchoolId_Id",
                table: "SchoolCurriculumAdoptions",
                columns: new[] { "SchoolId", "Id" });

            migrationBuilder.CreateIndex(
                name: "UX_CurriculumAdoption_ExplicitLevel",
                table: "SchoolCurriculumAdoptions",
                columns: new[] { "SchoolId", "AcademicYearId", "AcademicProgramId", "CurriculumLevelKey", "SubjectId", "FrameworkVersionId" },
                unique: true,
                filter: "\"CurriculumLevelKey\" IS NOT NULL")
                .Annotation("Npgsql:NullsDistinct", false);

            migrationBuilder.CreateIndex(
                name: "UX_CurriculumAdoption_LegacyScope",
                table: "SchoolCurriculumAdoptions",
                columns: new[] { "SchoolId", "AcademicYearId", "AcademicProgramId", "GradeLevelId", "SubjectId", "FrameworkVersionId" },
                unique: true,
                filter: "\"CurriculumLevelKey\" IS NULL")
                .Annotation("Npgsql:NullsDistinct", false);

            migrationBuilder.CreateIndex(
                name: "UX_CurriculumAdoption_PrimaryExplicitLevel",
                table: "SchoolCurriculumAdoptions",
                columns: new[] { "SchoolId", "AcademicYearId", "AcademicProgramId", "CurriculumLevelKey", "SubjectId" },
                unique: true,
                filter: "\"IsPrimary\" = TRUE AND \"CurriculumLevelKey\" IS NOT NULL")
                .Annotation("Npgsql:NullsDistinct", false);

            migrationBuilder.CreateIndex(
                name: "UX_CurriculumAdoption_PrimaryLegacyScope",
                table: "SchoolCurriculumAdoptions",
                columns: new[] { "SchoolId", "AcademicYearId", "AcademicProgramId", "GradeLevelId", "SubjectId" },
                unique: true,
                filter: "\"IsPrimary\" = TRUE AND \"CurriculumLevelKey\" IS NULL")
                .Annotation("Npgsql:NullsDistinct", false);

            migrationBuilder.AddCheckConstraint(
                name: "CK_CurriculumAdoption_LogicalLevel",
                table: "SchoolCurriculumAdoptions",
                sql: "\"CurriculumLogicalLevel\" IS NULL OR (\"CurriculumLogicalLevel\" BETWEEN 1 AND 13)");

            migrationBuilder.CreateIndex(
                name: "UX_LearningOutcome_Adoption_Code",
                table: "LearningOutcomes",
                columns: new[] { "SchoolId", "CurriculumAdoptionId", "Code" },
                unique: true,
                filter: "\"CurriculumAdoptionId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UX_LearningOutcome_Legacy_Code",
                table: "LearningOutcomes",
                columns: new[] { "SchoolId", "AcademicProgramId", "FrameworkVersionId", "SubjectId", "GradeLevelId", "Code" },
                unique: true,
                filter: "\"CurriculumAdoptionId\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "UX_CurriculumTopic_Adoption_Name",
                table: "CurriculumTopics",
                columns: new[] { "SchoolId", "CurriculumAdoptionId", "Name" },
                unique: true,
                filter: "\"CurriculumAdoptionId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UX_CurriculumTopic_Adoption_Order",
                table: "CurriculumTopics",
                columns: new[] { "SchoolId", "CurriculumAdoptionId", "Order" },
                unique: true,
                filter: "\"CurriculumAdoptionId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UX_CurriculumTopic_Legacy_Name",
                table: "CurriculumTopics",
                columns: new[] { "SchoolId", "AcademicProgramId", "FrameworkVersionId", "SubjectId", "GradeLevelId", "Name" },
                unique: true,
                filter: "\"CurriculumAdoptionId\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "UX_CurriculumTopic_Legacy_Order",
                table: "CurriculumTopics",
                columns: new[] { "SchoolId", "AcademicProgramId", "FrameworkVersionId", "SubjectId", "GradeLevelId", "Order" },
                unique: true,
                filter: "\"CurriculumAdoptionId\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ClassGroups_SchoolId_CurriculumAdoptionId",
                table: "ClassGroups",
                columns: new[] { "SchoolId", "CurriculumAdoptionId" });

            migrationBuilder.CreateIndex(
                name: "UX_ClassGroup_CurriculumAdoption_Name",
                table: "ClassGroups",
                columns: new[] { "SchoolId", "AcademicYearId", "CurriculumAdoptionId", "NormalizedName" },
                unique: true,
                filter: "\"CurriculumAdoptionId\" IS NOT NULL AND \"NormalizedName\" IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassGroups_SchoolCurriculumAdoptions_SchoolId_CurriculumAd~",
                table: "ClassGroups",
                columns: new[] { "SchoolId", "CurriculumAdoptionId" },
                principalTable: "SchoolCurriculumAdoptions",
                principalColumns: new[] { "SchoolId", "Id" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CurriculumTopics_SchoolCurriculumAdoptions_SchoolId_Curricu~",
                table: "CurriculumTopics",
                columns: new[] { "SchoolId", "CurriculumAdoptionId" },
                principalTable: "SchoolCurriculumAdoptions",
                principalColumns: new[] { "SchoolId", "Id" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LearningOutcomes_SchoolCurriculumAdoptions_SchoolId_Curricu~",
                table: "LearningOutcomes",
                columns: new[] { "SchoolId", "CurriculumAdoptionId" },
                principalTable: "SchoolCurriculumAdoptions",
                principalColumns: new[] { "SchoolId", "Id" },
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassGroups_SchoolCurriculumAdoptions_SchoolId_CurriculumAd~",
                table: "ClassGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_CurriculumTopics_SchoolCurriculumAdoptions_SchoolId_Curricu~",
                table: "CurriculumTopics");

            migrationBuilder.DropForeignKey(
                name: "FK_LearningOutcomes_SchoolCurriculumAdoptions_SchoolId_Curricu~",
                table: "LearningOutcomes");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_SchoolCurriculumAdoptions_SchoolId_Id",
                table: "SchoolCurriculumAdoptions");

            migrationBuilder.DropIndex(
                name: "UX_CurriculumAdoption_ExplicitLevel",
                table: "SchoolCurriculumAdoptions");

            migrationBuilder.DropIndex(
                name: "UX_CurriculumAdoption_LegacyScope",
                table: "SchoolCurriculumAdoptions");

            migrationBuilder.DropIndex(
                name: "UX_CurriculumAdoption_PrimaryExplicitLevel",
                table: "SchoolCurriculumAdoptions");

            migrationBuilder.DropIndex(
                name: "UX_CurriculumAdoption_PrimaryLegacyScope",
                table: "SchoolCurriculumAdoptions");

            migrationBuilder.DropCheckConstraint(
                name: "CK_CurriculumAdoption_LogicalLevel",
                table: "SchoolCurriculumAdoptions");

            migrationBuilder.DropIndex(
                name: "UX_LearningOutcome_Adoption_Code",
                table: "LearningOutcomes");

            migrationBuilder.DropIndex(
                name: "UX_LearningOutcome_Legacy_Code",
                table: "LearningOutcomes");

            migrationBuilder.DropIndex(
                name: "UX_CurriculumTopic_Adoption_Name",
                table: "CurriculumTopics");

            migrationBuilder.DropIndex(
                name: "UX_CurriculumTopic_Adoption_Order",
                table: "CurriculumTopics");

            migrationBuilder.DropIndex(
                name: "UX_CurriculumTopic_Legacy_Name",
                table: "CurriculumTopics");

            migrationBuilder.DropIndex(
                name: "UX_CurriculumTopic_Legacy_Order",
                table: "CurriculumTopics");

            migrationBuilder.DropIndex(
                name: "IX_ClassGroups_SchoolId_CurriculumAdoptionId",
                table: "ClassGroups");

            migrationBuilder.DropIndex(
                name: "UX_ClassGroup_CurriculumAdoption_Name",
                table: "ClassGroups");

            migrationBuilder.DropColumn(
                name: "CurriculumLevelKey",
                table: "SchoolCurriculumAdoptions");

            migrationBuilder.DropColumn(
                name: "CurriculumLevelLabel",
                table: "SchoolCurriculumAdoptions");

            migrationBuilder.DropColumn(
                name: "CurriculumLogicalLevel",
                table: "SchoolCurriculumAdoptions");

            migrationBuilder.DropColumn(
                name: "CurriculumPathway",
                table: "SchoolCurriculumAdoptions");

            migrationBuilder.DropColumn(
                name: "CurriculumStage",
                table: "SchoolCurriculumAdoptions");

            migrationBuilder.DropColumn(
                name: "CurriculumAdoptionId",
                table: "LearningOutcomes");

            migrationBuilder.DropColumn(
                name: "CurriculumAdoptionId",
                table: "CurriculumTopics");

            migrationBuilder.DropColumn(
                name: "CurriculumAdoptionId",
                table: "ClassGroups");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "ClassGroups");

            migrationBuilder.RenameIndex(
                name: "IX_LearningOutcomes_SchoolId_AcademicProgramId_FrameworkVersio~",
                table: "LearningOutcomes",
                newName: "IX_LearningOutcomes_SchoolId_AcademicProgramId_FrameworkVersi~1");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolCurriculumAdoptions_SchoolId_AcademicYearId_Academic~1",
                table: "SchoolCurriculumAdoptions",
                columns: new[] { "SchoolId", "AcademicYearId", "AcademicProgramId", "GradeLevelId", "SubjectId", "FrameworkVersionId" },
                unique: true)
                .Annotation("Npgsql:NullsDistinct", false);

            migrationBuilder.CreateIndex(
                name: "IX_SchoolCurriculumAdoptions_SchoolId_AcademicYearId_AcademicP~",
                table: "SchoolCurriculumAdoptions",
                columns: new[] { "SchoolId", "AcademicYearId", "AcademicProgramId", "GradeLevelId", "SubjectId" },
                unique: true,
                filter: "\"IsPrimary\" = TRUE")
                .Annotation("Npgsql:NullsDistinct", false);

            migrationBuilder.CreateIndex(
                name: "IX_LearningOutcomes_SchoolId_AcademicProgramId_FrameworkVersio~",
                table: "LearningOutcomes",
                columns: new[] { "SchoolId", "AcademicProgramId", "FrameworkVersionId", "SubjectId", "GradeLevelId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CurriculumTopics_SchoolId_AcademicProgramId_FrameworkVersi~1",
                table: "CurriculumTopics",
                columns: new[] { "SchoolId", "AcademicProgramId", "FrameworkVersionId", "SubjectId", "GradeLevelId", "Order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CurriculumTopics_SchoolId_AcademicProgramId_FrameworkVersio~",
                table: "CurriculumTopics",
                columns: new[] { "SchoolId", "AcademicProgramId", "FrameworkVersionId", "SubjectId", "GradeLevelId", "Name" },
                unique: true);
        }
    }
}
