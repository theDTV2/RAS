using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace control.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Door",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    EntryStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    Registered = table.Column<bool>(type: "INTEGER", nullable: false),
                    Secret = table.Column<string>(type: "TEXT", nullable: false),
                    LastCheckInTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: false),
                    SelfRegisterAllowed = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Door", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoginLink",
                columns: table => new
                {
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", nullable: false),
                    GenerationTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginLink", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserName = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    AccessLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AccessCode = table.Column<string>(type: "TEXT", nullable: false),
                    AccessCodeGenerationTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AcceptedEula = table.Column<bool>(type: "INTEGER", nullable: false),
                    CompletedRegistration = table.Column<bool>(type: "INTEGER", nullable: false),
                    SecretCode = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserName);
                });

            migrationBuilder.CreateTable(
                name: "DoorUser",
                columns: table => new
                {
                    AccessDoorsId = table.Column<string>(type: "TEXT", nullable: false),
                    AccessUsers = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoorUser", x => new { x.AccessDoorsId, x.AccessUsers });
                    table.ForeignKey(
                        name: "FK_DoorUser_Door_AccessDoorsId",
                        column: x => x.AccessDoorsId,
                        principalTable: "Door",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoorUser_User_AccessUsers",
                        column: x => x.AccessUsers,
                        principalTable: "User",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoorUser1",
                columns: table => new
                {
                    AdminDoorsId = table.Column<string>(type: "TEXT", nullable: false),
                    AdminUsers = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoorUser1", x => new { x.AdminDoorsId, x.AdminUsers });
                    table.ForeignKey(
                        name: "FK_DoorUser1_Door_AdminDoorsId",
                        column: x => x.AdminDoorsId,
                        principalTable: "Door",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoorUser1_User_AdminUsers",
                        column: x => x.AdminUsers,
                        principalTable: "User",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoorUser_AccessUsers",
                table: "DoorUser",
                column: "AccessUsers");

            migrationBuilder.CreateIndex(
                name: "IX_DoorUser1_AdminUsers",
                table: "DoorUser1",
                column: "AdminUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoorUser");

            migrationBuilder.DropTable(
                name: "DoorUser1");

            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "LoginLink");

            migrationBuilder.DropTable(
                name: "Door");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
