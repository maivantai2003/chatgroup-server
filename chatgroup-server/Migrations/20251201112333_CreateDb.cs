using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace chatgroup_server.Migrations
{
    /// <inheritdoc />
    public partial class CreateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    MaFile = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DuongDan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoaiFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KichThuocFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.MaFile);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.GroupId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sex = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoverPhoto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsOnline = table.Column<bool>(type: "bit", nullable: false),
                    FirstLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

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
                name: "Conversations",
                columns: table => new
                {
                    ConversationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserSend = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConversationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastMessage = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversations", x => x.ConversationId);
                    table.ForeignKey(
                        name: "FK_Conversations_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Friends",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FriendId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friends", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Friends_User_FriendId",
                        column: x => x.FriendId,
                        principalTable: "User",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Friends_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "GroupDetails",
                columns: table => new
                {
                    GroupDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupDetails", x => x.GroupDetailId);
                    table.ForeignKey(
                        name: "FK_GroupDetails_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupDetails_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupMessages",
                columns: table => new
                {
                    GroupedMessageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    ReplyToMessageId = table.Column<int>(type: "int", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMessages", x => x.GroupedMessageId);
                    table.ForeignKey(
                        name: "FK_GroupMessages_GroupMessages_ReplyToMessageId",
                        column: x => x.ReplyToMessageId,
                        principalTable: "GroupMessages",
                        principalColumn: "GroupedMessageId");
                    table.ForeignKey(
                        name: "FK_GroupMessages_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMessages_User_SenderId",
                        column: x => x.SenderId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDevices",
                columns: table => new
                {
                    UserDeviceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DeviceToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsOnline = table.Column<bool>(type: "bit", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastActiveAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Browser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDevices", x => x.UserDeviceId);
                    table.ForeignKey(
                        name: "FK_UserDevices_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserMessages",
                columns: table => new
                {
                    UserMessageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    ReceiverId = table.Column<int>(type: "int", nullable: false),
                    ReplyToMessageId = table.Column<int>(type: "int", nullable: true),
                    MessageType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMessages", x => x.UserMessageId);
                    table.ForeignKey(
                        name: "FK_UserMessages_UserMessages_ReplyToMessageId",
                        column: x => x.ReplyToMessageId,
                        principalTable: "UserMessages",
                        principalColumn: "UserMessageId");
                    table.ForeignKey(
                        name: "FK_UserMessages_User_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserMessages_User_SenderId",
                        column: x => x.SenderId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRefreshTokens",
                columns: table => new
                {
                    RefreshTokenId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsInvalidades = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRefreshTokens", x => x.RefreshTokenId);
                    table.ForeignKey(
                        name: "FK_UserRefreshTokens_User_UserId",
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
                    CloudMessageId = table.Column<int>(type: "int", nullable: false),
                    FileId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloudMessageFiles", x => x.CloudMessageFileId);
                    table.ForeignKey(
                        name: "FK_CloudMessageFiles_CloudMessages_CloudMessageId",
                        column: x => x.CloudMessageId,
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

            migrationBuilder.CreateTable(
                name: "GroupMessageFiles",
                columns: table => new
                {
                    GroupMessageFileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileId = table.Column<int>(type: "int", nullable: false),
                    GroupedMessageId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMessageFiles", x => x.GroupMessageFileId);
                    table.ForeignKey(
                        name: "FK_GroupMessageFiles_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "MaFile",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMessageFiles_GroupMessages_GroupedMessageId",
                        column: x => x.GroupedMessageId,
                        principalTable: "GroupMessages",
                        principalColumn: "GroupedMessageId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GroupMessageReactions",
                columns: table => new
                {
                    ReactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GroupedMessageId = table.Column<int>(type: "int", nullable: false),
                    ReactionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReactionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReactionCount = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMessageReactions", x => x.ReactionId);
                    table.ForeignKey(
                        name: "FK_GroupMessageReactions_GroupMessages_GroupedMessageId",
                        column: x => x.GroupedMessageId,
                        principalTable: "GroupMessages",
                        principalColumn: "GroupedMessageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMessageReactions_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "GroupMessageStatuses",
                columns: table => new
                {
                    GroupMessageStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupMessageId = table.Column<int>(type: "int", nullable: false),
                    ReceiverId = table.Column<int>(type: "int", nullable: false),
                    IsReceived = table.Column<bool>(type: "bit", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    ReceivedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMessageStatuses", x => x.GroupMessageStatusId);
                    table.ForeignKey(
                        name: "FK_GroupMessageStatuses_GroupMessages_GroupMessageId",
                        column: x => x.GroupMessageId,
                        principalTable: "GroupMessages",
                        principalColumn: "GroupedMessageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMessageStatuses_User_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "UserMessageFiles",
                columns: table => new
                {
                    UserMessageFileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileId = table.Column<int>(type: "int", nullable: false),
                    UserMessageId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMessageFiles", x => x.UserMessageFileId);
                    table.ForeignKey(
                        name: "FK_UserMessageFiles_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "MaFile",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserMessageFiles_UserMessages_UserMessageId",
                        column: x => x.UserMessageId,
                        principalTable: "UserMessages",
                        principalColumn: "UserMessageId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserMessageReactions",
                columns: table => new
                {
                    ReactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserMessageId = table.Column<int>(type: "int", nullable: false),
                    ReactionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReactionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReactionCount = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMessageReactions", x => x.ReactionId);
                    table.ForeignKey(
                        name: "FK_UserMessageReactions_UserMessages_UserMessageId",
                        column: x => x.UserMessageId,
                        principalTable: "UserMessages",
                        principalColumn: "UserMessageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserMessageReactions_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "UserMessageStatuses",
                columns: table => new
                {
                    UserMessageStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserMessageId = table.Column<int>(type: "int", nullable: false),
                    ReceiverId = table.Column<int>(type: "int", nullable: false),
                    IsReceived = table.Column<bool>(type: "bit", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    ReceivedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMessageStatuses", x => x.UserMessageStatusId);
                    table.ForeignKey(
                        name: "FK_UserMessageStatuses_UserMessages_UserMessageId",
                        column: x => x.UserMessageId,
                        principalTable: "UserMessages",
                        principalColumn: "UserMessageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserMessageStatuses_User_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CloudMessageFiles_CloudMessageId",
                table: "CloudMessageFiles",
                column: "CloudMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_CloudMessageFiles_FileId",
                table: "CloudMessageFiles",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_CloudMessages_UserId",
                table: "CloudMessages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_UserId",
                table: "Conversations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Friends_FriendId",
                table: "Friends",
                column: "FriendId");

            migrationBuilder.CreateIndex(
                name: "IX_Friends_UserId",
                table: "Friends",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupDetails_GroupId",
                table: "GroupDetails",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupDetails_UserId",
                table: "GroupDetails",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessageFiles_FileId",
                table: "GroupMessageFiles",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessageFiles_GroupedMessageId",
                table: "GroupMessageFiles",
                column: "GroupedMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessageReactions_GroupedMessageId",
                table: "GroupMessageReactions",
                column: "GroupedMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessageReactions_UserId",
                table: "GroupMessageReactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessages_CreateAt",
                table: "GroupMessages",
                column: "CreateAt");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessages_GroupId",
                table: "GroupMessages",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessages_ReplyToMessageId",
                table: "GroupMessages",
                column: "ReplyToMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessages_SenderId",
                table: "GroupMessages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessageStatuses_GroupMessageId",
                table: "GroupMessageStatuses",
                column: "GroupMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessageStatuses_ReceiverId",
                table: "GroupMessageStatuses",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_User_BirthDay",
                table: "User",
                column: "Birthday");

            migrationBuilder.CreateIndex(
                name: "IX_User_PhoneNumber",
                table: "User",
                column: "PhoneNumber",
                unique: true,
                filter: "[PhoneNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserDevices_UserId",
                table: "UserDevices",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMessageFiles_FileId",
                table: "UserMessageFiles",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMessageFiles_UserMessageId",
                table: "UserMessageFiles",
                column: "UserMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMessageReactions_UserId",
                table: "UserMessageReactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMessageReactions_UserMessageId",
                table: "UserMessageReactions",
                column: "UserMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMessages_CreateAt",
                table: "UserMessages",
                column: "CreateAt");

            migrationBuilder.CreateIndex(
                name: "IX_UserMessages_ReceiverId",
                table: "UserMessages",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMessages_ReplyToMessageId",
                table: "UserMessages",
                column: "ReplyToMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMessages_SenderId",
                table: "UserMessages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMessageStatuses_ReceiverId",
                table: "UserMessageStatuses",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMessageStatuses_UserMessageId",
                table: "UserMessageStatuses",
                column: "UserMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRefreshTokens_UserId",
                table: "UserRefreshTokens",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CloudMessageFiles");

            migrationBuilder.DropTable(
                name: "Conversations");

            migrationBuilder.DropTable(
                name: "Friends");

            migrationBuilder.DropTable(
                name: "GroupDetails");

            migrationBuilder.DropTable(
                name: "GroupMessageFiles");

            migrationBuilder.DropTable(
                name: "GroupMessageReactions");

            migrationBuilder.DropTable(
                name: "GroupMessageStatuses");

            migrationBuilder.DropTable(
                name: "UserDevices");

            migrationBuilder.DropTable(
                name: "UserMessageFiles");

            migrationBuilder.DropTable(
                name: "UserMessageReactions");

            migrationBuilder.DropTable(
                name: "UserMessageStatuses");

            migrationBuilder.DropTable(
                name: "UserRefreshTokens");

            migrationBuilder.DropTable(
                name: "CloudMessages");

            migrationBuilder.DropTable(
                name: "GroupMessages");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "UserMessages");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
