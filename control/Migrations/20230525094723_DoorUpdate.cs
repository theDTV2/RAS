using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace control.Migrations
{
    /// <inheritdoc />
    public partial class DoorUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccessToken",
                table: "Door",
                newName: "PublicKeyClient");

            migrationBuilder.AddColumn<int>(
                name: "DoorStatus",
                table: "Door",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PrivateKeyServer",
                table: "Door",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoorStatus",
                table: "Door");

            migrationBuilder.DropColumn(
                name: "PrivateKeyServer",
                table: "Door");

            migrationBuilder.RenameColumn(
                name: "PublicKeyClient",
                table: "Door",
                newName: "AccessToken");
        }
    }
}
