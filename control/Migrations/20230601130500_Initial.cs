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
                name: "Door",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    EntryStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    Registered = table.Column<bool>(type: "INTEGER", nullable: false),
                    Secret = table.Column<string>(type: "TEXT", nullable: false),
                    LastCheckInTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: false),
                    SelfRegisterAllowed = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessDoorIds = table.Column<string>(type: "TEXT", nullable: true),
                    AdminDoorIds = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Door", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Door_User_AccessDoorIds",
                        column: x => x.AccessDoorIds,
                        principalTable: "User",
                        principalColumn: "UserName");
                    table.ForeignKey(
                        name: "FK_Door_User_AdminDoorIds",
                        column: x => x.AdminDoorIds,
                        principalTable: "User",
                        principalColumn: "UserName");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Door_AccessDoorIds",
                table: "Door",
                column: "AccessDoorIds");

            migrationBuilder.CreateIndex(
                name: "IX_Door_AdminDoorIds",
                table: "Door",
                column: "AdminDoorIds");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Door");

            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "LoginLink");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
