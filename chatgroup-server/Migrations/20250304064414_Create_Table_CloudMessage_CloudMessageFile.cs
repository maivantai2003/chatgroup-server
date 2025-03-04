using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace chatgroup_server.Migrations
{
    /// <inheritdoc />
    public partial class Create_Table_CloudMessage_CloudMessageFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CloudMessages",
                columns: table => new
                {
                    CloudMessageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudMessages", x => x.CloudMessageId);
                    table.ForeignKey(
                        name: "FK_CloudMessages_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CloudMessageFiles",
                columns: table => new
                {
                    CloudMessageFileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClouMessageId = table.Column<int>(type: "int", nullable: false),
                    FileId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudMessageFiles", x => x.CloudMessageFileId);
                    table.ForeignKey(
                        name: "FK_CloudMessageFiles_CloudMessages_ClouMessageId",
                        column: x => x.ClouMessageId,
                        principalTable: "CloudMessages",
                        principalColumn: "CloudMessageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CloudMessageFiles_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "MaFile",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CloudMessageFiles_ClouMessageId",
                table: "CloudMessageFiles",
                column: "ClouMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_CloudMessageFiles_FileId",
                table: "CloudMessageFiles",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_CloudMessages_UserId",
                table: "CloudMessages",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CloudMessageFiles");

            migrationBuilder.DropTable(
                name: "CloudMessages");
        }
    }
}
