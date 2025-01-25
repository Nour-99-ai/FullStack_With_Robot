using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocalCrafts.Migrations
{
    /// <inheritdoc />
    public partial class test65 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryDate",
                table: "OrderItems",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryDate",
                table: "OrderItems");
        }
    }
}
