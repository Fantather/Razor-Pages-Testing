using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Razor_Pages_Testing.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "UserTasks",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UserTasks_OwnerId",
                table: "UserTasks",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTasks_AspNetUsers_OwnerId",
                table: "UserTasks",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTasks_AspNetUsers_OwnerId",
                table: "UserTasks");

            migrationBuilder.DropIndex(
                name: "IX_UserTasks_OwnerId",
                table: "UserTasks");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "UserTasks");
        }
    }
}
