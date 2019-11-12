using Microsoft.EntityFrameworkCore.Migrations;

namespace Lavoro.Data.Migrations
{
    public partial class unread : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Unread",
                table: "Chats",
                newName: "IdLastMessageSeenByStarterUser");

            migrationBuilder.AddColumn<int>(
                name: "IdLastMessageSeenByOtherUser",
                table: "Chats",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Chats_IdLastMessageSeenByOtherUser",
                table: "Chats",
                column: "IdLastMessageSeenByOtherUser",
                unique: true,
                filter: "[IdLastMessageSeenByOtherUser] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_IdLastMessageSeenByStarterUser",
                table: "Chats",
                column: "IdLastMessageSeenByStarterUser",
                unique: true,
                filter: "[IdLastMessageSeenByStarterUser] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Messages_IdLastMessageSeenByOtherUser",
                table: "Chats",
                column: "IdLastMessageSeenByOtherUser",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Messages_IdLastMessageSeenByStarterUser",
                table: "Chats",
                column: "IdLastMessageSeenByStarterUser",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Messages_IdLastMessageSeenByOtherUser",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Messages_IdLastMessageSeenByStarterUser",
                table: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Chats_IdLastMessageSeenByOtherUser",
                table: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Chats_IdLastMessageSeenByStarterUser",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "IdLastMessageSeenByOtherUser",
                table: "Chats");

            migrationBuilder.RenameColumn(
                name: "IdLastMessageSeenByStarterUser",
                table: "Chats",
                newName: "Unread");
        }
    }
}
