using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegMan.Backend.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDeleteFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_ScheduleSlots_ScheduleSlotId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Sections_SectionId",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_OfficeHourBookings_Students_StudentId",
                table: "OfficeHourBookings");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_ScheduleSlots_ScheduleSlotId",
                table: "CartItems",
                column: "ScheduleSlotId",
                principalTable: "ScheduleSlots",
                principalColumn: "ScheduleSlotId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Sections_SectionId",
                table: "Enrollments",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "SectionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfficeHourBookings_Students_StudentId",
                table: "OfficeHourBookings",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_ScheduleSlots_ScheduleSlotId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Sections_SectionId",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_OfficeHourBookings_Students_StudentId",
                table: "OfficeHourBookings");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_ScheduleSlots_ScheduleSlotId",
                table: "CartItems",
                column: "ScheduleSlotId",
                principalTable: "ScheduleSlots",
                principalColumn: "ScheduleSlotId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Sections_SectionId",
                table: "Enrollments",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "SectionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OfficeHourBookings_Students_StudentId",
                table: "OfficeHourBookings",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
