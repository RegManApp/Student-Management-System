using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegMan.Backend.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddTranscriptAndAcademicPlanFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_AcademicPlans_AcademicPlanId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Students_AcademicPlanId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Courses_AcademicPlanId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "AcademicPlanId",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "Credits",
                table: "AcademicPlans",
                newName: "TotalCreditsRequired");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AcademicPlans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AcademicPlans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExpectedYearsToComplete",
                table: "AcademicPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AcademicPlans",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AcademicPlanCourses",
                columns: table => new
                {
                    AcademicPlanCourseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AcademicPlanId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    RecommendedSemester = table.Column<int>(type: "int", nullable: false),
                    RecommendedYear = table.Column<int>(type: "int", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    CourseType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicPlanCourses", x => x.AcademicPlanCourseId);
                    table.ForeignKey(
                        name: "FK_AcademicPlanCourses_AcademicPlans_AcademicPlanId",
                        column: x => x.AcademicPlanId,
                        principalTable: "AcademicPlans",
                        principalColumn: "AcademicPlanId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AcademicPlanCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transcripts",
                columns: table => new
                {
                    TranscriptId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    SectionId = table.Column<int>(type: "int", nullable: false),
                    Grade = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    GradePoints = table.Column<double>(type: "float", nullable: false),
                    Semester = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    CreditHours = table.Column<int>(type: "int", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transcripts", x => x.TranscriptId);
                    table.ForeignKey(
                        name: "FK_Transcripts_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transcripts_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "SectionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transcripts_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_AcademicPlanId",
                table: "Students",
                column: "AcademicPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicPlanCourses_AcademicPlanId_CourseId",
                table: "AcademicPlanCourses",
                columns: new[] { "AcademicPlanId", "CourseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AcademicPlanCourses_CourseId",
                table: "AcademicPlanCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Transcripts_CourseId",
                table: "Transcripts",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Transcripts_SectionId",
                table: "Transcripts",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Transcripts_StudentId_CourseId_SectionId",
                table: "Transcripts",
                columns: new[] { "StudentId", "CourseId", "SectionId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcademicPlanCourses");

            migrationBuilder.DropTable(
                name: "Transcripts");

            migrationBuilder.DropIndex(
                name: "IX_Students_AcademicPlanId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AcademicPlans");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AcademicPlans");

            migrationBuilder.DropColumn(
                name: "ExpectedYearsToComplete",
                table: "AcademicPlans");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AcademicPlans");

            migrationBuilder.RenameColumn(
                name: "TotalCreditsRequired",
                table: "AcademicPlans",
                newName: "Credits");

            migrationBuilder.AddColumn<string>(
                name: "AcademicPlanId",
                table: "Courses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_AcademicPlanId",
                table: "Students",
                column: "AcademicPlanId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_AcademicPlanId",
                table: "Courses",
                column: "AcademicPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_AcademicPlans_AcademicPlanId",
                table: "Courses",
                column: "AcademicPlanId",
                principalTable: "AcademicPlans",
                principalColumn: "AcademicPlanId");
        }
    }
}
