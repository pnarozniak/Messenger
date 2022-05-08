using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessengerApi.Database.Migrations
{
    public partial class added_isDeleted_column_to_message_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "Message",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "Message");
        }
    }
}
