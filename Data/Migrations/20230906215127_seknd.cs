using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebGym.Data.Migrations
{
    /// <inheritdoc />
    public partial class seknd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Exercises");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
