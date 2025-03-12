using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace chatgroup_server.Migrations
{
    /// <inheritdoc />
    public partial class Create_Index : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserMessages_CreateAt",
                table: "UserMessages",
                column: "CreateAt");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessages_CreateAt",
                table: "GroupMessages",
                column: "CreateAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserMessages_CreateAt",
                table: "UserMessages");

            migrationBuilder.DropIndex(
                name: "IX_GroupMessages_CreateAt",
                table: "GroupMessages");
        }
    }
}
