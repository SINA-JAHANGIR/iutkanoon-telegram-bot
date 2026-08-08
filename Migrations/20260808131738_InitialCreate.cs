using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IutKanoon.TelegramBot.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserSessions",
                columns: table => new
                {
                    TelegramId = table.Column<long>(type: "INTEGER", nullable: false),
                    WpUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserEmail = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    UserRole = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    IsAuthenticated = table.Column<bool>(type: "INTEGER", nullable: false),
                    LinkedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSessions", x => x.TelegramId);
                });

            migrationBuilder.CreateTable(
                name: "UserStates",
                columns: table => new
                {
                    TelegramId = table.Column<long>(type: "INTEGER", nullable: false),
                    CurrentState = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    TempData = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStates", x => x.TelegramId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSessions");

            migrationBuilder.DropTable(
                name: "UserStates");
        }
    }
}
