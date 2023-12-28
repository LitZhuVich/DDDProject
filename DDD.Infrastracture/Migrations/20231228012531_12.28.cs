using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _1228 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_UserAccessFails_T_Users_UserId",
                table: "T_UserAccessFails");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "T_Users");

            migrationBuilder.RenameColumn(
                name: "Messsage",
                table: "T_UserLoginHistories",
                newName: "Message");

            migrationBuilder.RenameColumn(
                name: "isLockOut",
                table: "T_UserAccessFails",
                newName: "IsLockOut");

            migrationBuilder.RenameIndex(
                name: "IX_T_UserAccessFails_UserId",
                table: "T_UserAccessFails",
                newName: "IX_UserId");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "T_UserAccessFails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_UserId",
                table: "T_UserLoginHistories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                table: "T_UserAccessFails",
                column: "IsDeleted");

            migrationBuilder.AddForeignKey(
                name: "FK_T_UserAccessFails_T_Users_UserId",
                table: "T_UserAccessFails",
                column: "UserId",
                principalTable: "T_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_UserAccessFails_T_Users_UserId",
                table: "T_UserAccessFails");

            migrationBuilder.DropIndex(
                name: "IX_UserId",
                table: "T_UserLoginHistories");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                table: "T_UserAccessFails");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "T_UserAccessFails");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "T_UserLoginHistories",
                newName: "Messsage");

            migrationBuilder.RenameColumn(
                name: "IsLockOut",
                table: "T_UserAccessFails",
                newName: "isLockOut");

            migrationBuilder.RenameIndex(
                name: "IX_UserId",
                table: "T_UserAccessFails",
                newName: "IX_T_UserAccessFails_UserId");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "T_Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_T_UserAccessFails_T_Users_UserId",
                table: "T_UserAccessFails",
                column: "UserId",
                principalTable: "T_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
