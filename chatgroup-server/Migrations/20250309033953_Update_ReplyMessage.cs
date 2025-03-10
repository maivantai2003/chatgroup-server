using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace chatgroup_server.Migrations
{
    /// <inheritdoc />
    public partial class Update_ReplyMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReplyToMessageId",
                table: "UserMessages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserMessages_ReplyToMessageId",
                table: "UserMessages",
                column: "ReplyToMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMessages_UserMessages_ReplyToMessageId",
                table: "UserMessages",
                column: "ReplyToMessageId",
                principalTable: "UserMessages",
                principalColumn: "UserMessageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMessages_UserMessages_ReplyToMessageId",
                table: "UserMessages");

            migrationBuilder.DropIndex(
                name: "IX_UserMessages_ReplyToMessageId",
                table: "UserMessages");

            migrationBuilder.DropColumn(
                name: "ReplyToMessageId",
                table: "UserMessages");
        }
    }
}
