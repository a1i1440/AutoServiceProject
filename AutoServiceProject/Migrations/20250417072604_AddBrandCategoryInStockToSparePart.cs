using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoServiceProject.Migrations
{
    /// <inheritdoc />
    public partial class AddBrandCategoryInStockToSparePart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Parts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Parts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "InStock",
                table: "Parts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Parts");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Parts");

            migrationBuilder.DropColumn(
                name: "InStock",
                table: "Parts");
        }
    }
}
