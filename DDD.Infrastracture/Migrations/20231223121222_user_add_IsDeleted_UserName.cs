using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class user_add_IsDeleted_UserName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "T_Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "T_Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "T_Users");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "T_Users");
        }
    }
}
