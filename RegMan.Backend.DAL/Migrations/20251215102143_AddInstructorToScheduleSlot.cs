using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegMan.Backend.DAL.Migrations
{
    public partial class AddInstructorToScheduleSlot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ============================
            // Add InstructorId to ScheduleSlots
            // ============================
            migrationBuilder.AddColumn<int>(
                name: "InstructorId",
                table: "ScheduleSlots",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleSlots_InstructorId",
                table: "ScheduleSlots",
                column: "InstructorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleSlots_Instructors_InstructorId",
                table: "ScheduleSlots",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "InstructorId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleSlots_Instructors_InstructorId",
                table: "ScheduleSlots");

            migrationBuilder.DropIndex(
                name: "IX_ScheduleSlots_InstructorId",
                table: "ScheduleSlots");

            migrationBuilder.DropColumn(
                name: "InstructorId",
                table: "ScheduleSlots");
        }
    }
}
