using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace control.Migrations
{
    /// <inheritdoc />
    public partial class Update2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DoorRegistered",
                table: "Door",
                newName: "Registered");

            migrationBuilder.AddColumn<string>(
                name: "Secret",
                table: "Door",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Secret",
                table: "Door");

            migrationBuilder.RenameColumn(
                name: "Registered",
                table: "Door",
                newName: "DoorRegistered");
        }
    }
}
