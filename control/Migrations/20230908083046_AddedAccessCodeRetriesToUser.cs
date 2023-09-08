using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace control.Migrations
{
    /// <inheritdoc />
    public partial class AddedAccessCodeRetriesToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccessCodeRetries",
                table: "User",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessCodeRetries",
                table: "User");
        }
    }
}
