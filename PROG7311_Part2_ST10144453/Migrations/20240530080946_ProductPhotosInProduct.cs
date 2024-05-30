using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROG7311_Part2_ST10144453.Migrations
{
    public partial class ProductPhotosInProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "photo_1",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "photo_2",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "photo_3",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "photo_4",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "photo_5",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "photo_1",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "photo_2",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "photo_3",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "photo_4",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "photo_5",
                table: "Products");
        }
    }
}
