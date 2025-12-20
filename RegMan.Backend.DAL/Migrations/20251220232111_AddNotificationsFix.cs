using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegMan.Backend.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationsFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfficeHours_Rooms_RoomId",
                table: "OfficeHours");

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "OfficeHours",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_OfficeHours_Rooms_RoomId",
                table: "OfficeHours",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfficeHours_Rooms_RoomId",
                table: "OfficeHours");

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "OfficeHours",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OfficeHours_Rooms_RoomId",
                table: "OfficeHours",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
