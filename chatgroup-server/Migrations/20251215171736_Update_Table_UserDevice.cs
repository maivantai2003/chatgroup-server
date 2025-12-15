using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace chatgroup_server.Migrations
{
    /// <inheritdoc />
    public partial class Update_Table_UserDevice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserDevices_UserId",
                table: "UserDevices");

            migrationBuilder.AddColumn<string>(
                name: "DeviceId",
                table: "UserDevices",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDevices_UserId_DeviceId",
                table: "UserDevices",
                columns: new[] { "UserId", "DeviceId" },
                unique: true,
                filter: "[DeviceId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserDevices_UserId_DeviceId",
                table: "UserDevices");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "UserDevices");

            migrationBuilder.CreateIndex(
                name: "IX_UserDevices_UserId",
                table: "UserDevices",
                column: "UserId");
        }
    }
}
