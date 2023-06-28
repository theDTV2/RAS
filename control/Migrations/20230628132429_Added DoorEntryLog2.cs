using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace control.Migrations
{
    /// <inheritdoc />
    public partial class AddedDoorEntryLog2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Log",
                newName: "UserName");

            migrationBuilder.CreateIndex(
                name: "IX_Log_DoorId",
                table: "Log",
                column: "DoorId");

            migrationBuilder.CreateIndex(
                name: "IX_Log_UserName",
                table: "Log",
                column: "UserName");

            migrationBuilder.AddForeignKey(
                name: "FK_Log_Door_DoorId",
                table: "Log",
                column: "DoorId",
                principalTable: "Door",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Log_User_UserName",
                table: "Log",
                column: "UserName",
                principalTable: "User",
                principalColumn: "UserName",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Log_Door_DoorId",
                table: "Log");

            migrationBuilder.DropForeignKey(
                name: "FK_Log_User_UserName",
                table: "Log");

            migrationBuilder.DropIndex(
                name: "IX_Log_DoorId",
                table: "Log");

            migrationBuilder.DropIndex(
                name: "IX_Log_UserName",
                table: "Log");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Log",
                newName: "UserId");
        }
    }
}
