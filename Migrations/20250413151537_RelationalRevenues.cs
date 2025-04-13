using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expenses.Migrations
{
    /// <inheritdoc />
    public partial class RelationalRevenues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "AmountRevenue",
                table: "Revenues",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Revenues",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Revenues_UserId",
                table: "Revenues",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Revenues_Users_UserId",
                table: "Revenues",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Revenues_Users_UserId",
                table: "Revenues");

            migrationBuilder.DropIndex(
                name: "IX_Revenues_UserId",
                table: "Revenues");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Revenues");

            migrationBuilder.AlterColumn<float>(
                name: "AmountRevenue",
                table: "Revenues",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");
        }
    }
}
