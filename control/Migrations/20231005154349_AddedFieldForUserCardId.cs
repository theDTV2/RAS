using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace control.Migrations
{
    /// <inheritdoc />
    public partial class AddedFieldForUserCardId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Log_User_UserName",
                table: "Log");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Log",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "CardId",
                table: "Log",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Log_User_UserName",
                table: "Log",
                column: "UserName",
                principalTable: "User",
                principalColumn: "UserName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Log_User_UserName",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "CardId",
                table: "Log");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Log",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Log_User_UserName",
                table: "Log",
                column: "UserName",
                principalTable: "User",
                principalColumn: "UserName",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
