using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JacksFitnessApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCoachEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoachConversations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoachConversations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoachConversations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoachMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConversationId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoachMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoachMessages_CoachConversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "CoachConversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoachConversations_UserId",
                table: "CoachConversations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CoachMessages_ConversationId",
                table: "CoachMessages",
                column: "ConversationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoachMessages");

            migrationBuilder.DropTable(
                name: "CoachConversations");
        }
    }
}
