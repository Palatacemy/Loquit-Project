using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Loquit.Data.Migrations
{
    /// <inheritdoc />
    public partial class upg01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Invite",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invite", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invite_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invite_BaseChat_ChatId",
                        column: x => x.ChatId,
                        principalTable: "BaseChat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invite_ChatId",
                table: "Invite",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Invite_UserId",
                table: "Invite",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invite");
        }
    }
}
