using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessengerApi.Migrations
{
    public partial class added_isValidated_column_to_user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RegisterConfirmationToken",
                table: "User",
                newName: "EmailVerificationToken");

            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "User",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "EmailVerificationToken",
                table: "User",
                newName: "RegisterConfirmationToken");
        }
    }
}
