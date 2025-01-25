using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocalCrafts.Migrations
{
    /// <inheritdoc />
    public partial class test4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Sellers");

            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "Sellers",
                newName: "ProfilePictureUrl");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Sellers",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "Sellers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Sellers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bio",
                table: "Sellers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Sellers");

            migrationBuilder.RenameColumn(
                name: "ProfilePictureUrl",
                table: "Sellers",
                newName: "CategoryName");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Sellers",
                newName: "CategoryId");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Sellers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
