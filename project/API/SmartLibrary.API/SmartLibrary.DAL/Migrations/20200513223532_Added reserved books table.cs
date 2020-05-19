using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.DAL.Migrations
{
    public partial class Addedreservedbookstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReservedBook",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    BookId = table.Column<int>(nullable: false),
                    ReservedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ReturnedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservedBook", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservedBook_AvailableBook_BookId",
                        column: x => x.BookId,
                        principalTable: "AvailableBook",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservedBook_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservedBook_BookId",
                table: "ReservedBook",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservedBook_UserId",
                table: "ReservedBook",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservedBook");
        }
    }
}
