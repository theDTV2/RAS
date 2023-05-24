using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace control.Migrations
{
    /// <inheritdoc />
    public partial class AddedSecretCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SecretCode",
                table: "User",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecretCode",
                table: "User");
        }
    }
}
