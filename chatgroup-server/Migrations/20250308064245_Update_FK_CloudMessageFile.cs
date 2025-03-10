using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace chatgroup_server.Migrations
{
    /// <inheritdoc />
    public partial class Update_FK_CloudMessageFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CloudMessageFiles_CloudMessages_ClouMessageId",
                table: "CloudMessageFiles");

            migrationBuilder.RenameColumn(
                name: "ClouMessageId",
                table: "CloudMessageFiles",
                newName: "CloudMessageId");

            migrationBuilder.RenameIndex(
                name: "IX_CloudMessageFiles_ClouMessageId",
                table: "CloudMessageFiles",
                newName: "IX_CloudMessageFiles_CloudMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_CloudMessageFiles_CloudMessages_CloudMessageId",
                table: "CloudMessageFiles",
                column: "CloudMessageId",
                principalTable: "CloudMessages",
                principalColumn: "CloudMessageId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CloudMessageFiles_CloudMessages_CloudMessageId",
                table: "CloudMessageFiles");

            migrationBuilder.RenameColumn(
                name: "CloudMessageId",
                table: "CloudMessageFiles",
                newName: "ClouMessageId");

            migrationBuilder.RenameIndex(
                name: "IX_CloudMessageFiles_CloudMessageId",
                table: "CloudMessageFiles",
                newName: "IX_CloudMessageFiles_ClouMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_CloudMessageFiles_CloudMessages_ClouMessageId",
                table: "CloudMessageFiles",
                column: "ClouMessageId",
                principalTable: "CloudMessages",
                principalColumn: "CloudMessageId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
