using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace control.Migrations
{
    /// <inheritdoc />
    public partial class AddedDoorEntryLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Log");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Log",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Log",
                newName: "EntryTime");

            migrationBuilder.AddColumn<string>(
                name: "DoorId",
                table: "Log",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoorId",
                table: "Log");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Log",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "EntryTime",
                table: "Log",
                newName: "Description");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Log",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
