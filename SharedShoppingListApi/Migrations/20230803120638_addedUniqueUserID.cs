using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharedShoppingListApi.Migrations
{
    public partial class addedUniqueUserID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UniqueId",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniqueId",
                table: "Users");
        }
    }
}
