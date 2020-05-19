using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartLibrary.DAL.Migrations
{
    public partial class Addedreservedbookstablev2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservedBook_AvailableBook_BookId",
                table: "ReservedBook");

            migrationBuilder.DropIndex(
                name: "IX_ReservedBook_BookId",
                table: "ReservedBook");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "ReservedBook");

            migrationBuilder.AddColumn<int>(
                name: "AvailableBookId",
                table: "ReservedBook",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ReservedBook",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_ReservedBook_AvailableBookId",
                table: "ReservedBook",
                column: "AvailableBookId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservedBook_AvailableBook_AvailableBookId",
                table: "ReservedBook",
                column: "AvailableBookId",
                principalTable: "AvailableBook",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservedBook_AvailableBook_AvailableBookId",
                table: "ReservedBook");

            migrationBuilder.DropIndex(
                name: "IX_ReservedBook_AvailableBookId",
                table: "ReservedBook");

            migrationBuilder.DropColumn(
                name: "AvailableBookId",
                table: "ReservedBook");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ReservedBook");

            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "ReservedBook",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ReservedBook_BookId",
                table: "ReservedBook",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservedBook_AvailableBook_BookId",
                table: "ReservedBook",
                column: "BookId",
                principalTable: "AvailableBook",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
